using UnityEngine;

public class Bullet : MonoBehaviour {
    public GameObject hitEffect;
    public GameObject EnemyhitEffect;
    public bool Isenemy;

    private Vector2 lastPos;
    private Vector2 curPos;
    private Vector2 playerPos;

    public float bulletForce = 7f;

    void Start() {
        Destroy(gameObject, 1f);
    }

    void Update(){
        if(Isenemy){
            if(GameObject.FindWithTag(Constants.Tags.PLAYER) != null){
                playerPos = GameObject.FindWithTag(Constants.Tags.PLAYER).transform.position;
            }

            curPos = transform.position;
            transform.position = Vector2.MoveTowards(transform.position, playerPos, bulletForce * Time.deltaTime);
            if(curPos == lastPos){
                Destroy(gameObject);
            }
            lastPos = curPos;
        }
    }

    void OnTriggerEnter2D(Collider2D collision) {

        if(!Isenemy){
            if (collision.transform.tag != "Player" && collision.transform.tag != "Tear" && collision.transform.tag != "EnemyTear") {
                GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
                Destroy(effect, .2f);
                Destroy(gameObject);
            }
        }

        if(Isenemy){
            if(collision.transform.tag != "Enemy" && collision.transform.tag != "EnemyTear" && collision.transform.tag != "Tear"){
                GameObject Enemyeffect = Instantiate(EnemyhitEffect, transform.position, Quaternion.identity);
                Destroy(Enemyeffect, .2f);
                Destroy(gameObject);
            }
        }
    }

}  