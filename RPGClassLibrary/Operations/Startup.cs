﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using RPGClassLibrary.Actors;

namespace RPGClassLibrary.Operations
{
	public class Startup
	{
		// loads and deserializes on startup
		// Contains default player and settings
		public static string PlayerName { get; set; }
		public static int GlobalDifficultyModifier { get; set; } = 1;
		public static bool AutoLoadCharacter { get; set; } = true;
		public static bool DebugOn { get; set; } = false;

		public static void StartupSeq()
		{
			if (!File.Exists(Path.Combine(Utility.GetBasePath(), "Output") ))
			{
				Utility.CreateFileSystem();
			}
			string objPath = Path.Combine(Utility.GetBasePath(), "Output", "Startup.config.json");
			string json = File.ReadAllText(objPath);
			
		}
	}
}
