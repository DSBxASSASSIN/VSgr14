using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{    
    private Rigidbody rb;
    private float distToGround;
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
        rb = GetComponent<Rigidbody>();   
        distToGround = Collider.bounds.extents.y;
    }

    private void Update(){

    }

    public void takeDamage(int damage){
        _health -= damage;
    }
}
