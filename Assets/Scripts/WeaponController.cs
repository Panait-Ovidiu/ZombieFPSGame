using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private PlayerScript playerScript;
    public GameObject cameraGameObject;
    public float fireRate = 20;
    public ParticleSystem flash;
    public GameObject bulletEffect;

    private Animator animator;

    private float reloadAnimTime = 2.10f;
    private float reloadTime = 0;
    private float readyToFire;
    private bool isReloading = false;

    public int magazine = 30;
    public int ammo;
    public int mags = 3;

    public int magazineTemp = 30;

    private AiScript enemy;
    public int damageAmount = 10;
    public float force = 50;

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        animator.SetInteger("Movement", 0);
        ammo = magazine * mags;

        playerScript.guiManager.setAmmo(magazine + "/" + ammo);
    }

    private void Update()
    {
        checkEnemy();
        if (Time.time >= readyToFire)
        {
            animator.SetInteger("Fire", -1);
            animator.SetInteger("Movement", (playerScript.inputManager.vertical == 0 && playerScript.inputManager.horizontal == 0) ? 0 : 1);
        }

        if (isReloading && reloadTime <= 1)
        {
            reloadTime = 0;
            animator.SetInteger("Reload", -1);
            isReloading = false;
            ammo = ammo - 30 + magazine;
            magazine = magazineTemp;
            if (ammo < 0)
            {
                magazine += ammo;
                ammo = 0;
                playerScript.guiManager.setAmmo(magazine + "/" + ammo);
            }
            playerScript.guiManager.setAmmo(magazine + "/" + ammo);
        }
        else
        {
            reloadTime -= Time.deltaTime;
        }
    }

    public void Shoot()
    {
        if (!isReloading && magazine > 0)
        {
            animator.SetInteger("Fire", -1);
            animator.SetInteger("Movement", 0);
            if (Time.time >= readyToFire)
            {
                enemy = null;

                readyToFire = Time.time + 1f / fireRate;
                flash.Play();
                RaycastHit hit;
                magazine--;
                playerScript.guiManager.setAmmo(magazine + "/" + ammo);
                if (Physics.Raycast(cameraGameObject.transform.position, cameraGameObject.transform.forward, out hit))
                {
                    checkHit(hit);
                }
                animator.SetInteger("Fire", 2);
                animator.SetInteger("Movement", -1);
            }
        }
    }

    public void Reload()
    {
        if (!isReloading && ammo > 0)
        {
            isReloading = true;
            reloadTime = reloadAnimTime;
            animator.SetInteger("Reload", 1);
        }
    }

    public void SetGuiAmmo()
    {
        playerScript.guiManager.setAmmo(magazine + "/" + ammo);
    }

    public void AddAmmo()
    {
        int maxAmmo = magazineTemp * mags;
        if (ammo < maxAmmo || magazine < magazineTemp)
        {
            ammo += 30;
            if (ammo > maxAmmo +magazineTemp-magazine)
            {
                ammo = maxAmmo + magazineTemp - magazine;
            }
        }
    }

    private void checkEnemy()
    {
        RaycastHit hit;
        if (Physics.Raycast(cameraGameObject.transform.position, cameraGameObject.transform.forward, out hit))
        {
            try
            {
                enemy = hit.transform.GetComponent<AiScript>();
                enemy.ShowHealth();
            }
            catch
            {

            }
        }
    }

    private void checkHit(RaycastHit hit)
    {
        if (hit.rigidbody != null)
        {
            hit.rigidbody.AddForce(-hit.normal * force);
        }
        Instantiate(bulletEffect, hit.point, Quaternion.LookRotation(hit.normal));
        try
        {
            enemy = hit.transform.GetComponent<AiScript>();
            enemy.Hit(damageAmount);
        }
        catch
        {

        }
    }
}
