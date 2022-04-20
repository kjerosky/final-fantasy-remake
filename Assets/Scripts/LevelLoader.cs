using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour {

    public Animator transitionAnimator;
    public float transitionTime = 1.25f;
    public CanvasGroup transitionCanvasGroup;

    private SceneTransitionData sceneTransitionData;

    void Awake() {
        sceneTransitionData = GameObject.Find("SceneTransitionData").GetComponent<SceneTransitionData>();
        transitionCanvasGroup.alpha = 1f;
    }

    //TODO REMOVE THIS TEMPORARY CODE!!!
    void Update() {
        if (Input.GetKeyDown(KeyCode.N)) {
            startTransition("SampleScene", 153, 92);
        }
    }

    public void startTransition(string nextScene, int nextPlayerX, int nextPlayerY) {
        sceneTransitionData.setNextPlayerPosition(nextPlayerX, nextPlayerY);
        StartCoroutine(loadLevel(nextScene));
    }

    private IEnumerator loadLevel(string levelName) {
        transitionAnimator.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelName);
    } 
}
