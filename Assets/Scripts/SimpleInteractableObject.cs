using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleInteractableObject : MonoBehaviour, Interactable {

    public Dialog dialog;

    private DialogManager dialogManager;

    public void interact(Transform initiator) {
        if (dialogManager == null) {
            dialogManager = FindObjectOfType<DialogManager>();
        }

        StartCoroutine(dialogManager.showDialog(dialog));
    }
}
