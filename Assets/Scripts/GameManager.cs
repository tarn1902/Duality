using System;
using UnityEngine;

public sealed class GameManager : MonoBehaviour
{
    #region Static properties

    public static GameManager Instance { get; private set; }

    #endregion

    #region Static methods

    public static string GetSfx(string fileName) => "Audio/SFX/" + fileName;

    public static string GetClip(string fileName) => "Audio/Clips/" + fileName;

    #endregion

    #region Properties

    public MousePlayer MousePlayer { get; private set; }

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

        // Find player objects in scene
        GameObject o;
        MousePlayer = (o = GameObject.FindWithTag("MousePlayer")) != null ? o.GetComponent<MousePlayer>() : throw new NullReferenceException(nameof(MousePlayer));
        KeyboardPlayer = (o = GameObject.FindWithTag("KeyboardPlayer")) != null ? o.GetComponent<KeyboardPlayer>() : throw new NullReferenceException(nameof(KeyboardPlayer));
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
