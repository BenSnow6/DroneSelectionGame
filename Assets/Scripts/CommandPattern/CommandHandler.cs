using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandHandler
{
    public List<ICommand> commandList = new List<ICommand>();
    public List<Vector3Int> selectedLocations = new List<Vector3Int>();
    public List<float> riskValues = new List<float>();
    public int batteryLevel = 12;
    public int batteryMax = 12;
    public float accumulatedRisk = 0;
    public int index;

    // For the risk graph
    private Window_Graph windowGraph;

    public void AddCommand(ICommand command)
    {
        if (batteryLevel > 0)
        {
            {if (index < commandList.Count)
                commandList.RemoveRange(index, commandList.Count - index);
                selectedLocations.RemoveRange(index, selectedLocations.Count - index);

            commandList.Add(command);
            selectedLocations.Add(command.clickedLocation);
            addRisk(command.clickedLocation);
            removeEnergy(1);
            command.Execute();
            index++;
            }
        }
        else
        {
            Debug.Log("Battery is empty");
        }
    }

    public void UndoCommand()
    {
        if (commandList.Count == 0)
            return;
        if (index > 1)
        {   
            commandList[index - 1].Undo();
            Debug.Log($"Last selected: {selectedLocations[index]} and index is {index}. Deleting {selectedLocations[index - 1]}");
            selectedLocations.RemoveAt(index - 1);
            removeRisk(commandList[index - 1].clickedLocation); // not sure if index is 1 or 0
            addEnergy(1);
            index--;
        }
        Debug.Log("Command removed");
    }

    void addRisk(Vector3Int tileLocalPos)
    {
        accumulatedRisk += commandList[index].gridInfo.GetPositionProperty(tileLocalPos, "Risk", 0.0f);
        riskValues.Add(accumulatedRisk);
        ShowRiskGraph(riskValues);
    }

    void removeRisk(Vector3Int tileLocalPos)
    {
        accumulatedRisk -= commandList[index-1].gridInfo.GetPositionProperty(tileLocalPos, "Risk", 0.0f);
        riskValues.RemoveAt(index-1);
        ShowRiskGraph(riskValues);
    }

    public void ShowRiskGraph(List<float> riskValues)
    {
        windowGraph = Transform.FindObjectOfType<Window_Graph>();
        windowGraph.ShowGraph(riskValues);
    }
    

    void addEnergy(int energy)
    {
        batteryLevel += energy;
        Debug.Log($"adding energy, current level: {batteryLevel}");
        if(batteryLevel < 0.7*batteryMax)
        {
            Image batteryLevelText = GameObject.Find("Full").GetComponent<Image>();
            batteryLevelText.sprite = Resources.Load<Sprite>("Batteries/Half");
            if (batteryLevel < 0.3*batteryMax)
            {
                batteryLevelText.sprite = Resources.Load<Sprite>("Batteries/Low");
                if (batteryLevel <= 0)
                {
                    batteryLevelText.sprite = Resources.Load<Sprite>("Batteries/Zero");
                }
            }
        }
        else
        {
            Image batteryLevelText = GameObject.Find("Full").GetComponent<Image>();
            batteryLevelText.sprite = Resources.Load<Sprite>("Batteries/Full");
        }

    }
    void removeEnergy(int energy)
    {
        batteryLevel -= energy;
        if(batteryLevel < 0.7*batteryMax)
        {
            Image batteryLevelText = GameObject.Find("Full").GetComponent<Image>();
            batteryLevelText.sprite = Resources.Load<Sprite>("Batteries/Half");
            if (batteryLevel < 0.3*batteryMax)
            {
                batteryLevelText.sprite = Resources.Load<Sprite>("Batteries/Low");
                if (batteryLevel <= 0)
                {
                    batteryLevelText.sprite = Resources.Load<Sprite>("Batteries/Zero");
                }
            }

        }
        else
        {
            Image batteryLevelText = GameObject.Find("Full").GetComponent<Image>();
            batteryLevelText.sprite = Resources.Load<Sprite>("Batteries/Full");
        }
        Debug.Log($"removing energy, current level: {batteryLevel}");
    }
    void showBatteryIcon()
    {
        // TODO
    }
}

