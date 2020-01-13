using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{    
    public CharacterController controller;
    Vector3 movement;
    [SerializeField]
    private int _health = 5;
    [SerializeField]
    private float _speed = 8;
    private float _gravity = 14.0f;
    private float _verticalVelocity;
    private float _jumpForce = 10;
    private Animator _animator;
    private int _maxJumps = 2;
    private int _jumpsLeft = 2;

    private void Start(){
        _animator = GetComponent<Animator>();     
        controller = GetComponent<CharacterController>();   
    }

    private void Update(){
        if(controller.isGrounded){
            _verticalVelocity = -_gravity * Time.deltaTime;
            _jumpsLeft = 2;
            if(Input.GetKeyDown(KeyCode.Space)){
                _verticalVelocity = _jumpForce;
                _jumpsLeft--;
            }

        }else{
            _verticalVelocity -= _gravity * Time.deltaTime;
            if(Input.GetKeyDown(KeyCode.Space) && _jumpsLeft > 0){
                _verticalVelocity = _jumpForce;
                _jumpsLeft--;
            }
        }

        Vector3 moveVector = new Vector3(0, _verticalVelocity, 0);
        controller.Move(moveVector * Time.deltaTime);
    }
}
