namespace MyNews.Api.Options
{
    public class OpenAIOptions
    {
        public string ApiKey { get; set; } = string.Empty;

        public string Model { get; set; } = "gpt-4.1-mini";

        public int MaxTokens { get; set; } = 900;

        public int BatchSize { get; set; } = 3;

        public int ChunkSize { get; set; } = 3;

        public int MaxContentChars { get; set; } = 2500;

        public int Concurrency { get; set; } = 4;

        public int TimeoutSeconds { get; set; } = 180;
    }
}
