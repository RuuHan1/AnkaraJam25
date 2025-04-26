using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;

public class ZombieFollow : MonoBehaviour
{
    [SerializeField] private Transform carTransform;
    [SerializeField] private float jumpForce = 30f;
    [SerializeField] private float linearJumpForece = 4f;

    [SerializeField] private float jumpRadius = 2f;
    [SerializeField] private LayerMask carMask, groundMask;
    [SerializeField] private NavMeshAgent agent;
    private bool isAgentEnabled = false;
    //patrolling
    [SerializeField] private Vector3 walkPoint;
    bool walkPointSet;
    
    [SerializeField] private float followRadius = 10f;
    //Attacking
    bool jumped;
    //States
    public float sightRange;
    public bool carInSightRange, carInJumpRange;
    [SerializeField] private Animator animator;
    private bool isStopped = false;
    private void Awake()
    {
        agent.enabled = true;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        //carTransform = GameObject.Find("Car").transform;
    }
    private void Start()
    {
        
    }
    private void Update()
    {
        carInSightRange = Physics.CheckSphere(transform.position, followRadius, carMask);
        carInJumpRange = Physics.CheckSphere(transform.position, jumpRadius, carMask);
        //if(!carInSightRange && !carInJumpRange && !isAgentEnabled) { animator.SetFloat("Speed", 0f); }
        if (carInSightRange && !carInJumpRange && !isAgentEnabled){ FollowCar(); }
        if (carInSightRange && carInJumpRange) {

            
            JumpAtCar(); }
        
    }

    private void Patrolling()
    {
        if (!walkPointSet) { SerchWalkPoint(); }
        if (walkPointSet) { agent.SetDestination(walkPoint);
        
        }

        Vector3 distaceToWalkPoint = transform.position - walkPoint;
        if(distaceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }
    private void SerchWalkPoint()
    {
        float RandomZ = Random.Range(-followRadius, followRadius);
        float RandomX = Random.Range(-followRadius, followRadius);
        
        walkPoint = new Vector3(transform.position.x +RandomX, transform.position.y, transform.position.z + RandomZ);
        if (Physics.Raycast(walkPoint, -transform.up, 2f, groundMask))
        {
            walkPointSet = true;
            
        }
    }
    private void FollowCar()
    {
        animator.SetFloat("Speed", 1f);
        agent.SetDestination(carTransform.position);
        
        if (!isStopped) 
        {
            StartCoroutine(IdleState());
        }

    }

    private void JumpAtCar()
    {
        

        // E�er daha �nce z�plamad�ysak, z�plama i�lemini ba�lat
        if (!jumped)
        {
            
            animator.SetBool("isStopped",true);
            // Araba pozisyonuna do�ru hareket etmeye ba�la
            agent.SetDestination(carTransform.position);
            transform.LookAt(carTransform);
            agent.isStopped = true;
            agent.enabled = false;
            jumped = true;

            // NavMeshAgent'� ge�ici olarak devre d��� b�rak
            

            Rigidbody rb = GetComponent<Rigidbody>();

            // Zombi ile araba aras�ndaki y�n vekt�r�n� hesapla
            Vector3 direction = (carTransform.position - transform.position).normalized;

            // Z�plama kuvvetini hesapla
              // Kuvveti art�rarak daha y�ksek z�plama sa�la
            Vector3 jumpDir = direction * linearJumpForece + Vector3.up * jumpForce; // Yatay ve yukar� kuvveti birle�tir

            // Kuvveti uygula
            rb.AddForce(jumpDir, ForceMode.Impulse);
            
        }
        Destroy(gameObject,10f);
        // Z�plama i�lemi tamamland�ktan sonra NavMeshAgent'i tekrar etkinle�tir
        //StartCoroutine(ReactivateNavMeshAgent());
    }

    public void DestroyZombie()
    {
        Destroy(gameObject);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, jumpRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, followRadius);
    }
    private bool IsGrounded()
    {
        // Zombiyi yerden kontrol et (�ok k���k bir mesafe ile)
        return Physics.Raycast(transform.position, Vector3.down, 1f);
    }
    private IEnumerator ReactivateNavMeshAgent()
    {
        // Zombi yere inene kadar bekle
        yield return new WaitForSeconds(1f); // Bu s�reyi animasyona g�re ayarlayabilirsin

        // NavMeshAgent'i tekrar etkinle�tir
        agent.enabled = true;
        agent.isStopped = false;
    }
    private IEnumerator IdleState()
    {
        isStopped = true;
        yield return new WaitForSeconds(8f);
        animator.SetFloat("Speed", 0f);
        isStopped = false;
    }
}
