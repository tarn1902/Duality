using System.Linq;
using UnityEngine;

public sealed class Weight : Transformation
{
    #region Fields

    private WeightedObject _weighingDownObj;

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
        if ((obj = Physics.OverlapSphere(transform.position, 2f).Where(x => x.GetComponent<WeightedObject>()).Select(x => x.GetComponent<WeightedObject>()).FirstOrDefault()) == null)
        {
            // Cannot find weight so cancel ability
            ToggleAbility();
            return;
        }

        // Begin controlling the weight
        _weighingDownObj = obj;
        _weighingDownObj.BeginControl();
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

    #endregion
}
