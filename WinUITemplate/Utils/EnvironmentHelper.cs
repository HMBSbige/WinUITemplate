using System;
using System.Diagnostics;
using System.IO;

namespace WinUITemplate.Utils
{
	public static class EnvironmentHelper
	{
		public static string GetExecutablePath()
		{
			var p = Process.GetCurrentProcess();
			var res = p.MainModule?.FileName;
			if (res is not null)
			{
				return res;
			}

			var dllPath = AppContext.BaseDirectory;
			return Path.ChangeExtension(dllPath, @"exe");
		}

		public static void SetExePathAsCurrentDirectory()
		{
			var dir = Path.GetDirectoryName(GetExecutablePath());
			if (dir is not null)
			{
				Environment.CurrentDirectory = Path.GetFullPath(dir);
			}
		}
	}
}
