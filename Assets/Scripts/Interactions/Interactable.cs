using System;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    #region Properties

    /// <summary>
    ///     Current interaction state.
    /// </summary>
    public InteractableState State { get; private set; } = InteractableState.None;

    /// <summary>
    ///     Whether the mouse is currently interacting with the interactable.
    /// </summary>
    public bool IsMouseInteracting
    {
        get => (State & InteractableState.Mouse) != 0;
        private set => State = value ? (State | InteractableState.Mouse) : (State & ~InteractableState.Mouse);
    }

    /// <summary>
    ///     Whether the keyboard is currently interacting with the interactable.
    /// </summary>
    public bool IsKeyboardInteracting
    {
        get => (State & InteractableState.Keyboard) != 0;
        private set => State = value ? (State | InteractableState.Keyboard) : (State & ~InteractableState.Keyboard);
    }

    #endregion

    #region Methods

    public void StartMouseInteraction()
    {

    }

    public void EndMouseInteraction()
    {
        
    }

    public void StartKeyboardInteraction()
    {

    }

    public void EndKeyboardInteraction()
    {
        
    }

    protected abstract void OnStartMouseInteraction();

    protected abstract void OnEndMouseInteraction();

    protected abstract void OnStartKeyboardInteraction();

    protected abstract void OnEndKeyboardInteraction();

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
