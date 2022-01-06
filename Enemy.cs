using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Mover
{
    //Experience
    public int xpValue = 1;
    //Logic
    public float triggerLenght = 0.6f;
    public float chaseLenght = 0.6f;
    private bool chasing;
    private bool collidingWithPlayer;
    private Transform playerTransform;
    private Vector3 startingPosition;
    private string currenAanimation;
    public string monsterType;

    // Hitbox
    public ContactFilter2D filter;
    private BoxCollider2D hitbox;
    private Collider2D[] hits = new Collider2D[10];
    private Animator anim;
    private bool startChaseFalg;

    protected override void Start()
    {
        base.Start();
        playerTransform = GameManager.instance.player.transform;
        startingPosition = transform.position;
        hitbox = transform.GetChild(0).GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        currenAanimation = "";
        startChaseFalg = false;
    }

    private void FixedUpdate()
    {
        //Is the player in range
        if (Vector3.Distance(playerTransform.position, startingPosition) < chaseLenght)
        {
            if (Vector3.Distance(playerTransform.position, transform.position) < triggerLenght)
                chasing = true;

            if (chasing)
            {
                if (!collidingWithPlayer)
                {
                    if (startChaseFalg != chasing)
                        GameManager.instance.ShowText("!!!", 30, Color.red,
                                        transform.position, Vector3.up * 20, .2f);
                    startChaseFalg = chasing;
                    UpdateMotor((playerTransform.position - transform.position).normalized);
                    if (monsterType == "chestMonster")
                        ChangeAnimationState("chestMonster_walk");
                    else if (monsterType == "mudeMonster")
                        ChangeAnimationState("MonsterWalk");
                }
            }
            else
            {
                UpdateMotor(startingPosition - transform.position);
                if (monsterType == "chestMonster")
                    ChangeAnimationState("chestMonster_idle");
                else if (monsterType == "mudeMonster")
                    ChangeAnimationState("Monsetr_idle");
            }
        }
        else
        {
            UpdateMotor(startingPosition - transform.position);
            if (monsterType == "chestMonster")
                ChangeAnimationState("chestMonster_idle");
            else if (monsterType == "mudeMonster")
                ChangeAnimationState("Monsetr_idle");
            chasing = false;
            startChaseFalg = false;
        }

        if (Time.time - lastImmune > immuneTime / 2)
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);

        //check overlaps
        collidingWithPlayer = false;

        boxCollider.OverlapCollider(filter, hits);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i] == null)
                continue;

            if (hits[i].tag == "Fighter" && hits[i].name == "Player_0")
                collidingWithPlayer = true;

            hits[i] = null;
        }

    }

    public void ChangeAnimationState(string animation)
    {
        if (currenAanimation == animation)
            return;

        anim.Play(animation);
        currenAanimation = animation;
    }

    protected override void Death()
    {
        Destroy(gameObject);
        GameManager.instance.GrantXp(xpValue);
        GameManager.instance.ShowText("+ " + xpValue + " xp", 30, Color.magenta,
                        transform.position, Vector3.up * 40, 1.0f);
    }
}

