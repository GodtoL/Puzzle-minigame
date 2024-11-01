using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private MovementsManager movementsManager;
    [SerializeField] private GoalManager goalManager;
    [SerializeField] private UiManager uiManager;
    private SpriteRenderer spriteRenderer;

    public bool isMoving;
    private Dictionary<KeyCode, Vector3> movementDirections;
    public Animator animator;

    private void Awake()
    {
        InitializeMovementDirections();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void InitializeMovementDirections()
    {
        movementDirections = new Dictionary<KeyCode, Vector3>
        {
            { KeyCode.W, Vector3.up },
            { KeyCode.S, Vector3.down },
            { KeyCode.A, Vector3.left },
            { KeyCode.D, Vector3.right }
        };
    }

    private void Update()
    {
        if (!gameManager.levelcomplete){ 
            if (isMoving) return;

            Vector3 direction = GetInputDirection();
                if (direction != Vector3.zero)
                {
                    TryMovePlayer(direction);
                }
        }
    }

    private Vector3 GetInputDirection()
    {
        foreach (var kvp in movementDirections)
        {
            if (Input.GetKey(kvp.Key))
            {
                return kvp.Value;
            }
        }
        return Vector3.zero;
    }

    private void TryMovePlayer(Vector3 direction)
    {
        Vector3 targetPosition = transform.position + direction * gameManager.Movement;

        // Verifica si el movimiento es válido
        if (gameManager.MoveValidator(targetPosition, direction))
        {
            StartCoroutine(AnimateMovement(targetPosition));
        }
    }

    public void PositionPlayer(Vector3 position)
    {
        if (position.x < 0 || position.y < 0)
        {
            Debug.LogWarning("Posición inicial inválida");
            return;
        }

        Vector3 gridPosition = gameManager.GridCenters[(int)position.x, (int)position.y];
        transform.position = gridPosition;
        movementsManager.AddMove(transform.position);
    }

    private IEnumerator AnimateMovement(Vector3 targetPosition)
    {
        isMoving = true;
        Vector3 startPosition = transform.position;
        float elapsedTime = 0;
        animator.SetBool("isWalking", true);

        Vector3 direction = targetPosition - transform.position;
        checkDirection(direction);

        while (elapsedTime < gameManager.Duration)
        {
            float t = elapsedTime / gameManager.Duration;
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Asegura la posición final exacta
        transform.position = targetPosition;
        if (goalManager.isWin(targetPosition))
        {
            uiManager.showLevelCompletedUI();
        }
        animator.SetBool("isWalking", false);
        isMoving = false;
    }

    private void checkDirection(Vector3 direction)
    {
        if (direction.x != 0)
        {
            spriteRenderer.flipX = direction.x < 0;
        }
    }
}