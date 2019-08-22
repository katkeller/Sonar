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
    private TextAsset[] inkAsset = new TextAsset[3];

    [SerializeField]
    private TextMeshPro subtitleText;

    [SerializeField]
    private TextMeshPro[] choiceText = new TextMeshPro[2];

    [SerializeField]
    private AudioClip[] dialogueClip = new AudioClip[5];

    [SerializeField]
    private AudioClip WTButtonPushed, WTChoiceSelected;

    [SerializeField]
    private SonarShout playerSonarShout;

    [SerializeField]
    private float secondsBeforeChoiceFade = 2.0f, durationOfFade = 1.0f;

    private Story inkStory;
    private AudioSource audioSource;
    private MeshCollider headMeshCollider;
    private string currentAudioTag = "";
    private AudioClip clipToPlay;
    //private float clipToPlayLength = 0;
    private int clipToPlayNumber;
    private bool choicesAreUp = false;
    private bool wTIsNearFace = false;
    //private bool canTalk = false;
    private bool choiceIsMade = false;

    private int[] choiceIndex = new int[2];
    private int currentlySelectedChoice;
    private int currentlyNotSelectedChoice;
    private TextMeshPro choiceTextToHide, selectedChoiceText;

    private string dialogue;


    private void Awake()
    {
        //inkStory = new Story(inkAsset[0].text);
        audioSource = GetComponent<AudioSource>();
        headMeshCollider = GetComponent<MeshCollider>();

        for (int e = 0; e < 2; e++)
        {
            choiceText[e].text = "";
        }

        subtitleText.text = "";
    }

    //private void Start()
    //{
    //    StartCoroutine(WaitForAudioDialogue());
    //}

    private void Update()
    {
        if (SteamVR_Actions._default.DialogueChoice1.GetStateDown(SteamVR_Input_Sources.Any) && choicesAreUp)
        {
            currentlySelectedChoice = choiceIndex[0];
            currentlyNotSelectedChoice = choiceIndex[1];
            audioSource.PlayOneShot(WTButtonPushed);
            choiceText[0].color = new Color32(0, 236, 0, 50);
            choiceText[1].color = new Color32(255, 255, 255, 255);
            choiceText[0].fontSize = 0.25f;
            choiceText[1].fontSize = 0.2f;
            selectedChoiceText = choiceText[0];
            choiceTextToHide = choiceText[1];

            if (!choiceIsMade)
                choiceIsMade = true;

            //audioSource.PlayOneShot(WTChoiceSelected, 0.5f);
            //inkStory.ChooseChoiceIndex(choiceIndex[0]);
            //choiceText[1].text = "";
            //choicesAreUp = false;
            //StartCoroutine(WaitForAudioDialogue());
        }
        else if (SteamVR_Actions._default.DialogueChoice2.GetStateDown(SteamVR_Input_Sources.Any) && choicesAreUp)
        {
            currentlySelectedChoice = choiceIndex[1];
            currentlyNotSelectedChoice = choiceIndex[1];
            audioSource.PlayOneShot(WTButtonPushed);
            choiceText[1].color = new Color32(0, 236, 0, 50);
            choiceText[0].color = new Color32(255, 255, 255, 255);
            choiceText[1].fontSize = 0.25f;
            choiceText[0].fontSize = 0.2f;
            selectedChoiceText = choiceText[1];
            choiceTextToHide = choiceText[0];

            if (!choiceIsMade)
                choiceIsMade = true;

            //audioSource.PlayOneShot(WTChoiceSelected, 0.5f);
            //inkStory.ChooseChoiceIndex(choiceIndex[1]);
            //choiceText[0].text = "";
            //choicesAreUp = false;
            //StartCoroutine(WaitForAudioDialogue());
        }

        if (SteamVR_Actions._default.WTTrigger.GetStateDown(SteamVR_Input_Sources.Any) && choicesAreUp && choiceIsMade)
        {
            audioSource.PlayOneShot(WTChoiceSelected);
            inkStory.ChooseChoiceIndex(choiceIndex[currentlySelectedChoice]);
            choiceTextToHide.text = "";
            StartCoroutine(FadeSelectedChoiceText());
            choiceIsMade = false;
            choicesAreUp = false;
            StartCoroutine(WaitForAudioDialogue());

            //canTalk = true;
            //audioSource.PlayOneShot(WTChoiceSelected, 1.0f);
        }
        
        //else if (SteamVR_Actions._default.WTTrigger.GetStateUp(SteamVR_Input_Sources.Any))
        //{
        //    canTalk = false;
        //}
    }

    private void OnConvoTriggered(int conversationNumber)
    {
        inkStory = new Story(inkAsset[conversationNumber].text);
        StartCoroutine(WaitForAudioDialogue());
    }

    private void OnEnable()
    {
        DialogueTrigger.ConvoTriggered += OnConvoTriggered;
    }

    private void OnDisable()
    {
        DialogueTrigger.ConvoTriggered -= OnConvoTriggered;
    }

    IEnumerator WaitForAudioDialogue()
    {
        while (inkStory.canContinue)
        {
            Debug.Log(inkStory.Continue());
            //subtitleText.text = inkStory.currentText;
            //currentTagsList = inkStory.currentTags;
            currentAudioTag = string.Join("", inkStory.currentTags.ToArray());
            //clipToPlayNumber = Int32.Parse(currentTagsList);
            clipToPlayNumber = Int32.Parse(currentAudioTag);
            clipToPlay = dialogueClip[clipToPlayNumber - 1];
            //clipToPlayLength = clipToPlay.length;
            StartCoroutine(DisplayDialogue());
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

    IEnumerator DisplayDialogue()
    {
        // Split the current dialogue into two parts: the "speaker" at the beginning, and the actual dialogue at the end. Then determine who the speaker is, and set the subtitle color and the isTalking bool accordingly.
        dialogue = inkStory.currentText;
        string[] splitDialogue = dialogue.Split(':');

        if (splitDialogue[0] == "You")
        {
            playerSonarShout.isTalking = true;
            subtitleText.color = new Color32(255, 255, 255, 255);
        }
        else if (splitDialogue[0] == "Friend")
        {
            playerSonarShout.isTalking = false;
            subtitleText.color = new Color32(255, 169, 29, 255);
        }

        if (splitDialogue[1].Contains("^"))
        {
            string[] subtitleSplit = splitDialogue[1].Split('^');
            int textCycles = subtitleSplit.Length;
            float waitTime = clipToPlay.length / textCycles;

            for (int j = 0; j < textCycles; j++)
            {
                subtitleText.text = subtitleSplit[j];
                yield return new WaitForSeconds(waitTime);
            }
        }
        else
        {
            subtitleText.text = splitDialogue[1];
        }


        //// Check to see if the dialogue is longer than 90 char, a.k.a., the size of our subtitle text box.
        //if (splitDialogue[1].Length > 90)
        //{
        //    float textCyclesFloat = splitDialogue[1].Length / 90;
        //    int textCycles;

        //    // Check for a decimal value, and if there is one, add 1 to the number of text cycles
        //    if ((textCyclesFloat % 1) == float.Epsilon)
        //    {
        //        textCycles = (int)textCyclesFloat;
        //    }
        //    else
        //    {
        //        textCycles = (int)textCyclesFloat + 1;
        //    }

        //    int startingChar = 0;
        //    int endingChar = 89;
        //    float secondsToWait = clipToPlay.length / textCycles;

        //    // For each text cycle of 90 char, display the text for the length of the audio clip divided by the number of text cycles.
        //    for (int j = 0; j < textCycles; j++)
        //    {
        //        if (endingChar > splitDialogue[1].Length)
        //        {
        //            subtitleText.text = splitDialogue[1].Substring(startingChar);
        //        }
        //        else
        //        {
        //            subtitleText.text = splitDialogue[1].Substring(startingChar, endingChar);
        //            startingChar += 90;
        //            endingChar += 90;
        //        }

        //        yield return new WaitForSeconds(secondsToWait);
        //    }
        //}
        //else
        //{
        //    subtitleText.text = inkStory.currentText;
        //}
    }

    IEnumerator FadeSelectedChoiceText()
    {
        yield return new WaitForSeconds(secondsBeforeChoiceFade);
        Color startingColor = selectedChoiceText.color;

        for (float t = 0.1f; t < durationOfFade; t += Time.deltaTime)
        {
            selectedChoiceText.color = Color.Lerp(startingColor, Color.clear, Mathf.Min(1, t / durationOfFade));
            yield return null;
        }

        selectedChoiceText.text = "";
        choiceText[0].color = new Color32(255, 255, 255, 255);
        choiceText[1].color = new Color32(255, 255, 255, 255);
        choiceText[0].fontSize = 0.2f;
        choiceText[1].fontSize = 0.2f;
    }
}
