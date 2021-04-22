using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;


public class datacarrier : MonoBehaviourPun
{
    // Start is called before the first frame update
    public bool[] availableType;
    public int numOfPlayerType;
    public int[] colourpairlist;
    public Photon.Realtime.Player[] players;
    public bool onlinestate;
    public Text playernum;
    bool playerassign=false;
    public bool[] aiPlayer;

    
    void Awake()
    {

        int numOfDataCarrier=FindObjectsOfType<datacarrier>().Length;
        if(numOfDataCarrier>1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(gameObject);
       // availableType=new bool[4];

    }

    void identifyplayer(Photon.Realtime.Player player,int num)
    {
        if(playerassign)
        {
            return;
        }
        if(PhotonNetwork.LocalPlayer==player)
        {
            string number=(num+1).ToString();
            playernum.text="Player "+number;
            playerassign=true;
        }
    }
    public void findPlayers()
    {
        numOfPlayerType=0;
        players=new Photon.Realtime.Player[4];
        foreach(Photon.Realtime.Player player in PhotonNetwork.PlayerList)
        {
            identifyplayer(player,numOfPlayerType);
            players[numOfPlayerType]=player;
            numOfPlayerType++;
        }
        setAllData();
    }

    public void setAllData()
    {
        for(int i=0;i<numOfPlayerType;i++)
        {
            chooseColor(i);
        }
    }

    void chooseColor(int playerindex)
    {
        if(numOfPlayerType==2 && playerindex==1)
        {
            availableType[2]=true;
            colourpairlist[2]=playerindex+1;
        }
        else
        {
            availableType[playerindex]=true;
            colourpairlist[playerindex]=playerindex+1;
        }
    }
    
}
