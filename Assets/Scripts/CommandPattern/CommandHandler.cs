using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandHandler
{
    public List<ICommand> commandList = new List<ICommand>();
    public List<Vector3Int> selectedLocations = new List<Vector3Int>();
    private int index;

    public void AddCommand(ICommand command)
    {

        // if(!command.clickedLocation.Equals(commandList[0].clickedLocation))
        // {
        //     Debug.Log(command.clickedLocation);
        // }
    
        
        {if (index < commandList.Count)
            commandList.RemoveRange(index, commandList.Count - index);
            selectedLocations.RemoveRange(index, selectedLocations.Count - index);

        commandList.Add(command);
        selectedLocations.Add(command.clickedLocation);
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
            index--;
        }
        Debug.Log("Command removed");
    }

}

