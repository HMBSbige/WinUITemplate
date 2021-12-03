namespace WinUITemplate.Utils;

public static class EnvironmentHelper
{
	public static string GetExecutablePath()
	{
		Process p = Process.GetCurrentProcess();
		return p.MainModule?.FileName ?? Path.ChangeExtension(AppContext.BaseDirectory, @"exe");
	}

	public static void SetExePathAsCurrentDirectory()
	{
		string? dir = Path.GetDirectoryName(GetExecutablePath());
		if (dir is not null)
		{
			Environment.CurrentDirectory = Path.GetFullPath(dir);
		}
	}
}
