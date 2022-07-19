using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuCommand : MonoBehaviour {

    [SerializeField] GameObject selectionCursor;

    private Text text;

    void Awake() {
        text = GetComponent<Text>();
    }

    public void setText(string value) {
        text.text = value;
    }

    public void setSelected(bool isSelected) {
        selectionCursor.SetActive(isSelected);
    }
}
