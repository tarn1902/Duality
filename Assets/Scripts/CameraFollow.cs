using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
    #region Properties

    [field: SerializeField]
    public Transform Target { get; set; }

    [field: SerializeField]
    public float FollowSpeed { get; set; } = 10f;

    #endregion

    #region Methods

    private void LateUpdate()
    {
        if (Target == null)
        {
            return;
        }

        Vector3 pos = transform.position;
        pos.x = Mathf.Lerp(pos.x, Target.position.x, Time.deltaTime * FollowSpeed);
        transform.position = pos;
    }

    #endregion
}
