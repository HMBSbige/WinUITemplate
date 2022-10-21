namespace WinUITemplate.Utils;

public static class EnvironmentHelper
{
	public static void SetExePathAsCurrentDirectory()
	{
		string dir = Path.GetDirectoryName(Environment.ProcessPath) ?? AppContext.BaseDirectory;
		Environment.CurrentDirectory = Path.GetFullPath(dir);
	}
}
