using System;

public class ModifyPowerEffect : Effect
{
  public PowerModifier modifier;
  public int value;

  public ModifyPowerEffect(PowerModifier modifier, int value)
  {
    this.modifier = modifier;
    this.value = value;
  }
}