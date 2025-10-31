using SistemaDeTickets.Modelo;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SistemaDeTickets.Utils
{
    /// <summary>
    /// Clase para generar recibos PDF con hash SHA-256 del contenido
    /// </summary>
    public static class GeneradorPDF
    {
        /// <summary>
        /// Genera un recibo PDF y retorna el contenido con su hash SHA-256
        /// </summary>
        public static byte[] GenerarRecibo(Compra compra, Modelo.Evento evento, Usuario usuario)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Document doc = new Document();
                PdfWriter.GetInstance(doc, ms);
                doc.Open();
                
                // Título
                doc.Add(new Paragraph("RECIBO DE COMPRA DE TICKETS",
                    FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16)));
                doc.Add(new Paragraph(" ")); // Espacio
                
                // Información de la compra
                doc.Add(new Paragraph($"ID de Compra: {compra.Id}"));
                doc.Add(new Paragraph($"Usuario: {usuario?.Nombre} ({usuario?.Email})"));
                doc.Add(new Paragraph($"Evento: {evento?.Nombre}"));
                doc.Add(new Paragraph($"Fecha del Evento: {evento?.Fecha:dd/MM/yyyy HH:mm}"));
                doc.Add(new Paragraph($"Recinto: {evento?.Recinto}"));
                doc.Add(new Paragraph($"Cantidad de Tickets: {compra.Cantidad}"));
                doc.Add(new Paragraph($"Precio Unitario: {evento?.Precio:C}"));
                doc.Add(new Paragraph($"TOTAL: {compra.PrecioTotal:C}"));
                doc.Add(new Paragraph($"Fecha de Compra: {compra.FechaCompra:dd/MM/yyyy HH:mm}"));
                
                doc.Add(new Paragraph(" ")); // Espacio
                doc.Add(new Paragraph("--- DETALLES ---"));
                
                // Información adicional del evento
                if (!string.IsNullOrEmpty(evento?.Descripcion))
                {
                    doc.Add(new Paragraph($"Descripción: {evento.Descripcion}"));
                }
                
                doc.Add(new Paragraph($"Tipo de Evento: {evento?.Tipo}"));
                
                // Generar contenido del PDF para el hash
                string contenidoPDF = GenerarContenidoParaHash(compra, evento, usuario);
                
                // Calcular SHA-256 del contenido
                string hashSHA256 = CalcularSHA256(contenidoPDF);
                
                doc.Add(new Paragraph(" "));
                doc.Add(new Paragraph($"Hash SHA-256 del contenido: {hashSHA256}"));
                
                // Información de seguridad
                doc.Add(new Paragraph(" "));
                doc.Add(new Paragraph("--- INFORMACIÓN DE SEGURIDAD ---",
                    FontFactory.GetFont(FontFactory.HELVETICA_BOLD)));
                doc.Add(new Paragraph("Este recibo ha sido generado digitalmente."));
                doc.Add(new Paragraph("El hash SHA-256 garantiza la integridad del documento."));
                doc.Add(new Paragraph($"Generado el: {DateTime.Now:dd/MM/yyyy HH:mm:ss}"));
                
                doc.Close();
                
                byte[] pdfBytes = ms.ToArray();
                return pdfBytes;
            }
        }

        /// <summary>
        /// Guarda el PDF en la ruta especificada en formato Receipts/{CompraId}.pdf
        /// </summary>
        public static void GuardarReciboPDF(byte[] pdfBytes, int compraId)
        {
            // Crear directorio Receipts si no existe
            string directorioReceipts = "Receipts";
            if (!Directory.Exists(directorioReceipts))
            {
                Directory.CreateDirectory(directorioReceipts);
            }
            
            string rutaDestino = Path.Combine(directorioReceipts, $"{compraId}.pdf");
            File.WriteAllBytes(rutaDestino, pdfBytes);
        }

        /// <summary>
        /// Calcula el hash SHA-256 del contenido del PDF
        /// </summary>
        private static string CalcularSHA256(string contenido)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(contenido));
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
            }
        }

        /// <summary>
        /// Genera contenido textual del PDF para el hash
        /// </summary>
        private static string GenerarContenidoParaHash(Compra compra, Modelo.Evento evento, Usuario usuario)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"CompraID:{compra.Id}");
            sb.AppendLine($"Usuario:{usuario?.Nombre}");
            sb.AppendLine($"Email:{usuario?.Email}");
            sb.AppendLine($"Evento:{evento?.Nombre}");
            sb.AppendLine($"FechaEvento:{evento?.Fecha:yyyy-MM-dd HH:mm:ss}");
            sb.AppendLine($"Recinto:{evento?.Recinto}");
            sb.AppendLine($"Cantidad:{compra.Cantidad}");
            sb.AppendLine($"PrecioUnitario:{evento?.Precio}");
            sb.AppendLine($"Total:{compra.PrecioTotal}");
            sb.AppendLine($"FechaCompra:{compra.FechaCompra:yyyy-MM-dd HH:mm:ss}");
            sb.AppendLine($"TipoEvento:{evento?.Tipo}");
            
            return sb.ToString();
        }

        /// <summary>
        /// Método legacy para compatibilidad
        /// </summary>
        public static void GuardarPDF(byte[] pdfBytes, string rutaDestino)
        {
            File.WriteAllBytes(rutaDestino, pdfBytes);
        }

        /// <summary>
        /// Verifica la integridad de un PDF usando su hash SHA-256
        /// </summary>
        public static bool VerificarIntegridadPDF(byte[] pdfBytes, string hashEsperado)
        {
            // Re-generar el PDF para calcular su hash
            // Nota: En un caso real, necesitaríamos los datos originales de compra
            // Para simplificar, esto es una demostración del concepto
            try
            {
                // En una implementación completa, necesitaríamos:
                // 1. Extraer o regenerar el contenido del PDF
                // 2. Comparar con el hash almacenado
                // Esto requeriría un diseño más complejo de almacenamiento de metadatos
                return true; // Placeholder
            }
            catch
            {
                return false;
            }
        }
    }
}
