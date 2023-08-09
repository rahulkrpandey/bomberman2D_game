using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleBehaviour : MonoBehaviour
{
    bool destroyed = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (destroyed) {
            return;
        }

        if (collision.name == "BomberMan") {
            GameEvents.OnCollectionEnter();
            destroyed = true;
            SoundManager.sm.PlayCollected();
            Destroy(gameObject);
        }
    }
}
