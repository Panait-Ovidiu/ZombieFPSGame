using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] private PlayerScript playerScript;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localRotation = Quaternion.Euler(playerScript.inputManager.xRotation, 0f, 0f);
        playerScript.transform.Rotate(Vector3.up * playerScript.inputManager.mouseX);
    }
}
