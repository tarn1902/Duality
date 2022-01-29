using UnityEngine;

public sealed class Platform : Transformation
{
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
        ToggleAbility();
    }

    protected override void OnAbilityDisabled()
    {
    }

    protected override void OnUpdate()
    {
        if (Vector3.Distance(MousePlayer.transform.position, GameManager.Instance.KeyboardPlayer.transform.position) < 1f &&
            GameManager.Instance.KeyboardPlayer.transform.position.y > MousePlayer.transform.position.y)
        {
            MousePlayer.IsMovementDisabled = true;
            MousePlayer.GetComponent<Collider>().isTrigger = false;

            // Move down
            MousePlayer.GetComponent<Rigidbody>().MovePosition(MousePlayer.transform.position + Vector3.down * 2f * Time.deltaTime);
        }
        else
        {
            MousePlayer.IsMovementDisabled = false;
            MousePlayer.GetComponent<Collider>().isTrigger = true;
        }
    }

    #endregion
}
