using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleWeapon : MonoBehaviour {

    [SerializeField] Image weaponRaisedImage;
    [SerializeField] Image weaponStrikingImage;

    public RectTransform RaisedWeaponImageRectTransform => weaponRaisedImage.GetComponent<RectTransform>();

    public void raise() {
        weaponRaisedImage.gameObject.SetActive(true);
        weaponStrikingImage.gameObject.SetActive(false);
    }

    public void strike() {
        weaponRaisedImage.gameObject.SetActive(false);
        weaponStrikingImage.gameObject.SetActive(true);
    }

    public void putAway() {
        weaponRaisedImage.gameObject.SetActive(false);
        weaponStrikingImage.gameObject.SetActive(false);
    }
}
