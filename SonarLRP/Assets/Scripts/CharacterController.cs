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

    [SerializeField]
    private AudioClip[] footsteps = new AudioClip[8];

    private AudioSource audioSource;

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

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
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

        if (audioSource.isPlaying == false && translation != Vector2.zero)
        {
            AudioClip clipToPlay = footsteps[UnityEngine.Random.Range(0, footsteps.Length)];
            audioSource.PlayOneShot(clipToPlay);
        }
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
    }
}
