using UnityEngine;

public class SimpleCarMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 10f; // Araba hýzý
    public float turnSpeed = 50f; // Araba dönüþ hýzý

    private Rigidbody rb; // Araba Rigidbody'si

    private void Start()
    {
        // Rigidbody bileþenini al
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Araba hareketi
        MoveCar();
    }

    private void MoveCar()
    {
        // Ýleri ve geri hareket
        float moveInput = Input.GetAxis("Vertical"); // W/S veya Arrow Keys
        Vector3 moveDirection = transform.forward * moveInput * moveSpeed * Time.deltaTime;

        // Araba döndürme
        float turnInput = Input.GetAxis("Horizontal"); // A/D veya Arrow Keys
        float turnAmount = turnInput * turnSpeed * Time.deltaTime;

        // Araba hareketi ve dönüþü
        rb.MovePosition(rb.position + moveDirection);
        rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, turnAmount, 0f));
    }
}
