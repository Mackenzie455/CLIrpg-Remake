1. ApplyEffect function on entity, adds an Effect object to a list of ActiveEffects
2. EffectExecutor function in Entity, analyses each and every effect in both the Player and Enemy object at the end of every turn and applies them.
3. Debuffs:
     a. **Poison** and **Burn** would take `10 + (enemyDifficulty + poisonStrength) - playerStrength` amount of health away every turn
     b. **Frozen** would reduce accuracy and have a 1 in `ran.Next(0, 20) + advantage` chance to completely stop them during their turn
     c. **Mana Sickness** would reduce a players ManaStr and increase casting cost for duration
     d. **Slowness** would reduce Dex by 10, **Weakness** would reduce Str and Def by 10
4. Buffs:
     a. **Strength** would increase Strength by 10, **Haste** would increase Dex by 10, **Resistance** would increase
     b. **Regeneration** would increase health by `PotionStrength * (playerHealth / 10) + (int)PotionClass.Class` per turn, **Mana Regeneration** would do the same but `PotionStrength * (playerMana / 10) + (int)PotionClass.Class`

5. Effects can be called by anything, from Consumables, passive Effects caused by abilities or spells, or inflicted by an Enemies. As long as they're in the Entities ActiveEffects list then they will be caught by the EffectExecutor() and acted upon,

```cs
public static void EffectExecutor()
{
  if (this is Enemy e)
  {
    foreach (Effect in e.ActiveEffects)
    {
      //logic
    }
  }
  else if (this is Player p)
  {
     foreach (Effect in p.ActiveEffects)
    {
      //logic
    }
  }
}
```

```cs
public class Effect
{
  public string EffectName {get; set;}
}
```

```cs
