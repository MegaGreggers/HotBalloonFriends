using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToPoolAfterSeconds : MonoBehaviour {

    public float secondsUntilReturn = 1.0f;
    public string poolToReturnTo;

    private float timeDelay;

	// Use this for initialization
	void Start ()
    {
        timeDelay = secondsUntilReturn;
	}
	
	// Update is called once per frame
	void Update ()
    {
        timeDelay -= Time.deltaTime;
        if (timeDelay <= 0)
        {
            ObjectPooler.Instance.ReturnToPool(poolToReturnTo, gameObject);
        }
        
    }

    private void OnDisable()
    {
        timeDelay = secondsUntilReturn;
    }
}
