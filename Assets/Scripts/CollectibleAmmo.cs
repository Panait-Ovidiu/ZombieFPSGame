using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleAmmo : MonoBehaviour
{
    public PlayerScript player;
    public Transform bulletHolder;
    public float rotation = 0;
    public float rotationStep = 50f;

    public float radius = 10f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rotation = (bulletHolder.rotation.z + rotationStep) * Time.deltaTime;
        bulletHolder.Rotate(0, 0, rotation, Space.Self);
        if(InRange())
        {
            player.guiManager.ammoInfo.SetActive(true);
        } else
        {
            player.guiManager.ammoInfo.SetActive(false);
        }
    }

    public bool InRange()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance <= radius) // avem interactiune cu obiectul, pot sa afisez informatii: de ex "Press E to use"
        {
            return true;
        }

        return false;
    }
}
