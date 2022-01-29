using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DavidFDev.Tweening;
using DavidFDev.Audio;
using DavidFDev.Maths;

[RequireComponent(typeof(Rigidbody))]
public class MousePlayer : MonoBehaviour, IPlayer
{
    Rigidbody rb = null;
    [SerializeField] float speed = 5;
    [SerializeField] float zposition;
    [SerializeField] float interactRange = 5f;
    [SerializeField] float reboundForce = 10f;
    Interactable currentInteractable;

    public void Interact()
    {
        if (Input.GetButtonDown("MouseDrag"))
        {
            var i = Interactable.At(transform.position, interactRange, Interactable.InteractableState.Mouse);
            if (i != null)
            {
                i.StartMouseInteraction();
                currentInteractable = i;
            }
        }

        if (Input.GetButtonUp("MouseDrag"))
        {
            if (currentInteractable != null)
            {
                currentInteractable.EndMouseInteraction();
                currentInteractable = null;
            }
        }
    }

    public void Movement()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = transform.position.z - Camera.main.transform.position.z;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePosition);
        worldPos.z = zposition;
        rb.MovePosition(transform.position + (worldPos - transform.position) * Time.deltaTime * speed);
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Interact();
    }
}
