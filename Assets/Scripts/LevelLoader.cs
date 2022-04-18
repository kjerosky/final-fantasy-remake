using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour {

    public Animator transitionAnimator;
    public float transitionTime = 1.25f;

    public void startTransition() {
        if (SceneManager.GetActiveScene().name == "SampleScene") {
            StartCoroutine(loadLevel("TestTown"));
        } else {
            StartCoroutine(loadLevel("SampleScene"));
        }
    }

    private IEnumerator loadLevel(string levelName) {
        transitionAnimator.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelName);
    } 
}
