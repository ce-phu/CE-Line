using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class LevelGenerateManager : MonoBehaviour
{
    public readonly string dataPath = "Assets/LevelData/LevelData.json";
    [SerializeField] private TMP_InputField cellPrefab;
    [SerializeField] private Transform cellParent;

    [SerializeField] private TMP_InputField stageInput;
    [SerializeField] private TMP_InputField columnInput;
    [SerializeField] private TMP_InputField rowInput;
    [SerializeField] private TMP_InputField stepInput;

    [SerializeField] private Button showStageButton;
    [SerializeField] private Button createStageButton;
    [SerializeField] private Button saveStageButton;
    [SerializeField] private Button generateButton;

    [SerializeField] private Toggle autoResizeToggle;

    public List<LevelData> levelData = new List<LevelData>();
    private TMP_InputField[,] cells = new TMP_InputField[9, 7];
    private int currentStage = 0;

    int rowSize = -1;
    int colSize = -1;

    //----Generator
    private int stepCount = 0;
    private List<TMP_InputField> visitedCells = new List<TMP_InputField>();
    private bool stepFound = false;

    private void Start()
    {
        stageInput.onSubmit.AddListener(ShowStage);
        columnInput.onSubmit.AddListener(SaveStage);
        rowInput.onSubmit.AddListener(SaveStage);
        columnInput.onEndEdit.AddListener(StepChanged);
        rowInput.onEndEdit.AddListener(StepChanged);
        stepInput.onSubmit.AddListener(GenerateLevel);

        showStageButton.onClick.AddListener(ShowStage);
        createStageButton.onClick.AddListener(CreateStage);
        saveStageButton.onClick.AddListener(SaveStage);
        generateButton.onClick.AddListener(GenerateLevel);

        levelData = LoadData();
    }

    public void SaveData()
    {
        string json = JsonConvert.SerializeObject(levelData, Formatting.Indented);
        File.WriteAllText(dataPath, json);

        Debug.Log("Saved LevelData.json: " + dataPath);
    }

    public List<LevelData> LoadData()
    {
        if (!File.Exists(dataPath))
        {
            Debug.LogWarning($"Save file not found, creating new one at: {dataPath}");

            // Create default data
            List<LevelData> newData = new List<LevelData>();
            newData.Add(new LevelData()); // start with 1 empty stage (optional)

            // Save it immediately
            string newJson = JsonConvert.SerializeObject(newData, Formatting.Indented);
            File.WriteAllText(dataPath, newJson);

            return newData;
        }

        string json = File.ReadAllText(dataPath);
        return JsonConvert.DeserializeObject<List<LevelData>>(json);
        ;
    }

    private void ShowStage(string stage)
    {
        ShowStage();
    }

    private void ShowStage()
    {
        if (stageInput.text == String.Empty) return;
        currentStage = int.Parse(stageInput.text);

        columnInput.text = levelData[currentStage].column.ToString();
        rowInput.text = levelData[currentStage].row.ToString();

        ClearCells();
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                cells[i, j] = Instantiate(cellPrefab, cellParent);
                cells[i, j].GetComponent<SlotPrefab>().row = i;
                cells[i, j].GetComponent<SlotPrefab>().col = j;
            }
        }

        if (levelData.Count < int.Parse(stageInput.text))
        {
            Debug.Log("Empty");
        }
        else
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    cells[i, j].text = levelData[currentStage].size[i, j].ToString();
                    cells[i, j].onValueChanged.AddListener(SetColor);
                    cells[i, j].onValueChanged.AddListener(SetSize);
                    cells[i, j].GetComponent<SlotPrefab>().action.AddListener(SetValue);
                }
            }
        }

        SetColor();
        SetSize();
    }

    private void CreateStage()
    {
        levelData.Add(new LevelData());
        stageInput.text = (levelData.Count - 1).ToString();
        currentStage = (levelData.Count - 1);

        columnInput.text = levelData[currentStage].column.ToString();
        rowInput.text = levelData[currentStage].row.ToString();

        ClearCells();
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                cells[i, j] = Instantiate(cellPrefab, cellParent);
                cells[i, j].GetComponent<SlotPrefab>().row = i;
                cells[i, j].GetComponent<SlotPrefab>().col = j;
            }
        }

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                cells[i, j].text = levelData[currentStage].size[i, j].ToString();
                cells[i, j].onValueChanged.AddListener(SetColor);
                cells[i, j].onValueChanged.AddListener(SetSize);
                cells[i, j].GetComponent<SlotPrefab>().action.AddListener(SetValue);
            }
        }

        SetColor();
        SetSize();
    }

    private void SaveStage(string stage)
    {
        SaveStage();
    }

    private void SaveStage()
    {
        currentStage = int.Parse(stageInput.text);

        levelData[currentStage].column = int.Parse(columnInput.text);
        levelData[currentStage].row = int.Parse(rowInput.text);

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                levelData[currentStage].size[i, j] = cells[i, j].text == string.Empty ? 0 : int.Parse(cells[i, j].text);
            }
        }
        
        SaveData();
    }

    private void StepChanged(string text)
    {
        stepInput.text = (int.Parse(rowInput.text) * int.Parse(columnInput.text)).ToString();
    }

    private void ClearCells()
    {
        foreach (Transform child in cellParent.transform)
        {
            Destroy(child.gameObject);
        }

        cells = new TMP_InputField[9, 7];
    }

    private void SetColor(string color)
    {
        SetColor();
    }

    private void SetColor()
    {
        // Debug.Log("SetColor");

        foreach (TMP_InputField item in cells)
        {
            if (item.text == "0")
                item.gameObject.GetComponent<Image>().color = new Color(1, 1, 1, .05f);
            else if (item.text == "1")
                item.gameObject.GetComponent<Image>().color = Color.white;
            else if (item.text == "2")
                item.gameObject.GetComponent<Image>().color = Color.red;
        }
    }

    private void SetSize(string size)
    {
        SetSize();
    }

    private void SetSize()
    {
        if (!autoResizeToggle.isOn) return;
            
        rowSize = -1;
        colSize = -1;

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                if (cells[i, j].text != "0")
                {
                    if (i >= rowSize)
                    {
                        rowSize = i;
                    }

                    if (j >= colSize)
                    {
                        colSize = j;
                    }
                }
            }
        }

        columnInput.text = (colSize + 1).ToString();
        rowInput.text = (rowSize + 1).ToString();
    }

    private void SetValue(TMP_InputField input, int value)
    {
        input.text = value.ToString();
    }

    private void GenerateLevel(string text)
    {
        GenerateLevel();
    }
    
    private void GenerateLevel()
    {
        stepFound = false;
        visitedCells = new List<TMP_InputField>();
        stepCount = int.Parse(stepInput.text);
        int row = int.Parse(rowInput.text);
        int col = int.Parse(columnInput.text);

        //Filled the area with "1" - normal cell
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                cells[i, j].text = "1";
            }
        }

        //Create random base cell

        int randRow = Random.Range(0, row);
        int randCol = Random.Range(0, col);

        cells[randRow, randCol].text = "2";
        visitedCells.Add(cells[randRow, randCol]);
        StartSolve(cells[randRow, randCol], visitedCells);
    }

    private void StartSolve(TMP_InputField executeCell, List<TMP_InputField> _visitedCells)
    {
        if (stepFound) return;
        
        // string visitedCellString = "";
        // foreach (TMP_InputField item in _visitedCells)
        // {
        //     visitedCellString += item.GetComponent<SlotPrefab>().row + " " + item.GetComponent<SlotPrefab>().col + "|";
        // }
        //
        // Debug.Log(visitedCellString);

        List<TMP_InputField> adjacentCells = CheckAdjacent(executeCell);

        foreach (TMP_InputField item in adjacentCells)
        {
            if (!_visitedCells.Contains(item))
            {
                _visitedCells.Add(item);

                if (_visitedCells.Count >= stepCount)
                {
                    stepFound = true;
                    Debug.Log("found");

                    for (int i = 0; i < int.Parse(rowInput.text); i++)
                    {
                        for (int j = 0; j < int.Parse(columnInput.text); j++)
                        {
                            if (cells[i, j].text != "2")
                                cells[i, j].text = "0";
                        }
                    }

                    foreach (TMP_InputField cell in _visitedCells)
                    {
                        if (cell.text != "2")
                            cell.text = "1";
                    }
                }

                StartSolve(item, _visitedCells);
                _visitedCells.Remove(item);
            }
        }
    }

    private List<TMP_InputField> CheckAdjacent(TMP_InputField tempCell)
    {
        List<TMP_InputField> adjacentCells = new List<TMP_InputField>();
        int row = tempCell.GetComponent<SlotPrefab>().row;
        int col = tempCell.GetComponent<SlotPrefab>().col;

        List<int> randDir = new List<int>() { 0, 1, 2, 3 };

        while (randDir.Count != 0)
        {
            int chooseIndex = Random.Range(0, randDir.Count);

            switch (randDir[chooseIndex])
            {
                case 0:
                {
                    if (row - 1 >= 0)
                    {
                        adjacentCells.Add(cells[row - 1, col]);
                    }

                    randDir.Remove(randDir[chooseIndex]);

                    break;
                }
                case 1:
                {
                    if (row + 1 < int.Parse(rowInput.text))
                    {
                        adjacentCells.Add(cells[row + 1, col]);
                    }

                    randDir.Remove(randDir[chooseIndex]);

                    break;
                }
                case 2:
                {
                    if (col - 1 >= 0)
                    {
                        adjacentCells.Add(cells[row, col - 1]);
                    }

                    randDir.Remove(randDir[chooseIndex]);

                    break;
                }
                case 3:
                {
                    if (col + 1 < int.Parse(columnInput.text))
                    {
                        adjacentCells.Add(cells[row, col + 1]);
                    }

                    randDir.Remove(randDir[chooseIndex]);

                    break;
                }
                default:
                    break;
            }
        }


        return adjacentCells;
    }
}