using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonIsVisible : MonoBehaviour {

    private HotAirBalloon HAB_Script;

	
	void Start ()
    {
        HAB_Script = transform.parent.GetComponent<HotAirBalloon>();
        if(HAB_Script == null)
        {
            Debug.LogWarning("BalloonIsVisible script can't find parent HotAirBalloon script.");
        }
	}

    private void OnBecameInvisible()
    {
        HAB_Script.playerIsOffScreen = true;
    }

    private void OnBecameVisible()
    {
        HAB_Script.playerIsOffScreen = false;
    }
}
