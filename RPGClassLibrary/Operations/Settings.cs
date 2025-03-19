using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using RPGClassLibrary.Actors;

namespace RPGClassLibrary.Operations
{
	public class Settings
	{
		// Contains default player and settings
		public static string PlayerName;
		public static int GlobalDifficultyModifier = 1;
		public static bool AutoLoadCharacter = true;
		public static bool DebugOn = false;
		public static ConsoleColor DefaultConsoleColor = ConsoleColor.White;

		public static void SerializeStarter()
		{
			try
			{
				Settings config = new Settings();
				string path = Path.Combine(Utility.GetBasePath(), "Output", "start.config.json");
				string json = JsonSerializer.Serialize(config);

				File.WriteAllText(path, json);
			}
			catch (Exception ex)
			{
				Utility.bLog.ExceptionLog(LogLevel.ERROR, ex);
			}
		}

		public static void StartupSeq()
		{
			if (!File.Exists(Path.Combine(Utility.GetBasePath(), "Output") ))
			{
				Utility.CreateFileSystem();
			}
			string objPath = Path.Combine(Utility.GetBasePath(), "Output", "start.config.json");
			string json = File.ReadAllText(objPath);
			Settings config = JsonSerializer.Deserialize<Settings>(json);
		}
	}
}
