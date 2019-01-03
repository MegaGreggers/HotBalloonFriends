using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeagullMover : MonoBehaviour, IPooledObject
{
    public float moveSpeed = -5.0f;
    public float frequency = 2.0f;      // Speed of sine movement
    public float magnitude = 0.5f;      // Size of sine movement

    private Vector3 axis;
    private Vector3 pos;

    public void OnObjectSpawn()
    {
        pos = transform.position;
        axis = transform.up;            // May or may not be the axis you want
        PlayRandomSeagullSound();
    }

    void Update()
    {
        Move();
        // Play sounds
        // Attack Players
        // if m_Dead Death()
    }

    private void Move()
    { 
        pos += transform.right* Time.deltaTime * moveSpeed;
        transform.position = pos + axis * Mathf.Sin(Time.time* frequency) * magnitude;
    }

    private void PlayRandomSeagullSound()
    {
        int indexToPlay = 0;

        indexToPlay = (int)Mathf.Floor(Random.Range(0f,5f));

        switch (indexToPlay)
        {
            case 0:
                AudioManager.instance.Play("Seagull_1");
                return;
            case 1:
                AudioManager.instance.Play("Seagull_2");
                return;
            case 2:
                AudioManager.instance.Play("Seagull_3");
                return;
            case 3:
                AudioManager.instance.Play("Seagull_4");
                return;
            case 4:
                AudioManager.instance.Play("Seagull_5");
                return;
        }
    }
}
