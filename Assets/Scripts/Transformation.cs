using UnityEngine;

public abstract class Transformation : MonoBehaviour
{
    #region Properties

    public bool IsTransformationEnabled { get; private set; }

    public bool IsAbilityEnabled { get; private set; }

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
}
