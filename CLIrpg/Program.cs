using RPGClassLibrary.Mechanics;
using RPGClassLibrary.Operations;
using RPGClassLibrary.Actors;
using RPGClassLibrary.Items;

namespace CLIrpg
{
	internal class Program
	{
		public static Player player = null; //public player reference
		public static bool Running = true;
		static void Main(string[] args)
		{
			try
			{
				/*
				Startup
				|- Deserialize startup object and execute it
				|- Create output files if nessacary (Utility.CreateFileSystem)
				*/
				TempMenu();
			}
			catch (Exception ex)
			{
				Utility.fLog.ExceptionLog(LogLevel.ERROR, ex);
			}
		}

		static void OpeningSeq()
		{
			string pname = Utility.ValidateInput("What is your name?");
			string rname = Utility.ValidateInput("What is your role?");
			string rdesc = Utility.ValidateInput("How would you describe your role?");
			Player init = Player.PlayerFactory(pname, rname, rdesc);
		}

		static void TempMenu()
		{
			try
			{
				while (Running)
				{
					Console.WriteLine("[SERIALIZE / DESERIALIZE / WEAPON / REDISTRIBUTE / STARTER / ENEMY / EFFECT]");
					string input = Console.ReadLine().ToLower();

					if (input == "except")
					{
						throw new Exception("test");
					}

					string pname = "";
					switch (input)
					{
						case "weapon":
							Weapon w = Weapon.WeaponFactory(ItemRarity.Legendary);
							Console.ReadKey();
							Console.WriteLine("insert player name");
							pname = Console.ReadLine();
							Player temp = JSONOps.Deserialize.DeserializePlayer(pname);
							temp.AddItemToInventory(w);
							JSONOps.Serialize.SerializePlayer(temp);
							break;
						case "serialize":
							Console.WriteLine("insert player name");
							pname = Console.ReadLine();
							Console.WriteLine("insert role name");
							string rname = Console.ReadLine();
							Player j = Player.PlayerFactory(pname, rname);

							j.Info();
							Console.ReadKey();
							JSONOps.Serialize.SerializePlayer(j);
							break;

						case "deserialize":
							Console.WriteLine("insert player name");
							pname = Console.ReadLine();
							Player d = JSONOps.Deserialize.DeserializePlayer(pname);
							d.Info();
							Console.WriteLine("amount of exp");
							int amt = Convert.ToInt32(Console.ReadLine());
							d.GainExp(amt);
							d.Info();
							Console.WriteLine();
							d.PrintInventory();
							JSONOps.Serialize.SerializePlayer(d);
							Console.Read();
							break;

						case "exp":
							Console.WriteLine("insert player name");
							pname = Console.ReadLine();
							Player plrgrg = JSONOps.Deserialize.DeserializePlayer(pname);
							Console.WriteLine("How much exp?");
							int amount = Convert.ToInt32(Console.ReadLine());
							plrgrg.GainExp(amount);
							JSONOps.Serialize.SerializePlayer(plrgrg);
							Console.ReadLine();
							break;

						case "redistribute":
							Console.WriteLine("insert player name");
							pname = Console.ReadLine();
							Player r = JSONOps.Deserialize.DeserializePlayer(pname);
							r.Info();
							r.RedistributeStats();
							r.Info();
							JSONOps.Serialize.SerializePlayer(r);
							break;

						case "starter":
							Settings.SerializeStarter();
							Console.WriteLine("starter serialized");
							Console.ReadKey();
							break;

						case "enemy":
							string diff = Console.ReadLine();
							EnemyClass ec = diff switch
							{
								"basic" or "b" => EnemyClass.Basic,
								"strong" or "s" => EnemyClass.Strong,
								"boss" => EnemyClass.Boss,
								_ => EnemyClass.Basic
							};
							Enemy e = Enemy.EnemyFactory(ec, 5);
							e.Info();
							JSONOps.Serialize.SerializeEnemy(e);
							break;

						case "effect":
							Console.WriteLine("Insert effect to give.");
							string eff = Console.ReadLine();
							Player ep = JSONOps.Deserialize.DeserializePlayer("Craig");
							Effect effect = Effect.EffectGen(eff, 10, 3, 100);
							ep.EffectHandler.AddEffect(effect);
							JSONOps.Serialize.SerializePlayer(ep);
							ep.EffectHandler.EffectExecutor();
							ep.Info();
							Console.Read();
							//TODO: refactor logic
							break;

						case "process":
							Player plr = JSONOps.Deserialize.DeserializePlayer("Craig");
							plr.Stats.ShowStats();
							plr.EffectHandler.EffectExecutor();
							Console.WriteLine();
							plr.Stats.ShowStats();
							JSONOps.Serialize.SerializePlayer(plr);
							Console.Read();
							break;

					}
				}
			}
			catch (Exception ex)
			{
				Utility.fLog.ExceptionLog(LogLevel.ERROR, ex);
			}
		}
	}
}
