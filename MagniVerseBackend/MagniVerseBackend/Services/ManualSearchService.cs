using MagniVerseBackend.Models;

namespace MagniVerseBackend.Services
{
    public class ManualSearchService
    {
        public ManualChunk FindRelevantChunk(List<ManualChunk> chunks, string question)
        {
            return chunks
                .OrderByDescending(c => c.Text.Contains(question, StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault();
        }
    }
}