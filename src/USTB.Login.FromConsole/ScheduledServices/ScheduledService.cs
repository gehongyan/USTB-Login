namespace USTB.Login.FromConsole.ScheduledServices;

public abstract class ScheduledService : IHostedService, IDisposable
{
    private readonly ILogger _logger;
    private readonly string _serviceName;
    private readonly Timer _timer;
    private TimeSpan _dueTime;
    private TimeSpan _period;

    protected ScheduledService(TimeSpan dueTime, TimeSpan period, ILogger logger, string serviceName)
    {
        _logger = logger;
        _serviceName = serviceName;
        _dueTime = dueTime;
        _period = period;
        _timer = new Timer(Execute!, null, Timeout.Infinite, 0);
    }

    public virtual void Dispose()
    {
        _timer.Dispose();
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.Information("定时服务 {ServiceName} 已启动", _serviceName);
        _timer.Change(_dueTime, _period);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.Information("定时服务 {ServiceName} 已终止", _serviceName);
        _timer.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Execute(object state = null!)
    {
        try
        {
            _logger.Verbose("定时服务 {ServiceName} 已触发", _serviceName);
            ExecuteAsync().GetAwaiter().GetResult();
        }
        catch (Exception e)
        {
            _logger.Error(e, "定时服务 {ServiceName} 执行过程中发生异常", _serviceName);
        }
        finally
        {
            _logger.Verbose("定时服务 {ServiceName} 执行已完成", _serviceName);
        }
    }

    protected abstract Task ExecuteAsync();

    protected void ChangeTimer(TimeSpan? dueTime = null, TimeSpan? period = null)
    {
        _dueTime = dueTime ?? _dueTime;
        _period = period ?? _period;

        _timer.Change(_dueTime, _period);
    }
}