using UnityEngine;
using UnityEngine.UI;

public class StartTimerButton : MonoBehaviour
{
    public countdownTimer cdt;

    public void StartTimer()
    {
        cdt.StartCountdown();
    }
}
