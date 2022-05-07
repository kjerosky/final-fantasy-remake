using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleInteractableObject : MonoBehaviour, Interactable {

    public Dialog dialog;
    public DialogManager dialogManager;

    public void interact() {
        dialogManager.showDialog(dialog);
    }
}
