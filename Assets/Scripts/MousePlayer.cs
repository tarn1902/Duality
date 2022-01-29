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
    [SerializeField] float zposition;

    [SerializeField] private Transformation[] inputTransformations;
    private Dictionary<Transformation.Form, Transformation> transformations = new Dictionary<Transformation.Form, Transformation>();
    private Transformation currentTransformation;

    public void Interact()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Check for form-switcher
            FormSwitcher switcher;
            if ((switcher = Physics.OverlapSphere(transform.position, 2f).Where(x => x.GetComponent<FormSwitcher>()).Select(x => x.GetComponent<FormSwitcher>()).FirstOrDefault()) != null)
            {
                // Remove transformation if same
                if (switcher.SwitchTo == currentTransformation.TransformationForm)
                {
                    currentTransformation.DisableTransformation();
                    currentTransformation = null;
                    return;
                }

                // Disable current transformation
                if (currentTransformation != null)
                {
                    currentTransformation.DisableTransformation();
                }

                // Enable new transformation
                currentTransformation = transformations[switcher.SwitchTo];
                currentTransformation.EnableTransformation();

                return;
            }

            // Try to perform ability
            if (currentTransformation != null && !currentTransformation.LockAbilityChange)
            {
                currentTransformation.ToggleAbility();
            }
        }
    }

    public void Movement()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = transform.position.z - Camera.main.transform.position.z;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePosition);
        worldPos.z = zposition;
        rb.MovePosition(transform.position + (worldPos - transform.position) * Time.deltaTime * speed);
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Interact();
    }
}
