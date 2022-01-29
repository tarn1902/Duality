using System;
using System.Linq;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    #region Static methods

    /// <summary>
    ///     Search for an interactable at the given position, returning the closest one if there are multiple.
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="radius"></param>
    /// <param name="state"></param>
    /// <returns></returns>
    public static Interactable At(Vector3 pos, float radius, InteractableState state)
    {
        return Physics.OverlapSphere(pos, radius)
            .Where(x => x.GetComponent<Interactable>() != null && (x.GetComponent<Interactable>().AllowedInteractions & state) != 0)
            .OrderBy(x => Vector2.Distance(pos, x.transform.position))
            .FirstOrDefault()
            ?.GetComponent<Interactable>();
    }

    #endregion

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

    /// <summary>
    ///     Allowed interactions.
    /// </summary>
    public virtual InteractableState AllowedInteractions { get; private set; } = InteractableState.Both;

    /// <summary>
    ///     Whether mouse interactions are allowed.
    /// </summary>
    public bool IsMouseAllowed => (AllowedInteractions & InteractableState.Mouse) != 0;

    /// <summary>
    ///     Whether keyboard interactions are allowed.
    /// </summary>
    public bool IsKeyboardAllowed => (AllowedInteractions & InteractableState.Keyboard) != 0;

    #endregion

    #region Methods

    /// <summary>
    ///     Attempt to start a mouse interaction if allowed.
    /// </summary>
    /// <returns></returns>
    public bool StartMouseInteraction()
    {
        if (IsMouseInteracting || !IsMouseAllowed)
        {
            return false;
        }

        IsMouseInteracting = true;
        Debug.Log($"{nameof(StartMouseInteraction)} on {gameObject.name} ({State}).");
        OnStartMouseInteraction(GameManager.Instance.MousePlayer, !IsKeyboardInteracting);
        return true;
    }

    /// <summary>
    ///     End a mouse interaction.
    /// </summary>
    /// <returns></returns>
    public bool EndMouseInteraction()
    {
        if (!IsMouseInteracting)
        {
            return false;
        }

        IsMouseInteracting = false;
        Debug.Log($"{nameof(EndMouseInteraction)} on {gameObject.name} ({State}).");
        OnEndMouseInteraction(GameManager.Instance.MousePlayer, !IsKeyboardInteracting);
        return true;
    }

    /// <summary>
    ///     Attempt to start a keyboard interaction if allowed.
    /// </summary>
    /// <returns></returns>
    public bool StartKeyboardInteraction()
    {
        if (IsKeyboardInteracting || !IsKeyboardAllowed)
        {
            return false;
        }

        IsKeyboardInteracting = true;
        Debug.Log($"{nameof(StartKeyboardInteraction)} on {gameObject.name} ({State}).");
        OnStartKeyboardInteraction(GameManager.Instance.KeyboardPlayer, !IsMouseInteracting);
        return true;
    }

    /// <summary>
    ///  End a keyboard interaction.
    /// </summary>
    /// <returns></returns>
    public bool EndKeyboardInteraction()
    {
        if (!IsKeyboardInteracting)
        {
            return false;
        }

        IsKeyboardInteracting = false;
        Debug.Log($"{nameof(OnEndKeyboardInteraction)} on {gameObject.name} ({State}).");
        OnEndKeyboardInteraction(GameManager.Instance.KeyboardPlayer, !IsMouseInteracting);
        return true;
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
