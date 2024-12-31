using OrderFlow.Controllers.Identity;

namespace OrderFlow.Services.Security;

public class RevokedTokenCleanupService : IHostedService, IDisposable
{
    private Timer _timer;
    private bool _started;

    public void Dispose()
    {
        _timer?.Dispose();
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        if (_started) return Task.CompletedTask;
        _timer = new Timer(CleanupRevokedTokens, null, TimeSpan.Zero, TimeSpan.FromDays(1));
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    private void CleanupRevokedTokens(object? state)
    {
        var expirationThreshold = DateTime.UtcNow.AddDays(-1);

        foreach (var token in AccountController.RevokedTokens.Keys)
            if (AccountController.RevokedTokens[token].Item2 < expirationThreshold)
                AccountController.RevokedTokens.TryRemove(token, out _);
    }
}