using DavidFDev.Audio;
using System.Linq;
using UnityEngine;

public sealed class Weight : Transformation
{
    #region Fields

    private WeightedObject _weighingDownObj;

    #endregion

    #region Properties

    public override Form TransformationForm => Form.Weight;

    [field: SerializeField]
    public float WeightMoveSpeed { get; private set; } = 10f;

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
        WeightedObject obj;
        if ((obj = Physics.OverlapSphere(MousePlayer.transform.position, 2f).Where(x => x.GetComponent<WeightedObject>()).Select(x => x.GetComponent<WeightedObject>()).FirstOrDefault()) == null)
        {
            // Cannot find weight so cancel ability
            ToggleAbility();
            return;
        }

        // Begin controlling the weight
        _weighingDownObj = obj;
        _weighingDownObj.BeginControl();

        Vector3 pos = _weighingDownObj.transform.position;
        pos.z = MousePlayer.transform.position.z;
        MousePlayer.transform.position = pos;
        MousePlayer.IsMovementDisabled = true;

        Audio.PlaySfx(GameManager.GetSfx("SFX_WeightAdd"));
    }

    protected override void OnAbilityDisabled()
    {
        if (_weighingDownObj != null)
        {
            // Stop controlling the weight
            _weighingDownObj.EndControl();
            _weighingDownObj = null;
            MousePlayer.IsMovementDisabled = false;

            Audio.PlaySfx(GameManager.GetSfx("SFX_WeightRemove"));
        }
    }

    protected override void OnUpdate()
    {
        if (IsAbilityEnabled)
        {
            _weighingDownObj.ControlledMove(-WeightMoveSpeed * Time.deltaTime);

            Vector3 pos = MousePlayer.transform.position;
            pos.y = _weighingDownObj.transform.position.y;
            MousePlayer.transform.position = pos;
        }
    }

    #endregion
}
