using UnityEngine;

[RequireComponent(typeof(MousePlayer))]
public sealed class MouseTransformer : MonoBehaviour
{
    #region Properties

    public Form CurrentForm { get; private set; } = Form.None;

    [field: SerializeField]
    private GameObject NoneObj { get; set; }

    [field: SerializeField]
    private GameObject PlatformObj { get; set; }

    #endregion

    #region Methods

    public void SetForm(Form form)
    {
        if (CurrentForm == form)
        {
            return;
        }

        GetObj(CurrentForm).gameObject.SetActive(false);
        CurrentForm = form;
        GetObj(CurrentForm).gameObject.SetActive(true);
        Debug.Log($"Changed form to {CurrentForm}.");
    }

    private GameObject GetObj(Form form)
    {
        return form switch
        {
            Form.None => NoneObj,
            Form.Platform => PlatformObj,
            _ => throw new System.NotImplementedException(),
        };
    }

    #endregion

    #region Nested types

    public enum Form
    {
        None = 0,
        Platform = 1
    }

    #endregion
}
