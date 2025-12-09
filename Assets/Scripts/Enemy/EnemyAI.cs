using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyAI : MonoBehaviour
{
    [Header("Movement")]
    public float patrolSpeed = 2f;
    public float chaseSpeed = 4f;

    public Transform[] patrolPoints;
    public Transform player;

    [Header("Vision")]
    public float viewDistance = 7f;
    public float viewAngle = 60f;
    public VisionConeFOV vision;

    [Header("Hearing")]
    public float hearingMultiplier = 1f;
    public HearingCheck hearing; 
    [SerializeField] private NoiseEmitter playerNoise;

    [HideInInspector] public int patrolIndex = 0;

    private EnemyState currentState;
    private Rigidbody2D rb;
    

    public Rigidbody2D RB => rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        ChangeState(new PatrolState(this));
        if (playerNoise != null)
            playerNoise.OnNoiseChanged += OnPlayerNoise;
    }

    void OnDestroy()
    {
        if (playerNoise != null)
            playerNoise.OnNoiseChanged -= OnPlayerNoise;
    }

    private void Update()
    {
        currentState?.Update();
        vision.SetOrigin(transform.position + Vector3.up * 0.5f);
    }

    private void FixedUpdate()
    {
        currentState?.FixedUpdate();
    }

    public void ChangeState(EnemyState next)
    {
        currentState?.Exit();
        currentState = next;
        currentState.Enter();
    }

    public bool CanSeePlayer()
    {
        if (player == null) return false;

        Vector2 dir = player.position - transform.position;

        if (dir.magnitude > viewDistance)
            return false;

        float angle = Vector2.Angle(transform.right, dir.normalized);
        if (angle > viewAngle * 0.5f)
            return false;

        // Raycast 2D
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir.normalized, dir.magnitude);
        if (hit.collider == null)
            return false;

        if (hit.collider.transform != player)
            return false;
        Debug.Log("See you");
        return true;
    }

    public bool CanHearPlayer()
    {
        NoiseEmitter noise = player.GetComponent<NoiseEmitter>();
        if (noise == null) return false;

        float dist = Vector2.Distance(transform.position, player.position);

        return dist <= noise.CurrentRadius * hearingMultiplier;
       
    }

    void OnPlayerNoise(NoiseEventType type, float radius)
    {
        switch (type)
        {
            case NoiseEventType.Started:
            case NoiseEventType.Increased:
                {
                    Debug.Log("Hear you");
                    Vector3 pos = player.position;
                    ChangeState(new InvestigateState(this, pos));
                }
                break;

            case NoiseEventType.Decreased:
                {
                    // можно игнорировать
                }
                break;

            case NoiseEventType.Stopped:
                {
                    // вернуть врага назад
                }
                break;
        }
    }

    public void MoveTowards(Vector3 target, float speed)
    {
        Vector2 dir = (target - transform.position).normalized;
        Vector2 newPos = rb.position + dir * speed * Time.fixedDeltaTime;
        rb.MovePosition(newPos);
    }

    public bool IsAtPoint(Vector3 point, float tolerance = 0.1f)
    {
        return Vector2.Distance(transform.position, point) <= tolerance;
    }
}
