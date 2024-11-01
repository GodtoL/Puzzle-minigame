using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementsManager : MonoBehaviour
{
    [SerializeField] LineController lineController;
    List<Vector3> movementsList = new List<Vector3>();
    Vector3 lastMovement;
    // Start is called before the first frame update
    public void AddMove(Vector3 movement)
    {
        movementsList.Add(movement);
        lineController.DrawLine(movementsList);
    }

    public void ResetMoves()
    {
        movementsList.Clear();
    }
    public bool isBackwardMovement(Vector3 movement) 
    {
        if (movementsList.Count > 1) // posición inicial + minimo un movimiento
        {
            lastMovement = movementsList[movementsList.Count - 2];
            if (movement == lastMovement)
            {
                // Se borra los ultimos movimientos
                movementsList.RemoveAt(movementsList.Count - 1);
                movementsList.RemoveAt(movementsList.Count - 1);
                return true;
            }
        }
        return false;
    }

}
