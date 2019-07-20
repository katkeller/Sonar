using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class CharacterController : MonoBehaviour
{
    [SerializeField]
    private float speed = 10.0f;

    void Update()
    {
        Vector2 translation = SteamVR_Actions._default.Move.GetAxis(SteamVR_Input_Sources.Any) * speed;
        translation *= Time.deltaTime;
        transform.Translate(translation.x, 0, translation.y);
    }
}
