using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddDamagePlate : Plate
{
    protected override void KillPlate()
    {
        base.KillPlate();
        UseDamageSystem();
    }
    public void UseDamageSystem()
    {
        AddDamageSystem.damageSystem.UseDamage();
    }
}
