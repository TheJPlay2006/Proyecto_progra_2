using System;
using System.IO;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;

namespace SistemaDeTickets.Utils
{
    public static class PdfSelfTest
    {
        /// <summary>
        /// Ejecuta una prueba mínima de PDFsharp + MigraDoc sin depender de datos del dominio
        /// </summary>
        public static string Run()
        {
            try
            {
                Console.WriteLine("[SELFTEST] Iniciando prueba de PDFsharp + MigraDoc...");

                // Crear documento simple
                var doc = new Document();
                doc.Info.Title = "PDFsharp Self-Test";

                // Estilo básico
                var normalStyle = doc.Styles["Normal"];
                normalStyle.Font.Name = "Arial";
                normalStyle.Font.Size = 12;

                // Sección
                var section = doc.AddSection();

                // Contenido de prueba
                var para = section.AddParagraph("PDFsharp + MigraDoc funcionando correctamente");
                para.Format.Font.Size = 14;
                para.Format.Font.Bold = true;
                para.Format.SpaceAfter = Unit.FromCentimeter(0.5);

                var infoPara = section.AddParagraph();
                infoPara.AddText("Fecha de prueba: ");
                infoPara.AddText(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                infoPara.Format.SpaceAfter = Unit.FromCentimeter(0.3);

                var unicodePara = section.AddParagraph("Prueba Unicode: áéíóú ñ ÁÉÍÓÚ Ñ");
                unicodePara.Format.Font.Size = 10;

                // Renderizar
                var renderer = new PdfDocumentRenderer(unicode: true, embedding: PdfFontEmbedding.Always)
                {
                    Document = doc
                };
                renderer.RenderDocument();

                // Guardar archivo
                var outDir = AppDomain.CurrentDomain.BaseDirectory;
                var fileName = "pdf-selftest.pdf";
                var filePath = Path.Combine(outDir, fileName);
                renderer.PdfDocument.Save(filePath);

                Console.WriteLine($"[SELFTEST] PDF generado exitosamente: {filePath}");
                return filePath;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SELFTEST ERROR] Error en prueba PDF: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"[SELFTEST ERROR] Inner: {ex.InnerException.Message}");
                throw;
            }
        }
    }
}