using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetMousePlayer : MonoBehaviour
{
    public float offsetZ = 5;
    // Start is called before the first frame update
    void Update()
    {
        Vector3 vec = GameManager.Instance.MousePlayer.transform.position;
        vec.z -= offsetZ;
        transform.position = vec;
    }
}
