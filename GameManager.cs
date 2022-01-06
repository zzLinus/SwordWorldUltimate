using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        if (GameManager.instance != null)
        {
            Destroy(gameObject);
            return;
        }

        PlayerPrefs.DeleteAll();

        instance = this;

        SceneManager.sceneLoaded += LoadState;

        DontDestroyOnLoad(gameObject);

    }

    //Ressources
    public List<Sprite> playerSprites;
    public List<Sprite> weaponSprites;
    public List<Sprite> magicWeaponSprites;
    public List<int> weaponPrices;
    public List<int> magicWeaponPrices;
    public List<int> xpTable;

    //References
    public Player0 player;

    //Floating Text 
    public FloatingTextManager floatingTextManager;

    //public weapon 
    public Weapon weapon;

    //Logic
    public int pesos;
    public int experience;


    //Save State


    public void SaveState()
    {
        string s = "";

        s += "0" + "|";
        s += pesos.ToString() + "|";
        s += experience.ToString() + "|";
        s += weapon.weaponLevel.ToString() + "|";
        s += "0";

        PlayerPrefs.SetString("SaveState", s);

        Debug.Log("Save State");
    }

    public int GetXpToLevel(int level)
    {
        int r = 0;
        int xp = 0;
        while (r < level)
        {
            xp += xpTable[r];
            r++;
        }
        return xp;
    }

    public int GetCurrentLevel()
    {
        int r = 0;
        int add = 0;

        while (experience >= add)
        {
            add += xpTable[r];
            r++;

            if (r == xpTable.Count)
                return r;
        }

        return r;
    }

    //Upgrade weapon
    public bool TryUpgradeWeapon()
    {
        int playerType = GameManager.instance.player.currSkinID;

        if (playerType == 0)
        {
            //is the weapon Max level
            if (weaponPrices.Count <= weapon.weaponLevel)
                return false;

            if (pesos >= weaponPrices[weapon.weaponLevel])
            {
                pesos -= weaponPrices[weapon.weaponLevel];
                weapon.UpgradeWeapon(playerType);
                return true;
            }

        }
        else if (playerType == 1)
        {
            //is the weapon Max level
            if (magicWeaponPrices.Count <= weapon.magicWeaponLevel)
                return false;

            if (pesos >= magicWeaponPrices[weapon.magicWeaponLevel])
            {
                pesos -= magicWeaponPrices[weapon.magicWeaponLevel];
                weapon.UpgradeWeapon(playerType);
                return true;
            }

        }
        return false;
    }

    public void OnWeaponTypeChang()
    {
        int playerType = GameManager.instance.player.currSkinID;
        if (playerType == 0)
        {
            Debug.Log("knight" + playerType);
            weapon.anim.SetLayerWeight(playerType + 1, 0);
            weapon.anim.SetLayerWeight(playerType, 1);
        }
        else if (playerType == 1)
        {
            Debug.Log("magiction" + playerType);
            weapon.anim.SetLayerWeight(playerType - 1, 0);
            weapon.anim.SetLayerWeight(playerType, 1);
        }
    }

    public void OnLevelUp()
    {
        player.OnLevelUp();
    }

    public void GrantXp(int xp)
    {
        int curreLevel = GetCurrentLevel();
        experience += xp;
        if (curreLevel < GetCurrentLevel())
            OnLevelUp();
    }

    //floating text manager  
    public void ShowText(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration)
    {
        floatingTextManager.Show(msg, fontSize, color, position, motion, duration);
    }

    public void LoadState(Scene s, LoadSceneMode mode)
    {
        if (!PlayerPrefs.HasKey("SaveState"))
            return;

        string[] data = PlayerPrefs.GetString("SaveState").Split('|');

        //Cange player skin

        pesos = int.Parse(data[1]);
        experience = int.Parse(data[2]);

        if (GetCurrentLevel() != 1)
            player.SetLevel(GetCurrentLevel());

        //Change weapon level
        weapon.SetWeaponLevel(int.Parse(data[3]));

        player.transform.position = GameObject.Find("PortalPoints").transform.position;

        Debug.Log("Load State");
    }
}

