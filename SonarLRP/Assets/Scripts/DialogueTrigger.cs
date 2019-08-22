using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Tooltip("The number corrosponding to the Ink Story object on the Dialogue Manager that you'd like to trigger. The list begins at 0.")]
    [SerializeField]
    private int conversationToTrigger;

    private MeshCollider triggerCollider;
    private bool hasBeenTriggered = false;

    public static event Action<int> ConvoTriggered;

    void Awake()
    {
        triggerCollider = GetComponent<MeshCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !hasBeenTriggered)
        {
            Debug.Log("The player entered the dialogue trigger.");
            ConvoTriggered?.Invoke(conversationToTrigger);
            hasBeenTriggered = true;
        }
    }
}
