using UnityEngine;

public class Sil : MonoBehaviour
{
    public float customGravity = -20f;
    Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        if (rb.linearVelocity.y < 0)
        {
            rb.AddForce(Vector3.up * customGravity, ForceMode.Acceleration);
        }
    }
}
