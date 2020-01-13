using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{    
    public CharacterController controller;
    Vector3 movement;
    [SerializeField]
    private int _health = 100;
    [SerializeField]
    private float _speed = 8;
    private float _gravity = 14.0f;
    private float _verticalVelocity;
    private float _jumpForce = 10f;
    private Animator _animator;
    private int _maxJumps = 2;
    private int _jumpsLeft = 2;
    private Vector3 _moveDirection;

    private void Start(){
        _animator = GetComponent<Animator>();     
        controller = GetComponent<CharacterController>();   
    }

    private void Update(){

        if(controller.isGrounded){
            _moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            _moveDirection *= _speed;

            if(Input.GetButton("Jump")){
                _moveDirection.y = _jumpForce;
                _jumpsLeft --;
            }

        }else{

            if(Input.GetButton("Jump") && _jumpsLeft > 0){
                _moveDirection.y = _jumpForce;
                _jumpsLeft --;
            }
        }

        _moveDirection.y -= _gravity * Time.deltaTime;

        controller.Move(_moveDirection * Time.deltaTime);

    }

    public void takeDamage(int damage){
        _health -= damage;
    }
}
