using UnityEngine;
using System.Collections;

public class AnimatedSpriteRenderer : MonoBehaviour
{
    public Sprite[] sprites;
    public float animationSpeed = 0.1f;
    public bool loop = true;

    private void Start()
    {
        StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        while (true)
        {
            for (int i = 0; i < sprites.Length; i++)
            {
                if (sprites[i] != null)
                {
                    GetComponent<SpriteRenderer>().sprite = sprites[i];
                }
                yield return new WaitForSeconds(animationSpeed);
            }

            if (!loop)
            {
                break;
            }
        }
    }
}
