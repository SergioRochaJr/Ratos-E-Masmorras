using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float detectionRange = 5f;
    public float attackRange = 1f;

    public Transform player;
    public GameController gameController;
    public Tilemap tilemap;
    public LayerMask collisionLayers;

    public AudioClip attackSound;
    private AudioSource audioSource;

    private bool isMoving = false;
    private Vector3 targetPosition;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    public void TakeTurn()
    {
        if (isMoving)
        {
            Debug.Log($"{gameObject.name} ainda está se movendo. Ignorando turno.");
            return;
        }

        Debug.Log($"{gameObject.name} está tomando seu turno.");

        if (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            AttackPlayer();
        }
        else if (Vector3.Distance(transform.position, player.position) <= detectionRange)
        {
            MoveTowardsPlayer();
        }
        else
        {
            EndTurn();
        }
    }

    public void DestroyEnemy()
    {
        GameController gameController = FindObjectOfType<GameController>();
        if (gameController != null)
        {
            gameController.RemoveEnemy(this);
        }

        Destroy(gameObject);
    }

    private void MoveTowardsPlayer()
    {
        Vector3 direction = Vector3.zero;

        float diffX = player.position.x - transform.position.x;
        float diffY = player.position.y - transform.position.y;

        if (Mathf.Abs(diffX) > Mathf.Abs(diffY))
        {
            direction = new Vector3(Mathf.Sign(diffX), 0, 0);
        }
        else
        {
            direction = new Vector3(0, Mathf.Sign(diffY), 0);
        }

        Vector3 potentialPosition = transform.position + direction;

        if (CanMove(potentialPosition))
        {
            UpdateSpriteDirection(direction);
            targetPosition = potentialPosition;
            isMoving = true;
            StartCoroutine(MoveCoroutine(targetPosition));
        }
        else
        {
            Debug.Log($"{gameObject.name} não pode se mover na direção inicial. Tentando outra.");
            direction = (direction.x != 0) ? new Vector3(0, Mathf.Sign(diffY), 0) : new Vector3(Mathf.Sign(diffX), 0, 0);
            potentialPosition = transform.position + direction;

            if (CanMove(potentialPosition))
            {
                UpdateSpriteDirection(direction);
                targetPosition = potentialPosition;
                isMoving = true;
                StartCoroutine(MoveCoroutine(targetPosition));
            }
            else
            {
                Debug.Log($"{gameObject.name} está preso. Terminando turno.");
                EndTurn();
            }
        }
    }

    private IEnumerator MoveCoroutine(Vector3 target)
    {
        float step = moveSpeed * Time.deltaTime;

        while (Vector3.Distance(transform.position, target) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, step);
            yield return null;
        }

        transform.position = target;
        StopMoving();
    }

    private void AttackPlayer()
    {
        Debug.Log($"{gameObject.name} atacou o jogador!");

        if (attackSound != null)
        {
            audioSource.PlayOneShot(attackSound);
        }

        StartCoroutine(AttackJumpCoroutine());

        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage();
        }

        EndTurn();
    }

    private IEnumerator AttackJumpCoroutine()
    {
        Vector3 originalPosition = transform.position;

        transform.position += Vector3.up * 0.2f;
        yield return new WaitForSeconds(0.1f);

        transform.position = originalPosition;
    }

    private void StopMoving()
    {
        isMoving = false;
        EndTurn();
    }

    private void EndTurn()
    {
        gameController.EndEnemyAction();
    }

    private bool CanMove(Vector3 target)
    {
        Vector3Int cellPosition = tilemap.WorldToCell(target);
        TileBase tile = tilemap.GetTile(cellPosition);

        bool tileIsWalkable = tile == null || tilemap.GetColliderType(cellPosition) == Tile.ColliderType.None;

        bool hasCollision = Physics2D.OverlapCircle(target, 0.1f, collisionLayers);

        return tileIsWalkable && !hasCollision;
    }

    private void UpdateSpriteDirection(Vector3 direction)
    {
        if (direction.x != 0)
        {
            spriteRenderer.flipX = direction.x > 0;
        }
    }
}
