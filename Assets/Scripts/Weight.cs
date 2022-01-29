using System.Linq;
using UnityEngine;

public sealed class Weight : Transformation
{
    #region Fields

    private WeightedObject _weighingDownObj;

    private float _lockX;

    #endregion

    #region Properties

    public override Form TransformationForm => Form.Weight;

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
        _lockX = MousePlayer.transform.position.x;
    }

    protected override void OnAbilityDisabled()
    {
        if (_weighingDownObj != null)
        {
            // Stop controlling the weight
            _weighingDownObj.EndControl();
            _weighingDownObj = null;
        }
    }

    protected override void OnUpdate()
    {
        if (IsAbilityEnabled)
        {
            _weighingDownObj.ControlledMove(MousePlayer.GetComponent<Rigidbody>().velocity.y * 2.5f * Time.deltaTime);

            Vector3 pos = MousePlayer.transform.position;
            pos.x = _lockX;
            pos.y = _weighingDownObj.transform.position.y;
            MousePlayer.transform.position = pos;
        }
    }

    #endregion
}
