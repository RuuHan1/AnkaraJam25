using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{
    [SerializeField]
    private Bar fuelBar;
    public bool auto = false;
    [Header("Car Settings")]
    public float accelerationFactor = 30f;
    public float turnFactor = 3.5f;
    public float driftFactor = 0.5f;
    public float maxSpeed = 20f;
    public float maxAngularSpeed = 50f;
    public float maxAngle = 120f;

    float accelerationInput = 1;
    float steeringInput;

    int steerState = 0;


    float velocityVsForward;
    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        rb.angularDamping = 0f;
        rb.linearDamping = 0f;
    }

    private void Update()
    {
        accelerationInput = auto ? 1 : Input.GetAxis("Vertical");
        steeringInput = auto ? 0 : Input.GetAxis("Horizontal");

        if (steeringInput > 0.01f)
            steerState = 1;
        else if (steeringInput < -0.01f)
            steerState = -1;

        if (rb.linearVelocity.magnitude > 0.1f && fuelBar != null)
        {
            fuelBar.SetValue(fuelBar.CurrentValue - Time.deltaTime * 0.1f);
        }
    }

    private void FixedUpdate()
    {
        ApplyEngineForce();
        KillOrthogonalVelocity();
        ApplySteering();
    }

    private void ApplyEngineForce()
    {
        velocityVsForward = Vector3.Dot(rb.linearVelocity.normalized, transform.forward);

        if (velocityVsForward > maxSpeed && accelerationInput > 0)
            return;
        if (velocityVsForward < -maxSpeed * 0.5f && accelerationInput < 0)
            return;
        if (rb.linearVelocity.sqrMagnitude > maxSpeed * maxSpeed && accelerationInput > 0)
            return;

        if (Mathf.Approximately(accelerationInput, 0f))
            rb.linearDamping = Mathf.Lerp(rb.linearDamping, 3f, Time.fixedDeltaTime * 3f);
        else
            rb.linearDamping = 0f;

        Vector3 force = transform.forward * accelerationInput * accelerationFactor;
        rb.AddForce(force, ForceMode.Force);
    }

    private void KillOrthogonalVelocity()
    {
        Vector3 forwardVel = transform.forward * Vector3.Dot(rb.linearVelocity, transform.forward);
        Vector3 rightVel = transform.right * Vector3.Dot(rb.linearVelocity, transform.right);
        rb.linearVelocity = forwardVel + rightVel * driftFactor;
    }

    private void ApplySteering()
    {
        float speedFactor = Mathf.Clamp01(rb.linearVelocity.magnitude / maxSpeed);

        if (steerState != 0)
        {
            float torqueAmount = steerState * turnFactor * speedFactor;
            rb.AddTorque(transform.up * torqueAmount, ForceMode.Acceleration);
        }

        Vector3 angVel = rb.angularVelocity;
        float maxAv = maxAngularSpeed * speedFactor;
        angVel.y = Mathf.Clamp(angVel.y, -maxAv, maxAv);
        rb.angularVelocity = angVel;
    }
}
