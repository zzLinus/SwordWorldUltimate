using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public float speed = 0.2f;
    public Rigidbody2D rb;

    private int[] damagePoint = { 1, 2, 3, 4 };
    private float[] pushForce = { 2.0f, 3.0f, 4.0f, 5.0f };

    private int[] magciDamagePoint = { 10, 20, 30, 4 };
    private float[] magicPushForce = { 4.0f, 6.0f, 8.0f, 10.0f };
    private void Start()
    {
        if (GameManager.instance.player.transform.localScale.x == -1)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            rb.velocity = -transform.right * speed;
        }
        else
            rb.velocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {

        int magicWeaponLevel = GameManager.instance.weapon.magicWeaponLevel;
        Damage dmg = new Damage
        {
            damageAmount = magciDamagePoint[magicWeaponLevel],
            origin = transform.position,
            pushForce = magicPushForce[magicWeaponLevel],
        };

        Debug.Log(hitInfo.name);
        Destroy(gameObject);
        hitInfo.SendMessage("ReciveDamage", dmg);
    }
}
