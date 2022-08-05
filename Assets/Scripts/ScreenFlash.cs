using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ScreenFlash : MonoBehaviour {

    [SerializeField] Image flashImage;
    [SerializeField] float totalFlashSeconds;

    public void flash() {
        StartCoroutine(showFlashOverTime());
    }

    private IEnumerator showFlashOverTime() {
        Color transparentFlashColor = flashImage.color;
        transparentFlashColor.a = 0f;
        flashImage.color = transparentFlashColor;

        flashImage.gameObject.SetActive(true);
        yield return flashImage
            .DOFade(1f, totalFlashSeconds / 2)
            .SetEase(Ease.Linear)
            .WaitForCompletion();

        yield return flashImage
            .DOFade(0f, totalFlashSeconds / 2)
            .SetEase(Ease.Linear)
            .WaitForCompletion();
        flashImage.gameObject.SetActive(false);
    }
}
