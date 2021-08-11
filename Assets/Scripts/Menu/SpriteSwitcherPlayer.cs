using UnityEngine;
using UnityEngine.UI;

public class SpriteSwitcherPlayer : MonoBehaviour
{
    public Sprite[] sprites;
    public float interval = 0.1f;

    Image image;
    SpriteRenderer sr;
    float curTime;
    int spriteIdx;

    void Start()
    {
        image = GetComponent<Image>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        curTime += Time.deltaTime;
        if (curTime > interval)
        {
            curTime -= interval;
            spriteIdx = (spriteIdx + 1) % sprites.Length;

            if (image)
                image.sprite = sprites[spriteIdx];

            if (sr)
                sr.sprite = sprites[spriteIdx];
        }
    }
}