using UnityEngine;

public sealed class Platform : Transformation
{
    #region Fields

    private float _fallSpeed;

    #endregion

    #region Properties

    public override Form TransformationForm => Form.Platform;

    [field: SerializeField]
    public float Accel { get; private set; } = 0.5f;

    [field: SerializeField]
    public float MaxAccel { get; private set; } = 20f;

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
            _fallSpeed = Mathf.Min(MaxAccel, _fallSpeed + (Accel * Time.deltaTime));
            MousePlayer.GetComponent<Rigidbody>().MovePosition(MousePlayer.transform.position + Vector3.down * _fallSpeed * Time.deltaTime);
        }
        else
        {
            MousePlayer.IsMovementDisabled = false;
            MousePlayer.GetComponent<Collider>().isTrigger = true;
            _fallSpeed = 0f;
        }
    }

    #endregion
}
