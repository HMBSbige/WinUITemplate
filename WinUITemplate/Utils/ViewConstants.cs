namespace WinUITemplate.Utils
{
	public static class ViewConstants
	{
		public const long MaxLogFileSize = 10 * 1024 * 1024; // 10MB
		public const string OutputTemplate = @"[{Timestamp:yyyy-MM-dd HH:mm:ss}] [{Level}] {Message:lj}{NewLine}{Exception}";
		public const string LogFile = $@"Logs/{nameof(WinUITemplate)}.log";
		public const string Identifier = $@"Global\{nameof(WinUITemplate)}";
		public const string ParameterShow = @"-show";
	}
}
