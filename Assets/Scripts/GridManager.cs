using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int width = 10;
    public int height = 10;
    public float cellSize = 1f;
    //public GameObject nanika;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;

        // Dibuja las líneas horizontales
        for (int y = 0; y <= height; y++)
        {
            Gizmos.DrawLine(new Vector3(0, y * cellSize, 0), new Vector3(width * cellSize, y * cellSize, 0));
            
        }
        //Instantiate(nanika, new Vector3(cellSize / 2, cellSize / 2, 0), Quaternion.identity);

        // Dibuja las líneas verticales
        for (int x = 0; x <= width; x++)
        {
            Gizmos.DrawLine(new Vector3(x * cellSize, 0, 0), new Vector3(x * cellSize, height * cellSize, 0));
            //Instantiate(nanika, new Vector3(x, 0, 0), Quaternion.identity);
        }
    }
}
