namespace WinUITemplate.ViewModels;

public abstract class ViewModelBase : ReactiveObject
{
	public IAbpLazyServiceProvider LazyServiceProvider { get; set; } = null!;

	protected ILoggerFactory LoggerFactory => LazyServiceProvider.LazyGetRequiredService<ILoggerFactory>();

	protected ILogger Logger => LazyServiceProvider.LazyGetService<ILogger>(_ => LoggerFactory.CreateLogger(GetType()));
}
