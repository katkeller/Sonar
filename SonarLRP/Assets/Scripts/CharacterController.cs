using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class CharacterController : MonoBehaviour
{
    [SerializeField]
    private float speed = 10.0f;

    [SerializeField]
    private float turnSpeed = 2.0f;

    void Update()
    {
        MovePlayer();
        TurnPlayer();
    }

    private void MovePlayer()
    {
        Vector2 translation = SteamVR_Actions._default.Move.GetAxis(SteamVR_Input_Sources.Any);
        translation *= Time.deltaTime;
        transform.Translate(translation.x, 0, translation.y);
    }

    private void TurnPlayer()
    {
        Vector2 rotation = SteamVR_Actions._default.Turn.GetAxis(SteamVR_Input_Sources.Any);
        transform.localRotation = Quaternion.AngleAxis(rotation.x * turnSpeed, transform.up);

        //transform.Rotate(SteamVR_Actions._default.Turn.GetAxis(SteamVR_Input_Sources.Any) * turnSpeed);
        //transform.rotation = Quaternion.Euler(SteamVR_Actions._default.Turn.GetAxis(SteamVR_Input_Sources.Any) * turnSpeed, 0, 0);

        //Vector3 currentRotation = transform.localEulerAngles;
        //transform.localEulerAngles = Vector3.Slerp(currentRotation, Vector3.zero, Time.deltaTime * 2)
    }
}
