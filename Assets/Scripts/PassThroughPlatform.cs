using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassThroughPlatform : MonoBehaviour
{
    [SerializeField] Collider collider;
    // Start is called before the first frame update
    void Start()
    {
        collider.isTrigger = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other != collider)
            collider.enabled = false;
    }

    private void OnTriggerExit(Collider other)
    {
        collider.enabled = true;
    }
}
