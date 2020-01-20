using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{    
    private Rigidbody rb;
    private float disToGround = 1.5f;
    [SerializeField]
    private int _currentJumpAmmount = 2;
    [SerializeField]
    private float _speed = 8;
    private int _health = 100;
    private int _maxJumps = 1;
    private float _jumpforce = 8f;
    
 

    private void Start(){
       rb = GetComponent<Rigidbody>();
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.Space)){
            if(isGrounded() &&_currentJumpAmmount > 0){
                Vector3 jumpVelocity = new Vector3(0f, _jumpforce, 0f);
                rb.velocity += jumpVelocity;
                _currentJumpAmmount--;
            }else if(!isGrounded() && _currentJumpAmmount > 0){
                 Vector3 jumpVelocity = new Vector3(0f, _jumpforce, 0f);
                rb.velocity = Vector3.zero;
                rb.velocity += jumpVelocity;
                _currentJumpAmmount--;    
            }
        }

        if(isGrounded()){
            _currentJumpAmmount = _maxJumps;
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(horizontal * _speed * Time.deltaTime, 0, vertical * _speed * Time.deltaTime);
        rb.MovePosition(transform.position + movement);

    }

    bool isGrounded(){
        return Physics.Raycast(transform.position, Vector3.down, disToGround);
    }
    public void takeDamage(int damage){
        _health -= damage;
    }
}
