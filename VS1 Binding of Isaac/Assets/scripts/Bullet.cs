using UnityEngine;

public class Bullet : MonoBehaviour {
    public GameObject hitEffect;
    public GameObject EnemyhitEffect;
    public bool Isenemy;

    void Start() {
        Destroy(gameObject, 1f);
    }

    void OnTriggerEnter2D(Collider2D collision) {

        if(!Isenemy){
            if (collision.transform.tag != Constants.Tags.PLAYER && collision.transform.tag != Constants.Tags.TEAR) {
                GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
                Destroy(effect, .2f);
                Destroy(gameObject);
            }
        }

        if(Isenemy){
           
            if(collision.transform.tag != Constants.Tags.ENEMY){
                GameObject Enemyeffect = Instantiate(EnemyhitEffect, transform.position, Quaternion.identity);
                Destroy(Enemyeffect, .2f);
                Destroy(gameObject);
            }
        }
    }

}  