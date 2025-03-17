namespace RPGClassLibrary.Operations
{
	public class Utility
	{
		public static LoggingService bLog = new LoggingService(Sender.BACK);
		public static LoggingService fLog = new LoggingService(Sender.FRONT);

		public static string GetBasePath()
		{
			string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
			DirectoryInfo? directory = new DirectoryInfo(baseDirectory);

			string solutionName = "CLIrpg-Remake"; // TODO: Find a way to autofill this
			while (baseDirectory != null && directory.Name != solutionName)
			{
				directory = directory.Parent;
			}

			string bPath = directory.FullName;

			return bPath;
		}
		public static void CreateFileSystem()
		{
			try
			{
				string basePath = GetBasePath();
				string outputPath = Path.Combine(basePath, "Output");
				string[] folders = { "Characters", "Logs", "Abilities", "Items", "Enemies" };

				if (!Directory.Exists(outputPath))
				{
					Directory.CreateDirectory(outputPath);
				}

				foreach (string folder in folders)
				{
					if (!Directory.Exists(Path.Combine(outputPath, folder)))
					{
						Directory.CreateDirectory(Path.Combine(outputPath, folder));
					}
				}

				Directory.CreateDirectory(Path.Combine(outputPath, "Abilities", "Effects"));
				Directory.CreateDirectory(Path.Combine(outputPath, "Abilities", "Chants"));

				/*
					Output
						|- Characters
						|- Logs
						|- Abilities
							|- Effects
							|- Chants
						|- Items
						|- Enemies
				*/
			}
			catch (Exception ex)
			{
                Console.WriteLine(ex.Message);
			}
		}
		public static string ValidateInput(string message, string errMsg = null)
		{
			string output = "";
			do
			{
                Console.WriteLine(message);
				output = Console.ReadLine();
				if (String.IsNullOrEmpty(output))
				{
                    Console.WriteLine(errMsg);
					Console.ReadKey();
					Console.Clear();
				}
			}
			while (String.IsNullOrEmpty(output));

			return output;
		}
	}
}
