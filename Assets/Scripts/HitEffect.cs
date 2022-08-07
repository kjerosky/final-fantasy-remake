using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitEffect : MonoBehaviour {

    [SerializeField] Image effectImage;
    [SerializeField] List<Sprite> sharpAttackAnimationFrames;
    [SerializeField] List<Sprite> bluntAttackAnimationFrames;
    [SerializeField] float durationSeconds;

    public IEnumerator animate(DamageCalculationResult damageCalculationResult, WeaponHitType weaponHitType) {
        int damageTaken = damageCalculationResult.Damage;
        if (damageTaken <= 0) {
            yield return new WaitForSeconds(durationSeconds);
            yield break;
        }

        if (damageCalculationResult.WasCritical) {
            BattleComponents.Instance.ScreenFlash.flash();
        }

        yield return animateFrames(weaponHitType);
    }

    private IEnumerator animateFrames(WeaponHitType weaponHitType) {
        List<Sprite> animationFrames = weaponHitType == WeaponHitType.BLUNT ? bluntAttackAnimationFrames : sharpAttackAnimationFrames;
        float frameSeconds = durationSeconds / animationFrames.Count;

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
