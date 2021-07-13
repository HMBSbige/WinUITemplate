namespace WinUITemplate.Utils
{
	public static class ViewConstants
	{
		public const long MaxLogFileSize = 10 * 1024 * 1024; // 10MB
		public const string OutputTemplate = @"[{Timestamp:yyyy-MM-dd HH:mm:ss}] [{Level}] {Message:lj}{NewLine}{Exception}";
		public static readonly string LogFile = $@"Logs/{nameof(WinUITemplate)}.log"; //TODO: const in .NET6.0
		public static readonly string Identifier = $@"Global\{nameof(WinUITemplate)}"; //TODO: const in .NET6.0
		public const string ParameterShow = @"-show";
	}
}
