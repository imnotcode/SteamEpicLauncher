using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SteamEpicLauncher
{
	class Program
	{
		[DllImport("User32.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool ShowWindow([In] IntPtr hWnd, [In] int nCmdShow);

		static void Main(string[] args)
		{
			if (args.Length != 2)
			{
				Console.WriteLine("ERROR: Needs launch URL and EXE Name");
				return;
			}

			var epicUrl = args[0];
			var exeName = args[1];

			var ps = new ProcessStartInfo(epicUrl)
			{
				UseShellExecute = true,
				Verb = "open"
			};

			Console.WriteLine($"Starting url: {epicUrl}");
			Process.Start(ps);

			Thread.Sleep(15000);

			var gameProcesses = Process.GetProcessesByName(exeName);

			if (gameProcesses.Length == 0)
			{
				Console.WriteLine($"Could not find a single process with name: {exeName}");
				return;
			}

			Console.WriteLine($"Game started.");

			IntPtr handle = Process.GetCurrentProcess().MainWindowHandle;
			ShowWindow(handle, 6);

			gameProcesses[0].WaitForExit();
		}
	}
}