using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Range(0, 1)] public float smoothTime;
    public Vector3 positionOffset;

    [Header("Axis Limitation")] public Vector2 xLimit;

    public Vector2 yLimit;

    private Transform target;
    private Vector3 velocity = Vector3.zero;


    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void LateUpdate()
    {
        var targetPosition = target.position + positionOffset;
        targetPosition = new Vector3(
            Mathf.Clamp(targetPosition.x, xLimit.x, xLimit.y),
            Mathf.Clamp(targetPosition.y, yLimit.x, yLimit.y),
            -10
        );
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}