using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    public int level;
    public Vector3[] ballsPosition;
    public int steps = 5;
    public Vector3[] fencesPosition;
    public Vector3 player1Position;
    public Vector3[] goalsPosition;
}
