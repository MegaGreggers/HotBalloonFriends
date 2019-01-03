using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonJellySpriteBehaviour : MonoBehaviour
{
    void PopJellyLetter()
    {
        // Spawn POP FX:
        FindObjectOfType<AudioManager>().Play("Pop");
        ObjectPooler.Instance.SpawnFromPool("FX_BalloonPOP_01_Pool", transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    void OnJellyCollisionEnter2D(JellySprite.JellyCollision2D collision)
    {
        if (collision.Collision2D.gameObject.CompareTag("Basket"))
        {
            PopJellyLetter();
        }

        if (collision.Collision2D.gameObject.CompareTag("Balloon"))
        {
            if (collision.Collision2D.gameObject.GetComponent<HotAirBalloon>())
            {
                if (collision.Collision2D.gameObject.GetComponent<HotAirBalloon>().isOnFire)
                {
                    PopJellyLetter();
                }
            }
        }
    }
}
