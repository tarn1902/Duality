using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class KeyboardPlayer : MonoBehaviour, IPlayer
{

    CharacterController cc = null;

    public void Collide()
    {
        
    }

    public void Drag()
    {
        if (Input.GetButtonDown("KeyboardDrag"))
        {

        }
    }

    public void Movement()
    {
        cc.Move(new Vector3 (Input.GetAxis("Horizontal"), 0f, 0f));
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
        Drag();
        Collide();
    }
}
