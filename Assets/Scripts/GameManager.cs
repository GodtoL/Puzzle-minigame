using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    [Header("Configuración del Juego")]
    [SerializeField] private float movement = 1f;
    [SerializeField] private float duration = 0.5f;
    [SerializeField] private GameObject fencePrefab;

    [Header("Referencias")]
    [SerializeField] private LevelLoader levelLoader;
    [SerializeField] private MovementsManager movementsManager;
    [SerializeField] private BallManager ballManager;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private GoalManager goalManager;
    [SerializeField] private UiManager uiManager;

    private Vector3[,] gridCenters;
    private List<GameObject> fencesInstantiated = new List<GameObject>();
    private Vector3[] fencesPosition;
    private int currentLevel = 1;
    private int steps;
    public bool levelcomplete = false;

    // Propiedades públicas para acceso seguro
    public Vector3[,] GridCenters => gridCenters;
    public float Movement => movement;
    public float Duration => duration;

    private void Start()
    {
        InitializeGame();
    }

    private void InitializeGame()
    {
        gridCenters = new Vector3[gridManager.width, gridManager.height];
        GenerateCenterBoard();
        LoadCurrentLevel();
    }

    private void LoadCurrentLevel()
    {
        string levelFileName = $"level{currentLevel}";
        LevelData levelData = levelLoader.LoadLevelData(levelFileName);

        if (levelData != null)
        {
            DestroyObjects();
            SetupLevel(levelData);
        }
        else
        {
            Debug.LogWarning($"No se pudo cargar el nivel {currentLevel}");
        }
    }
    // Se emcarga de leer la informacion del nivel
    private void SetupLevel(LevelData levelData)
    {
        uiManager.hideLevelCompletedUI();
        fencesPosition = levelData.fencesPosition;
        steps = levelData.steps;

        playerController.PositionPlayer(levelData.player1Position);
        ballManager.InstantiateBalls(levelData.ballsPosition);
        goalManager.positionGoals(levelData.goalsPosition);
        uiManager.InitializeSteps(levelData.steps);
        CreateFences(levelData.fencesPosition);
        levelcomplete = false;

    }

    private void CreateFences(Vector3[] fencePositions)
    {
        foreach (Vector3 position in fencePositions)
        {
            Vector3 gridPosition = gridCenters[(int)position.x, (int)position.y];
            GameObject newFence = Instantiate(fencePrefab, gridPosition, Quaternion.identity);
            fencesInstantiated.Add(newFence);
        }
    }

    private void GenerateCenterBoard()
    {
        for (int x = 0; x < gridManager.width; x++)
        {
            for (int y = 0; y < gridManager.height; y++)
            {
                float centerX = x * gridManager.cellSize + gridManager.cellSize / 2;
                float centerY = y * gridManager.cellSize + gridManager.cellSize / 2;
                gridCenters[x, y] = new Vector3(centerX, centerY, 0);
            }
        }
    }

    public bool MoveValidator(Vector3 move, Vector3 direction)
    {
        if (CheckWinCondition(move)) return true;
        if (movementsManager.isBackwardMovement(move))
        {
            HandleSteps(true);
        }
        if (CheckInvalidMove(move)) return false;

        if (HandleBallInteraction(move, direction))
        {
            return ballManager.WasBallMoved;
        }

        // Movimiento normal del jugador
        RecordMove(move);
        return true;
    }

    private IEnumerator waitToAdvanceLevel()
    {
        yield return new WaitForSeconds(2);
        AdvanceToNextLevel();

    }
    private bool CheckWinCondition(Vector3 move)
    {
        if (goalManager.isWin(move))
        {
            Debug.Log("¡Nivel completado!");
            levelcomplete = true;
            StartCoroutine(waitToAdvanceLevel());
            movementsManager.ResetMoves();
            return true;
        }
        return false;
    }

    private void AdvanceToNextLevel()
    {
        currentLevel++;
        LoadCurrentLevel();
    }

    private bool HandleBallInteraction(Vector3 move, Vector3 direction)
    {
        if (ballManager.IsBallAtPosition(move))
        {
            Debug.Log("Interacción con pelota detectada.");
            StartCoroutine(ballManager.MoveBall(move, direction));
            if (ballManager.WasBallMoved)
            {
                RecordMove(move);
            }
            return true;
        }
        return false;
    }

    private void RecordMove(Vector3 move)
    {
        movementsManager.AddMove(move);
        HandleSteps(false);
    }

    public bool CheckInvalidMove(Vector3 move)
    {
        return move.x >= gridManager.width ||
               move.x < 0 ||
               move.y >= gridManager.height ||
               move.y < 0 ||
               IsFenceAtPosition(move) ||
               steps <= 0;
    }

    private bool IsFenceAtPosition(Vector3 move)
    {
        foreach (Vector3 fencePos in fencesPosition)
        {
            if (move == gridCenters[(int)fencePos.x, (int)fencePos.y])
                return true;
        }
        return false;
    }

    public void DestroyObjects()
    {
        ballManager.DestroyAllBalls();
        goalManager.DestroyGoals();

        foreach (GameObject fence in fencesInstantiated)
        {
            if (fence != null)
            {
                Destroy(fence);
            }
        }
        fencesInstantiated.Clear();
    }

    private void HandleSteps(bool isBackwardMovement)
    {
        steps += isBackwardMovement ? 2 : -1;
        uiManager.InitializeSteps(steps);
    }


}
