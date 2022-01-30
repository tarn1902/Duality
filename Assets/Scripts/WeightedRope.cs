using UnityEngine;

public class WeightedRope : MonoBehaviour
{
    #region Properties

    [field: SerializeField]
    public Transform Anchor { get; private set; }
    
    [field: SerializeField]
    public Transform Scaler { get; private set; }

    [field: SerializeField]
    public Transform EndConnectedTo { get; private set; }

    [field: SerializeField]
    public float UnitScale { get; private set; } = 1f;

    public float Scale
    {
        get => Scaler.localScale.y;
        set => Scaler.localScale = new Vector3(Scaler.localScale.x, value, Scaler.localScale.z);
    }

    #endregion

    private void Update()
    {
        if (EndConnectedTo == null)
        {
            return;
        }

        // Stop if above
        if (EndConnectedTo.position.y > Anchor.position.y)
        {
            Scale = 0f;
            return;
        }

        // Scale the rope so that it connects to the center of the connected object
        float dist = Mathf.Abs(Anchor.position.y - EndConnectedTo.position.y);
        Scale = dist * 0.5f * UnitScale;
    }
}
