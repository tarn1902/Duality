using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class MenuUI : MonoBehaviour
{
    #region Fields

    private bool _loading;

    #endregion

    #region Methods

    public void OnPlayButtonPressed()
    {
        if (_loading)
        {
            return;
        }

        SceneManager.LoadScene(1);
        _loading = true;
    }

    #endregion
}
