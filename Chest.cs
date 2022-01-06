using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Colectable
{
    public Sprite emptyChest;
    public int pesosAmount = 5;

    protected override void OnCollect()
    {
        if (!collected)
        {
            base.OnCollect();
            GetComponent<SpriteRenderer>().sprite = emptyChest;
            GameManager.instance.pesos += pesosAmount;
            Debug.Log("Grant " + pesosAmount + "pesos!");
            GameManager.instance.ShowText("+ " + pesosAmount + " pesos!", 25, Color.white,
                                transform.position, Vector3.up * 50, 3.0f);
        }
    }
}
