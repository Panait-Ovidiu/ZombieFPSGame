using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GUIManager : MonoBehaviour
{
    [SerializeField] public PlayerScript playerScript;
    public Text enemies, health, ammo;
    public GameObject[] weaponIndicator = new GameObject[2];
    [SerializeField] public GameObject ammoInfo;
    [SerializeField] public Image healthBar;

    [SerializeField] public GameObject waveInfo;
    [SerializeField] public Text waveText;

    private void Start()
    {
        for (int i = 0; i < weaponIndicator.Length; i++)
        {
            weaponIndicator[i].SetActive(false);
        }
    }

    public void setHealth(string text)
    {
        health.text = text;
        healthBar.fillAmount = playerScript.health / playerScript.maxHealth;
    }

    public void setAmmo(string text)
    {
        ammo.text = text;
    }

    public void setEnemies(int max , int left)
    {
        enemies.text = left + "/" + max;
    }

    public void SetWeaponToDisplay(int index)
    {
        for (int i = 0; i < weaponIndicator.Length; i++)
        {
            weaponIndicator[i].SetActive(false);
        }

        for (int i = 0; i < weaponIndicator.Length; i++)
        {
            if (i == index)
            {
                weaponIndicator[i].SetActive(true);
            }
        }
    }
}
