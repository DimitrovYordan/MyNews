using FastText.NetWrapper;

namespace MyNews.Api.Services
{
    public class FastTextLanguageDetector
    {
        private readonly FastTextWrapper _fastText;

        public FastTextLanguageDetector(ILogger<FastTextLanguageDetector> logger)
        {
            var modelPath = Path.Combine(AppContext.BaseDirectory, "DetectLanguage", "lid.176.bin");

            if (!File.Exists(modelPath))
            {
                logger.LogError("FastText model not found at {Path}", modelPath);
                throw new FileNotFoundException("FastText model not found", modelPath);
            }

            _fastText = new FastTextWrapper();
            _fastText.LoadModel(modelPath);

            logger.LogInformation("FastText model loaded successfully.");
        }

        public string Detect(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return "en";

            var predictions = _fastText.PredictMultiple(text, 1);

            if (predictions.Any())
            {
                return predictions.First().Label.Replace("__label__", "");
            }

            return "en";
        }
    }
}
