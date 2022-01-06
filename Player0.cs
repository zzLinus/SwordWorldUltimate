using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player0 : Mover
{
    private SpriteRenderer sprRenderer;
    private Animator anim;
    private string currAnimation;
    public int currSkinID;
    const string MIBIDLE = "playerMIB_idle";
    const string MIBWALK = "playerMIB_walk";
    const string MAGICIANWALK = "playerMagician_walk";
    const string MAGICIANIDLE = "playerMagician_idle";

    // private int characterType = 0;// 0 for knites 1 for shooter 2 for magician

    protected override void Start()
    {
        base.Start();
        sprRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        currSkinID = 0;
        currAnimation = "";
        DontDestroyOnLoad(gameObject);
    }

    private void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        UpdateMotor(new Vector3(x, y, 0));

        if (currSkinID == 0)
        {
            if (Mathf.Abs(x) != 0 || Mathf.Abs(y) != 0)
            {
                ChangeAnimationState("knight_walk");
                GameManager.instance.weapon.walkState = true;
            }
            else
            {
                ChangeAnimationState("knight_idle");
                GameManager.instance.weapon.walkState = false;
            }

            // anim.SetInteger("CharacterID", 0);
            // anim.SetFloat("xSpeed", Mathf.Abs(x));
        }
        else if (currSkinID == 1)
        {
            if (Mathf.Abs(x) != 0 || Mathf.Abs(y) != 0)
            {
                ChangeAnimationState(MAGICIANWALK);
                GameManager.instance.weapon.walkState = true;
            }
            else
            {
                ChangeAnimationState(MAGICIANIDLE);
                GameManager.instance.weapon.walkState = false;
            }


            // anim.SetInteger("CharacterID", 1);
            // anim.SetFloat("xSpeed1", Mathf.Abs(x));
        }

        if (Time.time - lastImmune > immuneTime / 2)
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
    }

    private void ChangeAnimationState(string animation)
    {
        if (currAnimation == animation)
            return;

        anim.Play(animation);
        currAnimation = animation;
    }

    public void OnLevelUp()
    {
        maxHitpoint++;
        hitpoints = maxHitpoint;
    }

    public void SetLevel(int level)
    {
        for (int i = 0; i < level; i++)
            OnLevelUp();
    }

    public void SwapSprite(int skinID)
    {
        currSkinID = skinID;
        sprRenderer.sprite = GameManager.instance.playerSprites[skinID];
    }
}
