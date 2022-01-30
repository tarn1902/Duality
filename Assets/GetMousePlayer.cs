using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetMousePlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Update()
    {
        transform.position = GameManager.Instance.MousePlayer.transform.position;
    }
}
