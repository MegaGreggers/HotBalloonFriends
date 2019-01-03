using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCollector : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Seagull"))
        {
            ObjectPooler.Instance.ReturnToPool("SeagullPool", other.gameObject);
        }
    }
}
