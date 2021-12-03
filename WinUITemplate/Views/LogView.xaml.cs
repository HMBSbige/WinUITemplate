namespace WinUITemplate.Views;

[ExposeServices(typeof(IViewFor<LogViewModel>))]
[UsedImplicitly]
public partial class LogView : ITransientDependency
{
	public LogView(LogViewModel viewModel)
	{
		InitializeComponent();
		ViewModel = viewModel;

		this.WhenActivated(d =>
		{
			this.OneWayBind(ViewModel, vm => vm.Text, v => v.TextBlock.Text).DisposeWith(d);
			this.BindCommand(ViewModel, vm => vm.ClickToShowText, v => v.Button).DisposeWith(d);
		});
	}
}
