using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{

    public Text timerText;
    private float startTime;
    private bool finnished = false;
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerText != null)
        {
            if (finnished)
                return;
            float t = Time.time - startTime;

            string hours = ((int)(t / 60) / 60).ToString();
            string minutes = ((int)t / 60).ToString();
            string seconds = (t % 60).ToString("f0");

            timerText.text = hours + ":" + minutes + ":" + seconds;
        }

    }

    public void Finish()
    {
        finnished = true;
        timerText.color = Color.yellow;
    }
}
