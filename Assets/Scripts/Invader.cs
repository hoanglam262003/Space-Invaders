using UnityEngine;
using UnityEngine.Rendering;

public class Invader : MonoBehaviour
{
    public Sprite[] sprites;
    public float animationTime = 1.0f;
    public System.Action killed;

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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Laser"))
        {
            this.killed.Invoke();
            this.gameObject.SetActive(false);
        }
    }
}
