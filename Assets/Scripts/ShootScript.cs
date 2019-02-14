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

    public int damage = 15;

    //timer for the fire rate
    private float fireFactor = 0f;
    //the main cam
    private GameObject mainCam;
    // Use this for initialization
    void Start()
    {
        mainCam = Camera.main.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer)
        {
            HandleInput();
        }
    }
    [Command]
    void Cmd_PlayerShot(string _id, int damage)
    {
        Debug.Log("Player" + _id + "has been shot for + " + damage + " damage!");
    }
    [Client]
    void Shoot()
    {
        Ray camRay = Camera.main.ViewportPointToRay(new Vector3(.5f, .5f));
        RaycastHit rayBullet;
        if(Physics.Raycast(camRay, out rayBullet, fireRange, mask))
        {
            if(rayBullet.collider.tag == "Player")
            {
                Cmd_PlayerShot(rayBullet.collider.gameObject.name, damage);
                HealthScript playerHealth = rayBullet.collider.gameObject.GetComponent<HealthScript>();
                playerHealth.SendMessage("Cmd_TakeDamage", damage);
            }
        }


    }
    void HandleInput()
    {
        fireFactor = fireFactor + Time.deltaTime;
        float fireInterval = 1 / fireRate;
        if (fireFactor >= fireInterval)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Shoot();
            }
            
        }
    }
}
