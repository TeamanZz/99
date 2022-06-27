using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifePlate : Plate
{
    protected override void OnMouseDown()
    {
        if (TakeDamage(1) == true)
            UseKnife();
    }

    protected override void OnMouseEnter()
    {
        if (Knife.knife.readyToUse == false)
            return;

        if (TakeDamage(1) == true)
            UseKnife();

    }

    public void UseKnife()
    {
        Knife.knife.UseKnife();
    }
}
