using UnityEngine;

public class ParticleController : MonoBehaviour
{
    //@formatter:off
    [Header("Movement Particle")]
    [SerializeField] private ParticleSystem movementParticle;
    [SerializeField] [Range(0, 10)] private int occurAfterVelocity;
    [SerializeField] [Range(0, 0.2f)] private float dustFormationPeriod;
    [SerializeField] private Rigidbody2D playerRb;

    [Header("")]
    [SerializeField] private ParticleSystem touchParticle;
    [SerializeField] private ParticleSystem fallParticle;
    
    private float counter;
    private bool isOnGround;
    
    //@formatter:on
    private void Start()
    {
        touchParticle.transform.parent = null;
    }

    private void Update()
    {
        counter += Time.deltaTime;

        if (isOnGround && Mathf.Abs(playerRb.velocity.x) > occurAfterVelocity)
            if (counter > dustFormationPeriod)
            {
                movementParticle.Play();
                counter = 0;
            }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            isOnGround = true;
            fallParticle.Play();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ground")) isOnGround = false;
    }

    public void PlayTouchParticle(Vector2 pos)
    {
        touchParticle.transform.position = pos;
        touchParticle.Play();
    }
}