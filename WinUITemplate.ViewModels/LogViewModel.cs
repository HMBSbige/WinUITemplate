namespace WinUITemplate.ViewModels;

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
		ClickToShowText = ReactiveCommand.CreateFromObservable<Unit, Unit>(unit =>
		{
			Text = ulong.TryParse(Text, out ulong i) ? $@"{++i}" : default(ulong).ToString();
			Logger.LogDebug(@"Text change to {text}", Text);
			return Observable.Return(unit);
		});
	}
}
