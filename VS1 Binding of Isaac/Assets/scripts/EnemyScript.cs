using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    private int health = 5;
    SpriteRenderer sr;
    

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
        Destroy(gameObject);
    }
}
