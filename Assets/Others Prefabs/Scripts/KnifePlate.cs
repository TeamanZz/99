using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifePlate : Plate
{
    protected override void KillPlate()
    {
        base.KillPlate();
        UseKnife();
    }
    public void UseKnife()
    {
        KnifeSystem.knife.UseKnife();
    }
}
