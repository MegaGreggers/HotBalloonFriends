using UnityEngine;

public class BalloonBasket : MonoBehaviour
{
    public bool myBalloonIsPopped = false;
    public HotAirBalloon HAB_Script;

    private void Start()
    {
        HAB_Script = transform.parent.GetComponent<HotAirBalloon>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            float randFloat = Random.Range(0f, 1f);
            int index = Mathf.RoundToInt(randFloat);

            switch (index)
            {
                case 0:
                    FindObjectOfType<AudioManager>().Play("Hit_Alex_Ugh_1");
                    return;
                case 1:
                    FindObjectOfType<AudioManager>().Play("Hit_Alex_Ugh_2");
                    return;
            }
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Coin"))
        {
            FindObjectOfType<AudioManager>().Play("Coin");
            HAB_Script.coinCounter.UpdateCounter(1);
            BalloonGameManager.Instance.ChangeScore((int)HAB_Script.thisPlayer, 1, BalloonGameManager.statType.coins);
            ObjectPooler.Instance.SpawnFromPool("FX_CoinGETPool", transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("PurpleCoin"))
        {
            FindObjectOfType<AudioManager>().Play("PurpleCoin");
            HAB_Script.coinCounter.UpdateCounter(5);
            BalloonGameManager.Instance.ChangeScore((int)HAB_Script.thisPlayer, 5, BalloonGameManager.statType.coins);
            ObjectPooler.Instance.SpawnFromPool("FX_CoinGETPool", transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
        }

        
    }
    
}
