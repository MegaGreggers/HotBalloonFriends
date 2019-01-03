using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetFire : MonoBehaviour
{
    GameObject fireParticleSystem;
	// Use this for initialization
	void Start ()
    {
        fireParticleSystem = transform.GetChild(0).gameObject;
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Fire"))
        {
            fireParticleSystem.SetActive(true);
            transform.parent.parent.GetComponent<HotAirBalloon>().isOnFire = true;
        }
    }
}
