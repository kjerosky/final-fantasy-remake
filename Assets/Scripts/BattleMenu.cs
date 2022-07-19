using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMenu : MonoBehaviour {

    [SerializeField] List<MenuCommand> menuCommands;

    public void initializeCommands(List<BattleMenuCommand> commands) {
        for (int i = 0; i < menuCommands.Count; i++) {
            menuCommands[i].setSelected(false);
            
            string menuCommandText = i < commands.Count ? commands[i].Text : "";
            menuCommands[i].setText(menuCommandText);
        }
    }

    public void setSelectedCommand(int selectedCommandIndex) {
        for (int i = 0; i < menuCommands.Count; i++) {
            menuCommands[i].setSelected(i == selectedCommandIndex);
        }
    }
}
