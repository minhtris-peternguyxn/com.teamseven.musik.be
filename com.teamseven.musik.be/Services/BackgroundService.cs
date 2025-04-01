using com.teamseven.musik.be.Models.Contexts;
using Microsoft.EntityFrameworkCore;

namespace com.teamseven.musik.be.Services
{
  public class KeepAliveService : BackgroundService
{
    private readonly MusikDbContext _context;
    private readonly ILogger<KeepAliveService> _logger;

    public KeepAliveService(MusikDbContext context, ILogger<KeepAliveService> logger)
    {
        _context = context;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await _context.Database.ExecuteSqlRawAsync("SELECT 1", stoppingToken);
                _logger.LogInformation("Keep-alive query executed at {Time}", DateTime.UtcNow);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Keep-alive query failed");
            }
            await Task.Delay(TimeSpan.FromHours(0.5), stoppingToken);
        }
    }
}
}
