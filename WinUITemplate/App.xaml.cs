#pragma warning disable VSTHRD100
namespace WinUITemplate;

public partial class App
{
	private readonly IAbpApplicationWithInternalServiceProvider _application;

	private readonly CompositeDisposable _disposable;
	private readonly SingleInstanceService _singleInstance;
	private readonly ArgumentsHandleService _argumentsHandleService;

	public App()
	{
		EnvironmentHelper.SetExePathAsCurrentDirectory();

		_application = AbpApplicationFactory.Create<WinUITemplateAppModule>(options => options.UseAutofac());

		_application.Initialize();
		_application.ServiceProvider.UseMicrosoftDependencyResolver();

		_disposable = _application.Services.GetRequiredService<CompositeDisposable>();
		_singleInstance = _application.Services.GetRequiredService<SingleInstanceService>().DisposeWith(_disposable);
		_argumentsHandleService = _application.Services.GetRequiredService<ArgumentsHandleService>();
	}

	protected override async void OnStartup(StartupEventArgs e)
	{
		try
		{
			Current.Events().DispatcherUnhandledException.Subscribe(args => UnhandledException(args.Exception));

			if (!_singleInstance.TryStartSingleInstance())
			{
				if (await _argumentsHandleService.SendShowCommandAsync())
				{
					Current.Shutdown((int)ExitCode.Success);
				}
				else
				{
					Current.Shutdown((int)ExitCode.NotFirstInstance);
				}
				return;
			}

			Log.Information(@"Starting WPF host...");
			_singleInstance.Received
				.ObserveOn(RxApp.TaskpoolScheduler)
				.Subscribe(_argumentsHandleService.ArgumentsReceived)
				.DisposeWith(_disposable);
			_singleInstance.StartListenServer();

			_application.Services.GetRequiredService<MainWindow>().ShowWindow();
		}
		catch (Exception ex)
		{
			Log.Fatal(ex, @"Host terminated unexpectedly!");
			Current.Shutdown((int)ExitCode.UnknownFailed);
		}
	}

	protected override async void OnExit(ExitEventArgs e)
	{
		Log.Information($@"Exiting with code: {(ExitCode)e.ApplicationExitCode}...");

		_disposable.Dispose();

		if (_application.ServiceProvider is not null)
		{
			await _application.ShutdownAsync();
		}

		await Log.CloseAndFlushAsync();

		Environment.Exit(e.ApplicationExitCode);
	}

	private static void UnhandledException(Exception ex)
	{
		try
		{
			Log.Fatal(ex, @"Unhandled exception");
			MessageBox.Show($@"Unhandled exception：{ex}", nameof(WinUITemplate), MessageBoxButton.OK, MessageBoxImage.Error);
		}
		finally
		{
			Current.Shutdown((int)ExitCode.UnknownFailed);
		}
	}
}
