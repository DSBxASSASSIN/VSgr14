using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

    public Text fairyScoreText;
    public Text animalScoreText;
    public int fairiesCaught;
    public int animalsFreed;

    private void Start()
    {
        fairiesCaught = 0;
        animalsFreed = 0;
    }

    void Update() {
        fairyScoreText.text = fairiesCaught.ToString() + "/5";
        animalScoreText.text = animalsFreed.ToString() + "/3";
    }
}
