using SistemaDeTickets.Modelo;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace SistemaDeTickets.Utils
{
    public static class GeneradorPDF
    {
        public static byte[] GenerarRecibo(Compra compra, Evento evento, Usuario usuario)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Document doc = new Document();
                PdfWriter.GetInstance(doc, ms);
                doc.Open();
                doc.Add(new Paragraph("Recibo de Compra de Tiques"));
                doc.Add(new Paragraph($"Usuario: {usuario?.Nombre}"));
                doc.Add(new Paragraph($"Evento: {evento?.Nombre}"));
                doc.Add(new Paragraph($"Cantidad: {compra.Cantidad}"));
                doc.Add(new Paragraph($"Total: {compra.PrecioTotal:C}"));
                doc.Add(new Paragraph($"Fecha: {compra.FechaCompra:dd/MM/yyyy HH:mm}"));
                doc.Close();
                return ms.ToArray();
            }
        }

        public static void GuardarPDF(byte[] pdfBytes, string rutaDestino)
        {
            File.WriteAllBytes(rutaDestino, pdfBytes);
        }
    }
}
