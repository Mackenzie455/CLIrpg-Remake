﻿using RPGClassLibrary.Actors;
using RPGClassLibrary.Items;
using RPGClassLibrary.Mechanics;
using RPGClassLibrary.Operations;

namespace RPGClassLibrary.Mechanics
{
	public enum EffectType
	{
		None,
		Poison, Burn, Frozen, Slowness, Weakness, ManaSickness, Curse, // Debuffs
		Strength, Haste, Resistance, Regeneration, ManaRegen, Lifesteal  // Buffs
	}
	public class Effect
	{
		public string Name { get; set; }
		public EffectType Type { get; set; }
		public int Strength { get; set; }
		public int Duration { get; set; }
		public int Chance { get; set; }

		public Effect(string name, int strength, int duration, int chance, EffectType type)
		{
			Name = name;
			Strength = strength;
			Duration = duration;
			Chance = Math.Clamp(chance, 0, 100);
			Type = type;
		}
		public bool IsDebuff()
		{
			return Type is EffectType.Poison or EffectType.Burn or EffectType.Frozen
				or EffectType.Slowness or EffectType.Weakness or EffectType.ManaSickness or EffectType.Curse;
		}
		public bool IsBuff()
		{
			return Type is EffectType.Strength or EffectType.Haste or EffectType.Resistance
				or EffectType.Regeneration or EffectType.ManaRegen or EffectType.Lifesteal;
		}
		public static Effect EffectFactory()
		{
			//points based system like with playerfactory?
			//TODO: logic
			return null;
		}
	}
	public class EffectHandler
	{
		public Entity entity;
		public EffectHandler(Entity _en)
		{
			this.entity = _en;
		}
		public EffectHandler() { }

		#region EffectMethods
		public void AddEffect(Effect e)
		{
			try
			{

				var existingEffect = entity.CurrentEffects.FirstOrDefault(effect => effect.Name == e.Name);
				if (existingEffect != null)
				{
					existingEffect.Duration = Math.Max(existingEffect.Duration, e.Duration);
					existingEffect.Strength = Math.Max(existingEffect.Strength, e.Strength);
				}
				else
				{
					entity.CurrentEffects.Add(e);
				}
			}
			catch (Exception ex)
			{
				Utility.bLog.ExceptionLog(LogLevel.ERROR, ex);
			}
		}
		public void EffectExecutor(int effectModifier = 10)
		{
			try
			{
				List<Effect> Expired = new List<Effect>();

				foreach (Effect effect in entity.CurrentEffects)
				{
					if (effect.Duration <= 0)
					{
						Expired.Add(effect);
						break;
					}

					if (effect.IsBuff())
					{
						ApplyBuff(effect, effectModifier);
					}
					else if (effect.IsDebuff())
					{
						ApplyDebuff(effect, effectModifier);
					}

					effect.Duration--; //decrements the effect duration. if it reaches 0 then its added to the expiry list
				}

				foreach (Effect expired in Expired)
				{
					Console.WriteLine($"{entity.Name}'s {expired.Name} has worn off!");
					entity.CurrentEffects.Remove(expired);
				}
			}
			catch (Exception ex)
			{
				Utility.bLog.ExceptionLog(LogLevel.ERROR, ex);
			}
		}
		public void ApplyBuff(Effect effect, int effectModifier)
		{
			try
			{
				ConsoleColor color = Settings.DefaultConsoleColor;
				Random ran = new Random();
				switch (effect.Type)
				{
					case EffectType.Regeneration:
						//TODO: Have entity work every turn for the duration
						break;

					case EffectType.ManaRegen:
						//TODO: Have entity work every turn for the duration
						break;

					case EffectType.Haste:
						int haste = effect.Strength + effectModifier;
						entity.Stats.Dexterity += haste;
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine($"{entity.Name} has become faster! Dexterity increased by {haste}!");
						Console.ForegroundColor = color;
						break;

					case EffectType.Strength:
						int strength = effect.Strength + effectModifier;
						entity.Stats.Strength += strength;

						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine($"{entity.Name} has become stronger! Defense increased by {strength}!");
						Console.ForegroundColor = color;
						break;

					case EffectType.Resistance:
						int resistance = effect.Strength + effectModifier;
						entity.Stats.Defense += resistance;

						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine($"{entity.Name} has become more resistant! Defense increased by {resistance}!");
						Console.ForegroundColor = color;
						break;
				}
			}
			catch (Exception ex)
			{
				Utility.bLog.ExceptionLog(LogLevel.ERROR, ex);
			}
		}
		public void ApplyDebuff(Effect effect, int effectModifier)
		{
			try
			{
				ConsoleColor color = Settings.DefaultConsoleColor;
				Random ran = new Random();
				switch (effect.Type)
				{
					case EffectType.Burn or EffectType.Poison:
						int damage = Math.Max(1, 1 + (effectModifier * effect.Strength) - entity.Stats.Strength);
						entity.Stats.Health -= damage;
						Console.ForegroundColor = ConsoleColor.Red;
						Console.WriteLine($"{entity.Name} took {damage} damage from the {effect.Type.ToString().ToLower()}!");
						Console.ForegroundColor = color;
						break;

					case EffectType.Frozen:
						int freezeChance = ran.Next(0, 20) + effect.Strength;
						if (freezeChance > 18)
						{
							Console.ForegroundColor = ConsoleColor.Red;
							Console.WriteLine($"{entity.Name} is frozen solid and cannot act entity turn!");
							Console.ForegroundColor = color;
						}
						else
						{
							Console.ForegroundColor = ConsoleColor.Yellow;
							Console.WriteLine($"{entity.Name}'s accuracy is reduced due to being frozen.");
							Console.ForegroundColor = color;

							//TODO: logic
							//Maybe impliment accuracy stat? Or should it be inherant to abilities or BattleHandler?
						}
						break;

					case EffectType.Weakness:
						int weakness = effect.Strength + (effectModifier + 1);
						entity.Stats.Strength -= weakness;
						Console.ForegroundColor = ConsoleColor.Red;
						Console.WriteLine($"{entity.Name} has lost {weakness} strength due to their weakness!");
						Console.ForegroundColor = color;
						break;

					case EffectType.Slowness:
						int slowness = effect.Strength + (effectModifier + 1);
						entity.Stats.Dexterity -= slowness;
						Console.ForegroundColor = ConsoleColor.Red;
						Console.WriteLine($"{entity.Name} has lost {slowness} dexterity due to their slowness! Reaction time and dodging is harder!");
						Console.ForegroundColor = color;
						break;

					case EffectType.ManaSickness:
						int sickness = effect.Strength + (effectModifier * 2);
						entity.Stats.Mana -= sickness;
						entity.Stats.MagicStr -= sickness;
						Console.ForegroundColor = ConsoleColor.Red;
						Console.WriteLine($"{entity.Name} has come down with mana sickness! Mana and Magic Strength reduced by {sickness}!");
						Console.ForegroundColor = color;
						break;

					case EffectType.Curse:
						int curse = (int)Math.Round((decimal)effect.Strength + effectModifier, 2);

						entity.Stats.Health -= Math.Max(0, curse);
						entity.Stats.Mana -= Math.Max(0, curse);
						entity.Stats.Strength -= Math.Max(0, curse);
						entity.Stats.MagicStr -= Math.Max(0, curse);
						entity.Stats.Defense -= Math.Max(0, curse);
						entity.Stats.Dexterity -= Math.Max(0, curse);

						Console.ForegroundColor = ConsoleColor.Red;
						Console.WriteLine($"A dark magic attack curses {entity.Name}! All stats reduced by {curse}!");
						Console.ForegroundColor = color;
						break;
				}
			}
			catch (Exception ex)
			{
				Utility.bLog.ExceptionLog(LogLevel.ERROR, ex);
			}

		}
		public void ClearEffect(Effect effect, int effectModifier)
		{
			switch (effect.Type)
			{
				//TODO: logic
			}
		}

		#endregion
	}
}
