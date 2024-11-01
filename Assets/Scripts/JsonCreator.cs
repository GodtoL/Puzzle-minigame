using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class JsonCreator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        LevelData levelData = new LevelData();
        levelData.level = 2;
        levelData.ballsPosition = new Vector3[] { new Vector3(5, 4, 0), new Vector3(5, 2, 0), new Vector3(6, 2, 0) };
        levelData.fencesPosition = new Vector3[] { new Vector3(3, 4, 0), new Vector3(4, 3, 0), new Vector3(6, 3, 0), new Vector3(7, 2, 0), new Vector3(4, 1, 0) };
        levelData.player1Position = new Vector3(2, 1, 0);
        levelData.goalsPosition = new Vector3[] { new Vector3(7, 3, 0) };
        levelData.steps = 9;

        string json = JsonUtility.ToJson(levelData, true);

        // Ruta para guardar el archivo
        string path = Application.dataPath + "/Resources/level2.json";

        // Escribir el archivo
        File.WriteAllText(path, json);

        Debug.Log("JSON creado y guardado en: " + path);

    }
}
