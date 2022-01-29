using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{

    private void Start()
    {
        GetComponent<Collider>().isTrigger = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        GameManager.Instance.KeyboardPlayer.RagdollOn();
    }
}
