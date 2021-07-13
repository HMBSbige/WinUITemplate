using JetBrains.Annotations;
using ReactiveUI;
using System.Reactive.Disposables;
using Volo.Abp.DependencyInjection;
using WinUITemplate.ViewModels;

namespace WinUITemplate.Views
{
	[ExposeServices(typeof(IViewFor<LogViewModel>))]
	[UsedImplicitly]
	public partial class LogView : ITransientDependency
	{
		public LogView()
		{
			InitializeComponent();

			this.WhenActivated(d =>
			{
				this.OneWayBind(ViewModel, vm => vm.Text, v => v.TextBlock.Text).DisposeWith(d);
				this.BindCommand(ViewModel, vm => vm.ClickToShowText, v => v.Button).DisposeWith(d);
			});
		}
	}
}
