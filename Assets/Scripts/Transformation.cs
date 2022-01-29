using UnityEngine;

public abstract class Transformation : MonoBehaviour
{
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
        OnTransformationDisabled();
    }

    public void ToggleAbility()
    {
        if (!IsTransformationEnabled)
        {
            return;
        }

        IsAbilityEnabled = !IsAbilityEnabled;

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
        Normal = 0,
        Platform = 1,
        Weight = 2,
        Pole = 3,
        Ladder = 4
    }

    #endregion
}
