using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue UI")]
    [SerializeField] private GameObject _dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    
    private static DialogueManager instance;

    private Story currentStory;

    private bool dialogueIsPlaying;

    private void Awake()
    {
        //prevent multiple Dialogue Managers in the scene
        if(instance != null)
        {
            Debug.LogWarning("More than one Dialogue Manager found");
        }
        instance = this;
    }

    public static DialogueManager GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        dialogueIsPlaying = false;
        _dialoguePanel.SetActive(false);
    }

    private void Update()
    {
        //return if dialogue is not playing
        if (!dialogueIsPlaying)
        {
            return;
        }

        //continue to the next line when submit is pressed
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            ContinueStory();
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        _dialoguePanel.SetActive(true);

        if (currentStory.canContinue)
        {
            dialogueText.text = currentStory.Continue();
        }
        else
        {
            ExitDialogueMode();
        }
    }

    private void ExitDialogueMode()
    {
        dialogueIsPlaying = false;
        _dialoguePanel.SetActive(false);
        dialogueText.text = "";

        ContinueStory();
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            dialogueText.text = currentStory.Continue();
        }
        else
        {
            ExitDialogueMode();
        }
    }
}
