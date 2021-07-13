using JetBrains.Annotations;
using ReactiveUI;
using Volo.Abp.DependencyInjection;

namespace WinUITemplate.ViewModels
{
	[UsedImplicitly]
	public class SettingViewModel : ViewModelBase, IRoutableViewModel, ITransientDependency
	{
		public string UrlPathSegment => @"Settings";
		public IScreen HostScreen => LazyServiceProvider.LazyGetRequiredService<IScreen>();
	}
}
