using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    //Public fields
    public int hitpoints;
    public int maxHitpoint;
    public float pushRecoverySpeed = 0.2f;


    //Immunity
    protected float immuneTime = 0.1f;
    protected float lastImmune;

    //Push
    protected Vector3 pushDirection;

    //All fighter can ReciveDamage /Die
    protected virtual void ReciveDamage(Damage dmg)
    {
        if (Time.time - lastImmune > immuneTime)
        {
            lastImmune = Time.time;
            hitpoints -= dmg.damageAmount;
            pushDirection = (transform.position - dmg.origin).normalized * dmg.pushForce;

            GameManager.instance.ShowText("-" + dmg.damageAmount.ToString(), 25, Color.red, transform.position, Vector3.zero, 0.5f);

            GetComponent<SpriteRenderer>().color = new Color(1, 0, 0);

            if (hitpoints <= 0)
            {
                hitpoints = 0;
                Death();

            }
        }
        if (Time.time - lastImmune > immuneTime / 2)
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
    }

    protected virtual void Death()
    {
        Debug.Log("I'm dead");
    }
}
