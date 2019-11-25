using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
   void Start() {

        Enemy enemy = new Enemy();

        enemy.Health = 5;
        int x = enemy.Health;
        Debug.Log(x);
    }
}
