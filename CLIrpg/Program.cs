using RPGClassLibrary.Mechanics;
using RPGClassLibrary.Operations;
using RPGClassLibrary.Actors;
using RPGClassLibrary.Items;

namespace CLIrpg
{
	internal class Program
	{
		public static LoggingService log = new LoggingService(Sender.FRONT);
		public static bool Running = true;
		static void Main(string[] args)
		{
			try
			{
				Utility.CreateFileSystem();
				while (Running)
				{
                    Console.WriteLine("[SERIALIZE / DESERIALIZE]");
					string input = Console.ReadLine().ToLower();

					switch (input)
					{
						case "weapon":
							Weapon w = Weapon.WeaponFactory(ItemRarity.Legendary);
							Console.ReadKey();
							Player temp = JSONOps.Deserialize.DeserializePlayer("Craig");
							temp.AddItemToInventory(w);
							JSONOps.Serialize.SerializePlayer(temp);
							break;
						case "serialize":
							Console.WriteLine("insert player name");
							string pname = Console.ReadLine();
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
							Player p = JSONOps.Deserialize.DeserializePlayer(pname);
							p.Info();
                            Console.WriteLine("amount of exp");
							int amt = Convert.ToInt32(Console.ReadLine());
							p.GainExp(amt);
							p.Info();
                            Console.WriteLine();
							p.PrintInventory();
							JSONOps.Serialize.SerializePlayer(p);
							Console.ReadKey();
							break;
					}
				}
			}
			catch (Exception ex)
			{
				log.ExceptionLog(LogLevel.ERROR, ex);
			}
		}
	}
}
