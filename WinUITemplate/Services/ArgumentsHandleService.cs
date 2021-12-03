namespace WinUITemplate.Services;

[UsedImplicitly]
public class ArgumentsHandleService : ServiceBase, ITransientDependency
{
	private SingleInstanceService SingleInstance => LazyServiceProvider.LazyGetRequiredService<SingleInstanceService>();

	public async ValueTask<bool> SendShowCommandAsync(CancellationToken token = default)
	{
		try
		{
			Logger.LogInformation(@"This is not the first application instance, sending show command...");

			string response = await SingleInstance.SendMessageToFirstInstanceAsync(ViewConstants.ParameterShow, token);

			if (response is ViewConstants.ParameterShow)
			{
				Logger.LogInformation(@"Show command response received, activating and quitting...");
				return true;
			}

			throw new Exception($@"Receive error message: {response}");
		}
		catch (Exception ex)
		{
			Logger.LogError(ex, @"Show command response received failed!");
			return false;
		}
	}

	public void ArgumentsReceived((string, Action<string>) receive)
	{
		(string message, Action<string> endFunc) = receive;
		HashSet<string> args = message
			.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
			.ToHashSet();

		if (args.Contains(ViewConstants.ParameterShow))
		{
			RxApp.MainThreadScheduler.Schedule(() => LazyServiceProvider.LazyGetRequiredService<MainWindow>().ShowWindow());
			endFunc(ViewConstants.ParameterShow);
			return;
		}

		endFunc(@"???");
	}
}
