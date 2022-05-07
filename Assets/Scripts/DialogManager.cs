using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour {

    public GameObject dialogBox;
    public Text dialogText;
    public int lettersPerSecond;

    public event Action OnShowDialog;
    public event Action OnDialogClosed;

    private bool isTyping;
    private Dialog currentDialog;
    private int currentDialogLineNumber;
    private bool requestedTypingSkip;

    void Start() {
        isTyping = false;
        requestedTypingSkip = false;
    }

    public void showDialog(Dialog dialog) {
        OnShowDialog?.Invoke();

        dialogBox.SetActive(true);

        currentDialog = dialog;
        currentDialogLineNumber = 0;
        StartCoroutine(typeDialogLine(currentDialog.Lines[currentDialogLineNumber]));
    }

    public void handleUpdate() {
        bool interactButtonWasPressed = Input.GetKeyDown(KeyCode.Space);
        if (interactButtonWasPressed) {
            if (isTyping) {
                requestedTypingSkip = true;
            } else {
                currentDialogLineNumber++;
                if (currentDialogLineNumber < currentDialog.Lines.Count) {
                    StartCoroutine(typeDialogLine(currentDialog.Lines[currentDialogLineNumber]));
                } else {
                    dialogBox.SetActive(false);
                    OnDialogClosed?.Invoke();
                }
            }
        }
    }

    private IEnumerator typeDialogLine(string line) {
        isTyping = true;

        dialogText.text = "";
        string convertedLine = line.Replace("\\n", "\n");

        for (int currentCharacterIndex = 0; currentCharacterIndex < convertedLine.Length; currentCharacterIndex++) {
            if (requestedTypingSkip) {
                requestedTypingSkip = false;
                break;
            }

            // Skip spaces to give the animation a slightly cleaner look.
            while (convertedLine[currentCharacterIndex] == ' ') {
                currentCharacterIndex++;
            }

            string displayLine = convertedLine.Insert(currentCharacterIndex, "<color=\"#00000000\">") + "</color>";
            dialogText.text = displayLine;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }

        dialogText.text = convertedLine;
        yield return new WaitForEndOfFrame();

        isTyping = false;
    }
}
