using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class BattleTransitionManager : MonoBehaviour {

    [SerializeField] Player player;
    [SerializeField] Camera playerCamera;
    [SerializeField] Image outsideBattleTransitionFullscreenImage;
    [SerializeField] Color intoBattleStartColor;
    [SerializeField] Color outOfBattleStartColor;
    [SerializeField] Color beginBattleStartColor;
    [SerializeField] Color endBattleStartColor;
    [SerializeField] float intoBattleSeconds;
    [SerializeField] float outOfBattleSeconds;
    [SerializeField] float startBattleSeconds;
    [SerializeField] float endBattleSeconds;

    public event Action OnStartTransitionIntoBattle;
    public event Action OnEndTransitionIntoBattle;
    public event Action OnStartTransitionOutOfBattle;
    public event Action OnEndTransitionOutOfBattle;

    private int returnSceneIndex;
    private Image battleSceneTransitionFullscreenImage;

    public void transitionIntoBattle(Transform initiator) {
        StartCoroutine(performTransitionIntoBattle(initiator.position));
    }

    public void transitionOutOfBattle() {
        StartCoroutine(performTransitionOutOfBattle());
    }

    private IEnumerator performTransitionIntoBattle(Vector3 initiatorPosition) {
        OnStartTransitionIntoBattle?.Invoke();

        Vector3 playerCameraOriginalPosition = playerCamera.transform.position;
        float playerCameraOriginalOrthographicSize = playerCamera.orthographicSize;

        yield return animateTransitionIntoBattleInWorld(initiatorPosition);

        returnSceneIndex = SceneManager.GetActiveScene().buildIndex;
        yield return SceneManager.LoadSceneAsync("Battle");

        playerCamera.gameObject.SetActive(false);
        playerCamera.transform.position = playerCameraOriginalPosition;
        playerCamera.orthographicSize = playerCameraOriginalOrthographicSize;
        outsideBattleTransitionFullscreenImage.gameObject.SetActive(false);

        animateTransitionIntoBattle();

        OnEndTransitionIntoBattle?.Invoke();
    }

    private IEnumerator performTransitionOutOfBattle() {
        OnStartTransitionOutOfBattle?.Invoke();

        yield return animateTransitionOutOfBattle();

        yield return SceneManager.LoadSceneAsync(returnSceneIndex);
        playerCamera.gameObject.SetActive(true);
        player.handleBattleSceneUnloaded();

        yield return animateTransitionOutOfBattleInWorld();

        OnEndTransitionOutOfBattle?.Invoke();
    }

    private IEnumerator animateTransitionIntoBattleInWorld(Vector3 initiatorPosition) {
        outsideBattleTransitionFullscreenImage.gameObject.SetActive(true);
        outsideBattleTransitionFullscreenImage.color = intoBattleStartColor;
        outsideBattleTransitionFullscreenImage
            .DOFade(1f, intoBattleSeconds)
            .SetEase(Ease.InCubic);

        playerCamera.transform
            .DOMoveX(initiatorPosition.x, intoBattleSeconds)
            .SetEase(Ease.Linear);
        playerCamera.transform
            .DOMoveY(initiatorPosition.y, intoBattleSeconds)
            .SetEase(Ease.Linear);
        yield return playerCamera
            .DOOrthoSize(0.15f, intoBattleSeconds)
            .SetEase(Ease.InBack)
            .WaitForCompletion();
    }

    private void animateTransitionIntoBattle() {
        battleSceneTransitionFullscreenImage = GameObject.Find("BattleTransitionFullscreenImage").GetComponent<Image>();
        battleSceneTransitionFullscreenImage.color = beginBattleStartColor;
        battleSceneTransitionFullscreenImage
            .DOFade(0f, startBattleSeconds)
            .SetEase(Ease.Linear)
            .OnComplete(() => battleSceneTransitionFullscreenImage.gameObject.SetActive(false));
    }

    private IEnumerator animateTransitionOutOfBattle() {
        battleSceneTransitionFullscreenImage.color = endBattleStartColor;
        battleSceneTransitionFullscreenImage.gameObject.SetActive(true);
        yield return battleSceneTransitionFullscreenImage
            .DOFade(1f, endBattleSeconds)
            .SetEase(Ease.Linear)
            .WaitForCompletion();
    }

    private IEnumerator animateTransitionOutOfBattleInWorld() {
        outsideBattleTransitionFullscreenImage.gameObject.SetActive(true);
        outsideBattleTransitionFullscreenImage.color = outOfBattleStartColor;
        yield return outsideBattleTransitionFullscreenImage
            .DOFade(0f, outOfBattleSeconds)
            .SetEase(Ease.Linear)
            .WaitForCompletion();

        outsideBattleTransitionFullscreenImage.gameObject.SetActive(false);
    }
}
