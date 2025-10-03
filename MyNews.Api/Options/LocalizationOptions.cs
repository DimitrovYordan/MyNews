namespace MyNews.Api.Options
{
    public class LocalizationOptions
    {
        public string DefaultLanguage { get; set; } = "EN";

        public string[] TargetLanguages { get; set; } = Array.Empty<string>();
    }
}
