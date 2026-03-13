using MagniVerseBackend.Models;

namespace MagniVerseBackend.Services
{
    public class ManualRepository
    {
        private static List<ManualChunk> _chunks = new List<ManualChunk>();

        public static void LoadManual(string pdfPath, PDFService pdfService)
        {
            if (_chunks.Count == 0)
            {
                _chunks = pdfService.LoadManualChunks(pdfPath);
            }
        }

        public static List<ManualChunk> GetChunks()
        {
            return _chunks;
        }
    }
}