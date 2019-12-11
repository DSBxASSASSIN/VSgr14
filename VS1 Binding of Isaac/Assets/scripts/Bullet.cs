using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public GameObject hitEffect;


    void Start() {
        Destroy(gameObject, 1f);
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.transform.tag != Constants.Tags.PLAYER && collision.transform.tag != Constants.Tags.TEAR) {
            GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
            Destroy(effect);
            Destroy(gameObject, 6f);
        }
    }

}  