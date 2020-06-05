using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QuitMenuController : MonoBehaviour
{
    public GameObject quitPanel;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            quitPanel.SetActive(true);
            Time.timeScale = 0;
        }
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}
