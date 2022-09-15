using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandHandler
{
    public List<ICommand> commandList = new List<ICommand>();
    public List<Vector3Int> selectedLocations = new List<Vector3Int>();
    public List<float> riskValues = new List<float>();
    public float accumulatedRisk = 0;
    public int index;
    // For the risk graph
    private Window_Graph windowGraph;

    public void AddCommand(ICommand command)
    {       
        {if (index < commandList.Count)
            commandList.RemoveRange(index, commandList.Count - index);
            selectedLocations.RemoveRange(index, selectedLocations.Count - index);

        commandList.Add(command);
        selectedLocations.Add(command.clickedLocation);
        addRisk(command.clickedLocation);
        command.Execute();
        index++;
        }
    }

    public void UndoCommand()
    {
        if (commandList.Count == 0)
            return;
        if (index > 0)
        {   
            commandList[index - 1].Undo();
            selectedLocations.RemoveAt(index - 1);
            removeRisk(commandList[index - 1].clickedLocation); // not sure if index is 1 or 0
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
    
}

