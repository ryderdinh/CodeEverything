using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    public Transform respawnPoint;
    private GameController gameController;

    private void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("Player").GetComponent<GameController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) gameController.UpdateCheckpoint(respawnPoint.position);
    }
}