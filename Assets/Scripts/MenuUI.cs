using DavidFDev.Audio;
using DavidFDev.Maths;
using DavidFDev.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public sealed class MenuUI : MonoBehaviour
{
    #region Static fields

    private static bool _init = true;

    #endregion

    #region Fields

    private bool _loading;

    #endregion

    #region Properties

    [field: SerializeField]
    public Image DarkImage { get; private set; }

    [field: SerializeField]
    public float FadeInDuration { get; private set; } = 0.8f;

    [field: SerializeField]
    public float FadeOutDuration { get; private set; } = 1f;

    [field: SerializeField, Space]
    public Transform TitleTransform { get; private set; }

    [field: SerializeField]
    public float WiggleFrequency { get; private set; } = 1f;

    [field: SerializeField]
    public float WiggleDeviance { get; private set; } = 25f;

    [field: SerializeField, Space]
    public AudioMixerGroup MusicGroup { get; private set; }

    #endregion

    #region Methods

    public void OnPlayButtonPressed()
    {
        if (_loading)
        {
            return;
        }

        StartCoroutine(LoadScene(2));
        _loading = true;
    }

    public void OnCreditsButtonPressed()
    {
        if (_loading)
        {
            return;
        }

        StartCoroutine(LoadScene(1));
        _loading = true;
    }

    public void OnQuitButtonPressed()
    {
        if (_loading)
        {
            return;
        }

        _loading = true;
        Application.Quit();
    }

    public void OnBackButtonPressed()
    {
        if (_loading)
        {
            return;
        }

        StartCoroutine(LoadScene(0));
        _loading = true;
    }

    public void OnOpenUrl(string url)
    {
        Application.OpenURL(url);
    }

    private void Start()
    {
        // INIT GAME
        if (_init && MusicGroup != null)
        {
            _init = false;

            Audio.MusicPlayback.Output = MusicGroup;
        }

        Cursor.visible = true;

        StartCoroutine(FadeIn());
    }

    private void Update()
    {
        Vector3 rot = TitleTransform.localEulerAngles;
        rot.z = MathsHelper.Pulse(Time.time, WiggleFrequency, 0f, WiggleDeviance * 2f) - WiggleDeviance;
        TitleTransform.localEulerAngles = rot;
    }

    private IEnumerator LoadScene(int buildIndex)
    {
        Audio.PlaySfx(GameManager.GetSfx("SFX_ButtonPress"));

        DarkImage.gameObject.SetActive(true);
        DarkImage.TweenAlpha(0f, 1f, FadeOutDuration, Ease.SineOut);
        yield return new WaitForSeconds(FadeOutDuration);
        SceneManager.LoadScene(buildIndex);
    }

    private IEnumerator FadeIn()
    {
        DarkImage.gameObject.SetActive(true);
        DarkImage.TweenAlpha(1f, 0f, FadeInDuration, Ease.SineIn);
        yield return new WaitForSeconds(FadeInDuration);
        DarkImage.gameObject.SetActive(false);
    }

    #endregion
}
