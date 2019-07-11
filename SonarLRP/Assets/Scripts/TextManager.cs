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
    private TextMeshPro subtitleText;

    [SerializeField]
    private TextMeshPro[] choiceText = new TextMeshPro[2];

    [SerializeField]
    private AudioClip[] dialogueClip = new AudioClip[5];

    private Story inkStory;
    private AudioSource audioSource;
    private string currentAudioTag = "";
    private AudioClip clipToPlay;
    //private float clipToPlayLength = 0;
    private int clipToPlayNumber;
    private bool choicesAreUp = false;
    private int[] choiceIndex = new int[2];

    private void Awake()
    {
        //SteamVR.Initialize(true);
        inkStory = new Story(inkAsset.text);
        audioSource = GetComponent<AudioSource>();

        for (int e = 0; e < 2; e++)
        {
            choiceText[e].text = "";
        }

        subtitleText.text = "";
    }

    private void Start()
    {
        StartCoroutine(WaitForAudioDialogue());
    }

    private void Update()
    {
        if (SteamVR_Actions._default.DialogueChoice1.GetStateDown(SteamVR_Input_Sources.Any) && choicesAreUp)
        {
            inkStory.ChooseChoiceIndex(choiceIndex[0]);
            choiceText[1].text = "";
            choicesAreUp = false;
            StartCoroutine(WaitForAudioDialogue());
        }
        else if (SteamVR_Actions._default.DialogueChoice2.GetStateDown(SteamVR_Input_Sources.Any) && choicesAreUp)
        {
            inkStory.ChooseChoiceIndex(choiceIndex[1]);
            choiceText[0].text = "";
            choicesAreUp = false;
            StartCoroutine(WaitForAudioDialogue());
        }
    }

    IEnumerator WaitForAudioDialogue()
    {
        while (inkStory.canContinue)
        {
            Debug.Log(inkStory.Continue());
            subtitleText.text = inkStory.currentText;
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
                choiceText[i].text = inkStory.currentChoices[i].text;
                choicesAreUp = true;
                choiceIndex[i] = choice.index;
            }
        }
    }
}
