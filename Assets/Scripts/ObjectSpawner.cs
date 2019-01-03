using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    // private int maxSpawnedObjects = 10;
    private float verticalSpawnRange_Max = 10f;
    private float verticalSpawnRange_Min = -10f;

    public float spawnDelay = 5.0f;
    public float spawnRandomTimeRange = 1.5f;
    private float tempTimer = 0f;

    private void Start()
    {
        tempTimer = spawnDelay + Random.Range(-spawnRandomTimeRange, spawnRandomTimeRange);
    }

    private void Update()
    {
        tempTimer -= Time.deltaTime;
        if(tempTimer <= 0)
        {
            SpawnSeagull();
            tempTimer = spawnDelay + Random.Range(-spawnRandomTimeRange, spawnRandomTimeRange);
        }
    }

    public void SpawnSeagull()
    {
        Vector3 randomSpawnOffset = new Vector3(transform.position.x, transform.position.y + Random.Range(verticalSpawnRange_Min,verticalSpawnRange_Max), transform.position.z);
        ObjectPooler.Instance.SpawnFromPool("SeagullPool", randomSpawnOffset, Quaternion.identity);
    }

    /*
    void OnGUI()
    {
        GUIStyle gUIStyleWhite = new GUIStyle();
        GUIStyle gUIStyleBlack = new GUIStyle();

        gUIStyleWhite.normal.textColor = Color.white;
        gUIStyleBlack.normal.textColor = Color.black;

        GUI.Label(new Rect(new Vector2(10, 10), new Vector2(500, 200)), "tempTime: " + tempTimer, gUIStyleBlack);
        GUI.Label(new Rect(new Vector2(9, 9), new Vector2(500, 200)), "tempTime: " + tempTimer, gUIStyleWhite);
    }
    */
}
