//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class SoundManagerScript : MonoBehaviour
//{

//    public static AudioClip FireSound;
//    static AudioSource audioSrc;
//    // Start is called before the first frame update
//    void Start()
//    {
//        FireSound = Resources.Load<AudioClip>("fire1");

//        audioSrc = GetComponent<AudioSource>();
//    }

//    // Update is called once per frame
//    void Update()
//    {
        
//    }

//    public static void PlaySound (string clip)
//    {
//        switch (clip)
//        {
//            case "fire1":
//                audioSrc.PlayOneShot();
//                Debug.Log("heyy");
//                break;
//        }
//    }
//}
