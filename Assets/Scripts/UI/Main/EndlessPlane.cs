using UnityEngine;

public class EndlessPlane : MonoBehaviour
{
    [SerializeField]
    private GameObject next;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Body"))
        {
            next.transform.position = transform.position + new Vector3(0, 0, 250);
        }
    }
}
