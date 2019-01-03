using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMover : MonoBehaviour
{
    public float moveSpeed = -5.0f;
    public float frequency = 2.0f;   // Speed of sine movement
    public float magnitude = 0.5f;   // Size of sine movement
    public float offset = 0.00f;
    [HideInInspector]
    public Vector3 pos;


    public float poofForce = 0.1f;
    public GameObject puff_FX;
    public List<GameObject> droppableItems;
    private float droppedItemsForce = 300f;

    private Vector3 axis;

    private Vector2 angleToCollision;

    /*
    public void OnObjectSpawn() // for use with ObjectPooler
    {
        pos = transform.position;
        axis = transform.up;            // May or may not be the axis you want
    }
    */
    
    public void Start()
    {
        pos = transform.position;
        axis = transform.up;
        if(puff_FX != null)
            puff_FX.SetActive(false);
        if(offset == -1)
            offset = Random.Range(0f,2f);
    }

    void Update()
    {
        Move();
    }

    private void Move()
    {
        pos += transform.right * Time.deltaTime * moveSpeed;
        transform.position = pos + axis * Mathf.Sin((Time.time + offset) * frequency) * magnitude;
    }

    
    public void OnTriggerEnter2D(Collider2D collision)
    {     
        if (collision.gameObject.CompareTag("Balloon") || collision.gameObject.CompareTag("Basket"))
        {
            // Debug.Log("player triggered cloud!");
            angleToCollision = transform.position - collision.gameObject.transform.position;

            GameObject puffInstance = Instantiate(puff_FX, collision.transform.position, Quaternion.identity) as GameObject;
            puffInstance.SetActive(true);
            // FadeAway fade_a = puffInstance.gameObject.AddComponent<FadeAway>();
            // fade_a.fadeAmountPerFrame = Random.Range(0.005f, 0.02f);

            SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
            Collider2D[] collider2DArray = GetComponentsInChildren<Collider2D>();
            for (int i = 0; i < spriteRenderers.Length; i++)
            {
                spriteRenderers[i].enabled = true;
                collider2DArray[i].enabled = true;
                spriteRenderers[i].transform.parent = null;
                // Cloud Poofs AT you:
                // spriteRenderers[i].gameObject.GetComponent<Rigidbody2D>().AddForceAtPosition( gameObject.transform.position - collision.transform.position * poofForce, collision.gameObject.transform.position);
                spriteRenderers[i].gameObject.GetComponent<Rigidbody2D>().AddForceAtPosition(Vector3.Normalize(collision.transform.position - gameObject.transform.position) * poofForce, collision.gameObject.transform.position);
                FadeAway fade_b = spriteRenderers[i].gameObject.AddComponent<FadeAway>();
                fade_b.fadeAmountPerFrame = Random.Range(0.005f, 0.02f);
            }
            GetComponent<Collider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;

            // Play "POOF!" sound
            PlayRandomPoofSound();

            // Slow down Player's Balloon
            Vector2 velocity = collision.attachedRigidbody.velocity;
            collision.attachedRigidbody.velocity = new Vector2(0.2f, 0.2f);
            DropRandomItems((int)Mathf.Floor(Random.Range(1f, 5f)));
            Destroy(gameObject, 2f);
        }
    }

    private void DropRandomItems(int numDropped)
    {
        if(droppableItems.Count > 0)
        {
            for (int i = 0; i < numDropped; i++)
            {
                int randItemIndex = (int)Mathf.Floor(Random.Range(0f, droppableItems.Count));
                GameObject droppedItem = Instantiate(droppableItems[randItemIndex]) as GameObject;
                droppedItem.transform.position = transform.position;
                droppedItem.GetComponent<Rigidbody2D>().AddForce(droppedItemsForce * Random.Range(0.2f,1f) * angleToCollision);
            }
        }
    }

    private void PlayRandomPoofSound()
    {
        int indexToPlay = 0;

        indexToPlay = (int)Mathf.Floor(Random.Range(0f, 3f));

        switch (indexToPlay)
        {
            case 0:
                AudioManager.instance.Play("Poof_1");
                return;
            case 1:
                AudioManager.instance.Play("Poof_2");
                return;
            case 2:
                AudioManager.instance.Play("Poof_3");
                return;
        }
    }

    /*
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Balloon"))
        {
            
            // GetComponent<SpriteRenderer>().enabled = false;
            // Debug.Log("player hit cloud!");

            SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
            Collider2D[] collider2DArray = GetComponentsInChildren<Collider2D>();
            for (int i = 0; i < spriteRenderers.Length; i++)
            {
                spriteRenderers[i].enabled = true;
                collider2DArray[i].enabled = true;
                // spriteRenderers[i].transform.parent = null;
            }

            GetComponent<Collider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;

            // Play "POOF!" sound
            // Slow down Player's Balloon

            Destroy(gameObject, 2f);
        }
    }
    */

}
