using System;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    #region Fields

    #endregion

    #region Methods



    #endregion

    #region Nested types

    [Flags]
    public enum InteractableState
    {
        None = 0,
        Mouse = 1,
        Keyboard = 2,
        Both = Mouse | Keyboard
    }

    #endregion
}
