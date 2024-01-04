using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    public Transform respawnPoint;
    public Sprite passive, active;
    private Collider2D coll;
    private GameController gameController;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("Player").GetComponent<GameController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        coll = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) gameController.UpdateCheckpoint(respawnPoint.position);
        spriteRenderer.sprite = active;
        coll.enabled = false;
    }
}