using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class AutoSolverManager : MonoBehaviour
{
    public static AutoSolverManager Instance;

    private List<Cell> visitedCells;
    private List<Cell> path;
    private Cell currentCell;
    
    private float elapsedTime;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public static void StartSolve(Cell executeCell)
    {
        Instance.elapsedTime = 0;

        Instance.visitedCells = new List<Cell>();
        Instance.path = new List<Cell>();
        Instance.visitedCells.Add(executeCell);
        
        Instance._StartSolve(executeCell, Instance.visitedCells);
        
        Instance.Move();
    }

    private void Move()
    {
        IEnumerator Action()
        {
            foreach (Cell cell in path)
            {
                yield return new WaitForSeconds(0.1f);
                cell.TriggerAction();
            }

        }

        StartCoroutine(Action());
    }

    private void _StartSolve(Cell _executeCell, List<Cell> _visitedCells)
    {
        List<Cell> adjacentCells = GameManager.CheckAdjacentWithCoord(_executeCell);

        foreach (Cell item in adjacentCells)
        {
            if (!_visitedCells.Contains(item))
            {
                _visitedCells.Add(item);

                if (visitedCells.Count >= GameManager.totalCell + 1)
                {
                    Debug.Log("found");

                    string pathString = "Path: ";

                    foreach (Cell cell in _visitedCells)
                    {
                        path.Add(cell);
                        pathString += cell.row + " " + cell.col + "|";
                    }
                    Debug.Log(pathString);
                }
                
                _StartSolve(item, _visitedCells);
                _visitedCells.Remove(item);
            }
        }
    }
}