using UnityEngine;

public class Fairy : MonoBehaviour
{
    public GameObject FairyScoreText;

    void Start()
    {
        FairyScoreText = GameObject.FindGameObjectWithTag("FairyScoreText");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            FairyScoreText.GetComponent<Score>().fairiesCaught += 1;
            Destroy(gameObject);
        }
    }
}
