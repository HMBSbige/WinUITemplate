using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReactiveMarbles.ObservableEvents;
using ReactiveUI;
using Serilog;
using Serilog.Events;
using SingleInstance;
using Splat.Microsoft.Extensions.DependencyInjection;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows;
using Volo.Abp;
using WinUITemplate.Services;
using WinUITemplate.Utils;

#pragma warning disable VSTHRD100
namespace WinUITemplate
{
	public partial class App
	{
		private readonly IHost _host;
		private readonly CompositeDisposable _disposable;
		private readonly SingleInstanceService _singleInstance;
		private readonly IAbpApplicationWithExternalServiceProvider _application;
		private readonly ArgumentsHandleService _argumentsHandleService;

		public App()
		{
			EnvironmentHelper.SetExePathAsCurrentDirectory();
			Log.Logger = new LoggerConfiguration()
#if DEBUG
				.MinimumLevel.Debug()
				.WriteTo.Async(c => c.Debug(outputTemplate: ViewConstants.OutputTemplate))
#else
				.MinimumLevel.Information()
#endif
				.MinimumLevel.Override(@"Microsoft", LogEventLevel.Information)
				.Enrich.FromLogContext()
				.WriteTo.Async(c => c.File(ViewConstants.LogFile,
						outputTemplate: ViewConstants.OutputTemplate,
						rollingInterval: RollingInterval.Day,
						rollOnFileSizeLimit: true,
						fileSizeLimitBytes: ViewConstants.MaxLogFileSize))
				.CreateLogger();

			_host = CreateHostBuilder();
			_disposable = _host.Services.GetRequiredService<CompositeDisposable>();
			_singleInstance = _host.Services.GetRequiredService<SingleInstanceService>().DisposeWith(_disposable);
			_application = _host.Services.GetRequiredService<IAbpApplicationWithExternalServiceProvider>();
			_argumentsHandleService = _host.Services.GetRequiredService<ArgumentsHandleService>();
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

				await _host.StartAsync();
				Initialize(_host.Services);
				_host.Services.GetRequiredService<MainWindow>().ShowWindow();
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
				_application.Shutdown();
			}

			await _host.StopAsync();
			_host.Dispose();

			Log.CloseAndFlush();
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

		private void Initialize(IServiceProvider serviceProvider)
		{
			_application.Initialize(serviceProvider);
			serviceProvider.UseMicrosoftDependencyResolver();
		}

		private static IHost CreateHostBuilder()
		{
			return Host.CreateDefaultBuilder()
					.UseAutofac()
					.UseSerilog()
					.ConfigureServices((hostContext, services) => services.AddApplication<WinUITemplateAppModule>())
					.Build();
		}
	}
}
