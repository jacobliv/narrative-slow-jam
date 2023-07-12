using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeManager : MonoBehaviour
{
    public static int minute {get; private set; }
    public static int hour { get; private set; }
    public static int dayNumber { get; private set; }

    // How many real-life seconds are = to 1 in-game minutes
    float gameMinuteToRealTime = 1f;
    float timer;

    [SerializeField] TMP_Text phoneTime;

    void Start()
    {
        dayNumber = 0;
        minute = 0;
        hour = 0;

        timer = gameMinuteToRealTime;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        
        if (timer <= 0)
        {
            minute++;
            if (minute > 59)
            {
                minute = 0;
                hour++;

                if (hour > 23)
                {
                    hour = 0;
                    dayNumber++;
                }
            }

            timer = gameMinuteToRealTime;
        }

        string timeDisplay = $"{hour.ToString("D2")} : {minute.ToString("D2")}";
        phoneTime.text = timeDisplay;
    }
}
