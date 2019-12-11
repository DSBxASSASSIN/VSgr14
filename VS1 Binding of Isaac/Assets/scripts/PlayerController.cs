using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    Vector2 movement;

    private float _health = 3;

    void Update() {
        movement.x = Input.GetAxisRaw(Constants.Input.HORIZON);
        movement.y = Input.GetAxisRaw(Constants.Input.VERTICAL);
    }

    void FixedUpdate() {
        rb.MovePosition(rb.position + movement * Constants.Player.SPEED * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.CompareTag(Constants.Tags.ENEMY)){
            _health -= .5f;
            transform.gameObject.GetComponent<SpriteRenderer>().color = Color.red;

            if(_health <= 0){
                Destroy(gameObject);
            }else{
                Invoke("resetMaterial", .2f);
            }
        }
    }

    void resetMaterial(){
        transform.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }
}
