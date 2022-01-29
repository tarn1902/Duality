using UnityEngine;

public sealed class GameManager : MonoBehaviour
{
    #region Static properties

    public GameManager Instance { get; private set; }

    #endregion

    #region Properties

    [field: SerializeField]
    public MousePlayer MousePlayer { get; private set; }

    [field: SerializeField]
    public KeyboardPlayer KeyboardPlayer { get; private set; }

    #endregion

    #region Methods

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
