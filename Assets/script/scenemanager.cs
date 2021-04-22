using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class scenemanager : MonoBehaviour
{
    public GameObject exitpopup;
    void Update()
    {
        if(Application.platform==RuntimePlatform.Android)
        {
            if(Input.GetKey(KeyCode.Escape))
            {
                exitpopup.SetActive(true);
                
            }
        }
    }

    public void loadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void closeExitmenu()
    {
        exitpopup.SetActive(false);
    }
}
