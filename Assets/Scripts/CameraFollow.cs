using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
    #region Fields

    private float _minY;

    #endregion

    #region Properties

    [field: SerializeField]
    public Transform Target { get; set; }

    [field: SerializeField]
    public float FollowSpeed { get; set; } = 10f;

    #endregion

    #region Methods

    private void Awake()
    {
        _minY = transform.position.y;
    }

    private void LateUpdate()
    {
        if (Target == null)
        {
            return;
        }

        Vector3 pos = transform.position;
        
        pos.x = Mathf.Lerp(pos.x, Target.position.x, Time.deltaTime * FollowSpeed);
        pos.y = Mathf.Lerp(pos.y, Target.position.y, Time.deltaTime * FollowSpeed);

        if (pos.y < _minY)
        {
            pos.y = _minY;
        }

        transform.position = pos;
    }

    #endregion
}
