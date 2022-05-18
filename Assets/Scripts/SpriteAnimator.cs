using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimator {

    private SpriteRenderer spriteRenderer;
    public List<Sprite> frames { get; set; }
    public float frameTime { get; set; }

    private int currentFrame;
    private float timeInCurrentFrame;

    public SpriteAnimator(SpriteRenderer spriteRenderer, List<Sprite> frames, float frameTime = 0.0167f) {
        this.spriteRenderer = spriteRenderer;
        this.frames = frames;
        this.frameTime = frameTime;
    }

    public void restart() {
        currentFrame = 0;
        timeInCurrentFrame = 0f;
        spriteRenderer.sprite = frames[0];
    }

    public void handleUpdate() {
        timeInCurrentFrame += Time.deltaTime;
        while (timeInCurrentFrame > frameTime) {
            currentFrame = (currentFrame + 1) % frames.Count;
            spriteRenderer.sprite = frames[currentFrame];
            timeInCurrentFrame -= frameTime;
        }
    }
}
