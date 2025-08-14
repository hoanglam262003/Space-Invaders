using UnityEngine;
using UnityEngine.Rendering;

public class Invader : MonoBehaviour
{
    public Sprite[] sprites;
    public float animationTime = 1.0f;
    private SpriteRenderer spriteRenderer;
    private int animationFrame;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        InvokeRepeating(nameof(AnimateSprite), this.animationTime, this.animationTime);
    }

    private void AnimateSprite()
    {
        animationFrame++;

        if (animationFrame >= this.sprites.Length)
        {
            animationFrame = 0;
        }

        spriteRenderer.sprite = this.sprites[animationFrame];
    }
}
