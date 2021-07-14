using ModernWpf;
using ReactiveUI;
using System.Reactive.Concurrency;

namespace WinUITemplate.Utils
{
	public static class ThemeManagerExtensions
	{
		public static void SetTheme(this ThemeManager manager, ElementTheme theme)
		{
			switch (theme)
			{
				case ElementTheme.Light:
				{
					manager.SetThemeInternal(ApplicationTheme.Light);
					break;
				}
				case ElementTheme.Dark:
				{
					manager.SetThemeInternal(ApplicationTheme.Dark);
					break;
				}
				default:
				{
					manager.SetThemeInternal(default);
					break;
				}
			}
		}

		private static void SetThemeInternal(this ThemeManager manager, ApplicationTheme? theme)
		{
			RxApp.MainThreadScheduler.Schedule(() => manager.ApplicationTheme = theme);
		}
	}
}
