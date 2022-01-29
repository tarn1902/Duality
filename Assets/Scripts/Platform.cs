using DavidFDev.Tweening;
using UnityEngine;

public sealed class Platform : Transformation
{
    #region Fields

    private float _fallSpeed;

    private bool _doKick;

    private Tween _kickTween;

    #endregion

    #region Properties

    public override Form TransformationForm => Form.Platform;

    [field: SerializeField]
    public float Accel { get; private set; } = 0.5f;

    [field: SerializeField]
    public float MaxAccel { get; private set; } = 20f;

    [field: SerializeField]
    public float KickbackDistance { get; private set; } = 2.5f;

    [field: SerializeField]
    public float KickbackDuration { get; private set; } = 1.8f;

    [field: SerializeField]
    public EaseType KickbackAnimation { get; private set; } = EaseType.QuadIn;

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
        if (IsPlayerStandingOn() && (!_kickTween?.IsActive ?? true))
        {
            if (!MousePlayer.IsMovementDisabled)
            {
                MousePlayer.IsMovementDisabled = true;
                MousePlayer.GetComponent<Collider>().isTrigger = false;
                _doKick = true;
            }

            // Move down under weight
            _fallSpeed = Mathf.Min(MaxAccel, _fallSpeed + (Accel * Time.deltaTime));
            MousePlayer.GetComponent<Rigidbody>().MovePosition(MousePlayer.transform.position + Vector3.down * _fallSpeed * Time.deltaTime);
        }
        else if (_doKick)
        {
            MousePlayer.GetComponent<Collider>().isTrigger = true;
            _fallSpeed = 0f;
            _doKick = false;

            _kickTween?.Stop();
            _kickTween = MousePlayer.transform.TweenY(MousePlayer.transform.position.y, MousePlayer.transform.position.y - KickbackDistance, KickbackDuration, KickbackAnimation.GetEasingFunction(), true, null, () => MousePlayer.IsMovementDisabled = false);
        }
    }

    private bool IsPlayerStandingOn()
    {
        return Vector3.Distance(MousePlayer.transform.position, GameManager.Instance.KeyboardPlayer.transform.position) < 1f &&
            GameManager.Instance.KeyboardPlayer.transform.position.y > MousePlayer.transform.position.y;
    }

    #endregion
}
