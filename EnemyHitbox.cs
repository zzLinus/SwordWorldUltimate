using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox : Collidable
{
    public int damagePoint = 1;
    public float pushForce = 3;

    protected override void OnCollida(Collider2D coll)
    {
        if (coll.tag == "Fighter" && coll.name == "Player_0")
        {
            //create a new damage object,before sending it to the player
            Damage dmg = new Damage
            {
                damageAmount = damagePoint,
                origin = transform.position,
                pushForce = pushForce,
            };


						coll.SendMessage("ReciveDamage", dmg);
				}
		}
}

