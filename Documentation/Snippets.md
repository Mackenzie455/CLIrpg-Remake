# Player.RedistributeStats()
```cs
this.Stats.Health = (this.Stats.Health - StatDistribution[0]) + intList[0];
this.Stats.Mana = (this.Stats.Mana - StatDistribution[1]) + intList[1];
this.Stats.Strength = (this.Stats.Strength - StatDistribution[2]) + intList[2];
this.Stats.MagicStr = (this.Stats.MagicStr - StatDistribution[3]) + intList[3];
this.Stats.Defense = (this.Stats.Defense - StatDistribution[4]) + intList[4];
this.Stats.Dexterity = (this.Stats.Dexterity - StatDistribution[5]) + intList[5];
```

```cs
public void RedistributeStats()
		{
			try
			{
				bool Running = true;
				while (Running)
				{
					Console.WriteLine("Redistribute your stats.");
					Console.WriteLine("HEALTH, MANA, STRENGTH, MAGIC STRENGTH, DEFENSE and DEXTERITY.");
					string input = Utility.ValidateInput("Please format your input like this. (0 0 0 0 0 0)");

					string[] scores = input.Split(" ", StringSplitOptions.RemoveEmptyEntries);
					List<int> ints = new List<int>();

					foreach (string str in scores)
					{
						if (int.TryParse(str, out int result))
						{
							ints.Add(result);
						}
						else
						{
							Console.WriteLine($"{str} is an invalid input.");
						}
					}

					if (ints.Count < 6)
					{
						Console.WriteLine("You must enter a number for each stat!");
						continue;
					}

					int[] thisStats = {this.Stats.Health, this.Stats.Mana, this.Stats.Strength, this.Stats.MagicStr, this.Stats.Defense, this.Stats.Dexterity};
					for (int i = 0; i < this.StatDistribution.Length; i++)
					{
						thisStats[i] = (thisStats[i] - this.StatDistribution[0]) + ints[0];
					}
					this.StatDistribution = ints.ToArray();
					Running = false;
				}
			}
			catch (Exception ex)
			{
				Utility.bLog.ExceptionLog(LogLevel.ERROR, ex);
			}
		}
```
