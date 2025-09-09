using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LevelGenerateManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField cellPrefab;
    [SerializeField] private Transform cellParent;
    
    [SerializeField] private TMP_InputField stageInput;
    [SerializeField] private TMP_InputField columnInput;
    [SerializeField] private TMP_InputField rowInput;

    [SerializeField] private Button showStageButton;
    [SerializeField] private Button createStageButton;
    [SerializeField] private Button saveStageButton;

    public List<LevelData> levelData = new List<LevelData>();
    private TMP_InputField[,] cells = new TMP_InputField[9, 7];
    private int currentStage = 0;
    
    int rowSize = -1;
    int colSize = -1;
    
    private void Start()
    {
        stageInput.onSubmit.AddListener(ShowStage);
        columnInput.onSubmit.AddListener(SaveStage);
        rowInput.onSubmit.AddListener(SaveStage);
        
        showStageButton.onClick.AddListener(ShowStage);
        createStageButton.onClick.AddListener(CreateStage); 
        saveStageButton.onClick.AddListener(SaveStage);
        
        levelData = LoadData();
    }

    public void SaveData()
    {
        string json = JsonConvert.SerializeObject(levelData, Formatting.Indented);
        File.WriteAllText(Player.Data.dataPath, json);
        
        Debug.Log("Saved LevelData.json: " + Player.Data.dataPath);
    }

    public List<LevelData> LoadData()
    {
        if (!File.Exists(Player.Data.dataPath))
        {
            Debug.LogWarning($"Save file not found, creating new one at: {Player.Data.dataPath}");

            // Create default data
            List<LevelData> newData = new List<LevelData>();
            newData.Add(new LevelData()); // start with 1 empty stage (optional)

            // Save it immediately
            string newJson = JsonConvert.SerializeObject(newData, Formatting.Indented);
            File.WriteAllText(Player.Data.dataPath, newJson);

            return newData;
        }

        string json = File.ReadAllText(Player.Data.dataPath);
        return JsonConvert.DeserializeObject<List<LevelData>>(json);;
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
                cells[i,j] = Instantiate(cellPrefab, cellParent);
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
                    cells[i,j].onValueChanged.AddListener(SetColor);
                    cells[i,j].onValueChanged.AddListener(SetSize);
                    cells[i,j].GetComponent<SlotPrefab>().action.AddListener(SetValue);
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
                cells[i,j] = Instantiate(cellPrefab, cellParent);;
            }
        }
        
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                cells[i, j].text = levelData[currentStage].size[i, j].ToString();
                cells[i,j].onValueChanged.AddListener(SetColor);
                cells[i,j].onValueChanged.AddListener(SetSize);
                cells[i,j].GetComponent<SlotPrefab>().action.AddListener(SetValue);
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
                levelData[currentStage].size[i, j] = int.Parse(cells[i, j].text);
            }
        }
        
        SaveData();
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
        Debug.Log("SetColor");
        
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
}
