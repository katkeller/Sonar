using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using UnityEngine.UI;
using TMPro;
using Valve.VR;

public class TextManager : MonoBehaviour
{
    [SerializeField]
    private TextAsset inkAsset;

    [SerializeField]
    private TextMeshPro dialogueText;

    [SerializeField]
    private AudioClip[] dialogueClip = new AudioClip[5];

    private Story inkStory;
    private AudioSource audioSource;
    private string currentAudioTag = "";
    private AudioClip clipToPlay;
    //private float clipToPlayLength = 0;
    private int clipToPlayNumber;

    private void Awake()
    {
        //SteamVR.Initialize(true);
        inkStory = new Story(inkAsset.text);
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        StartCoroutine(WaitForAudioDialogue());
    }

    IEnumerator WaitForAudioDialogue()
    {
        while (inkStory.canContinue)
        {
            Debug.Log(inkStory.Continue());
            //currentTagsList = inkStory.currentTags;
            currentAudioTag = string.Join("", inkStory.currentTags.ToArray());
            //clipToPlayNumber = Int32.Parse(currentTagsList);
            clipToPlayNumber = Int32.Parse(currentAudioTag);
            clipToPlay = dialogueClip[clipToPlayNumber - 1];
            //clipToPlayLength = clipToPlay.length;
            audioSource.PlayOneShot(clipToPlay);
            yield return new WaitForSeconds(clipToPlay.length);
        }

        if (inkStory.currentChoices.Count > 0)
        {
            for (int i = 0; i < inkStory.currentChoices.Count; i++)
            {
                Choice choice = inkStory.currentChoices[i];
                Debug.Log("Choice " + (i + 1) + ". " + choice.text);
            }
        }
    }
}
