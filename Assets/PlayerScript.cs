using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerScript : NetworkBehaviour
{
    public float mSpeed = 5f;
    public float rotationSpeed = 5f;
    public float jumpHeight = 10f;
    public Rigidbody pRigid;
    public bool isGrounded = false;
    
	// Use this for initialization
	void Start ()
    {
        //obtains rigidbody at start
        pRigid = GetComponent<Rigidbody>();
        //Gets Audio Lister from Cam
        AudioListener audioListener = GetComponentInChildren<AudioListener>();
        //Gets Camera
        Camera cam = GetComponentInChildren<Camera>();

        if (isLocalPlayer)
        {
            cam.enabled = true;
            audioListener.enabled = true;
        }
        else
        {
            cam.enabled = false;
            audioListener.enabled = false;

        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        //if the player that is playing is on their local machine
        if (isLocalPlayer)
        {

            //then players can control their own character
            HandleInput();
        }
    }
    void Move(KeyCode _key)
    {
        //position in scene is equal to the pRigid position 
        Vector3 position = pRigid.position;
        //rotation of quaternion angles is equal to rigid rotation
        Quaternion rotation = pRigid.rotation;

        switch (_key)
        {
            case KeyCode.W:
                position += transform.forward * mSpeed * Time.deltaTime;
                break;
            //go forward in scene via transform forward times the mSpeed variable times delta Time
            case KeyCode.S:
                position += -transform.forward * mSpeed * Time.deltaTime;
                break;
            //go back in scene via negative transform forward (reverse) * move Speed variable times delta Time
            case KeyCode.A:
                rotation *= Quaternion.AngleAxis(-rotationSpeed, Vector3.up);
                break;
            //rotation is timed and equal to the quaternion angle axis, turning counter clockwise
            case KeyCode.D:
                rotation *= Quaternion.AngleAxis(rotationSpeed, Vector3.up);
                break;
            //rotation is timed and equal to the quaternion angle axis turning  clockwise
            case KeyCode.Space:
                if (isGrounded) {
                    pRigid.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
                    isGrounded = false;
                }
                break;
                //if is Grounded is equal to false, adds force to the rigidbody on the up Vector * the jumpheight float. 
                //The force that is added is instance (impulse)
        }
        pRigid.MovePosition(position);
        //moves the player (via its rigidbody) to new position
        pRigid.MoveRotation(rotation);
        //moves the players rotation (via rigidbody) to new rotation

    }
    void HandleInput()
    {
        KeyCode[] keys =
        {
            KeyCode.W,
            KeyCode.S,
            KeyCode.A,
            KeyCode.D,
            KeyCode.Space
        };
        //for each button pressed in keys
        foreach (var key in keys)
        {
            //if the input pressed is equal to that key
            if (Input.GetKey(key))
            {
                //calls the command in Move Function
                Move(key);
            }
        }
    }
    private void OnCollisionEnter(Collision _col)
    {
        //if the player is touching a collider, then isGrounded is true
        isGrounded = true;
    }
}
