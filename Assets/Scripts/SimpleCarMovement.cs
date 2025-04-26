using UnityEngine;

public class SimpleCarMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 10f; // Araba h�z�
    public float turnSpeed = 50f; // Araba d�n�� h�z�

    private Rigidbody rb; // Araba Rigidbody'si

    private void Start()
    {
        // Rigidbody bile�enini al
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Araba hareketi
        MoveCar();
    }

    private void MoveCar()
    {
        // �leri ve geri hareket
        float moveInput = Input.GetAxis("Vertical"); // W/S veya Arrow Keys
        Vector3 moveDirection = transform.forward * moveInput * moveSpeed * Time.deltaTime;

        // Araba d�nd�rme
        float turnInput = Input.GetAxis("Horizontal"); // A/D veya Arrow Keys
        float turnAmount = turnInput * turnSpeed * Time.deltaTime;

        // Araba hareketi ve d�n���
        rb.MovePosition(rb.position + moveDirection);
        rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, turnAmount, 0f));
    }
}
