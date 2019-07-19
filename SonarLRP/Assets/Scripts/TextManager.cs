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

    [SerializeField]
    private AudioClip WTTriggerPushed, WTChoiceSelected;

    [SerializeField]
    private SonarShout playerSonarShout;

    private Story inkStory;
    private AudioSource audioSource;
    private MeshCollider headMeshCollider;
    private string currentAudioTag = "";
    private AudioClip clipToPlay;
    //private float clipToPlayLength = 0;
    private int clipToPlayNumber;
    private bool choicesAreUp = false;
    private bool wTIsNearFace = false;
    private bool canTalk = false;
    private int[] choiceIndex = new int[2];

    private string dialogue;


    private void Awake()
    {
        //SteamVR.Initialize(true);
        inkStory = new Story(inkAsset.text);
        audioSource = GetComponent<AudioSource>();
        headMeshCollider = GetComponent<MeshCollider>();

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
        if (SteamVR_Actions._default.DialogueChoice1.GetStateDown(SteamVR_Input_Sources.Any) && choicesAreUp && canTalk)
        {
            audioSource.PlayOneShot(WTChoiceSelected, 0.5f);
            inkStory.ChooseChoiceIndex(choiceIndex[0]);
            choiceText[1].text = "";
            choicesAreUp = false;
            StartCoroutine(WaitForAudioDialogue());
        }
        else if (SteamVR_Actions._default.DialogueChoice2.GetStateDown(SteamVR_Input_Sources.Any) && choicesAreUp && canTalk)
        {
            audioSource.PlayOneShot(WTChoiceSelected, 0.5f);
            inkStory.ChooseChoiceIndex(choiceIndex[1]);
            choiceText[0].text = "";
            choicesAreUp = false;
            StartCoroutine(WaitForAudioDialogue());
        }

        if (SteamVR_Actions._default.WTTrigger.GetStateDown(SteamVR_Input_Sources.Any) && choicesAreUp)
        {
            canTalk = true;
            audioSource.PlayOneShot(WTTriggerPushed, 1.0f);
        }
        
        else if (SteamVR_Actions._default.WTTrigger.GetStateUp(SteamVR_Input_Sources.Any))
        {
            canTalk = false;
        }
    }

    IEnumerator WaitForAudioDialogue()
    {
        while (inkStory.canContinue)
        {
            Debug.Log(inkStory.Continue());
            subtitleText.text = inkStory.currentText;
            CheckForPlayerDialogue();
            //currentTagsList = inkStory.currentTags;
            currentAudioTag = string.Join("", inkStory.currentTags.ToArray());
            //clipToPlayNumber = Int32.Parse(currentTagsList);
            clipToPlayNumber = Int32.Parse(currentAudioTag);
            clipToPlay = dialogueClip[clipToPlayNumber - 1];
            //clipToPlayLength = clipToPlay.length;
            audioSource.PlayOneShot(clipToPlay, 1.0f);
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

            playerSonarShout.isTalking = false;
        }
    }

    private void CheckForPlayerDialogue()
    {
        dialogue = inkStory.currentText;
        string[] splitDialogue = dialogue.Split(':');

        if (splitDialogue[0] == "You")
        {
            playerSonarShout.isTalking = true;
        }
        else if (splitDialogue[0] == "Friend")
        {
            playerSonarShout.isTalking = false;
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.tag == "WalkieTalkie")
    //    {
    //        wTIsNearFace = true;
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.tag == "WalkieTalkie")
    //    {
    //        wTIsNearFace = false;
    //    }
    //}
}
