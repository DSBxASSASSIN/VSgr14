using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Image _healthBar;
    private float _startHealth = 100;
    private Rigidbody rb;
    private float _disToGround = 1.5f;
    private int _currentJumpAmmount = 2;
    private float _speed = 8;
    private float _health;
    private int _maxJumps = 2;
    private float _jumpforce = 8f;
    

    private void Start(){
       rb = GetComponent<Rigidbody>();
        _health = _startHealth;
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.Space)){
            if(_currentJumpAmmount > 0){
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
        _healthBar.fillAmount = _health / _startHealth;

        if (isGrounded()){
            _currentJumpAmmount = _maxJumps;
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(horizontal * _speed * Time.deltaTime, 0, vertical * _speed * Time.deltaTime);
        rb.MovePosition(transform.position + movement);
    }

    bool isGrounded(){
        return Physics.Raycast(transform.position, Vector3.down, _disToGround);
    }
    public void takeDamage(int damage){
        _health -= damage;
    }
}
