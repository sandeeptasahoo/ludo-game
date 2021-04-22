using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;


public class networkconnector : MonoBehaviourPunCallbacks
{
    public GameObject bottons;
    public GameObject lostConnectionpanel;
    public GameObject connecting;
    public GameObject searching;
    public Text roomtext;
    GameObject waitingPanel;
    int maxPlayer;
    int playerrollnumber;
    string roomname;
    
    
    void Awake()
    {
        Destroy(FindObjectOfType<datacarrier>().gameObject);
    }
    void Update()
    {
        
    }

    public void loadOnline()
    {
        
    }

    
    public void disconnectFromNetwork()
    {
        PhotonNetwork.Disconnect();
        waitingPanel.SetActive(false);
        SceneManager.LoadScene("MainMenu");
    }

    public void ConnectToNetwork(int playernum)
    {
        maxPlayer=playernum;
         searching.SetActive(true);
        
        Debug.Log("join room");
        PhotonNetwork.JoinRandomRoom(null,(byte) playernum);
       
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("no room available creating a room");
        Debug.Log(message);
        PhotonNetwork.CreateRoom(null,new RoomOptions{ MaxPlayers=(byte)maxPlayer});
        
        /*if(PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.InRoom.name=
        }*/
        
        /*datacarrier.players[playerrollnumber]=PhotonNetwork.LocalPlayer;
        playerrollnumber++;
        */

    }

    
    public override void OnCreatedRoom()
    {
        
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("Room joined With Name "+PhotonNetwork.CurrentRoom.Name);
        roomname=PhotonNetwork.CurrentRoom.Name;
       // int roomcount=PhotonNetwork.room.Lenght;
        roomtext.text=maxPlayer.ToString()+"Room Name : "+roomname;
        if(PhotonNetwork.CurrentRoom.PlayerCount==maxPlayer)
        {
            
        }
        else
        {
           
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {

        if(PhotonNetwork.CurrentRoom.PlayerCount==maxPlayer)
        {
            PhotonNetwork.CurrentRoom.IsOpen=false;
            Debug.Log("all player enter");
            PhotonNetwork.LoadLevel("multiplayerludo");
        }
    }

    public void reconnect()
    {
        PhotonNetwork.ConnectUsingSettings();
        lostConnectionpanel.SetActive(false);
        connecting.SetActive(true);
    }

    public override void OnConnectedToMaster()
    {
        connecting.SetActive(false);
        
        bottons.SetActive(true);
        
        Debug.Log("Connected to masterserver");
    }
    public override void OnDisconnected	(DisconnectCause cause)	
    {
        bottons.SetActive(false);
        connecting.SetActive(false);
        lostConnectionpanel.SetActive(true);
        Debug.Log("Connection lost");
    }
}
