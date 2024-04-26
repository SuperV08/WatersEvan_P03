using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueTrigger : MonoBehaviour
{
    [Header("VisualCue")]
    [SerializeField] private GameObject _visualCue;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    private bool playerInRange;

    private void Awake()
    {
        //turn off visual cue and say the player is not in range at start
        playerInRange = false;
        _visualCue.SetActive(false);
    }

    private void Update()
    {
        if (playerInRange)
        {
            _visualCue.SetActive(true);
            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                Debug.Log(inkJSON.text);
            }
        }
        else
        {
            _visualCue.SetActive(false);
        }
    }

    //detect player collider
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerInRange = false;
        }
    }
}
