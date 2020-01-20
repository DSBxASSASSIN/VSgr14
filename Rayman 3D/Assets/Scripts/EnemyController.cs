using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private int _health = 5;
    void takeDamage(int damage){
        _health -= damage;
    }
}
