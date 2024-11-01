using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LineController : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private List<Vector3> linePoints = new List<Vector3>();

    void Start()
    {
        // Obtener referencia al LineRenderer
        lineRenderer = GetComponent<LineRenderer>();

        // Configuración básica
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
        lineRenderer.numCornerVertices = 20;

        // Definir que la línea tendrá 2 puntos
        //lineRenderer.positionCount = 2;

        // Establecer las posiciones de inicio y fin
        //lineRenderer.SetPosition(0, new Vector3(0, 0, 0));    // Punto inicial
        //lineRenderer.SetPosition(1, new Vector3(0, 5, 0));    // Punto final
    }
    public void DrawLine(List<Vector3> lines)
    {       
        lineRenderer.positionCount = lines.Count;
        for (int i = 0; i < lines.Count; i++)
        {
            lineRenderer.SetPosition(i, lines[i]);
        }
    }
    // Si quieres mover la línea en tiempo real
    void Update()
    {
        // Ejemplo: Mover el punto final de la línea
        // Vector3 newEndPosition = new Vector3(0, 5 * Mathf.Sin(Time.time), 0);
        // lineRenderer.SetPosition(1, newEndPosition);
    }
}