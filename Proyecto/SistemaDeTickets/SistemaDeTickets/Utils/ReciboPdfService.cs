using System;
using System.Globalization;
using System.IO;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using SistemaDeTickets.Modelo;

namespace SistemaDeTickets.Utils
{
    public static class ReciboPdfService
    {
        public static string GenerarRecibo(Compra compra, Evento evento, Usuario usuario, string logoPath = null)
        {
            var culture = new CultureInfo("es-CR");
            var doc = new Document();
            doc.Info.Title = $"Recibo #{compra?.Id}";
            doc.Info.Author = "JE Tickets CR";

            // Configurar estilos
            doc.Styles["Normal"].Font.Name = "Calibri";
            doc.Styles["Normal"].Font.Size = 11;

            var h1 = doc.Styles.AddStyle("Heading1", "Normal");
            h1.Font.Size = 18;
            h1.Font.Bold = true;
            h1.ParagraphFormat.Alignment = ParagraphAlignment.Center;

            var h2 = doc.Styles.AddStyle("Heading2", "Normal");
            h2.Font.Size = 14;
            h2.Font.Bold = true;
            h2.ParagraphFormat.SpaceBefore = Unit.FromPoint(15);
            h2.ParagraphFormat.SpaceAfter = Unit.FromPoint(8);

            var totalStyle = doc.Styles.AddStyle("Total", "Normal");
            totalStyle.Font.Size = 14;
            totalStyle.Font.Bold = true;
            totalStyle.ParagraphFormat.Alignment = ParagraphAlignment.Right;

            var sec = doc.AddSection();
            sec.PageSetup.TopMargin = Unit.FromCentimeter(2.5);
            sec.PageSetup.BottomMargin = Unit.FromCentimeter(2.5);
            sec.PageSetup.LeftMargin = Unit.FromCentimeter(2.5);
            sec.PageSetup.RightMargin = Unit.FromCentimeter(2.5);

            // ENCABEZADO
            var headerPara = sec.AddParagraph("JE Tickets CR", "Heading1");
            headerPara.Format.SpaceAfter = Unit.FromPoint(5);

            var titlePara = sec.AddParagraph("Recibo de Compra de Tickets", "Heading1");
            titlePara.Format.SpaceAfter = Unit.FromPoint(20);

            // Línea horizontal
            var linePara = sec.AddParagraph();
            linePara.Format.Borders.Bottom.Width = Unit.FromPoint(1);
            linePara.Format.Borders.Bottom.Color = Colors.Gray;
            linePara.Format.SpaceAfter = Unit.FromPoint(15);

            // DETALLE DE COMPRA
            var detallePara = sec.AddParagraph("DETALLE DE COMPRA", "Heading2");

            var table = sec.AddTable();
            table.Borders.Width = Unit.FromPoint(0.5);
            table.Borders.Color = Colors.Gray;
            table.AddColumn(Unit.FromCentimeter(4));
            table.AddColumn(Unit.FromCentimeter(12));

            void AddRow(string label, string value)
            {
                var row = table.AddRow();
                row.Cells[0].AddParagraph(label).Format.Font.Bold = true;
                row.Cells[1].AddParagraph(value ?? "");
                row.Format.Font.Size = 10;
            }

            AddRow("Código de Compra", compra?.Id.ToString());
            AddRow("Fecha y Hora", $"{compra?.FechaCompra:dd/MM/yyyy HH:mm:ss}");

            // INFORMACIÓN DEL EVENTO
            sec.AddParagraph("INFORMACIÓN DEL EVENTO", "Heading2");

            var eventoTable = sec.AddTable();
            eventoTable.Borders.Width = Unit.FromPoint(0.5);
            eventoTable.Borders.Color = Colors.Gray;
            eventoTable.AddColumn(Unit.FromCentimeter(4));
            eventoTable.AddColumn(Unit.FromCentimeter(12));

            AddRowToTable(eventoTable, "Evento", evento?.Nombre);
            AddRowToTable(eventoTable, "Recinto", evento?.Recinto);
            AddRowToTable(eventoTable, "Fecha y Hora", $"{evento?.Fecha:dd/MM/yyyy HH:mm}");
            AddRowToTable(eventoTable, "Tipo", evento?.Tipo);

            // DATOS DEL USUARIO
            sec.AddParagraph("DATOS DEL USUARIO", "Heading2");

            var usuarioTable = sec.AddTable();
            usuarioTable.Borders.Width = Unit.FromPoint(0.5);
            usuarioTable.Borders.Color = Colors.Gray;
            usuarioTable.AddColumn(Unit.FromCentimeter(4));
            usuarioTable.AddColumn(Unit.FromCentimeter(12));

            AddRowToTable(usuarioTable, "Nombre", usuario?.Nombre);
            AddRowToTable(usuarioTable, "Email", usuario?.Email);

            // TABLA DETALLE DE TICKETS
            sec.AddParagraph("DETALLE DE TICKETS", "Heading2");

            var ticketsTable = sec.AddTable();
            ticketsTable.Borders.Width = Unit.FromPoint(0.5);
            ticketsTable.Borders.Color = Colors.Gray;
            ticketsTable.AddColumn(Unit.FromCentimeter(8)); // Tipo
            ticketsTable.AddColumn(Unit.FromCentimeter(2)); // Cantidad
            ticketsTable.AddColumn(Unit.FromCentimeter(3)); // Precio Unitario
            ticketsTable.AddColumn(Unit.FromCentimeter(3)); // Subtotal

            // Header de tabla
            var headerRow = ticketsTable.AddRow();
            headerRow.Shading.Color = Colors.LightGray;
            headerRow.Cells[0].AddParagraph("Tipo de Ticket").Format.Font.Bold = true;
            headerRow.Cells[1].AddParagraph("Cant.").Format.Font.Bold = true;
            headerRow.Cells[2].AddParagraph("Precio Unit.").Format.Font.Bold = true;
            headerRow.Cells[3].AddParagraph("Subtotal").Format.Font.Bold = true;

            // Fila de datos
            var dataRow = ticketsTable.AddRow();
            dataRow.Cells[0].AddParagraph($"Tickets para {evento?.Nombre}");
            dataRow.Cells[1].AddParagraph(compra?.Cantidad.ToString());
            dataRow.Cells[2].AddParagraph($"₡{evento?.Precio:N0}");
            dataRow.Cells[3].AddParagraph($"₡{(compra?.Cantidad ?? 0) * (evento?.Precio ?? 0):N0}");

            // Fila total
            var totalRow = ticketsTable.AddRow();
            totalRow.Borders.Top.Width = Unit.FromPoint(1);
            totalRow.Cells[2].AddParagraph("TOTAL A PAGAR:").Format.Font.Bold = true;
            totalRow.Cells[3].AddParagraph($"₡{compra?.PrecioTotal:N0}").Format.Font.Bold = true;

            // PIE DE PÁGINA
            sec.AddParagraph().Format.SpaceBefore = Unit.FromPoint(30);
            var footerPara = sec.AddParagraph("Gracias por su compra en JE Tickets CR", "Normal");
            footerPara.Format.Alignment = ParagraphAlignment.Center;
            footerPara.Format.Font.Size = 10;
            footerPara.Format.SpaceAfter = Unit.FromPoint(10);

            var soportePara = sec.AddParagraph("Para soporte técnico: soporte@jetickets.cr | Tel: 2222-3333", "Normal");
            soportePara.Format.Alignment = ParagraphAlignment.Center;
            soportePara.Format.Font.Size = 9;
            soportePara.Format.SpaceAfter = Unit.FromPoint(5);

            var generadoPara = sec.AddParagraph("Documento generado automáticamente por el sistema JE Tickets CR", "Normal");
            generadoPara.Format.Alignment = ParagraphAlignment.Center;
            generadoPara.Format.Font.Size = 8;
            generadoPara.Format.Font.Italic = true;

            // Generar PDF
            var rnd = new PdfDocumentRenderer(unicode: true, embedding: PdfFontEmbedding.Always) { Document = doc };
            rnd.RenderDocument();

            var outPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"receipt_{compra?.Id}.pdf");
            rnd.PdfDocument.Save(outPath);

            return outPath;
        }

        private static void AddRowToTable(Table table, string label, string value)
        {
            var row = table.AddRow();
            row.Cells[0].AddParagraph(label).Format.Font.Bold = true;
            row.Cells[1].AddParagraph(value ?? "");
            row.Format.Font.Size = 10;
        }
    }
}