using JetBrains.Annotations;
using ReactiveUI;
using Volo.Abp.DependencyInjection;
using WinUITemplate.ViewModels;

namespace WinUITemplate.Views
{
	[ExposeServices(typeof(IViewFor<SettingViewModel>))]
	[UsedImplicitly]
	public partial class SettingView : ITransientDependency
	{
		public SettingView(SettingViewModel viewModel)
		{
			InitializeComponent();
			ViewModel = viewModel;
		}
	}
}
