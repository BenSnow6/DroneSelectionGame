using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandHandler
{
    public List<ICommand> commandList = new List<ICommand>();
    private int index;

    public void AddCommand(ICommand command)
    {

        // if(!command.clickedLocation.Equals(commandList[0].clickedLocation))
        // {
        //     Debug.Log(command.clickedLocation);
        // }
    
        
        {if (index < commandList.Count)
            commandList.RemoveRange(index, commandList.Count - index);

        commandList.Add(command);
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
            index--;
        }
        Debug.Log("Command removed");
    }

    public void RedoCommand()
    {
        if (commandList.Count == 0)
            return;

        if (index < commandList.Count)
        {
            index++;
            commandList[index - 1].Execute();
        }
    }
}

