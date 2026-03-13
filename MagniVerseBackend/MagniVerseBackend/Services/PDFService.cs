using UglyToad.PdfPig;
using MagniVerseBackend.Models;

namespace MagniVerseBackend.Services
{
    public class PDFService
    {
        public List<ManualChunk> LoadManualChunks(string filePath)
        {
            var chunks = new List<ManualChunk>();

            using (var document = PdfDocument.Open(filePath))
            {
                foreach (var page in document.GetPages())
                {

                    Console.WriteLine(page.Text);

                    chunks.Add(new ManualChunk
                    {
                        Text = page.Text
                    });
                }
            }

            return chunks;
        }
    }
}