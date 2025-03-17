using System.Text;

namespace RPGClassLibrary.Operations
{
	public enum LogLevel
	{
		DEBUG,
		INFO,
		WARN,
		ERROR,
		FATAL
	}

	public enum Sender
	{
		FRONT,
		BACK
	}
	/// <summary>
	/// CLIrpg logging module. Writes simple or verbose logs to a specified file.
	/// </summary>
	public class LoggingService
	{
		private readonly object logLock = new object();
		private static string logPath;
		private static string prefix;
		private static Sender origin;

		public LoggingService(Sender sender, string _prefix = "CLIRPG_log")
		{
			try
			{
				string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
				DirectoryInfo? directory = new DirectoryInfo(baseDirectory);

				while (baseDirectory != null && directory.Name != "CommandLineRPG")
				{
					directory = directory.Parent;
				}

				if (directory == null)
				{
					throw new InvalidOperationException("Unable to find target directory.");
				}

				logPath = Path.Combine(directory.FullName, "Output", "Logs");
				prefix = _prefix;
				origin = sender;

				if (!Directory.Exists(logPath))
				{
					Directory.CreateDirectory(logPath);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		public void Log(LogLevel level, string message)
		{
			string content = $"{DateTime.Now:dd-MM-yyyy HH:mm:ss} | [{origin}][{level}]: {message}";
			string path = GetLogFilePath();
			// 05-03-2025 15:33:53 [CLIENT][WARNING]: Client 122.23.21.2 disconnected unexpectedly.
			// 05-03-2025 18:43:22 [SERVER][INFO]: Server started at 127.0.0.1 : 15000.
			Console.WriteLine(message);

			lock (logLock)
			{
				File.AppendAllText(path, content + Environment.NewLine);
			}
		}

		public void ExceptionLog(LogLevel level, Exception ex)
		{
			string content = $"{DateTime.Now:dd-MM-yyyy HH:mm:ss} | [{origin}][{level}]: {ex.InnerException}" +
				$"{Environment.NewLine}Message: {ex.Message}" +
				$"{Environment.NewLine}Stacktrace: {ex.StackTrace}";
			string path = GetLogFilePath();
            Console.WriteLine(content);
			lock (logLock)
			{
				File.AppendAllText(path, content + Environment.NewLine);
			}
		}

		private string GetLogFilePath()
		{
			// Rotate logs daily (e.g., server_log_2025-03-05.txt)
			string date = DateTime.Now.ToString("yyyy-MM-dd");
			return Path.Combine(logPath, $"{prefix}_{date}.txt");
		}
	}
}
