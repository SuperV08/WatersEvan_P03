using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 5f;

    public InputAction playerControls;

    Rigidbody _rb = null;

    Vector3 moveDirection = Vector2.zero;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void Update()
    {
        moveDirection = playerControls.ReadValue<Vector2>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        //halts movement when dialogue is playing
        if (DialogueManager.GetInstance().dialogueIsPlaying)
        {
            return;
        }

        _rb.velocity = new Vector2(moveDirection.x * _moveSpeed, moveDirection.z * _moveSpeed);
    }
}
