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
    [SerializeField] float zposition = 0;
    [SerializeField] float deathVelocity = 0;
    private bool isRagdoll = false;
    private Vector3 movingDirection = Vector3.zero;
    public void Interact()
    {

    }

    public void Movement()
    {
        if (!isRagdoll)
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
        else
        {
            if (Input.GetButtonDown("Jump"))
            {
                transform.position = Vector3.zero;
                RagdollOff();
            }

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        RagdollOff();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Interact();
    }

    public void RagdollOn()
    {
        foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>())
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }
        GetComponentInChildren<Animator>().enabled = false;
        isRagdoll = true;
        cc.enabled = false;
    }

    public void RagdollOff()
    {
        foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>())
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }
        GetComponentInChildren<Animator>().enabled = true;
        isRagdoll = false;
        cc.enabled = true;
    }
}
 