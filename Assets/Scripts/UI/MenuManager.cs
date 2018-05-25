using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public void StartGame()
    {
        GameObject.Find("GameManager(Clone)").GetComponent<Completed.GameManager>().InitGame();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
