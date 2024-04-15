using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerUI : MonoBehaviour
{
    private float timeF;
    public Text timeText;
    private float clearTime;
    private int min;
    private float sec;


    public float TotalTime;
    [HideInInspector]
    public string showClearTime;

    void Start()
    {
        gameObject.SetActive(true);
    }
    private void FixedUpdate()
    {
        timeF += Time.deltaTime;
        showClearTime = string.Format("{0:N2}", timeF);
        timeText.text = $"{showClearTime}ÃÊ";
        if (timeF > TotalTime)
        {
            GameManager.Instance.GameOver();
        }
    }

    public void StopTimer()
    {
        clearTime = timeF;
        timeText.text = clearTime.ToString();
        min = (int)(clearTime / 60);
        sec = clearTime - (float)(60 * min);
    }
}
