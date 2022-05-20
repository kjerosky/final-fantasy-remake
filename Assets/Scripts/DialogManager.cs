using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour {

    public GameObject dialogBox;
    public Text dialogText;
    public int lettersPerSecond;
    public float boxAppearanceSeconds;

    public event Action OnShowDialog;
    public event Action OnDialogClosed;

    private bool isTyping;
    private Dialog currentDialog;
    private int currentDialogLineNumber;
    private bool requestedTypingSkip;
    private bool isChangingAppearance;
    private Action onFinished;

    void Start() {
        isTyping = false;
        requestedTypingSkip = false;
        isChangingAppearance = false;
    }

    public IEnumerator showDialog(Dialog dialog, Action onDialogFinished = null) {
        onFinished = onDialogFinished;

        OnShowDialog?.Invoke();

        dialogText.text = "";
        dialogBox.SetActive(true);

        yield return changeDialogBoxAppearance(0f, 1f);

        currentDialog = dialog;
        currentDialogLineNumber = 0;
        StartCoroutine(typeDialogLine(currentDialog.Lines[currentDialogLineNumber]));
    }

    private IEnumerator closeDialog() {
        dialogText.text = "";
        yield return changeDialogBoxAppearance(1f, 0f);

        dialogBox.SetActive(false);
        OnDialogClosed?.Invoke();
        onFinished?.Invoke();
    }

    public void handleUpdate() {
        if (isChangingAppearance) {
            return;
        }

        bool interactButtonWasPressed = Input.GetKeyDown(KeyCode.Space);
        if (interactButtonWasPressed) {
            if (isTyping) {
                requestedTypingSkip = true;
            } else {
                currentDialogLineNumber++;
                if (currentDialogLineNumber < currentDialog.Lines.Count) {
                    StartCoroutine(typeDialogLine(currentDialog.Lines[currentDialogLineNumber]));
                } else {
                    StartCoroutine(closeDialog());
                }
            }
        }
    }

    private IEnumerator changeDialogBoxAppearance(float startScale, float endScale) {
        isChangingAppearance = true;

        float dialogBoxAppearanceRate = Mathf.Abs(endScale - startScale) / boxAppearanceSeconds;

        dialogBox.transform.localScale = new Vector3(startScale, startScale, startScale);

        Vector3 targetScale = new Vector3(endScale, endScale, endScale);
        while (dialogBox.transform.localScale != targetScale) {
            dialogBox.transform.localScale = Vector3.MoveTowards(
                dialogBox.transform.localScale, targetScale, dialogBoxAppearanceRate * Time.deltaTime
            );
            yield return null;
        }

        isChangingAppearance = false;
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
