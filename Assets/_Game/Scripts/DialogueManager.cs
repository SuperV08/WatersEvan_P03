using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue UI")]
    [SerializeField] private GameObject _dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [Header("Choices UI")]

    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;

    private Story currentStory;

    [Header("Text Customization")]

    private TextData _data;

    private TextMeshProUGUI _text;
    [SerializeField] Color _textColor;
    [SerializeField] TMP_FontAsset _font;

    public bool dialogueIsPlaying { get; private set; }

    private static DialogueManager instance;

    private void Awake()
    {
        //change text color
        _text.color = _textColor;
        _text.font = _font;
        _text.fontStyle = _fontStyle;
        
        
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

        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach (GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
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
            StartCoroutine(ExitDialogueMode());
        }
    }

    private IEnumerator ExitDialogueMode()
    {
        yield return new WaitForSeconds(0.2f);
        
        dialogueIsPlaying = false;
        _dialoguePanel.SetActive(false);
        dialogueText.text = "";

        ContinueStory();
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            //Set text for current line of dialogue
            dialogueText.text = currentStory.Continue();
            //display choices, if any
            DisplayChoices();
        }
        else
        {
            StartCoroutine(ExitDialogueMode());
        }

    }

    private void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;

        //check if enough UI is available for implmented choices
        if (currentChoices.Count > choices.Length)
        {
            Debug.LogError("More choices given than the UI can support. Number ofchoices given: " + currentChoices.Count);
        }

        int index = 0;
        //enable and initialize the choices upto the amount of choices for this line of dialogue
        foreach (Choice choice in currentChoices)
        {
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }
        //go through remaining choices the UI supports and make sure they are hidden
        for (int i = index; i < choices.Length; i++)
        {
            choices[i].gameObject.SetActive(false);
        }

        StartCoroutine(SelectFirstChoice());
    }

    private IEnumerator SelectFirstChoice()
    {
        //clear first
        //wait for frame
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
    }

    public void MakeChoice(int choiceIndex)
    {
        currentStory.ChooseChoiceIndex(choiceIndex);
        // handle continuing to the next line in the dialogue when submit is pressed
        if (currentStory.currentChoices.Count == 0 && Mouse.current.leftButton.wasPressedThisFrame)
        {
            ContinueStory();
        }
    }
}
