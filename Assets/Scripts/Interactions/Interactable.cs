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
        if (IsMouseInteracting)
        {
            return;
        }

        Debug.Log($"{nameof(StartMouseInteraction)} on {gameObject.name} ({State}).");
        OnStartMouseInteraction(GameManager.Instance.MousePlayer, !IsKeyboardInteracting);
    }

    public void EndMouseInteraction()
    {
        if (!IsMouseInteracting)
        {
            return;
        }

        Debug.Log($"{nameof(EndMouseInteraction)} on {gameObject.name} ({State}).");
        OnEndMouseInteraction(GameManager.Instance.MousePlayer, !IsKeyboardInteracting);
    }

    public void StartKeyboardInteraction()
    {
        if (!IsKeyboardInteracting)
        {
            return;
        }

        Debug.Log($"{nameof(StartKeyboardInteraction)} on {gameObject.name} ({State}).");
        OnStartKeyboardInteraction(GameManager.Instance.KeyboardPlayer, !IsMouseInteracting);
    }

    public void EndKeyboardInteraction()
    {
        if (IsKeyboardInteracting)
        {
            return;
        }

        Debug.Log($"{nameof(OnEndKeyboardInteraction)} on {gameObject.name} ({State}).");
        OnEndKeyboardInteraction(GameManager.Instance.KeyboardPlayer, !IsMouseInteracting);
    }

    protected abstract void OnStartMouseInteraction(MousePlayer mousePlayer, bool onlyMouse);

    protected abstract void OnEndMouseInteraction(MousePlayer mousePlayer, bool onlyMouse);

    protected abstract void OnStartKeyboardInteraction(KeyboardPlayer keyboardPlayer, bool onlyKeyboard);

    protected abstract void OnEndKeyboardInteraction(KeyboardPlayer keyboardPlayer, bool onlyKeyboard);

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
