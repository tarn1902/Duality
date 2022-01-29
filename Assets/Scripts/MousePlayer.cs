using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MousePlayer : MonoBehaviour, IPlayer
{
    Rigidbody rb = null;
    public void Collide()
    {
        throw new System.NotImplementedException();
    }

    public void Drag()
    {
        if (Input.GetButtonDown("MouseDrag"))
        {

        }
    }

    public void Movement()
    {
        rb.MovePosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
