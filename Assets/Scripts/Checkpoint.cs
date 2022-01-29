using UnityEngine;

[RequireComponent(typeof(Collider))]
[AddComponentMenu("Gameplay/Checkpoint")]
public sealed class Checkpoint : MonoBehaviour
{
    #region Properties

    [field: SerializeField]
    public Transform RespawnTransform { get; private set; }

    #endregion

    #region Methods

    private void OnTriggerEnter(Collider other)
    {
        // Check for collision with keyboard player
        if (GameManager.Instance.IsKeyboardPlayer(other.gameObject))
        {
            // Check if checkpoint is not activated
            if (GameManager.Instance.CurrentCheckpoint != this)
            {
                // Activate checkpoint
                GameManager.Instance.SetCheckpoint(this);
            }
        }
    }

    #endregion
}
