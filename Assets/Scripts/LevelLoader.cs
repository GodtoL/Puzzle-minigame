using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public LevelData LoadLevelData(string levelFileName)
    {
        Debug.Log("El level file name quellego es " +levelFileName);
        TextAsset jsonFile = Resources.Load<TextAsset>(levelFileName);
        if (jsonFile != null)
        {
            return JsonUtility.FromJson<LevelData>(jsonFile.text);
        }
        else
        {
            Debug.LogError("No se pudo cargar el archivo JSON del nivel.");
            return null;
        }
    }
}
