using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

[RequireComponent(typeof(CharacterController))]
public class KeyboardPlayer : MonoBehaviour, IPlayer
{
    CharacterController cc = null;
    [SerializeField] float speed = 1;
    [SerializeField] float jumpSpeed = 1;
    [SerializeField] float gravity = 10.0f;
    [SerializeField] float zposition = 0;
    [SerializeField] float deathFallDistance = 0;
    [SerializeField] TwoBoneIKConstraint rightReach = null;
    [SerializeField] TwoBoneIKConstraint leftReach = null;
    [SerializeField] MultiAimConstraint headLook = null;

    [SerializeField] float rightReachRange = 1;
    [SerializeField] float leftReachRange = 1;
    [SerializeField] float headLookRange = 1;

    private bool isRagdoll = false;
    private Vector3 movingDirection = Vector3.zero;
    public void Interact()
    {

    }

    public void Movement()
    {
        if (!isRagdoll)
        {
            if (transform.position.y < -deathFallDistance)
                RagdollOn();

            if (cc.isGrounded)
            {
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
        MouseControllerReaction();
    }

    public void RagdollOn()
    {
        foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>())
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }
        foreach (Collider c in GetComponentsInChildren<Collider>())
        {
            c.enabled = true;
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
        foreach (Collider c in GetComponentsInChildren<Collider>())
        {
            c.enabled = false;
        }
        GetComponentInChildren<Animator>().enabled = true;
        isRagdoll = false;
        cc.enabled = true;
    }
    
    void MouseControllerReaction()
    {
        rightReach.weight = rightReachRange / Vector3.Distance(transform.position, GameManager.Instance.MousePlayer.transform.position);
        leftReach.weight = leftReachRange / Vector3.Distance(transform.position, GameManager.Instance.MousePlayer.transform.position);
        headLook.weight = headLookRange / Vector3.Distance(transform.position, GameManager.Instance.MousePlayer.transform.position);
    }
}
 