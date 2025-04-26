using UnityEngine;

public class BearFollow : MonoBehaviour
{
    [Header("Movement Settings")]
    
    public float turnSpeed = 3f; // Dönüþ hýzý
    public float followRadius = 10f; // Takip mesafesi
    private float currentSpeed = 0f; // Þu anki hýz, baþlangýçta sýfýr
    [SerializeField] private float acceleration = 2f; // Hýzlanma oraný
    [SerializeField] private float maxSpeed = 5f; // Maksimum hýz
    [Header("Target Settings")]
    public Transform carTransform; // Araba hedefi

    private Rigidbody rb; // Ayýnýn Rigidbody'si
    bool isCollide = false;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Ayý arabayý takip etsin
        if (!isCollide) { FollowCarIfInRange(); }

    }

    private void FollowCarIfInRange()
    {
        // Ayý ile araba arasýndaki mesafeyi hesapla
        float distanceToCar = Vector3.Distance(transform.position, carTransform.position);

        // Eðer araba takip mesafesindeyse, arabayý takip et
        if (distanceToCar <= followRadius)
        {
            MoveBearTowardsCar();
        }
    }

    private void MoveBearTowardsCar()
    {
        // Araba hedefiyle arasýndaki yönü hesapla
        Vector3 directionToCar = (carTransform.position - transform.position).normalized;

        // Hýzý artýr (acceleration) ve belirli bir maksimum hýza ulaþtýðýnda durdur
        currentSpeed = Mathf.Lerp(currentSpeed, maxSpeed, acceleration * Time.deltaTime); // Lerp ile hýzlanmayý yapýyoruz

        // Ayýyý hedefe doðru hareket ettir
        Vector3 moveDirection = directionToCar * currentSpeed * Time.deltaTime;

        // Hareketi uygula
        rb.MovePosition(transform.position + moveDirection);

        // Dönüþ
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
