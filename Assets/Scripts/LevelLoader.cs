using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour {

    public Animator transitionAnimator;
    public float transitionTime = 1.25f;
    public CanvasGroup transitionCanvasGroup;

    void Awake() {
        transitionCanvasGroup.alpha = 1f;
    }

    public IEnumerator transitionOutOfScene() {
        transitionAnimator.SetBool("shouldCloseBars", true);
        yield return new WaitForSeconds(transitionTime);
    }

    public IEnumerator transitionIntoScene() {
        transitionAnimator.SetBool("shouldCloseBars", false);
        yield return new WaitForSeconds(transitionTime);
    }
}
