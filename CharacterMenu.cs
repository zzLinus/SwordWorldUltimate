using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMenu : MonoBehaviour
{
    //Text fields
    public Text levelText, hitpointText, pesosText, upgradeCosstText, xpText;

    //Logic
    private int currentCharacterSelection;//0 for shooters 1 for knights 2 formagiction
    public Image characterSelectionSprite, weaponSprite;
    public RectTransform xpBar;


    //Character Selection
    public void OnArrowClick(bool right)
    {
        if (right)
        {
            currentCharacterSelection++;
            if (currentCharacterSelection == GameManager.instance.playerSprites.Count)
                currentCharacterSelection = 0;

            OnSelectionChanged();
            UpdateMenu();
            //TODO:Change weapon animation base on the weapon type
            // GameManager.instance.OnWeaponTypeChang();
        }
        else
        {
            currentCharacterSelection--;
            if (currentCharacterSelection < 0)
                currentCharacterSelection = GameManager.instance.playerSprites.Count - 1;

            OnSelectionChanged();
            UpdateMenu();
            // GameManager.instance.OnWeaponTypeChang();
        }
    }

    private void OnSelectionChanged()
    {
        int weaponLevel = GameManager.instance.weapon.weaponLevel;
        int magicWeaponLevel = GameManager.instance.weapon.magicWeaponLevel;
        characterSelectionSprite.sprite = GameManager.instance.playerSprites[currentCharacterSelection];

        if (currentCharacterSelection == 0)
        {
            weaponSprite.sprite = GameManager.instance.weaponSprites[weaponLevel];
        }

        else if (currentCharacterSelection == 1)
        {
            weaponSprite.sprite = GameManager.instance.magicWeaponSprites[magicWeaponLevel];
        }
        GameManager.instance.weapon.SwapSprite(currentCharacterSelection);
        GameManager.instance.player.SwapSprite(currentCharacterSelection);
    }

    //WeaponUpgrade
    public void OnUpgradeClick()
    {
        if (GameManager.instance.TryUpgradeWeapon())
            UpdateMenu();
    }


    //UpdateCharacter Information
    public void UpdateMenu()
    {
        int playerType = GameManager.instance.player.currSkinID;

        //Weapon
        if (playerType == 0)
        {
            weaponSprite.sprite = GameManager.instance.weaponSprites[GameManager.instance.weapon.weaponLevel];
            if (GameManager.instance.weapon.weaponLevel == GameManager.instance.weaponPrices.Count)
                upgradeCosstText.text = "MAX";
            else
                upgradeCosstText.text = GameManager.instance.weaponPrices[GameManager.instance.weapon.weaponLevel].ToString();

        }
        else if (playerType == 1)
        {
            weaponSprite.sprite = GameManager.instance.magicWeaponSprites[GameManager.instance.weapon.magicWeaponLevel];
            if (GameManager.instance.weapon.magicWeaponLevel == GameManager.instance.magicWeaponPrices.Count)
                upgradeCosstText.text = "MAX";
            else
                upgradeCosstText.text = GameManager.instance.magicWeaponPrices[GameManager.
                                    instance.weapon.magicWeaponLevel].ToString();
        }


        //Meta
        hitpointText.text = GameManager.instance.player.hitpoints.ToString();
        pesosText.text = GameManager.instance.pesos.ToString();
        levelText.text = GameManager.instance.GetCurrentLevel().ToString();

        //xp Bar
        int currLevel = GameManager.instance.GetCurrentLevel();
        if (currLevel == GameManager.instance.xpTable.Count)
        {
            xpText.text = GameManager.instance.experience.ToString() + "total experience points";
            xpBar.localScale = Vector3.one;
        }
        else
        {
            int prevLevelXp = GameManager.instance.GetXpToLevel(currLevel - 1);
            int currLevelXp = GameManager.instance.GetXpToLevel(currLevel);
            int diff = currLevelXp - prevLevelXp;
            int currXpIntoLevel = GameManager.instance.experience - prevLevelXp;
            float completionRatio = (float)currXpIntoLevel / (float)diff;
            xpBar.localScale = new Vector3(completionRatio, 1, 1);
            xpText.text = currXpIntoLevel.ToString() + "/" + diff.ToString();
        }

    }
}
