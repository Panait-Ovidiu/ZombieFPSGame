using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] public Camera playerCamera;
    [HideInInspector] public InputManager inputManager;
    [HideInInspector] public PlayerMovement playerMovement;
    [SerializeField] public WeaponController weaponController;
    [SerializeField] public GUIManager guiManager;
    [SerializeField] public CollectibleAmmo collectibleAmmo;

    public int weaponIndex;
    public GameObject[] weapons = new GameObject[2];
    public WeaponController[] weaponControllers = new WeaponController[2];

    public float health = 100f;
    public float maxHealth;

    // Start is called before the first frame update
    void Start()
    {
        inputManager = GetComponent<InputManager>();
        playerMovement = GetComponent<PlayerMovement>();
        maxHealth = health;

        SwitchWeapons(0);
        guiManager.SetWeaponToDisplay(0);
        guiManager.setHealth(health.ToString());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {

    }

    public void SwitchWeapons(int index)
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].SetActive(false);
        }
        weapons[index].SetActive(true);
        weaponIndex = index;
        weaponController = weaponControllers[index];
        weaponController.SetGuiAmmo();
        guiManager.SetWeaponToDisplay(index);
    }

    public void AddAmmo()
    {
        foreach(WeaponController controller in weaponControllers) {
            controller.AddAmmo();
        }
        weaponController.SetGuiAmmo();
    }

    public void Hit(int value)
    {
        setHealth(value);
      //  healthBar.GetComponent<Image>().fillAmount = health / maxHealth;
    }

    public void setHealth(float value)
    {
        if (health <= 0) return;
        if (health - value <= 0) { Die(); }
        health -= value;
        guiManager.setHealth(health.ToString());
    }

    public void Die()
    {
        Destroy(inputManager);
    }
}
