using UnityEngine;
using TMPro;

public class countdownTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public float countdownDuration = 240.0f; // 4 minutes = 240 seconds
    private float startTime;
    private bool isCounting = false;

    private void Update()
    {
        if (isCounting)
        {
            float currentTime = Time.time - startTime;
            float timeRemaining = Mathf.Max(countdownDuration - currentTime, 0);

            // Calculate minutes and seconds
            int minutes = Mathf.FloorToInt(timeRemaining / 60);
            int seconds = Mathf.FloorToInt(timeRemaining % 60);

            // Update the timer text with minutes and seconds
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

            if (timeRemaining <= 0)
            {
                // Timer has reached zero, handle your event here
                isCounting = false;
            }
        }
    }

    public void StartCountdown()
    {
        startTime = Time.time;
        isCounting = true;
    }
}
