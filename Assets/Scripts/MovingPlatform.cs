using System.Collections;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed;
    public GameObject ways;
    public Transform[] wayPoints;
    public float waitDuration;
    private Vector3 moveDirection;
    private MovementController movementController;
    private Rigidbody2D playerRb;
    private int pointIndex, pointCount, direction = 1;
    private Rigidbody2D rb;
    private Vector3 targetPos;


    private void Awake()
    {
        movementController = GameObject.FindGameObjectWithTag("Player").GetComponent<MovementController>();
        rb = GetComponent<Rigidbody2D>();
        playerRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();

        wayPoints = new Transform[ways.transform.childCount];
        for (var i = 0; i < ways.gameObject.transform.childCount; i++)
            wayPoints[i] = ways.transform.GetChild(i).gameObject.transform;
    }

    private void Start()
    {
        pointIndex = 1;
        pointCount = wayPoints.Length;
        targetPos = wayPoints[1].transform.position;
        DirectionCalculate();
    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, targetPos) < 0.05f) NextPoint();
    }

    private void FixedUpdate()
    {
        rb.velocity = moveDirection * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            movementController.isOnPlatform = true;
            movementController.platformRb = rb;
            playerRb.gravityScale *= 50;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            movementController.isOnPlatform = false;
            playerRb.gravityScale /= 50;
        }
    }

    private void DirectionCalculate()
    {
        moveDirection = (targetPos - transform.position).normalized;
    }

    private void NextPoint()
    {
        transform.position = targetPos;
        moveDirection = Vector3.zero;

        if (pointIndex == pointCount - 1) direction = -1;

        if (pointIndex == 0) direction = 1;

        pointIndex += direction;
        targetPos = wayPoints[pointIndex].transform.position;

        StartCoroutine(WaitNextPoint());
    }

    private IEnumerator WaitNextPoint()
    {
        yield return new WaitForSeconds(waitDuration);
        DirectionCalculate();
    }
}