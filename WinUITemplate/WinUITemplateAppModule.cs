using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace WinUITemplate
{
	[UsedImplicitly]
	[DependsOn(typeof(AbpAutofacModule))]
	public class WinUITemplateAppModule : AbpModule
	{
		public override void ConfigureServices(ServiceConfigurationContext context)
		{
			context.Services.AddSingleton<MainWindow>();
		}
	}
}
