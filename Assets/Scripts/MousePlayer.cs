using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DavidFDev.Tweening;
using DavidFDev.Audio;
using DavidFDev.Maths;

[RequireComponent(typeof(Rigidbody))]
public class MousePlayer : MonoBehaviour, IPlayer
{
    Rigidbody rb = null;
    [SerializeField] float speed = 5;
    [SerializeField] float zposition = 0;

    [SerializeField] private Transformation[] inputTransformations;
    private Dictionary<Transformation.Form, Transformation> transformations = new Dictionary<Transformation.Form, Transformation>();
    private Transformation currentTransformation;

    [SerializeField] private SpriteRenderer defaultRenderer;
    private Tween _untransformTween;
    private Tween _flipTween;
    
    public bool IsMovementDisabled { get; set; }

    public void Interact()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Check for form-switcher
            FormSwitcher switcher;
            if ((switcher = Physics.OverlapSphere(transform.position, 0.6f).Where(x => x.GetComponent<FormSwitcher>()).Select(x => x.GetComponent<FormSwitcher>()).FirstOrDefault()) != null)
            {
                // Remove transformation if same
                if (currentTransformation != null && switcher.SwitchTo == currentTransformation.TransformationForm)
                {
                    currentTransformation.DisableTransformation();
                    currentTransformation = null;
                    defaultRenderer.enabled = true;

                    // Animate
                    Vector3 scale = defaultRenderer.transform.localScale;
                    _untransformTween ??= defaultRenderer.transform.TweenScale(scale, scale * 1.25f, 0.3f, Ease.Spike, false);
                    _untransformTween.Stop();
                    _untransformTween.Start();

                    return;
                }

                // Disable current transformation
                if (currentTransformation != null)
                {
                    currentTransformation.DisableTransformation();
                }

                if (!transformations.ContainsKey(switcher.SwitchTo))
                {
                    Debug.LogError($"{switcher.SwitchTo} not set up in {nameof(MousePlayer)} component.");
                    return;
                }

                // Enable new transformation
                currentTransformation = transformations[switcher.SwitchTo];
                currentTransformation.EnableTransformation();

                defaultRenderer.enabled = false;
                _flipTween?.Stop();

                return;
            }

            // Try to perform ability
            if (currentTransformation != null && !currentTransformation.LockAbilityChange)
            {
                currentTransformation.ToggleAbility();
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
        rb.MovePosition(transform.position + (worldPos - transform.position) * Time.deltaTime * speed);

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
        Transform t = currentTransformation == null ? defaultRenderer.transform : currentTransformation.transform;
        Vector3 euler = t.localEulerAngles;
        euler.z = MathsHelper.Map01(Mathf.Abs(rb.velocity.x), 0f, speed * 2f) * 20f * -Mathf.Sign(rb.velocity.x);
        t.localEulerAngles = euler;
    }

    private void UpdateSpriteScale()
    {
        Transform t = currentTransformation == null ? defaultRenderer.transform : currentTransformation.transform;
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
}
