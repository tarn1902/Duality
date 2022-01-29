using UnityEngine;

[RequireComponent(typeof(Collider))]
public sealed class FormSwitcher : MonoBehaviour
{
    #region Properties

    [field: SerializeField]
    public Transformation.Form SwitchTo { get; private set; }

    #endregion
}
