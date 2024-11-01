using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BallManager : MonoBehaviour
{
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private GameManager gameManager;

    private List<GameObject> activeBalls = new List<GameObject>();
    private Vector3[] ballSpawnPositions;

    public bool WasBallMoved { get; private set; } = true;
    private Vector3 GetGridPosition(Vector3 position)
    {
        return gameManager.GridCenters[(int)position.x, (int)position.y];
    }

    private void LogBallPositions()
    {
        foreach (GameObject ball in activeBalls)
        {
            if (ball != null)
            {
                Debug.Log($"Posición de la bola: {ball.transform.position}");
            }
        }
    }
    public void InstantiateBalls(Vector3[] positions)
    {
        if (positions == null || positions.Length == 0)
        {
            Debug.LogWarning("No se proporcionaron posiciones para las bolas.");
            return;
        }

        ballSpawnPositions = positions;

        foreach (Vector3 position in ballSpawnPositions)
        {
            Vector3 gridPosition = GetGridPosition(position);
            GameObject newBall = Instantiate(ballPrefab, gridPosition, Quaternion.identity);
            activeBalls.Add(newBall);
        }

        LogBallPositions();
    }

    public bool IsBallAtPosition(Vector3 position)
    {
        Vector3 gridPosition = GetGridPosition(position);
        return activeBalls.Exists(ball => ball.transform.position == gridPosition);
    }

    public void DestroyAllBalls()
    {
        foreach (GameObject ball in activeBalls)
        {
            if (ball != null)
            {
                Destroy(ball);
            }
        }

        activeBalls.Clear();
        LogBallPositions();
    }
    public IEnumerator MoveBall(Vector3 startPos, Vector3 direction)
    {
        GameObject ballToMove = FindBallAtPosition(GetGridPosition(startPos));
        if (!IsValidBallMove(ballToMove, startPos, direction))
        {
            WasBallMoved = false;
            yield break;
        }

        yield return StartCoroutine(AnimateBallMovement(ballToMove, direction));
        WasBallMoved = true;
    }

    private GameObject FindBallAtPosition(Vector3 position)
    {
        return activeBalls.Find(ball => ball.transform.position == position);
    }

    private bool IsValidBallMove(GameObject ball, Vector3 startPos, Vector3 direction)
    {
        if (ball == null)
        {
            Debug.LogWarning("No se encontró una bola en la posición especificada.");
            return false;
        }

        Vector3 targetPosition = GetGridPosition(startPos) + direction * gameManager.Movement;
        
        // Verificamos que no haya una pelota en dicha posicion
        if (FindBallAtPosition(targetPosition))
        {
            return false;
        }
        return !gameManager.CheckInvalidMove(targetPosition);
    }



    private IEnumerator AnimateBallMovement(GameObject ball, Vector3 direction)
    {
        Vector3 startPosition = ball.transform.position;
        Vector3 targetPosition = startPosition + direction * gameManager.Movement;
        float elapsedTime = 0;

        while (elapsedTime < gameManager.Duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / gameManager.Duration;
            ball.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        ball.transform.position = targetPosition;
    }
}

