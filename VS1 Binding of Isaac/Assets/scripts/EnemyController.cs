using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Enemystate{
    Wander,
    Die
};

public class EnemyController : MonoBehaviour
{

    GameObject player;
    Rigidbody2D rb;
    Vector2 movement;
    public Enemystate currState = Enemystate.Wander;
    public float speed;
    private bool chooseDir = false;
    private bool dead = false;
    private Vector3 randomDir;
    private Animator animator;
    
    void Start(){
        player = GameObject.FindGameObjectWithTag(Constants.Tags.PLAYER);
        rb = transform.GetComponent<Rigidbody2D>();
        animator = transform.GetComponent<Animator>();
    }
    
    void Update(){
        if(movement.x < 0){
            animator.SetBool("isMovingLeft", true);
        }else if(movement.x > 0){
            animator.SetBool("isMovingRight", true);
        }

        if(movement.y < 0){
            animator.SetBool("isMovingUpDown", true);
        }else if(movement.y > 0){
            animator.SetBool("isMovingUpDown", true);
        }

        if(movement.x == 0){
            animator.SetBool("isMovingLeft", false);
            animator.SetBool("isMovingRight", false);
        }

        if(movement.y == 0){
            animator.SetBool("isMovingUpDown", false);
            animator.SetBool("isMovingUpDown", false);
        }


        if(currState != Enemystate.Die){
            currState = Enemystate.Wander;
        }

        if(currState == Enemystate.Wander){
            Wander();
        }
    }

    private IEnumerator chooseDirection(){
        chooseDir = true;
        yield return new WaitForSeconds(Random.Range(2f, 8f));
        movement.x = Random.Range(-1f, 1f);
        movement.y = Random.Range(-1f, 1f);
        chooseDir = false;
    }
    void Wander(){
        if(!chooseDir){
            StartCoroutine(chooseDirection());
        }

         rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);

    }
    private int health = 3;
    SpriteRenderer sr;
    public GameObject DeathEffect;

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

    
}
