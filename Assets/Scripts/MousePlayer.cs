using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using DavidFDev.Tweening;
using DavidFDev.Audio;
using DavidFDev.Maths;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class MousePlayer : MonoBehaviour, IPlayer
{
    Rigidbody rb = null;
    [SerializeField] float speed = 5;
    [SerializeField] float zposition = 0;

    [SerializeField] private Transformation[] inputTransformations;
    private Dictionary<Transformation.Form, Transformation> transformations = new Dictionary<Transformation.Form, Transformation>();
    public Transformation CurrentTransformation { get; private set; }

    [SerializeField] private SpriteRenderer defaultRenderer;
    private Tween _untransformTween;
    private Tween _flipTween;

    private bool _inLava;
    
    public bool IsMovementDisabled { get; set; }

    public void Interact()
    {
        if (_inLava)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            // Check for form-switcher
            FormSwitcher switcher;
            if ((switcher = Physics.OverlapSphere(transform.position, 0.6f).Where(x => x.GetComponent<FormSwitcher>()).Select(x => x.GetComponent<FormSwitcher>()).FirstOrDefault()) != null)
            {
                // Remove transformation if same
                if (CurrentTransformation != null && switcher.SwitchTo == CurrentTransformation.TransformationForm)
                {
                    CurrentTransformation.DisableTransformation();
                    CurrentTransformation = null;
                    defaultRenderer.enabled = true;

                    // Animate
                    Vector3 scale = defaultRenderer.transform.localScale;
                    _untransformTween ??= defaultRenderer.transform.TweenScale(scale, scale * 1.25f, 0.3f, Ease.Spike, false);
                    _untransformTween.Stop();
                    _untransformTween.Start();

                    Audio.PlaySfx(GameManager.GetSfx("SFX_Untransform"));

                    return;
                }

                // Disable current transformation
                if (CurrentTransformation != null)
                {
                    CurrentTransformation.DisableTransformation();
                }

                if (!transformations.ContainsKey(switcher.SwitchTo))
                {
                    Debug.LogError($"{switcher.SwitchTo} not set up in {nameof(MousePlayer)} component.");
                    return;
                }

                // Enable new transformation
                CurrentTransformation = transformations[switcher.SwitchTo];
                CurrentTransformation.EnableTransformation();

                Audio.PlaySfx(GameManager.GetSfx("SFX_Transform"));

                defaultRenderer.enabled = false;
                _flipTween?.Stop();

                return;
            }

            // Try to perform ability
            if (CurrentTransformation != null && !CurrentTransformation.LockAbilityChange)
            {
                CurrentTransformation.ToggleAbility();
                return;
            }

            Debug.Log("Nothing can be done...");
        }
    }

    public void Movement()
    {
        if (IsMovementDisabled)
        {
            return;
        }

        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = transform.position.z - Camera.main.transform.position.z;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePosition);
        worldPos.z = zposition;
        rb.MovePosition(transform.position + (worldPos - transform.position) * Time.deltaTime * speed * (_inLava ? 0.2f : 1f));

        // Rotate animation
        UpdateRotateAnimation();

        // Flip sprite
        UpdateSpriteScale();
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Set up transformations dictionary
        foreach (Transformation t in inputTransformations)
        {
            if (t == null)
            {
                continue;
            }

            transformations[t.TransformationForm] = t;
            t.gameObject.SetActive(false);
        }

        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        Interact();
    }

    void FixedUpdate()
    {
        Movement();
    }

    private void UpdateRotateAnimation()
    {
        // Rotate a little bit along with the horizontal movement speed
        Transform t = CurrentTransformation == null ? defaultRenderer.transform : CurrentTransformation.transform;
        Vector3 euler = t.localEulerAngles;
        euler.z = MathsHelper.Map01(Mathf.Abs(rb.velocity.x), 0f, speed * 2f) * 20f * -Mathf.Sign(rb.velocity.x);
        t.localEulerAngles = euler;
    }

    private void UpdateSpriteScale()
    {
        Transform t = CurrentTransformation == null ? defaultRenderer.transform : CurrentTransformation.transform;
        if (t == defaultRenderer.transform)
        {
            if (rb.velocity.x > 0.01f && (_flipTween == null || (float)_flipTween.EndValue != -0.15f))
            {
                // -1
                if (_untransformTween == null || !_untransformTween.IsActive)
                {
                    _flipTween?.Stop();
                    _flipTween = t.TweenScaleX(t.localScale.x, -0.15f, 0.2f, Ease.Linear);
                }
                else
                {
                    Vector3 scale = t.localScale;
                    scale.x = -0.15f;
                    t.localScale = scale;
                }
            }
            else if (rb.velocity.x < -0.01f && (_flipTween == null || (float)_flipTween.EndValue != 0.15f))
            {
                // 1
                if (_untransformTween == null || !_untransformTween.IsActive)
                {
                    _flipTween?.Stop();
                    _flipTween = t.TweenScaleX(t.localScale.x, 0.15f, 0.2f, Ease.Linear);
                }
                else
                {
                    Vector3 scale = t.localScale;
                    scale.x = 0.15f;
                    t.localScale = scale;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hazard") && CurrentTransformation == null)
        {
            StartCoroutine(LavaEvent());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Hazard"))
        {
            defaultRenderer.color = Color.white;
            _inLava = false;
        }
    }

    private IEnumerator LavaEvent()
    {
        _inLava = true;
        IsMovementDisabled = true;

        Audio.PlaySfx(GameManager.GetSfx("SFX_MouseScream"));

        defaultRenderer.TweenColour(Color.white, new Color(1f, 155f / 255f, 128f / 255f), 1.4f, Ease.SineOut);
        transform.TweenY(transform.position.y, transform.position.y - 0.6f, 3.4f, Ease.QuadIn);

        float wait = 3.4f;
        while (wait > 0f)
        {
            Vector3 r = defaultRenderer.transform.localEulerAngles;
            r.z = MathsHelper.Pulse(Time.time, 0.7f, 0f, 20f) - 10f;
            defaultRenderer.transform.localEulerAngles = r;
            yield return null;
            wait -= Time.deltaTime;
        }

        if (!_inLava)
        {
            defaultRenderer.color = Color.white;
        }

        _inLava = false;
        IsMovementDisabled = false;
    }
}
