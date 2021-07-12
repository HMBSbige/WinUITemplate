using JetBrains.Annotations;
using ReactiveUI;

namespace WinUITemplate.ViewModels
{
	[UsedImplicitly]
	public class SettingViewModel : ViewModelBase, IRoutableViewModel
	{
		public string UrlPathSegment => @"Settings";
		public IScreen HostScreen { get; }

		public SettingViewModel(IScreen hostScreen)
		{
			HostScreen = hostScreen;
		}
	}
}
