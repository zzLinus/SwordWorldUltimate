using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Collidable
{
    //Damage structer

    public int[] damagePoint = { 1, 2, 3, 4 };
    public float[] pushForce = { 2.0f, 3.0f, 4.0f, 5.0f };

    public int[] magciDamagePoint = { 10, 20, 30, 4 };
    public float[] magicPushForce = { 2.0f, 3.0f, 4.0f, 5.0f };

    //Upgrade

    public int weaponLevel = 0;
    public int magicWeaponLevel = 0;
    public Animator anim;
    public Transform firePoint;
    public Transform firemagicFirePoint;
    public GameObject bullet;

    private const string SWORDIDLE = "weapon_idle";
    private const string SWORDSWING = "Weapon_Swing";
    private const string MAGICWANDIDLE = "MagicWand_idle";
    private const string MAGICWANDFIRE = "MagicWand_fire";

    private float cooldown = 0.15f;
    private float lastSwing;
    public SpriteRenderer sprRenderer;
    public LineRenderer lineRend;
    private int playerType;
    private int currAnimationState;
    private string currAnimation;
    private float swordSwingTime = 0.3f;
    private float magicWandFireTime = 0.9f;
    private float fireMagicWandFireTime = 1.0f;
    private bool lastState;
    public bool walkState;

    public void SetWeaponLevel(int level)
    {
        weaponLevel = level;
        sprRenderer.sprite = GameManager.instance.weaponSprites[weaponLevel];
        currAnimationState = -1;
    }

    protected override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
        sprRenderer = GetComponent<SpriteRenderer>();
        walkState = false;
        currAnimation = "";
    }

    private void lazerShoot()
    {
        lineRend.enabled = true;
        if (GameManager.instance.player.transform.localScale.x == -1)
            firemagicFirePoint.right = new Vector3(-1f, 0f, 0f);
        else if (GameManager.instance.player.transform.localScale.x == 1)
            firemagicFirePoint.right = new Vector3(1f, 0f, 0f);
        RaycastHit2D hitInfo = Physics2D.Raycast(firemagicFirePoint.position, firemagicFirePoint.right);
        int magicWeaponLevel = GameManager.instance.weapon.magicWeaponLevel;
        if (hitInfo)
        {
            lineRend.SetPosition(0, firemagicFirePoint.position);
            lineRend.SetPosition(1, hitInfo.point);
        }
        else
        {
            lineRend.SetPosition(0, firemagicFirePoint.position);
            lineRend.SetPosition(1, firemagicFirePoint.position + firemagicFirePoint.right * 100);
        }

        if (hitInfo.collider.name == "monster1" || hitInfo.collider.name == "monster0")
        {
            Damage dmg = new Damage
            {
                damageAmount = magciDamagePoint[magicWeaponLevel],
                origin = transform.position,
                pushForce = magicPushForce[magicWeaponLevel],
            };

            hitInfo.collider.SendMessage("ReciveDamage", dmg);

        }
    }

    private void turnOffLazer()
    {
        lineRend.enabled = false;
    }

    private void Shoot()
    {
        Debug.Log("bullet spawn");
        Instantiate(bullet, firePoint.position, firePoint.rotation, GameManager.instance.weapon.transform);
    }

    private void Swing(int playerType, int state)
    {
        if (currAnimationState == state)
            return;

        if (playerType == 0 && state == 1)
        {
            if (weaponLevel == 0)
                anim.Play(SWORDSWING);
            if (weaponLevel == 1)
                anim.Play("greenSword_swing");
            if (weaponLevel == 2)
                anim.Play("redSword_swing");
        }
        else if (playerType == 0 && state == 0)
        {
            if (weaponLevel == 0)
                anim.Play(SWORDIDLE);
            if (weaponLevel == 1)
                anim.Play("greenSword_idle");
            if (weaponLevel == 2)
                anim.Play("redSword_idle");
        }

        if (playerType == 1 && state == 1)
        {

            if (magicWeaponLevel == 0)
                anim.Play("fireMagicWand_fire");
            if (magicWeaponLevel == 1)
                anim.Play(MAGICWANDFIRE);
        }
        else if (playerType == 1 && state == 0)
        {
            if (magicWeaponLevel == 0)
                anim.Play("fireMagicWand_idle");
            if (magicWeaponLevel == 1)
                anim.Play(MAGICWANDIDLE);

            Debug.Log("magicwand idle");
        }
        currAnimationState = state;
    }

    public void ChangeAnimationState(string animation)
    {
        if (currAnimation == animation)
            return;

        anim.Play(animation);
        currAnimation = animation;
    }

    protected override void Update()
    {
        base.Update();
        playerType = GameManager.instance.player.currSkinID;


        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Time.time - lastSwing > cooldown)
            {
                lastSwing = Time.time;
                if (playerType == 0)
                {
                    if (weaponLevel == 0)
                        ChangeAnimationState(SWORDSWING);
                    else if (weaponLevel == 1)
                        ChangeAnimationState("greenSword_swing");
                    else if (weaponLevel == 2)
                        ChangeAnimationState("redSword_swing");
                }
                else if (playerType == 1)
                {
                    if (magicWeaponLevel == 0)
                    {
                        ChangeAnimationState("fireMagicWand_fire");
                        Invoke("lazerShoot", 0.6f);
                        Invoke("turnOffLazer", 1.0f);
                    }
                    else if (magicWeaponLevel == 1)
                    {
                        ChangeAnimationState(MAGICWANDFIRE);
                        Invoke("Shoot", 0.6f);
                    }
                }
            }
        }
        else if (walkState == true)
        {
            if (playerType == 0 && Time.time - lastSwing > swordSwingTime)
            {
                if (weaponLevel == 0)
                    ChangeAnimationState(SWORDIDLE);
                else if (weaponLevel == 1)
                    ChangeAnimationState("greenSword_idle");
                else if (weaponLevel == 2)
                    ChangeAnimationState("redSword_idle");
            }
            else if (playerType == 1 && Time.time - lastSwing > magicWandFireTime && magicWeaponLevel == 1)
            {
                if (magicWeaponLevel == 1)
                    ChangeAnimationState("magicWand_walk");
                else if (magicWeaponLevel == 0)
                    ChangeAnimationState("fireMagicWand_walk");
            }
            else if (playerType == 1 && Time.time - lastSwing > fireMagicWandFireTime && magicWeaponLevel == 0)
            {
                if (magicWeaponLevel == 1)
                    ChangeAnimationState("magicWand_walk");
                else if (magicWeaponLevel == 0)
                    ChangeAnimationState("fireMagicWand_walk");
            }
        }
        else if (walkState == false)
        {
            if (playerType == 0 && Time.time - lastSwing > swordSwingTime)
            {
                if (weaponLevel == 0)
                    ChangeAnimationState(SWORDIDLE);
                else if (weaponLevel == 1)
                    ChangeAnimationState("greenSword_idle");
                else if (weaponLevel == 2)
                    ChangeAnimationState("redSword_idle");
            }
            else if (playerType == 1 && Time.time - lastSwing > magicWandFireTime && magicWeaponLevel == 1)
            {
                if (magicWeaponLevel == 0)
                    ChangeAnimationState("fireMagicWand_idle");
                else if (magicWeaponLevel == 1)
                    ChangeAnimationState(MAGICWANDIDLE);
            }
            else if (playerType == 1 && Time.time - lastSwing > fireMagicWandFireTime && magicWeaponLevel == 0)
            {
                if (magicWeaponLevel == 0)
                    ChangeAnimationState("fireMagicWand_idle");
                else if (magicWeaponLevel == 1)
                    ChangeAnimationState(MAGICWANDIDLE);
            }
        }

    }



    public void UpgradeWeapon(int weaponType)//0 for knight 1 for magician
    {
        int skinID = GameManager.instance.player.currSkinID;
        if (weaponType == 0)
        {
            weaponLevel++;
            sprRenderer.sprite = GameManager.instance.weaponSprites[weaponLevel];
            Swing(skinID, 1);

            //change stats

        }
        else if (weaponType == 1)
        {
            magicWeaponLevel++;
            sprRenderer.sprite = GameManager.instance.magicWeaponSprites[magicWeaponLevel];
            Swing(skinID, 1);

            //change stats

        }
    }

    public void SwapSprite(int skinID)
    {
        int weaponLevel = GameManager.instance.weapon.weaponLevel;
        int magicWeaponLevel = GameManager.instance.weapon.magicWeaponLevel;
        if (skinID == 0)
        {
            sprRenderer.sprite = GameManager.instance.weaponSprites[weaponLevel];
            Swing(skinID, 1);
        }
        else if (skinID == 1)
        {
            sprRenderer.sprite = GameManager.instance.magicWeaponSprites[magicWeaponLevel];
            Swing(skinID, 1);
        }
    }


    protected override void OnCollida(Collider2D coll)
    {
        if (coll.tag == "Fighter")
        {
            if (coll.name == "Player_0")
                return;
            //Create a new damage object, then send it to the fighter we've hit
            Damage dmg = new Damage
            {
                damageAmount = damagePoint[weaponLevel],
                origin = transform.position,
                pushForce = pushForce[weaponLevel],
            };

            coll.SendMessage("ReciveDamage", dmg);
        }
    }
}

