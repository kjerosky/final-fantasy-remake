using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleInteractableObject : MonoBehaviour, Interactable {

    public string dialogLine;

    public void interact() {
        Debug.Log(dialogLine);
    }
}
