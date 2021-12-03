global using JetBrains.Annotations;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.DependencyInjection.Extensions;
global using Microsoft.Extensions.Hosting;
global using Microsoft.Extensions.Logging;
global using ModernWpf;
global using ModernWpf.Controls;
global using ReactiveMarbles.ObservableEvents;
global using ReactiveUI;
global using Serilog;
global using Serilog.Events;
global using SingleInstance;
global using Splat;
global using Splat.Microsoft.Extensions.DependencyInjection;
global using System.Diagnostics;
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
	}

	public override void ConfigureServices(ServiceConfigurationContext context)
	{
		context.Services.TryAddSingleton(_ => new SingleInstanceService(ViewConstants.Identifier));
		context.Services.TryAddTransient<RoutingState>();
		context.Services.TryAddTransient(_ => new CompositeDisposable());
	}
}
