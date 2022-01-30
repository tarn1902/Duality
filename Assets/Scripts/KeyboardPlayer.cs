using DavidFDev.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

[RequireComponent(typeof(CharacterController))]
public class KeyboardPlayer : MonoBehaviour, IPlayer
{
    CharacterController cc = null;
    [SerializeField] float speed = 1;
    [SerializeField] float climbSpeed = 1;
    [SerializeField] float jumpSpeed = 1;
    [SerializeField] float gravity = 10.0f;
    [SerializeField] float zposition = 0;
    [SerializeField] float deathFallDistance = 0;
    [SerializeField] float ladderPopSpeed = 10;
    [SerializeField] float pendulumPopSpeed = 10;
    [SerializeField] float lavaPopSpeed = 9f;
    [SerializeField] TwoBoneIKConstraint rightReach = null;
    [SerializeField] TwoBoneIKConstraint leftReach = null;
    [SerializeField] MultiAimConstraint headLook = null;

    [SerializeField] float rightReachRange = 1;
    [SerializeField] float leftReachRange = 1;
    [SerializeField] float headLookRange = 1;

    [SerializeField] ParticleSystem dropCloud = null;
    [SerializeField] Animator anim = null;
    [SerializeField] Transform ragdollRoot = null;
    public GameObject[] grabHands = null;
    private Rigidbody[] joints = null;
    private bool isTouchingLadder = false;
    private bool isMovingUpLadder = false;
    private bool isTopOfLadder = false;
    bool justJumpedFromPendulum = false;
    private GameObject lastPendulumn = null;
    


    enum Direction
    {
        left,
        right,
        none
    }


    private bool isRagdoll = false;
    private bool isDead = false;
    private Vector3 movingDirection = Vector3.zero;
    private Direction direction = Direction.none;
    private bool isJustLanded = true;

    private bool hasWon;

    public void Interact()
    {

    }

    public void Movement()
    {
        if (!isRagdoll)
        {
            Climbing();
            Jump();
            Move();

            if(hasWon)
            {
                movingDirection.x = 0f;
            }

            cc.Move(movingDirection * Time.deltaTime);

            FaceDirection();

            //if (transform.position.y < -deathFallDistance)
            //    RagdollOn();

        }

        else
        {
            if (Input.GetButtonDown("Jump"))
            {
                if (isDead)
                {
                    GameManager.Instance.Respawn();
                    isDead = false;
                }
                else
                {
                    transform.position = ragdollRoot.position;
                    justJumpedFromPendulum = true;

                }
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

    void Climbing()
    {
        if (isTouchingLadder)
        {
            Transformation t = GameManager.Instance.MousePlayer.CurrentTransformation;
            if (t.TransformationForm != Transformation.Form.Ladder || !t.IsAbilityEnabled)
            {
                isTouchingLadder = false;
            }

            if (Input.GetAxis("Vertical") > 0 && cc.isGrounded)
            {
                isMovingUpLadder = true;
                anim.SetBool("IsClimbing", true);
            }

            if (isMovingUpLadder)
            {
                movingDirection.y = Input.GetAxis("Vertical") * climbSpeed;
                if (isTopOfLadder)
                {
                    isMovingUpLadder = false;
                    anim.SetBool("IsClimbing", false) ;

                }
            }
        }
    }

    void Jump()
    {
        if (cc.isGrounded)
        {
            if (!isJustLanded)
            {
                dropCloud.Play();
                anim.SetBool("IsFalling", false);
                anim.SetBool("IsLanding", true);
                isJustLanded = true;
                justJumpedFromPendulum = false;
                lastPendulumn = null;
            }
            if (Input.GetButtonDown("Jump"))
            {
                movingDirection.y = jumpSpeed;
                anim.SetTrigger("OnJump");

                Audio.PlaySfx(GameManager.GetSfx("SFX_Jump"));
            }
        }
        else
        {
            if (isTopOfLadder)
            {
                movingDirection.y = ladderPopSpeed;
                isTopOfLadder = false;
            }
            else if (justJumpedFromPendulum)
            {
                movingDirection.y = ladderPopSpeed;
                justJumpedFromPendulum = false;
            }
            movingDirection.y -= gravity * Time.deltaTime;
            anim.SetBool("IsFalling", true);
            isJustLanded = false;
            anim.SetBool("IsLanding", false);
        }
    }

    void Move()
    {
        if (!isMovingUpLadder || (isMovingUpLadder && cc.isGrounded))
        {
            movingDirection.x = Input.GetAxis("Horizontal") * speed;
            anim.SetFloat("MovementDirection", (Input.GetAxis("Horizontal") + 1) / 2);
            movingDirection.z = (zposition - transform.position.z) * speed;
        }
        else
        {
            movingDirection.x = 0;
        }
    }

    void FaceDirection()
    {
        if (isMovingUpLadder)
        {
            transform.GetChild(0).transform.LookAt(transform.position + new Vector3(0, 0, 1));
        }
        else if (movingDirection.x != 0)
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

    public void RagdollOn()
    {
        foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>())
        {
            if (rb == GetComponent<Rigidbody>())
            {
                continue;
            }
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
        if (joints != null)
        {
            foreach (Rigidbody joint in joints)
            {
                Destroy(joint.GetComponent<FixedJoint>());
            }
            justJumpedFromPendulum = true;
        }

        GetComponentInChildren<Animator>().enabled = true;
        isRagdoll = false;
        cc.enabled = true;
    }
    
    void MouseControllerReaction()
    {
        if (Vector3.Distance(transform.position, GameManager.Instance.MousePlayer.transform.position) > 5.5f)
        {
            headLook.weight = 0;
            rightReach.weight = 0;
            leftReach.weight = 0;
            return;
        }

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ladder")
        {
            isTouchingLadder = true;
            if (cc.isGrounded)
            {
                isTopOfLadder = false;
            }
        }

        // Die when touching a hazard
        if (other.CompareTag("Hazard"))
        {
            isDead = true;

            RagdollOn();

            foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>())
            {
                rb.AddForce(Vector3.up * lavaPopSpeed, ForceMode.Impulse);
            }

            Audio.PlaySfx(GameManager.GetSfx("SFX_DeathScream"));
            Audio.PlaySfx(GameManager.GetSfx("SFX_DeathSizzle"));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Ladder")
        {
            if (!cc.isGrounded && movingDirection.x == 0)
            {
                isTopOfLadder = true;
            }
            isTouchingLadder = false;
            isMovingUpLadder = false;
            anim.SetBool("IsClimbing", false);
        }

        if (other.CompareTag("Finish") && !hasWon)
        {
            StartCoroutine(WinEvent());
        }
    }

    public void AttachPlayer(Rigidbody[] handPlacments, GameObject pendulmn)
    {
        if (lastPendulumn == null || lastPendulumn != pendulmn)
        {
            GameManager.Instance.KeyboardPlayer.RagdollOn();
            joints = handPlacments;
            for (int i = 0; i < handPlacments.Length; i++)
            {
                if (handPlacments[i].GetComponent<FixedJoint>() == null)
                {
                    FixedJoint joint = handPlacments[i].gameObject.AddComponent<FixedJoint>();
                    joint.connectedBody = GameManager.Instance.KeyboardPlayer.grabHands[i].GetComponent<Rigidbody>();
                    handPlacments[i].gameObject.transform.position = handPlacments[i].transform.position;
                    joint.autoConfigureConnectedAnchor = false;
                    joint.connectedAnchor = Vector3.zero;
                }
            }
            lastPendulumn = pendulmn;
        }

    }

    private IEnumerator WinEvent()
    {
        hasWon = true;

        // Start dance
        anim.SetTrigger("OnDance");

        // Stop music
        Audio.StopMusic();

        // Play sfx
        Audio.PlaySfx(GameManager.GetSfx("SFX_Victory"));

        yield return new WaitForSeconds(2.5f);
        
        // Start victory music
        Audio.PlayMusic(GameManager.GetClip("SND_VictorySong"));
    }
}
 