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

    [SerializeField] ParticleSystem dropCloud = null;


    enum Direction
    {
        left,
        right,
        none
    }


    private bool isRagdoll = false;
    private Vector3 movingDirection = Vector3.zero;
    private Direction direction = Direction.none;
    private bool isLanded = true;
    public void Interact()
    {

    }

    public void Movement()
    {
        if (!isRagdoll)
        {
            if (cc.isGrounded)
            {
                if (!isLanded)
                {
                    dropCloud.Play();
                    isLanded = true;
                }
                if (Input.GetButtonDown("Jump"))
                    movingDirection.y = jumpSpeed;
            }
            else
            {
                movingDirection.y -= gravity * Time.deltaTime;
                isLanded = false;
            }
            
            movingDirection.x = Input.GetAxis("Horizontal") * speed;
            movingDirection.z = (zposition - transform.position.z) * speed;

            cc.Move(movingDirection * Time.deltaTime);
            if (movingDirection.x != 0)
            {
                transform.GetChild(0).transform.LookAt(transform.position + new Vector3(Input.GetAxis("Horizontal"), 0, 0));
                direction = Input.GetAxis("Horizontal") > 0 ? Direction.right : Direction.left;
            }
            else
            {
                transform.GetChild(0).transform.LookAt(transform.position + new Vector3(0, 0, -1));
                direction = Direction.none;
            }
                
        }
        else
        {
            if (Input.GetButtonDown("Jump"))
            {
                transform.position = Vector3.zero;
                RagdollOff();
            }

        }
        if (transform.position.y < -deathFallDistance)
            RagdollOn();
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
        if ((transform.position.x < GameManager.Instance.MousePlayer.transform.position.x && direction == Direction.right) || (transform.position.x > GameManager.Instance.MousePlayer.transform.position.x && direction == Direction.left) || direction == Direction.none)
        {
            headLook.weight = headLookRange / Vector3.Distance(transform.position, GameManager.Instance.MousePlayer.transform.position);
            rightReach.weight = rightReachRange / Vector3.Distance(transform.position, GameManager.Instance.MousePlayer.transform.position);
            leftReach.weight = leftReachRange / Vector3.Distance(transform.position, GameManager.Instance.MousePlayer.transform.position);
        }
        else
        {
            headLook.weight = 0;
            rightReach.weight = 0;
            leftReach.weight = 0;
        }
    }
}
 