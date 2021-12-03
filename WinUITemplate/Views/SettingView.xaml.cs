namespace WinUITemplate.Views;

[ExposeServices(typeof(IViewFor<SettingViewModel>))]
[UsedImplicitly]
public partial class SettingView : ITransientDependency
{
	public SettingView(SettingViewModel viewModel)
	{
		InitializeComponent();
		ViewModel = viewModel;

		this.WhenActivated(d =>
		{
			this.Bind(ViewModel, vm => vm.CurrentTheme, v => v.ThemeRadioButtons.SelectedIndex).DisposeWith(d);
			ViewModel.WhenAnyValue(vm => vm.CurrentTheme)
				.Throttle(TimeSpan.FromSeconds(0.1))
				.DistinctUntilChanged()
				.Subscribe(i => ThemeManager.Current.SetTheme((ElementTheme)i))
				.DisposeWith(d);
		});
	}
}
