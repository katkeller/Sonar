using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFacingDirection : MonoBehaviour
{
    //private Vector2 cameraLook;
    [SerializeField]
    private GameObject player;

    void Awake()
    {
        //player = this.transform.parent.gameObject;
    }

    void Update()
    {
        player.transform.localRotation = Quaternion.AngleAxis(this.transform.localRotation.x, player.transform.up);
    }
}
