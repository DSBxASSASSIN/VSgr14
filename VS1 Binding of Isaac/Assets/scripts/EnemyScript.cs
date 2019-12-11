using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    private int health = 5;
    SpriteRenderer sr;
    public GameObject DeathEffect;
    
    void Update(){
        Move();
    }
    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.CompareTag(Constants.Tags.TEAR)){
            Destroy(collision.gameObject);

            health--;
            transform.gameObject.GetComponent<SpriteRenderer>().color =  new Color(0.509f, 0.0f, 0.0f, 1.0f);

            
            if(health <= 0){
                killSelf();
            }
            else{
                Invoke("ResetMaterial", .2f);
            }
        }
    }

    void ResetMaterial(){
        transform.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }

    private void killSelf(){
        GameObject effect = Instantiate(DeathEffect, transform.position, Quaternion.identity);
        Destroy(effect, .2f);
        Destroy(gameObject);
    }

    void Move(){
        
    }
}
