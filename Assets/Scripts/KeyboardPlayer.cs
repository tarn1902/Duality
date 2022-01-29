using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class KeyboardPlayer : MonoBehaviour, IPlayer
{

    CharacterController cc = null;
    [SerializeField] float speed = 1;
    [SerializeField] float jumpSpeed = 1;
    [SerializeField] float gravity = 10.0f;
    [SerializeField] float interactRange = 5f;
    [SerializeField] float zposition;
    private Vector3 movingDirection = Vector3.zero;
    Interactable currentInteractable;
    public void Interact()
    {
        if (Input.GetButtonDown("KeyboardDrag"))
        {
            var i = Interactable.At(transform.position, interactRange, Interactable.InteractableState.Keyboard);
            if (i != null)
            {
                i.StartKeyboardInteraction();
                currentInteractable = i;
            }
        }

        if (Input.GetButtonUp("KeyboardDrag"))
        {
            if (currentInteractable != null)
            {
                currentInteractable.EndKeyboardInteraction();
                currentInteractable = null;
            }
        }
    }

    public void Movement()
    {
        if (cc.isGrounded)
        {
            movingDirection.y = 0;
            if (Input.GetButtonDown("Jump"))
                movingDirection.y = jumpSpeed;
        }

        movingDirection.y -= gravity * Time.deltaTime; 
        movingDirection.x = Input.GetAxis("Horizontal") * speed;
        movingDirection.z = (zposition - transform.position.z) * speed;
        cc.Move(movingDirection * Time.deltaTime);
    }

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Interact();
    }
}
