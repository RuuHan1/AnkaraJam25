using UnityEngine;

public class BearFollow : MonoBehaviour
{
    [Header("Movement Settings")]
    
    public float turnSpeed = 3f; // D�n�� h�z�
    public float followRadius = 10f; // Takip mesafesi
    private float currentSpeed = 0f; // �u anki h�z, ba�lang��ta s�f�r
    [SerializeField] private float acceleration = 2f; // H�zlanma oran�
    [SerializeField] private float maxSpeed = 5f; // Maksimum h�z
    [Header("Target Settings")]
    public Transform carTransform; // Araba hedefi

    private Rigidbody rb; // Ay�n�n Rigidbody'si
    bool isCollide = false;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Ay� arabay� takip etsin
        if (!isCollide) { FollowCarIfInRange(); }

    }

    private void FollowCarIfInRange()
    {
        // Ay� ile araba aras�ndaki mesafeyi hesapla
        float distanceToCar = Vector3.Distance(transform.position, carTransform.position);

        // E�er araba takip mesafesindeyse, arabay� takip et
        if (distanceToCar <= followRadius)
        {
            MoveBearTowardsCar();
        }
    }

    private void MoveBearTowardsCar()
    {
        // Araba hedefiyle aras�ndaki y�n� hesapla
        Vector3 directionToCar = (carTransform.position - transform.position).normalized;

        // H�z� art�r (acceleration) ve belirli bir maksimum h�za ula�t���nda durdur
        currentSpeed = Mathf.Lerp(currentSpeed, maxSpeed, acceleration * Time.deltaTime); // Lerp ile h�zlanmay� yap�yoruz

        // Ay�y� hedefe do�ru hareket ettir
        Vector3 moveDirection = directionToCar * currentSpeed * Time.deltaTime;

        // Hareketi uygula
        rb.MovePosition(transform.position + moveDirection);

        // D�n��
        Quaternion targetRotation = Quaternion.LookRotation(directionToCar);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position,followRadius);
    }
    private void OnCollisionEnter(Collision collision)
    {
        //Car carScript = collision.gameObject.GetComponent<Car>();
        if (collision.gameObject.CompareTag("Car"))
        {
            isCollide = true;
            Destroy(gameObject, 3f);
        }
    }
}
