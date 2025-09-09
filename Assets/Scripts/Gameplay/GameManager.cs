using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private RectTransform canvas;

    [SerializeField] private Cell cellPrefabs;
    [SerializeField] private Transform cellParent;
    [SerializeField] private GridLayoutGroup levelDisplay;

    private List<LevelData> masterLevelData = new List<LevelData>();
    private LevelData levelData = new LevelData();
    private Cell[,] cell = new Cell[7, 7];

    public static bool isPointerDown = false;

    private Stack<Cell> cellStack = new Stack<Cell>();
    private List<Cell> adjacentCells = new List<Cell>();
    private int[] baseCell = new int[2];

    private int totalCell = 0;
    private int maxInstructionStep = 5;
    private int instructionStepCurrentIndex = -1;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
            isPointerDown = true;

        if (Input.GetMouseButtonUp(0))
            isPointerDown = false;
    }

    public static void Init()
    {
        Instance._Init();
    }

    private void _Init()
    {
        LoadData();
        DisplayLevel();
    }

    private void LoadData()
    {
        if (!File.Exists(Path.Combine(Application.persistentDataPath, "LevelData.json"))) return;

        string json = File.ReadAllText(Path.Combine(Application.persistentDataPath, "LevelData.json"));

        masterLevelData = JsonConvert.DeserializeObject<List<LevelData>>(json);
        levelData = masterLevelData[Player.Data.currentStage];

        foreach (LevelData.SolvedStep solvedStep in masterLevelData[Player.Data.currentStage].solvedSteps)
        {
            Debug.Log(solvedStep.row + " " + solvedStep.column);
        }
    }

    private void SaveData()
    {
        string json = JsonConvert.SerializeObject(masterLevelData, Formatting.Indented);
        File.WriteAllText(Path.Combine(Application.persistentDataPath, "LevelData.json"), json);

        Debug.Log("Saved LevelData.json: " + Path.Combine(Application.persistentDataPath, "LevelData.json"));
    }

    private void DisplayLevel()
    {
        StartCoroutine(Execute());

        IEnumerator Execute()
        {
            ClearLevel();

            int rowCount = levelData.row;
            int columnCount = levelData.column;

            baseCell = new int[2];

            levelDisplay.constraintCount = columnCount;
            cell = new Cell[rowCount, columnCount];

            SetScaleCanvas(rowCount, columnCount);

            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    cell[i, j] = Instantiate(cellPrefabs, cellParent);
                    cell[i, j].row = i;
                    cell[i, j].col = j;

                    if (levelData.size[i, j] == 2)
                    {
                        cell[i, j].SetBase();
                        baseCell[0] = i;
                        baseCell[1] = j;
                    }
                    else if (levelData.size[i, j] == 1)
                    {
                        cell[i, j].SetNormal();
                        totalCell++;
                    }
                    else if (levelData.size[i, j] == 0)
                    {
                        cell[i, j].SetDisable();
                    }

                    cell[i, j].In();
                }
            }

            CheckAdjacentCell(cell[baseCell[0], baseCell[1]]);
            yield return new WaitForEndOfFrame();
        }
    }

    private void ClearLevel()
    {
        foreach (Transform child in cellParent.transform)
        {
            Destroy(child.gameObject);
        }

        totalCell = 0;
        cellStack.Clear();
        instructionStepCurrentIndex = -1;
    }

    private void LevelWin()
    {
#if CE_DEBUG

        masterLevelData[Player.Data.currentStage].solvedSteps.Clear();
        foreach (Cell item in cellStack)
        {
            masterLevelData[Player.Data.currentStage].solvedSteps.Add(new LevelData.SolvedStep()
            {
                row = item.row,
                column = item.col,
            });
        }

        SaveData();
#endif

        StartCoroutine(Execute());

        IEnumerator Execute()
        {
            foreach (Cell item in cellStack)
            {
                item.isDisable = true;
            }
            
            yield return new WaitForSeconds(0.5f);
            
            foreach (Cell item in cellStack)
            {
                item.GetComponent<Cell>().Out();
            }

            cell[baseCell[0], baseCell[1]].Out();

            totalCell = 0;
            cellStack.Clear();
            instructionStepCurrentIndex = -1;
            yield return new WaitForSeconds(0.1f);
            Ingame.SetNextLevel(true);
        }

        //Save Solved Steps
    }

    private void SetScaleCanvas(int row, int column)
    {
        if (row <= 3 && column <= 3)
        {
            canvas.localScale = new Vector3(1.75f, 1.75f, 1.75f);
        }
        else if (row <= 5 && column <= 5)
        {
            canvas.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        }
        else if (row <= 7 && column <= 7)
        {
            canvas.localScale = new Vector3(1.75f, 1.75f, 1.75f);
        }
    }

    public static void CheckAdjacentCell(Cell tempCell)
    {
        Instance._CheckAdjacentCell(tempCell);
    }

    private void _CheckAdjacentCell(Cell tempCell)
    {
        foreach (Cell item in adjacentCells)
        {
            item.SetUnAdjacent();
        }

        adjacentCells.Clear();

        if (tempCell.row - 1 >= 0 && tempCell.row - 1 < levelData.row)
        {
            if (!cell[tempCell.row - 1, tempCell.col].isBase && !cell[tempCell.row - 1, tempCell.col].isDisable)
            {
                adjacentCells.Add(cell[tempCell.row - 1, tempCell.col]);
            }
        }

        if (tempCell.row + 1 >= 0 && tempCell.row + 1 < levelData.row)
        {
            if (!cell[tempCell.row + 1, tempCell.col].isBase && !cell[tempCell.row + 1, tempCell.col].isDisable)
            {
                adjacentCells.Add(cell[tempCell.row + 1, tempCell.col]);
            }
        }

        if (tempCell.col - 1 >= 0 && tempCell.col - 1 < levelData.column)
        {
            if (!cell[tempCell.row, tempCell.col - 1].isBase && !cell[tempCell.row, tempCell.col - 1].isDisable)
            {
                adjacentCells.Add(cell[tempCell.row, tempCell.col - 1]);
            }
        }

        if (tempCell.col + 1 >= 0 && tempCell.col + 1 < levelData.column)
        {
            if (!cell[tempCell.row, tempCell.col + 1].isBase && !cell[tempCell.row, tempCell.col + 1].isDisable)
            {
                adjacentCells.Add(cell[tempCell.row, tempCell.col + 1]);
            }
        }

        foreach (Cell item in adjacentCells)
        {
            item.SetAdjacent();
        }
    }

    public static void ResetAllCell()
    {
        foreach (Cell item in Instance.cell)
        {
            item.SetNormal();
        }

        Instance.cellStack.Clear();
    }

    public static void SetActiveCell(Cell tempCell)
    {
        Instance._SetActiveCell(tempCell);
    }

    private void _SetActiveCell(Cell tempCell)
    {
        if (!cellStack.Contains(tempCell))
        {
            cellStack.Push(tempCell);
            CheckWin();
        }
        else
        {
            bool isContinue = true;
            while (isContinue)
            {
                Cell temp = cellStack.Pop();
                if (temp == tempCell)
                {
                    isContinue = false;
                    cellStack.Push(temp);
                }
                else
                {
                    temp.SetNormal();
                }
            }
        }

        ShowStack();
        SetTextureForCell();
    }

    private void SetTextureForCell()
    {
        CheckCellTexture();
    }

    private void CheckCellTexture()
    {
        Cell[] cellArray = cellStack.ToArray();

        for (int i = 0; i < cellStack.Count; i++)
        {
            if (i == 0 && cellStack.Count != 1)
            {
                if (cellArray[i].row - 1 == cellArray[i + 1].row && cellArray[i].col == cellArray[i + 1].col) //top
                {
                    cellArray[i].SetTexture(Cell.PreviousDirection.TOP);
                }

                if (cellArray[i].row + 1 == cellArray[i + 1].row && cellArray[i].col == cellArray[i + 1].col) // bottom
                {
                    cellArray[i].SetTexture(Cell.PreviousDirection.BOTTOM);
                }

                if (cellArray[i].col - 1 == cellArray[i + 1].col && cellArray[i].row == cellArray[i + 1].row) //left
                {
                    cellArray[i].SetTexture(Cell.PreviousDirection.LEFT);
                }

                if (cellArray[i].col + 1 == cellArray[i + 1].col && cellArray[i].row == cellArray[i + 1].row) //right
                {
                    cellArray[i].SetTexture(Cell.PreviousDirection.RIGHT);
                }
            }
            else if (i == 0 && cellStack.Count == 1)
            {
                if (cellArray[i].row - 1 == baseCell[0] && cellArray[i].col == baseCell[1]) //top
                {
                    cellArray[i].SetTexture(Cell.PreviousDirection.TOP);
                }

                if (cellArray[i].row + 1 == baseCell[0] && cellArray[i].col == baseCell[1]) // bottom
                {
                    cellArray[i].SetTexture(Cell.PreviousDirection.BOTTOM);
                }

                if (cellArray[i].col - 1 == baseCell[1] && cellArray[i].row == baseCell[0]) //left
                {
                    cellArray[i].SetTexture(Cell.PreviousDirection.LEFT);
                }

                if (cellArray[i].col + 1 == baseCell[1] && cellArray[i].row == baseCell[0]) //right
                {
                    cellArray[i].SetTexture(Cell.PreviousDirection.RIGHT);
                }
            }
            else if (i == cellArray.Length - 1)
            {
                Cell.PreviousDirection tempPrevDirection = Cell.PreviousDirection.NOTINITIALIZE;
                Cell.NextDirection tempNextDirection = Cell.NextDirection.NONE;

                if (cellArray[i].row - 1 == baseCell[0] && cellArray[i].col == baseCell[1]) //top
                {
                    tempPrevDirection = Cell.PreviousDirection.TOP;
                }

                if (cellArray[i].row + 1 == baseCell[0] && cellArray[i].col == baseCell[1]) // bottom
                {
                    tempPrevDirection = Cell.PreviousDirection.BOTTOM;
                }

                if (cellArray[i].col - 1 == baseCell[1] && cellArray[i].row == baseCell[0]) //left
                {
                    tempPrevDirection = Cell.PreviousDirection.LEFT;
                }

                if (cellArray[i].col + 1 == baseCell[1] && cellArray[i].row == baseCell[0]) //right
                {
                    tempPrevDirection = Cell.PreviousDirection.RIGHT;
                }

                if (cellArray[i].row - 1 == cellArray[i - 1].row && cellArray[i].col == cellArray[i - 1].col) //top
                {
                    tempNextDirection = Cell.NextDirection.TOP;
                }

                if (cellArray[i].row + 1 == cellArray[i - 1].row && cellArray[i].col == cellArray[i - 1].col) // bottom
                {
                    tempNextDirection = Cell.NextDirection.BOTTOM;
                }

                if (cellArray[i].col - 1 == cellArray[i - 1].col && cellArray[i].row == cellArray[i - 1].row) //left
                {
                    tempNextDirection = Cell.NextDirection.LEFT;
                }

                if (cellArray[i].col + 1 == cellArray[i - 1].col && cellArray[i].row == cellArray[i - 1].row) //right
                {
                    tempNextDirection = Cell.NextDirection.RIGHT;
                }

                cellArray[i].SetTexture(tempPrevDirection, tempNextDirection);
            }
            else
            {
                Cell.PreviousDirection tempPrevDirection = Cell.PreviousDirection.NOTINITIALIZE;
                Cell.NextDirection tempNextDirection = Cell.NextDirection.NONE;

                if (cellArray[i].row - 1 == cellArray[i - 1].row && cellArray[i].col == cellArray[i - 1].col) //top
                {
                    tempPrevDirection = Cell.PreviousDirection.TOP;
                }

                if (cellArray[i].row + 1 == cellArray[i - 1].row && cellArray[i].col == cellArray[i - 1].col) // bottom
                {
                    tempPrevDirection = Cell.PreviousDirection.BOTTOM;
                }

                if (cellArray[i].col - 1 == cellArray[i - 1].col && cellArray[i].row == cellArray[i - 1].row) //left
                {
                    tempPrevDirection = Cell.PreviousDirection.LEFT;
                }

                if (cellArray[i].col + 1 == cellArray[i - 1].col && cellArray[i].row == cellArray[i - 1].row) //right
                {
                    tempPrevDirection = Cell.PreviousDirection.RIGHT;
                }

                if (cellArray[i].row - 1 == cellArray[i + 1].row && cellArray[i].col == cellArray[i + 1].col) //top
                {
                    tempNextDirection = Cell.NextDirection.TOP;
                }

                if (cellArray[i].row + 1 == cellArray[i + 1].row && cellArray[i].col == cellArray[i + 1].col) // bottom
                {
                    tempNextDirection = Cell.NextDirection.BOTTOM;
                }

                if (cellArray[i].col - 1 == cellArray[i + 1].col && cellArray[i].row == cellArray[i + 1].row) //left
                {
                    tempNextDirection = Cell.NextDirection.LEFT;
                }

                if (cellArray[i].col + 1 == cellArray[i + 1].col && cellArray[i].row == cellArray[i + 1].row) //right
                {
                    tempNextDirection = Cell.NextDirection.RIGHT;
                }

                cellArray[i].SetTexture(tempPrevDirection, tempNextDirection);
            }
        }
    }

    private void CheckWin()
    {
        if (cellStack.Count == totalCell)
        {
            LevelWin();
        }
    }

    private void ShowStack()
    {
        string message = "";
        foreach (var item in cellStack)
        {
            message += ("\nCell: " + item.row + ", " + item.col);
        }

        DebugManager.AddDebugText("cellStack", message);
    }

    public static void ShowInstructions()
    {
        Instance._ShowInstructions();
    }

    private void _ShowInstructions()
    {
        // masterLevelData[Player.Data.currentStage].solvedSteps;
        int remainingInstructionStep = maxInstructionStep - 1;
        int row = 0;
        int col = 0;

        if (instructionStepCurrentIndex == -1)
        {
            instructionStepCurrentIndex = masterLevelData[Player.Data.currentStage].solvedSteps.Count - 1;
        }

        for (int i = instructionStepCurrentIndex; i >= 0; i--)
        {
            row = masterLevelData[Player.Data.currentStage].solvedSteps[i].row;
            col = masterLevelData[Player.Data.currentStage].solvedSteps[i].column;
            cell[row, col].SetInstructionShown();

            if (remainingInstructionStep == 0)
                break;

            if (i - remainingInstructionStep > 0)
                remainingInstructionStep--;

            instructionStepCurrentIndex = i;
        }
    }
}