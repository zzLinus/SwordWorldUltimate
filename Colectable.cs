using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colectable : Collidable
{
    protected bool collected;

    protected override void OnCollida(Collider2D coll)
    {
        if (coll.name == "Player_0")
            OnCollect();
    }

    protected virtual void OnCollect()
    {
        collected = true;
    }
}
