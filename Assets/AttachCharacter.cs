using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachCharacter : MonoBehaviour
{
    public Rigidbody[] handPlacments = null;

    private void OnTriggerEnter(Collider other)
    {
        GameManager.Instance.KeyboardPlayer.AttachPlayer(handPlacments);
    }
}
