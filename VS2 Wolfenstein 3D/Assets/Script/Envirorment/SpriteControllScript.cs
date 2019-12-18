//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class SpriteControllScript : MonoBehaviour
//{
//    public GameObject hearts3, hearts2, hearts1, dead, gameOver;
//    public GameObject[] hearts;

//    void Start()
//    {
//        hearts3.gameObject.SetActive(true);
//        hearts2.gameObject.SetActive(false);
//        hearts1.gameObject.SetActive(false);
//        dead.gameObject.SetActive(false);
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        switch (PlayerHpScript.hp)
//        {
//            case 3:
//                Debug.Log("3 HP");
//                hearts3.gameObject.SetActive(true);
//                hearts2.gameObject.SetActive(false);
//                hearts1.gameObject.SetActive(false);
//                dead.gameObject.SetActive(false);
//                break;
//            case 2:
//                Debug.Log("2 HP");
//                hearts3.gameObject.SetActive(false);
//                hearts2.gameObject.SetActive(true);
//                hearts1.gameObject.SetActive(false);
//                dead.gameObject.SetActive(false);
//                break;
//            case 1:
//                Debug.Log("1 HP");
//                hearts3.gameObject.SetActive(false);
//                hearts2.gameObject.SetActive(false);
//                hearts1.gameObject.SetActive(true);
//                dead.gameObject.SetActive(false);
//                break;
//            case 0:
//                Debug.Log("Dead");
//                hearts3.gameObject.SetActive(false);
//                hearts2.gameObject.SetActive(false);
//                hearts1.gameObject.SetActive(false);
//                dead.gameObject.SetActive(true);
//                break;

//        }
//    }
//}
