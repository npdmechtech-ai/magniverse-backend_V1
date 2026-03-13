using Microsoft.AspNetCore.Mvc;
using MagniVerseBackend.Services;
using MagniVerseBackend.Models;
using System.Text.Json;

namespace MagniVerseBackend.Controllers
{
    [ApiController]
    [Route("chat")]
    public class ChatController : ControllerBase
    {
        private readonly AIService _aiService = new AIService();
        private readonly ManualSearchService _searchService = new ManualSearchService();

        [HttpPost]
        public async Task<IActionResult> AskQuestion([FromBody] ChatRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Question))
            {
                return BadRequest("Question cannot be empty.");
            }

            try
            {
                // Get cached manual chunks
                var chunks = ManualRepository.GetChunks();

                // Find relevant section
                var relevantChunk = _searchService.FindRelevantChunk(chunks, request.Question);

                // AI prompt
                var prompt = $@"
You are a professional mechanical repair assistant.

Using the manual section below, extract the repair procedure.

Return ONLY JSON in this format:

{{
  ""tools"": [""tool1"", ""tool2""],
  ""steps"": [
    {{ ""step"": 1, ""text"": ""instruction"", ""image"": """" }}
  ],
  ""note"": ""optional note"",
  ""warning"": ""safety warning""
}}

Extract the tools required if mentioned in the manual.

Manual Section:
{relevantChunk?.Text}

User Question:
{request.Question}
";

                // Ask AI
                var aiJson = await _aiService.AskAI(prompt);

                RepairInstruction instruction;

                try
                {
                    instruction = JsonSerializer.Deserialize<RepairInstruction>(
                        aiJson,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        }
                    );
                }
                catch
                {
                    // Fallback if AI returns non-JSON text
                    instruction = new RepairInstruction
                    {
                        Tools = new List<string>(),
                        Steps = new List<RepairStep>
                        {
                            new RepairStep
                            {
                                Step = 1,
                                Text = aiJson,
                                Image = ""
                            }
                        },
                        Note = "",
                        Warning = ""
                    };
                }

                return Ok(instruction);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "AI processing failed",
                    error = ex.Message
                });
            }
        }
    }

    public class ChatRequest
    {
        public string? Question { get; set; }
    }
}