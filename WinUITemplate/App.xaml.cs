using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using System;
using System.Windows;
using Volo.Abp;

namespace WinUITemplate
{
	public partial class App
	{
		private readonly IHost _host;
		private readonly IAbpApplicationWithExternalServiceProvider _application;

		public App()
		{
			Log.Logger = new LoggerConfiguration()
#if DEBUG
				.MinimumLevel.Debug()
				.WriteTo.Async(c => c.Debug(outputTemplate: Constants.OutputTemplate))
#else
				.MinimumLevel.Information()
#endif
				.MinimumLevel.Override(@"Microsoft", LogEventLevel.Information)
				.Enrich.FromLogContext()
				.WriteTo.Async(c => c.File(Constants.LogFile,
							outputTemplate: Constants.OutputTemplate,
							rollingInterval: RollingInterval.Day,
							rollOnFileSizeLimit: true,
							fileSizeLimitBytes: Constants.MaxLogFileSize))
				.CreateLogger();

			_host = CreateHostBuilder();
			_application = _host.Services.GetRequiredService<IAbpApplicationWithExternalServiceProvider>();
		}

		protected override async void OnStartup(StartupEventArgs e)
		{
			try
			{
				Log.Information(@"Starting WPF host...");
				await _host.StartAsync();
				Initialize(_host.Services);
				_host.Services.GetRequiredService<MainWindow>().Show();
			}
			catch (Exception ex)
			{
				Log.Fatal(ex, @"Host terminated unexpectedly!");
			}
		}

		protected override async void OnExit(ExitEventArgs e)
		{
			_application.Shutdown();
			await _host.StopAsync();
			_host.Dispose();
			Log.CloseAndFlush();
		}

		private void Initialize(IServiceProvider serviceProvider)
		{
			_application.Initialize(serviceProvider);
		}

		private static IHost CreateHostBuilder()
		{
			return Host.CreateDefaultBuilder()
					.UseAutofac()
					.UseSerilog()
					.ConfigureServices((hostContext, services) =>
					{
						services.AddApplication<WinUITemplateAppModule>();
					})
					.Build();
		}
	}
}
