using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ReactiveUI;
using SingleInstance;
using Splat;
using Splat.Microsoft.Extensions.DependencyInjection;
using System.Reactive.Disposables;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
using WinUITemplate.Utils;
using WinUITemplate.ViewModels;

namespace WinUITemplate
{
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
}
