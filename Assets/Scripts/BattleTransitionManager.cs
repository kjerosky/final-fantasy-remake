using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleTransitionManager : MonoBehaviour {

    [SerializeField] Player player;

    public event Action OnStartTransitionIntoBattle;
    public event Action OnEndTransitionIntoBattle;
    public event Action OnStartTransitionOutOfBattle;
    public event Action OnEndTransitionOutOfBattle;

    private int returnSceneIndex;
    private GameObject playerCamera;

    public void transitionIntoBattle() {
        StartCoroutine(performTransitionIntoBattle());
    }

    public void transitionOutOfBattle() {
        StartCoroutine(performTransitionOutOfBattle());
    }

    private IEnumerator performTransitionIntoBattle() {
        OnStartTransitionIntoBattle?.Invoke();

        //TODO PERFORM START ANIMATION(S) INTO BATTLE AND DATA COLLECTION
        returnSceneIndex = SceneManager.GetActiveScene().buildIndex;
        yield return SceneManager.LoadSceneAsync("Battle");

        playerCamera = GameObject.Find("PlayerCamera");
        playerCamera.SetActive(false);

        //TODO PERFORM END ANIMATION(S) INTO BATTLE

        OnEndTransitionIntoBattle?.Invoke();
    }

    private IEnumerator performTransitionOutOfBattle() {
        OnStartTransitionOutOfBattle?.Invoke();

        //TODO PERFORM START ANIMATION(S) OUT OF BATTLE

        //TODO LOAD THE PREVIOUS SCENE CORRECTLY!!!!!!!!!!!!!!!!!!!!!!!!!!!
        yield return SceneManager.LoadSceneAsync(returnSceneIndex);
        playerCamera.SetActive(true);
        player.handleBattleSceneUnloaded();

        //TODO PERFORM END ANIMATION(S) OUT OF BATTLE

        OnEndTransitionOutOfBattle?.Invoke();
    }
}
