using UnityEngine;

public class FruitEnemy : MonoBehaviour
{
    // Movement and basic settings
    public float moveSpeed = 3f;
    public float knockbackForce = 2000f;
    public float spinForce = 2000f;
    public float scale = 4f;
    public float stopDistance = 8f;

    // Sound settings
    [Header("Sound")]
    [Tooltip("Sound played when fruit is hit")]
    public AudioClip hitSound;
    [Range(0f, 1f)]
    public float hitSoundVolume = 0.7f;
    [Tooltip("Random pitch variation for hit sound")]
    [Range(0f, 0.5f)]
    public float hitSoundPitchVariation = 0.1f;

    // Floating behavior
    public float hoverHeight = 2f;
    public float bobSpeed = 2f;
    public float bobAmount = 0.5f;
    public float minHeight = 2f;
    public float bounceForce = 500f;

    // Private variables
    private Transform player;
    private Rigidbody rb;
    private bool isKnockedBack = false;
    private CharacterController playerController;
    private float spawnTime;  // Changed from startTime to spawnTime
    private Vector3 startPosition;
    private AudioSource audioSource;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerController = player.GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        
        // Set up audio
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f; // 2D sound
        
        // Physics setup
        rb.useGravity = true;
        rb.mass = 0.8f;
        rb.linearDamping = 0.1f;
        rb.angularDamping = 0.1f;
        rb.constraints = RigidbodyConstraints.None;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        
        Physics.IgnoreCollision(GetComponent<Collider>(), playerController);
        transform.localScale = Vector3.one * scale;
        
        spawnTime = Time.time;  // Changed from startTime to spawnTime
        startPosition = transform.position;

        // Set up directional arrow
        GameObject arrow = GameObject.Find("DirectionalArrow");
        if (arrow != null)
        {
            arrow.GetComponent<DirectionalArrow>().SetTarget(transform);
        }
    }

    void FixedUpdate()
    {
        // Anti-gravity check
        if (transform.position.y < minHeight)
        {
            Vector3 bounceVelocity = rb.linearVelocity;
            bounceVelocity.y = bounceForce * Time.fixedDeltaTime;
            rb.linearVelocity = bounceVelocity;
        }

        if (!isKnockedBack)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            
            if (distanceToPlayer > stopDistance)
            {
                // Bobbing motion
                float bobOffset = Mathf.Sin((Time.time - spawnTime) * bobSpeed) * bobAmount;  // Updated to use spawnTime
                Vector3 targetPos = transform.position;
                targetPos.y = startPosition.y + hoverHeight + bobOffset;

                // Movement
                Vector3 directionToPlayer = (player.position - transform.position);
                directionToPlayer.y = 0;
                directionToPlayer = directionToPlayer.normalized;

                Vector3 moveTarget = transform.position + directionToPlayer * moveSpeed * Time.fixedDeltaTime;
                moveTarget.y = targetPos.y;
                rb.MovePosition(moveTarget);

                // Rotation to face player
                if (directionToPlayer != Vector3.zero)
                {
                    transform.rotation = Quaternion.Slerp(
                        transform.rotation,
                        Quaternion.LookRotation(directionToPlayer),
                        Time.deltaTime * 5f
                    );
                }
            }
        }
        else
        {
            // Keep floating when knocked back
            if (rb.linearVelocity.y < -2f)
            {
                rb.AddForce(Vector3.up * bounceForce * 0.5f * Time.fixedDeltaTime, ForceMode.Impulse);
            }
        }
    }

    public void TakeDamage(float damage, Vector3 hitDirection)
    {
        if (!isKnockedBack)
        {
            // Calculate lifetime for scoring
            float lifetime = Time.time - spawnTime;
            
            // Update score
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.BananaHit(transform.position, lifetime);
            }

            // Play hit sound with random pitch variation
            if (hitSound != null)
            {
                audioSource.clip = hitSound;
                audioSource.volume = hitSoundVolume;
                audioSource.pitch = 1f + Random.Range(-hitSoundPitchVariation, hitSoundPitchVariation);
                audioSource.Play();
            }

            // Launch with upward force
            Vector3 launchDirection = (hitDirection + Vector3.up * 1.5f).normalized;
            rb.AddForce(launchDirection * knockbackForce, ForceMode.Impulse);
            
            // Add spin
            rb.AddTorque(
                Random.Range(-spinForce, spinForce),
                Random.Range(-spinForce, spinForce),
                Random.Range(-spinForce, spinForce)
            );

            isKnockedBack = true;
            Destroy(gameObject, 10f);
        }
    }
}