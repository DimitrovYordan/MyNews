using Microsoft.EntityFrameworkCore;
using MyNews.Api.Data;
using MyNews.Api.Interfaces;
using MyNews.Api.Models;

namespace MyNews.Api.Services
{
    public class UserActivityService : IUserActivityService
    {
        private readonly AppDbContext _context;

        public UserActivityService(AppDbContext context)
        {
            _context = context;
        }

        public async Task RecordLoginAsync(Guid userId)
        {
            var activity = await _context.UserActivities
                .FirstOrDefaultAsync(a => a.UserId == userId);

            if (activity == null)
            {
                activity = new UserActivity
                {
                    UserId = userId,
                    LoginCount = 1,
                    LastLoginUtc = DateTime.UtcNow,
                };

                _context.UserActivities.Add(activity);
            }
            else
            {
                activity.LoginCount++;
                activity.LastLoginUtc = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
        }
    }
}
