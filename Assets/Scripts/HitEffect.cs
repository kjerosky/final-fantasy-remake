using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitEffect : MonoBehaviour {

    [SerializeField] Image effectImage;
    [SerializeField] List<Sprite> animationFrames;
    [SerializeField] float durationSeconds;

    public IEnumerator animate(int damageTaken) {
        if (damageTaken <= 0) {
            yield return new WaitForSeconds(durationSeconds);
            yield break;
        }

        float frameSeconds = durationSeconds / animationFrames.Count;
        yield return animateFrames(frameSeconds);
    }

    private IEnumerator animateFrames(float frameSeconds) {
        effectImage.gameObject.SetActive(true);

        float timer = 0f;
        for (int i = 0; i < animationFrames.Count; i++) {
            effectImage.sprite = animationFrames[i];

            while (timer < frameSeconds) {
                timer += Time.deltaTime;
                yield return null;
            }

            while (timer >= frameSeconds) {
                timer -= frameSeconds;
            }
        }

        effectImage.gameObject.SetActive(false);
    }
}
