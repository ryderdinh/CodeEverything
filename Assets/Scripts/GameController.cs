using System.Collections;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private CameraController cameraController;
    private Rigidbody2D playerRb;
    private Vector2 startPos;

    private void Awake()
    {
        cameraController = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
        playerRb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        startPos = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstacle")) Die();
    }

    private void Die()
    {
        cameraController.anim.Play("RespawnCamera");
        StartCoroutine(Respawn(0.5f));
    }

    private IEnumerator Respawn(float duration)
    {
        // ReSharper disable all Unity.InefficientPropertyAccess

        playerRb.simulated = false;
        playerRb.velocity = Vector2.zero;
        transform.localScale = Vector3.zero;
        yield return new WaitForSeconds(duration);
        transform.position = startPos;
        transform.localScale = Vector3.one;
        playerRb.simulated = true;
    }
}