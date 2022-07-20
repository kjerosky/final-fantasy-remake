using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleMenu : MonoBehaviour {

    [SerializeField] List<MenuCommand> menuCommands;

    private Image commandMenuBackground;

    void Awake() {
        commandMenuBackground = GetComponent<Image>();
    }

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

    public void dimCommandCursor(int commandIndex) {
        menuCommands[commandIndex].dimCursor();
    }

    public void brightenCommandCursor(int commandIndex) {
        menuCommands[commandIndex].brightenCursor();
    }

    public void setShowingCommandsMenu(bool isShowing) {
        commandMenuBackground.enabled = isShowing;
        menuCommands.ForEach(menuCommand => menuCommand.gameObject.SetActive(isShowing));
    }
}
