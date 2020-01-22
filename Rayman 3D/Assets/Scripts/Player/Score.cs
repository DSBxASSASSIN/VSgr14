using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

    public Transform player;
    public Text fairyScoreText;
    public Text animalScoreText;
 

    void Update() {
        // fairyScoreText.text = fairiesCaught.ToString() + "/50";
        // animalScoreText.text = animalsFreed.ToString() + "/0";
        fairyScoreText.text = "0/50";
        animalScoreText.text = "0/0";
    }
}
