using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private PlayerScript playerScript;
    [HideInInspector] public float mouseX;
    [HideInInspector] public float mouseY;
    [HideInInspector] public float vertical;
    [HideInInspector] public float horizontal;
    public float mouseSensitivity = 300f;
    public float xRotation = 0f;

    private bool isPaused = false;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Paused();
        }

        if (isPaused)
        {
            mouseX = 0;
            mouseY = 0;
            horizontal = 0;
            vertical = 0;
        }
        else
        {
            mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            if (Input.GetButtonDown("Jump"))
            {
                playerScript.playerMovement.Jump();
            }

            if (Input.GetButton("Fire1"))
            {
                playerScript.weaponController.Shoot();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                playerScript.weaponController.Reload();
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                playerScript.SwitchWeapons(playerScript.weaponIndex < 1 ? playerScript.weaponIndex + 1 : 0);
            }

            if(Input.GetKeyDown(KeyCode.E) && playerScript.collectibleAmmo.InRange())
            {
                playerScript.AddAmmo();
            }
        }

    }

    private void Paused()
    {
        if (isPaused)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            isPaused = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            isPaused = true;
        }
    }
}
