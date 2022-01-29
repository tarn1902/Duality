using UnityEngine;

public sealed class GameManager : MonoBehaviour
{
    #region Static properties

    public static GameManager Instance { get; private set; }

    #endregion

    #region Properties

    [field: SerializeField]
    public MousePlayer MousePlayer { get; private set; }

    [field: SerializeField]
    public KeyboardPlayer KeyboardPlayer { get; private set; }

    public Checkpoint CurrentCheckpoint { get; private set; }

    #endregion

    #region Methods

    public void Respawn()
    {
        KeyboardPlayer.transform.position = CurrentCheckpoint.RespawnTransform.position;
    }

    public void SetCheckpoint(Checkpoint checkpoint)
    {
        CurrentCheckpoint = checkpoint;
        Debug.Log($"Checkpoint updated to {checkpoint.RespawnTransform.position}.");
    }

    public bool IsMousePlayer(GameObject obj)
    {
        return obj == MousePlayer.gameObject;
    }

    public bool IsKeyboardPlayer(GameObject obj)
    {
        return obj == KeyboardPlayer.gameObject;
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError($"Only one {nameof(GameManager)} can exist at a time!");
            return;
        }

        Instance = this;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    #endregion
}
