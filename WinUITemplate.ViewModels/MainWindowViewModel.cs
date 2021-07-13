using ReactiveUI;
using Volo.Abp.DependencyInjection;

namespace WinUITemplate.ViewModels
{
	[ExposeServices(
		typeof(MainWindowViewModel),
		typeof(IScreen)
		)]
	public sealed class MainWindowViewModel : ViewModelBase, IScreen, ISingletonDependency
	{
		public RoutingState Router => LazyServiceProvider.LazyGetRequiredService<RoutingState>();
	}
}
