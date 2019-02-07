using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SyncTransform : NetworkBehaviour
{
    //Speed of lerping rotation & position
    public float lerpRate = 15f;
    //Vars to be synced across the network
    [SyncVar] private Vector3 syncPosition;
    [SyncVar] private Quaternion syncRotation;

    //gets rigidbody
    private Rigidbody rigid;

    //Threshold for when to send commands
    public float positionThreshold = 0.5f;
    public float rotationThreshold = 5.0f;

    //Records previous position and rotation that was send to server
    private Vector3 lastPosition;
    private Quaternion lastRotation;
	void Start ()
    {
        //Gets rigidbody
        rigid = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        //transmit's the position every other update to the server
        TransmitPosition();
        //Lerp's the position every other update To the server. Smoothes the controls on the receiving end
        LerpPosition();
        //Transmits the rotation every other update to the server
        TransmitRotation();
        //Lerps the rotation every other update
        LerpRotation();
	}
    void LerpPosition()
    {
        //if it isnt the local player
        if (!isLocalPlayer)
        {
            //updates the other players poition on the local players screen
            rigid.position = Vector3.Lerp(rigid.position, syncPosition, Time.deltaTime * lerpRate);
        }
    }
    void LerpRotation()
    {
        //if it isnt the local player
        if (!isLocalPlayer)
        {
            //updates the other players rotation on the local players screen
            rigid.rotation = Quaternion.Lerp(rigid.rotation, syncRotation, Time.deltaTime * lerpRate);
        }
    }
    //A command that sends the players position to the server
    [Command] void CmdSendPositionToServer(Vector3 _position)
    {
        //syncs the position
        syncPosition = _position;
        Debug.Log("Position Command");
    }

    //a command that sends the players rotation to the server
    [Command] void CmdSendRotationToServer(Quaternion _rotation)
    {
        //syncs the rotation
        syncRotation = _rotation;
        Debug.Log("Rotation Command");
    }
    [ClientCallback] void TransmitPosition()
    {
        if (isLocalPlayer && Vector3.Distance(rigid.position, lastPosition) > positionThreshold)
        {
            CmdSendPositionToServer(rigid.position);
            lastPosition = rigid.position;
        }
    }
    [ClientCallback] void TransmitRotation()
    {
        if (isLocalPlayer && Quaternion.Angle(rigid.rotation, lastRotation) > rotationThreshold)
        {
            CmdSendRotationToServer(rigid.rotation);
            lastRotation = rigid.rotation;
        }
    }
}
