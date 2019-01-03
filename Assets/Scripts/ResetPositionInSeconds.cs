using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPositionInSeconds : MonoBehaviour
{

    public float secondsUntilReset = 5.0f;

    private Vector3 initionalPosition;
    private float tempTimer;

    // Use this for initialization
    void Start ()
    {
        initionalPosition = transform.position;
        tempTimer = secondsUntilReset;
	}
	
	// Update is called once per frame
	void LateUpdate ()
    {
        tempTimer -= Time.deltaTime;
        // Debug.Log(tempTimer);

        if(tempTimer <= 0.0f)
        {
            if (GetComponent<CloudMover>())
            {
                GetComponent<CloudMover>().pos = initionalPosition;
            }
            else
            {
                transform.localPosition = initionalPosition;
            }
            tempTimer = secondsUntilReset;
        }
	}
}
