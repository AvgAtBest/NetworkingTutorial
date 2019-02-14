using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HealthScript : NetworkBehaviour
{
    public int maxHealth = 100;
    public int curHealth;
	// Use this for initialization

	void Start ()
    {
        curHealth = maxHealth;
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (curHealth >= maxHealth)
        {
            curHealth = maxHealth;
        }

	}

    [Command]void Cmd_TakeDamage(int damage)
    {
        curHealth -= damage;
        Debug.Log("dmg");
        if (curHealth <= 0)
        {
            curHealth = 0;
            Cmd_Dead();
        }
    }
    
    [Command]void Cmd_Dead()
    {
        Debug.Log("Player" + gameObject.name + "is Dead");
        this.gameObject.SetActive(false);
    }
}
