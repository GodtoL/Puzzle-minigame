using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalManager : MonoBehaviour
{
    public Vector3[] goalsPosition;
    public GameManager gameManager;
    public GameObject goalPrefab;
    public List<GameObject> goals = new List<GameObject>();
    // Start is called before the first frame update
    public bool isWin(Vector3 move)
    {
        foreach (Vector3 position in goalsPosition)
        {
            if (move == gameManager.GridCenters[(int)position.x, (int)position.y])
            {
                return true;
            }
        }
        return false;
    }

    public void positionGoals(Vector3[] goalsPositions)
    {
        goalsPosition = goalsPositions;
        foreach (Vector3 positionGoal in goalsPosition)
        {
            GameObject newGoal = Instantiate(goalPrefab, gameManager.GridCenters[(int)positionGoal.x, (int)positionGoal.y], Quaternion.identity);
            goals.Add(newGoal);
        }
    }

    public void DestroyGoals()
    {
        foreach (GameObject goal in goals)
        {
            Destroy(goal);
        }
    }
}
