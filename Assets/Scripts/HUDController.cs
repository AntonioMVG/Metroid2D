using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDController : MonoBehaviour
{
    public TextMeshProUGUI timeTxt;
    public TextMeshProUGUI livesTxt;
    public TextMeshProUGUI powerupsTxt;
    public Canvas canvas;

    public void SetTimeTxt(int levelTime)
    {
        int mins = (int)levelTime / 60;
        int secs = (int)levelTime % 60;
        timeTxt.text = mins.ToString("Time 00" + ":" + secs.ToString("00"));
    }

    public void SetLivesTxt(int lives)
    {
        livesTxt.text = "Lives: " + lives.ToString();
    }

    public void SetPowerUpsTxt(int count)
    {
        powerupsTxt.text = "Power Ups: " + count.ToString();
    }

    public void SetTimesUpBox()
    {
        canvas.transform.Find("TimesUpBox").gameObject.SetActive(true);
    }

    public void SetWinBox()
    {
        canvas.transform.Find("WinBox").gameObject.SetActive(true);
    }

    public void SetLoseLivesBox()
    {
        canvas.transform.Find("LoseLivesBox").gameObject.SetActive(true);
    }
}
