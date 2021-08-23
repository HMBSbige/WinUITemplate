using Microsoft.Extensions.DependencyInjection;
using ModernWpf.Controls;
using ReactiveMarbles.ObservableEvents;
using ReactiveUI;
using System;
using System.Linq;
using System.Reactive.Disposables;
using Volo.Abp.DependencyInjection;
using WinUITemplate.ViewModels;

namespace WinUITemplate
{
	public partial class MainWindow : ISingletonDependency
	{
		public IAbpLazyServiceProvider LazyServiceProvider { get; set; } = null!;

		// ReSharper disable once MemberInitializerValueIgnored
		public IServiceProvider ServiceProvider { get; set; } = null!;

		public MainWindow(MainWindowViewModel viewModel)
		{
			InitializeComponent();
			ViewModel = viewModel;

			this.WhenActivated(d =>
			{
				this.Bind(ViewModel, vm => vm.Router, v => v.RoutedViewHost.Router).DisposeWith(d);

				NavigationView.Events().SelectionChanged
				.Subscribe(parameter =>
				{
					if (parameter.args.IsSettingsSelected)
					{
						ViewModel.Router.NavigateAndReset.Execute(ServiceProvider.GetRequiredService<SettingViewModel>());
						return;
					}

					if (parameter.args.SelectedItem is not NavigationViewItem { Tag: string tag })
					{
						return;
					}

					switch (tag)
					{
						case @"1":
						{
							ViewModel.Router.NavigateAndReset.Execute(ServiceProvider.GetRequiredService<LogViewModel>());
							break;
						}
					}
				}).DisposeWith(d);

				NavigationView.SelectedItem = NavigationView.MenuItems.OfType<NavigationViewItem>().First();
			});
		}
	}
}
