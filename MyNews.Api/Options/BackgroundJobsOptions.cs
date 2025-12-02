namespace MyNews.Api.Options
{
    public class BackgroundJobsOptions
    {
        public int CleanupIntervalDays { get; set; } = 2;

        public int RssFetchIntervalHours { get; set; } = 6;
    }
}
