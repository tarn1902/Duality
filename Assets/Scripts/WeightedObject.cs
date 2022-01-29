using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public sealed class WeightedObject : MonoBehaviour
{
    #region Fields

    private Rigidbody rb;

    private float initY;

    #endregion

    #region Properties

    [field: SerializeField]
    public WeightedObject Linked { get; private set; }

    [field: SerializeField]
    public float ComeToRestSpeed { get; private set; } = 4f;

    public bool BeingControlled { get; private set; }

    #endregion

    #region Methods

    public void BeginControl()
    {
        BeingControlled = true;
        Linked.BeingControlled = true;
    }

    public void EndControl()
    {
        BeingControlled = false;
        Linked.BeingControlled = false;
    }

    public void ControlledMove(float delta)
    {
        if (!BeingControlled)
        {
            return;
        }

        rb.MovePosition(transform.position + new Vector3(0f, delta, 0f));
        Linked.rb.MovePosition(transform.position - new Vector3(0f, delta, 0f));
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        initY = transform.position.y;
    }

    private void Update()
    {
        if (BeingControlled)
        {
            return;
        }

        ComeToRest();
    }

    private void ComeToRest()
    {
        if (transform.position.y != initY)
        {
            Vector3 pos = transform.position;
            pos.y = DavidFDev.Maths.MathsHelper.Approach(pos.y, initY, ComeToRestSpeed * Time.deltaTime);
            rb.MovePosition(pos);
        }
    }

    #endregion
}
