using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI msgText;

    public static bool isRunning = true;
    public float time = 0; // in seconds
    private int minutes, seconds;

    void Start()
    {
        msgText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isRunning)
        {
            time -= Time.deltaTime;

            if (time <= 0) time = 0;

            minutes = (int)time / 60;
            seconds = (int)time % 60;
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

            if (time <= 0)
            {
                isRunning = false;
                msgText.text = "Oops! Time's up!";
                msgText.gameObject.SetActive(true);
            }
        }
    }
}
