using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using ReactiveUI;
using System.Reactive;
using System.Reactive.Linq;
using Volo.Abp.DependencyInjection;

namespace WinUITemplate.ViewModels
{
	[UsedImplicitly]
	public class LogViewModel : ViewModelBase, IRoutableViewModel, ITransientDependency
	{
		public string UrlPathSegment => @"Log";
		public IScreen HostScreen => LazyServiceProvider.LazyGetRequiredService<IScreen>();

		#region Properties

		private string? _text;
		public string? Text
		{
			get => _text;
			set => this.RaiseAndSetIfChanged(ref _text, value);
		}

		#endregion

		#region Commands

		public ReactiveCommand<Unit, Unit> ClickToShowText { get; }

		#endregion

		public LogViewModel()
		{
			ClickToShowText = ReactiveCommand.CreateFromObservable<Unit, Unit>(_ =>
			{
				Text = ulong.TryParse(Text, out var i) ? $@"{++i}" : $@"{default(ulong)}";
				Logger.LogDebug(@"Text change to {0}", Text);
				return Observable.Return(Unit.Default);
			});
		}
	}
}
