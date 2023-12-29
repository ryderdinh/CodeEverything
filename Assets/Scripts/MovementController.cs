using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{
    [SerializeField] private int speed;
    [SerializeField] [Range(1, 10)] private float acceleration;

    public LayerMask wallLayer;
    public Transform wallCheckpoint;
    private bool btnPressed;
    private bool isWallTouch;
    private Rigidbody2D rb;
    private float speedMultiplier;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        UpdateSpeedMultiplier();
        var targetSpeed = speed * speedMultiplier;
        rb.velocity = new Vector2(targetSpeed, rb.velocity.y);

        isWallTouch = Physics2D.OverlapBox(wallCheckpoint.position, new Vector2(0.06f, 1.61f), 0, wallLayer);

        if (isWallTouch) Flip();
    }

    public void Flip()
    {
        transform.Rotate(0, 180, 0);
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