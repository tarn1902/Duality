using UnityEngine;
using DavidFDev.Maths;

public sealed class Pendulum : MonoBehaviour
{
    #region Properties

    [field: SerializeField]
    public Transform Hand { get; private set; }

    [field: SerializeField]
    public float Frequency { get; private set; } = 1f;

    [field: SerializeField]
    public float Deviance { get; private set; } = 70f;

    #endregion

    #region Methods

    private void Update()
    {
        Vector3 euler = Hand.localEulerAngles;
        euler.z = MathsHelper.Pulse(Time.time, Frequency, 0f, Deviance * 2f) - Deviance;
        Hand.localEulerAngles = euler;
    }

    #endregion
}
