namespace MyNews.Api.Options
{
    public class BackgroundJobsOptions
    {
        public int CleanupIntervalDays { get; set; } = 3;

        public int RssFetchIntervalHours { get; set; } = 8;
    }
}
