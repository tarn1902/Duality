using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachCharacter : MonoBehaviour
{
    public Rigidbody[] handPlacments = null;

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.Instance.IsKeyboardPlayer(other.gameObject))
            GameManager.Instance.KeyboardPlayer.AttachPlayer(handPlacments);
    }
}
