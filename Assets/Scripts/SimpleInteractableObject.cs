using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleInteractableObject : MonoBehaviour, Interactable {

    public Dialog dialog;

    private DialogManager dialogManager;

    public void Awake() {
        dialogManager = FindObjectOfType<DialogManager>();
    }

    public void interact() {
        StartCoroutine(dialogManager.showDialog(dialog));
    }
}
