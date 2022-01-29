using UnityEngine;
using DavidFDev.Tweening;

public abstract class Transformation : MonoBehaviour
{
    #region Fields

    private Tween _enabledTween;

    #endregion

    #region Properties

    public abstract Form TransformationForm { get; }

    public bool IsTransformationEnabled { get; private set; }

    public bool IsAbilityEnabled { get; private set; }

    public bool LockAbilityChange { get; protected set; }

    public MousePlayer MousePlayer => GameManager.Instance.MousePlayer;

    #endregion

    #region Methods

    public void EnableTransformation()
    {
        IsTransformationEnabled = true;
        gameObject.SetActive(true);

        _enabledTween ??= transform.TweenScale(transform.localScale, transform.localScale * 1.25f, 0.3f, Ease.Spike, false);
        _enabledTween.Stop();
        _enabledTween.Start();

        Debug.Log($"Enabled {TransformationForm}.");

        OnTransformationEnabled();
    }

    public void DisableTransformation()
    {
        if (IsAbilityEnabled)
        {
            ToggleAbility();
        }

        IsTransformationEnabled = false;
        gameObject.SetActive(false);

        Debug.Log($"Disabled {TransformationForm}.");

        OnTransformationDisabled();
    }

    public void ToggleAbility()
    {
        if (!IsTransformationEnabled)
        {
            return;
        }

        IsAbilityEnabled = !IsAbilityEnabled;

        Debug.Log($"Toggled ability: {IsAbilityEnabled}");

        if (IsAbilityEnabled)
        {
            OnAbilityEnabled();
        }
        else
        {
            OnAbilityDisabled();
        }
    }

    protected abstract void OnTransformationEnabled();

    protected abstract void OnTransformationDisabled();

    protected virtual void OnUpdate()
    {
    }

    protected abstract void OnAbilityEnabled();

    protected abstract void OnAbilityDisabled();

    private void Update()
    {
        if (!IsTransformationEnabled)
        {
            return;
        }

        OnUpdate();
    }

    #endregion

    #region Nested types

    public enum Form
    {
        Platform = 0,
        Weight = 1,
        Pole = 2,
        Ladder = 3
    }

    #endregion
}
