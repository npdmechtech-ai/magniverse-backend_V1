namespace MagniVerseBackend.Models
{
    public class RepairInstruction
    {
        public List<string>? Tools { get; set; }
        public List<RepairStep>? Steps { get; set; }
        public string? Note { get; set; }
        public string? Warning { get; set; }
    }

    public class RepairStep
    {
        public int Step { get; set; }
        public string? Text { get; set; }
        public string? Image { get; set; }
    }
}