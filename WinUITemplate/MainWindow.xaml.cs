using ModernWpf.Controls;
using ReactiveUI;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Volo.Abp.DependencyInjection;
using WinUITemplate.ViewModels;

namespace WinUITemplate
{
	public partial class MainWindow : ISingletonDependency
	{
		public MainWindow(
			MainWindowViewModel viewModel,
			SettingViewModel settings
			)
		{
			InitializeComponent();
			ViewModel = viewModel;

			this.WhenActivated(d =>
			{
				this.Bind(ViewModel, vm => vm.Router, v => v.RoutedViewHost.Router).DisposeWith(d);

				Observable.FromEventPattern<NavigationViewSelectionChangedEventArgs>(NavigationView, nameof(NavigationView.SelectionChanged))
				.Subscribe(args =>
				{
					if (args.EventArgs.IsSettingsSelected)
					{
						ViewModel.Router.Navigate.Execute(settings);
						return;
					}

					if (args.EventArgs.SelectedItem is not NavigationViewItem { Tag: string tag })
					{
						return;
					}

					switch (tag)
					{
						case @"1":
						{
							//ViewModel.Router.Navigate.Execute();
							break;
						}
					}
				}).DisposeWith(d);
			});
		}
	}
}
