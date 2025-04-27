using UnityEngine;

public class PenguenFollow : MonoBehaviour
{
    [SerializeField] private Transform carTransform;
    [SerializeField] private float moveSpeed =5f;
    [SerializeField] private float turnSpeed = 3f;
    [SerializeField] private float linearJumpForece = 4f;
    [SerializeField] private float followRadius = 10f;
    [SerializeField] private float dashRadius = 2f;
    private Rigidbody rb;
    private bool jumped;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
         
    }
    private void Update()
    {
        if (!jumped) FollowCarIfInRange();

    }
    private void FollowCarIfInRange()
    {
        
        float distanceToCar = Vector3.Distance(transform.position, carTransform.position);

        if (distanceToCar <=dashRadius)
        {
            DashAtCar();
        }
        if (distanceToCar <= followRadius)
        {
            MovePenguenTowardsCar();
        }
    }
    private void MovePenguenTowardsCar()
    {
        // Araba hedefiyle aras�ndaki y�n� hesapla
        Vector3 directionToCar = (carTransform.position - transform.position).normalized;

        
        Vector3 moveDirection = directionToCar * moveSpeed * Time.deltaTime;

        // Hareketi uygula
        rb.MovePosition(transform.position + moveDirection);

        // D�n��
        Quaternion targetRotation = Quaternion.LookRotation(directionToCar);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);

    }

    private void DashAtCar()
    {


        // E�er daha �nce z�plamad�ysak, z�plama i�lemini ba�lat
        if (!jumped)
        {
            // Araba pozisyonuna do�ru hareket etmeye ba�la
           
            jumped = true;

            

            Rigidbody rb = GetComponent<Rigidbody>();

            // Zombi ile araba aras�ndaki y�n vekt�r�n� hesapla
            Vector3 direction = (carTransform.position - transform.position).normalized;

            // Z�plama kuvvetini hesapla
            // Kuvveti art�rarak daha y�ksek z�plama sa�la


            // Kuvveti uygula
            //transform.right = direction;
            rb.AddForce(direction*linearJumpForece, ForceMode.Impulse);

        }
        Destroy(gameObject, 5f);
        
        //StartCoroutine(ReactivateNavMeshAgent());
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, dashRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, followRadius);
    }
}
