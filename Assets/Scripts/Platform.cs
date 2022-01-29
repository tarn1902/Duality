using UnityEngine;

public sealed class Platform : Transformation
{
    #region Fields

    private Vector3 _oldPos;

    #endregion

    #region Properties

    public override Form TransformationForm => Form.Platform;

    #endregion

    #region Methods

    protected override void OnTransformationEnabled()
    {
        
    }

    protected override void OnTransformationDisabled()
    {
        
    }

    protected override void OnAbilityEnabled()
    {
    }

    protected override void OnAbilityDisabled()
    {
    }

    protected override void OnUpdate()
    {
        if (Vector3.Distance(MousePlayer.transform.position, GameManager.Instance.KeyboardPlayer.transform.position) < 3f &&
            GameManager.Instance.KeyboardPlayer.transform.position.y > MousePlayer.transform.position.y)
        {
            MousePlayer.transform.position = _oldPos;
        }

        _oldPos = MousePlayer.transform.position;
    }

    #endregion
}
