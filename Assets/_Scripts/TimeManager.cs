using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeManager : MonoBehaviour
{
    public static int minute {get; private set; }
    public static int hour { get; private set; }
    public static int dayNumber { get; private set; }

    float timer;

    [SerializeField] TMP_Text phoneTime;

    void Start()
    {
        dayNumber = 0;
        minute = 0;
        hour = 0;

    }

    void Update()
    {
        string timeDisplay = $"{hour.ToString("D2")} : {minute.ToString("D2")}";
        string dateDisplay = $"Day: {dayNumber.ToString("D2")}, {hour.ToString("D2")} : {minute.ToString("D2")}";
        phoneTime.text = timeDisplay;
    }

    /* Just call this function from your script and just pass the wanted hour and minute.
    Note that there is no logic to check if the time inputted is a valid time, so make sure the time is correct. */
    public void SetTime(int hourSet, int minuteSet)
    {
        hour = hourSet;
        minute = minuteSet;
    }
}
