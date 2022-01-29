using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassThroughPlatform : MonoBehaviour
{
    [SerializeField] Collider toggleCollider = null;
    // Start is called before the first frame update
    void Start()
    {
        toggleCollider.isTrigger = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other != toggleCollider)
            toggleCollider.enabled = false;
    }

    private void OnTriggerExit(Collider other)
    {
        toggleCollider.enabled = true;
    }
}
