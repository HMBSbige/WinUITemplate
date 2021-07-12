using JetBrains.Annotations;
using ReactiveUI;
using Splat;
using Splat.Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
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
	}
}
