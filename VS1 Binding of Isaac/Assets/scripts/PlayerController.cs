using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    Vector2 movement;
    [SerializeField]private float _health = 3;
     private Animator animator;

    void Start(){
      animator = GetComponent<Animator>();   
    }
    
    void Update() {
        movement.x = Input.GetAxisRaw(Constants.Input.HORIZON);
        movement.y = Input.GetAxisRaw(Constants.Input.VERTICAL);

        if(movement.x < 0){
            animator.SetBool("isMovingLeft", true);
        }else if(movement.x > 0){
            animator.SetBool("isMovingRight", true);
        }

        if(movement.y < 0){
            animator.SetBool("isMovingUp", true);
        }else if(movement.y > 0){
            animator.SetBool("isMovingDown", true);
        }

        if(movement.x == 0){
            animator.SetBool("isMovingLeft", false);
            animator.SetBool("isMovingRight", false);
        }

        if(movement.y == 0){
            animator.SetBool("isMovingUp", false);
            animator.SetBool("isMovingDown", false);
        }
    }

    void FixedUpdate() {
        rb.MovePosition(rb.position + movement * Constants.Player.SPEED * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision){
        Debug.Log(collision.transform.tag);
        if(collision.transform.tag == "EnemyTear"){
            _health -= .5f;
            transform.gameObject.GetComponent<SpriteRenderer>().color = new Color(0.509f, 0.0f, 0.0f, 1.0f);

            if(_health <= 0){
                Destroy(gameObject);
            }else{
                Invoke("resetMaterial", .2f);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D col){
        if(col.gameObject.CompareTag(Constants.Tags.ENEMY)){
            _health -= .5f;
            transform.gameObject.GetComponent<SpriteRenderer>().color = new Color(0.509f, 0.0f, 0.0f, 1.0f);

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
