using UnityEngine;

public class PassThroughPlatform : MonoBehaviour
{
    [SerializeField] Collider toggleCollider = null;
    // Start is called before the first frame update
    void Start()
    {
        if (toggleCollider == null)
        {
            return;
        }

        toggleCollider.isTrigger = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("KeyboardPlayer"))
        {
            return;
        }

        if (other != toggleCollider)
            toggleCollider.enabled = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("KeyboardPlayer"))
        {
            return;
        }

        toggleCollider.enabled = true;
    }
}
