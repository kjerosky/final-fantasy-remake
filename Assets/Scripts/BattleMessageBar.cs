using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleMessageBar : MonoBehaviour {

    [SerializeField] Image backgroundImage;
    [SerializeField] Text messageText;
    [SerializeField] float displaySeconds;

    public IEnumerator displayMessage(string message) {
        show(message);
        yield return new WaitForSeconds(displaySeconds);
        hide();
    }

    private void show(string message) {
        messageText.text = message;

        backgroundImage.enabled = true;
        messageText.enabled = true;
    }

    private void hide() {
        backgroundImage.enabled = false;
        messageText.enabled = false;
    }
}
