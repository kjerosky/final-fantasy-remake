using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionFrame : MonoBehaviour {

    [SerializeField] float hueRotationSeconds;
    [SerializeField] [Range(0f, 1f)] float colorSaturation;

    private Image frameImage;

    private float colorHue;

    void Start() {
        frameImage = GetComponent<Image>();

        colorHue = 0f;
    }

    void Update() {
        float hueChangeRate = (1f - 0f) / hueRotationSeconds;
        colorHue += hueChangeRate * Time.deltaTime;
        while (colorHue > 1f) {
            colorHue -= 1f;
        }

        frameImage.color = Color.HSVToRGB(colorHue, colorSaturation, 1f);
    }
}
