using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{
    [SerializeField] [Range(1, 10)] private float acceleration;
    [SerializeField] private int speed;
    public Transform wallCheckpoint;

    public LayerMask wallLayer;
    private bool btnPressed;
    private bool isWallTouch;
    private Rigidbody2D rb;

    private Vector2 relativeTransform;
    private float speedMultiplier;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        UpdateRelativeTransform();
    }


    private void FixedUpdate()
    {
        UpdateSpeedMultiplier();
        var targetSpeed = speed * speedMultiplier * relativeTransform.x;
        rb.velocity = new Vector2(targetSpeed, rb.velocity.y);

        isWallTouch = Physics2D.OverlapBox(wallCheckpoint.position, new Vector2(0.06f, 0.7f), 0, wallLayer);

        if (isWallTouch) Flip();
    }

    // private void OnDrawGizmos()
    // {
    //     Gizmos.DrawCube(wallCheckpoint.position, new Vector2(0.04f, f1));
    // }

    public void Flip()
    {
        transform.Rotate(0, 180, 0);
        UpdateRelativeTransform();
    }

    private void UpdateRelativeTransform()
    {
        relativeTransform = transform.InverseTransformVector(Vector3.one);
    }

    public void Move(InputAction.CallbackContext value)
    {
        if (value.started)
            btnPressed = true;
        else if (value.canceled) btnPressed = false;
    }

    private void UpdateSpeedMultiplier()
    {
        if (btnPressed && speedMultiplier < 1)
        {
            speedMultiplier += Time.deltaTime * acceleration;
        }
        else if (!btnPressed && speedMultiplier > 0)
        {
            speedMultiplier -= Time.deltaTime * acceleration;
            if (speedMultiplier < 0) speedMultiplier = 0;
        }
    }
}