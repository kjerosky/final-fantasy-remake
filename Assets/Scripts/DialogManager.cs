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

        float dialogBoxAppearanceRate = 1 / boxAppearanceSeconds;
        float lowerScaleBound = startScale;
        float upperScaleBound = endScale;
        if (startScale > endScale) {
            dialogBoxAppearanceRate = -dialogBoxAppearanceRate;
            lowerScaleBound = endScale;
            upperScaleBound = startScale;
        }

        float dialogBoxScale = startScale;
        while (dialogBoxScale != endScale) {
            dialogBox.transform.localScale = new Vector3(dialogBoxScale, dialogBoxScale, dialogBoxScale);
            dialogBoxScale += dialogBoxAppearanceRate * Time.deltaTime;
            dialogBoxScale = Mathf.Clamp(dialogBoxScale, lowerScaleBound, upperScaleBound);
            yield return null;
        }

        dialogBox.transform.localScale = new Vector3(endScale, endScale, endScale);
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
