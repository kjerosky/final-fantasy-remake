using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour {

    public GameObject dialogBox;
    public Text dialogText;

    public event Action OnShowDialog;
    public event Action OnDialogClosed;

    private bool isTyping;
    private Dialog currentDialog;
    private int currentDialogLineNumber;

    void Start() {
        isTyping = false;
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
        if (!isTyping && interactButtonWasPressed) {
            currentDialogLineNumber++;
            if (currentDialogLineNumber < currentDialog.Lines.Count) {
                StartCoroutine(typeDialogLine(currentDialog.Lines[currentDialogLineNumber]));
            } else {
                dialogBox.SetActive(false);
                OnDialogClosed?.Invoke();
            }
        }
    }

    private IEnumerator typeDialogLine(string line) {
        isTyping = true;

        // Let's allow newlines, for now.
        dialogText.text = line.Replace("\\n", "\n");
        yield return new WaitForEndOfFrame();

        isTyping = false;
    }
}
