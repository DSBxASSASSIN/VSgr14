using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    Vector2 movement;

    void Update() {
        movement.x = Input.GetAxisRaw(Constants.Input.HORIZON);
        movement.y = Input.GetAxisRaw(Constants.Input.VERTICAL);
    }

    void FixedUpdate() {
        rb.MovePosition(rb.position + movement * Constants.Player.SPEED * Time.fixedDeltaTime);
    }
}
