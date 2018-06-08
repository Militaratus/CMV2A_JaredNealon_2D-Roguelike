using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class MenuManager : MonoBehaviour
{
    public void StartGame()
    {
        AnalyticsEvent.Custom("StartGame");
        GameObject.Find("GameManager(Clone)").GetComponent<Completed.GameManager>().InitGame();
    }

    public void QuitGame()
    {
        AnalyticsEvent.Custom("QuitGame");
        Application.Quit();
    }
}
