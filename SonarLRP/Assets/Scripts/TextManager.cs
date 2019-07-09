using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class TextManager : MonoBehaviour
{
    [SerializeField]
    private TextAsset inkAsset;

    [SerializeField]
    private AudioClip[] dialogueClip = new AudioClip[5];

    private Story inkStory;
    private AudioSource audioSource;
    //private float[] dialogueClipLength = new float[5];
    private string currentAudioTag = "";
    //private List<string> currentTagsList = new List<string>();
    private AudioClip clipToPlay;
    //private float clipToPlayLength = 0;
    private int clipToPlayNumber;

    private void Awake()
    {
        inkStory = new Story(inkAsset.text);
        audioSource = GetComponent<AudioSource>();
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
            clipToPlay = dialogueClip[clipToPlayNumber];
            //clipToPlayLength = clipToPlay.length;
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
