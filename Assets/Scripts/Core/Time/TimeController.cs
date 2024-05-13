using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
    const float secondsInDay = 86400f;

    [Header("Components References")]
    [SerializeField] Color nightLightColor;
    [SerializeField] AnimationCurve nightTimeCurve;
    [SerializeField] Color dayLightColor = Color.white;
    [SerializeField] TMP_Text timeText;
    [SerializeField] TMP_Text dayNumText;

    float time;
    [Header("Stats")]
    [SerializeField] float timeScale = 60f;
    [SerializeField] float startAtTime = 28800f; // in seconds

    [SerializeField] UnityEngine.Rendering.Universal.Light2D globalLight;
    public int days = 1;
    public int years = 1;

    List<TimeAgent> agents;

    private void Awake()
    {
        agents = new List<TimeAgent>();
    }

    private void Start()
    {
        time = startAtTime;
        TimeAgents();
    }

    float Hours
    {
        get { return time / 3600f; }
    }

    float Minutes
    {
        get { return time % 3600f / 60f; }
    }

    // Currently at 24 minutes per day
    private void Update()
    {
        time += Time.deltaTime * timeScale;

        TimeValueCalculation();
        DayLight();

        if (time > secondsInDay)
        {
            NextDay();
            TimeAgents();
        }      
    }

    private void TimeAgents()
    {
        for (int i = 0; i < agents.Count; i++)
        {
            agents[i].Invoke();
        }
    }

    private void TimeValueCalculation()
    {
        int hh = (int)Hours;
        int mm = (int)Minutes;

        timeText.text = hh.ToString("00") + ":" + mm.ToString("00");    
    }

    private void DayLight()
    {
        float v = nightTimeCurve.Evaluate(Hours);
        Color c = Color.Lerp(dayLightColor, nightLightColor, v);
        globalLight.color = c;
    }

    private void NextDay()
    {
        time = 0;
        days += 1;

        int d = (int)days;
        dayNumText.text = d.ToString();
    }

    public void Subscribe(TimeAgent timeAgent)
    {
        agents.Add(timeAgent);
    }

    public void UnSubscribe(TimeAgent timeAgent)
    {
        agents.Remove(timeAgent);
    }
}