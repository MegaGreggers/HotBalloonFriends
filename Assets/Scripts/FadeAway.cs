
using UnityEngine;

public class FadeAway : MonoBehaviour {

    public float fadeAmountPerFrame = 0.01f;
    private SpriteRenderer sprite;
    private float tempAlpha = 0f;
    private Color color;

    void Start ()
    {
        sprite = GetComponent<SpriteRenderer>();
        tempAlpha = sprite.color.a;
        color = sprite.color;
    }
	
	void Update ()
    {
        
        tempAlpha -= fadeAmountPerFrame;
        color.a = tempAlpha;
        sprite.color = color;
        if(tempAlpha <= 0)
        {
            // later we will object pool these cloud puffs
            Destroy(gameObject, 0f);
        }
    }
}
