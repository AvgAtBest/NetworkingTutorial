using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ShootScript : NetworkBehaviour
{
    //fire rate of weapon
    public float fireRate = 1f;
    //range of weapon
    public float fireRange = 100f;
    //LayerMask we wil be hitting
    public LayerMask mask;

    //timer for the fire rate
    private float fireFactor = 0f;
    //the main cam
    private GameObject mainCam;
    // Use this for initialization
    void Start()
    {
        mainCam.GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    [Command]
    void Cmd_PlayerShot(string _id)
    {
        Debug.Log("Player" + _id + "has been shot!");
    }
    [Client]
    void Shoot()
    {
        RaycastHit rayBullet;
        Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out rayBullet, fireRange, mask);


    }
}
