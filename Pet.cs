using System;

namespace TamagotchiApp
{
  public class Pet
  {
    public int ID { get; set; }
    public string Name { get; set; }
    public DateTime Birthday { get; set; } = DateTime.Now;
    public DateTime DeathDate { get; set; }
    public int HungerLevel { get; set; } = 0;
    public int HappinessLevel { get; set; } = 0;
    public bool IsDead { get; set; } = false;
    public DateTime LastInteractedWithDate { get; set; } = DateTime.Now;
  }
}