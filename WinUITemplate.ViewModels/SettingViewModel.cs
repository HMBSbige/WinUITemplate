using JetBrains.Annotations;
using ReactiveUI;
using Volo.Abp.DependencyInjection;

namespace WinUITemplate.ViewModels
{
	[UsedImplicitly]
	public class SettingViewModel : ViewModelBase, IRoutableViewModel, ISingletonDependency
	{
		public string UrlPathSegment => @"Settings";
		public IScreen HostScreen => LazyServiceProvider.LazyGetRequiredService<IScreen>();

		#region Properties

		private int _currentTheme;
		public int CurrentTheme
		{
			get => _currentTheme;
			set => this.RaiseAndSetIfChanged(ref _currentTheme, value);
		}

		#endregion
	}
}
