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

    public void startTransition(string nextScene, int nextPlayerX, int nextPlayerY) {
        SceneTransitionData.setNextPlayerPosition(nextPlayerX, nextPlayerY);
        StartCoroutine(loadLevel(nextScene));
    }

    private IEnumerator loadLevel(string levelName) {
        transitionAnimator.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelName);
    } 
}
