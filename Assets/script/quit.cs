using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;


public class quit : MonoBehaviourPunCallbacks
{

     public GameObject connectingPanel;
     public GameObject bottons;
     bool online;
     bool closeapp=false;
    void Update()
    {
        if(Application.platform==RuntimePlatform.Android)
        {
            if(Input.GetKey(KeyCode.Escape))
            {
                if(online)
                {
                    disconnectFromNetwork();
                    return;
                }
                doquit();
            }
        }
    }
    public string level;
    public void loadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
   public void doquit()
   {
       Application.Quit();
   }
   public void loadludomaze()
    {
        
        SceneManager.LoadScene("playerselection");
        
    }
    public void loadOnline()
    {
       online=true;
        PhotonNetwork.AutomaticallySyncScene=true;
        PhotonNetwork.ConnectUsingSettings();
        connectingPanel.SetActive(true);
        
    }

    void disconnectFromNetwork()
    {
        PhotonNetwork.Disconnect();
        Debug.Log("Disconnecting");
        closeapp=true;
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to masterserver");
        SceneManager.LoadScene("joininghalt");
        
    }
    public override void OnDisconnected	(DisconnectCause cause)	
    {
        online=false;
        connectingPanel.GetComponent<connecting>().stopconnecting=true;
        connectingPanel.GetComponent<connecting>().search.text="Connection Lost";
        Debug.Log("Connection lost");
        Invoke("shutPanel",2);
        if(closeapp)
        {
            doquit();
        }
    }
    public void shutPanel()
    {
        connectingPanel.SetActive(false);
        bottons.SetActive(true);
    }
}

