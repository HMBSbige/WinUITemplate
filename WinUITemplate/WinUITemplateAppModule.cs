global using JetBrains.Annotations;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.DependencyInjection.Extensions;
global using Microsoft.Extensions.Logging;
global using ModernWpf;
global using ModernWpf.Controls;
global using ReactiveMarbles.ObservableEvents;
global using ReactiveUI;
global using Serilog;
global using SingleInstance;
global using Splat;
global using Splat.Microsoft.Extensions.DependencyInjection;
global using System.IO;
global using System.Reactive.Concurrency;
global using System.Reactive.Disposables;
global using System.Reactive.Linq;
global using System.Windows;
global using Volo.Abp;
global using Volo.Abp.Autofac;
global using Volo.Abp.DependencyInjection;
global using Volo.Abp.Modularity;
global using WinUITemplate.Services;
global using WinUITemplate.Utils;
global using WinUITemplate.ViewModels;
global using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace WinUITemplate;

[DependsOn(
	typeof(AbpAutofacModule),
	typeof(WinUITemplateViewModelsModule)
)]
[UsedImplicitly]
public class WinUITemplateAppModule : AbpModule
{
	public override void PreConfigureServices(ServiceConfigurationContext context)
	{
		context.Services.UseMicrosoftDependencyResolver();
		Locator.CurrentMutable.InitializeSplat();
		Locator.CurrentMutable.InitializeReactiveUI(RegistrationNamespace.Wpf);

		context.Services.ReplaceConfiguration(new ConfigurationBuilder()
#if DEBUG
			.AddJsonFile(@"appsettings.Development.json", true, true)
#else
			.AddJsonFile(@"appsettings.json", true, true)
#endif
			.AddEnvironmentVariables()
			.Build());
	}

	public override void ConfigureServices(ServiceConfigurationContext context)
	{
		IConfiguration configuration = context.Services.GetConfiguration();

		context.Services.TryAddSingleton(_ => new SingleInstanceService(ViewConstants.Identifier));
		context.Services.TryAddTransient<RoutingState>();
		context.Services.TryAddTransient(_ => new CompositeDisposable());
		context.Services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger(), true));
	}
}
