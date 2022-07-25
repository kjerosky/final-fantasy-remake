using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionCursor : MonoBehaviour {

    [SerializeField] Color dimColor;

    private Image cursorImage;

    void Awake() {
        cursorImage = GetComponent<Image>();
        setShowing(false);
    }

    public void setShowing(bool isShowing) {
        cursorImage.enabled = isShowing;
    }

    public void dim() {
        cursorImage.color = dimColor;
    }

    public void brighten() {
        cursorImage.color = Color.white;
    }
}
