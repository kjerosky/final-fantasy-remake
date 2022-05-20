using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionManager : MonoBehaviour {

    [SerializeField] GameObject transition;
    [SerializeField] GameObject transitionTopBar;
    [SerializeField] GameObject transitionBottomBar;
    [SerializeField] float transitionSeconds;

    public event Action OnStartTransition;
    public event Action OnEndTransition;

    void Start() {
        StartCoroutine(endTransition());
    }

    public IEnumerator startTransition() {
        OnStartTransition?.Invoke();
        transition.SetActive(true);

        yield return changeBarScales(0f, 1f);
    }

    public IEnumerator endTransition() {
        yield return changeBarScales(1f, 0f);

        transition.SetActive(false);
        OnEndTransition?.Invoke();
    }

    private IEnumerator changeBarScales(float startScale, float endScale) {
        float barScaleRate = Mathf.Abs(endScale - startScale) / transitionSeconds;

        Vector3 startingScale = new Vector3(1f, startScale, 1f);
        transitionTopBar.transform.localScale = startingScale;
        transitionBottomBar.transform.localScale = startingScale;

        Vector3 targetScale = new Vector3(1f, endScale, 1f);
        while (transitionTopBar.transform.localScale != targetScale) {
            transitionTopBar.transform.localScale = Vector3.MoveTowards(
                transitionTopBar.transform.localScale, targetScale, barScaleRate * Time.deltaTime
            );
            transitionBottomBar.transform.localScale = Vector3.MoveTowards(
                transitionBottomBar.transform.localScale, targetScale, barScaleRate * Time.deltaTime
            );
            yield return null;
        }

        transitionTopBar.transform.localScale = new Vector3(1f, endScale, 1f);
        transitionBottomBar.transform.localScale = new Vector3(1f, endScale, 1f);
    }
}
