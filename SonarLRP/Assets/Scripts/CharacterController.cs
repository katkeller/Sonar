using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class CharacterController : MonoBehaviour
{
    [SerializeField]
    private float walkSpeed = 10.0f;

    [SerializeField]
    private float turnSpeed = 2.0f;

    float GetAngle(float input)
    {
        if (input < 0f)
        {
            return -Mathf.LerpAngle(0, 0, -input);
        }
        else if (input > 0f)
        {
            return Mathf.LerpAngle(0, 0, input);
        }
        return 0f;
    }


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
        Quaternion orientation = this.transform.rotation;
        var joystickVector = SteamVR_Actions._default.Turn.GetAxis(SteamVR_Input_Sources.Any);

        Vector3 eulur = transform.rotation.eulerAngles;
        float angle = GetAngle(-joystickVector.y);
        eulur.x = Mathf.LerpAngle(eulur.x, angle, turnSpeed * Time.deltaTime);
        eulur.y += joystickVector.x * turnSpeed * Time.deltaTime;
        this.transform.rotation = Quaternion.Euler(eulur);

        //Vector2 rotation = SteamVR_Actions._default.Turn.GetAxis(SteamVR_Input_Sources.Any);
        //transform.localRotation = Quaternion.AngleAxis(rotation.x * turnSpeed, transform.up);
        

        //transform.Rotate(SteamVR_Actions._default.Turn.GetAxis(SteamVR_Input_Sources.Any) * turnSpeed);
        //transform.rotation = Quaternion.Euler(SteamVR_Actions._default.Turn.GetAxis(SteamVR_Input_Sources.Any) * turnSpeed, 0, 0);

        //Vector3 currentRotation = transform.localEulerAngles;
        //transform.localEulerAngles = Vector3.Slerp(currentRotation, Vector3.zero, Time.deltaTime * 2)
    }
}
