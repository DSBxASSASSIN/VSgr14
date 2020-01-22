using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cage : MonoBehaviour
{
    public GameObject FairyScoreText;
    public GameObject fairy;

    void Start()
    {
        FairyScoreText = GameObject.FindGameObjectWithTag("FairyScoreText");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            FairyScoreText.GetComponent<Score>().animalsFreed += 1;
            Instantiate(fairy, transform.position, transform.rotation);
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
