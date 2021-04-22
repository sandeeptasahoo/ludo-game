using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class player_piece : MonoBehaviour
{
    showPlayer showPlayer;
    public GameObject killMenu;
    public GameObject winnerMenu;
    public GameObject[] redPawns;
    public GameObject[] greenPawns;
    public GameObject[] yellowPawns;
    public GameObject[] bluePawns;
    public GameObject[][] pawns;
    GameObject[] bottons;
    GameObject placeholder;
    public int[] playerEligibilty;
    public int[] playerAtHome;
    
    public int playerindex;
    public int moves;
    GameObject redOriginGm;
    GameObject greenOriginGm;
    GameObject yellowOriginGm;
    GameObject blueOriginGm;
    public int numOfActivePlayer;
    public GameObject activePlayer;
    AudioSource audioSource;
    public AudioClip enterGoalSound;
    public AudioClip allInGoalSound;
    public AudioClip gameOverSound;
    int winners;
    string[] place={"First","Second","Third"};
    int numOfPlayerType;
    datacarrier datacarrier;
    Photon.Realtime.Player[] players;
    bool[] availableType;
    int[] colourpairlist;
    bool[] aiPlayers;
    public AI[] playerAI;

    void Awake()
    {
        storePawns();
        
        bottons=new GameObject[4];
        for(int i=0;i<4;i++)
        {
            bottons[i]=killMenu.transform.GetChild(i).gameObject;
        }
        playerEligibilty=new int[4];
        showPlayer=FindObjectOfType<showPlayer>();
        playerAtHome=new int[4];
        audioSource=GetComponent<AudioSource>();
        playerAI=new AI[4];

        
        
        
    }

    void Start()
    {
        datacarrier=FindObjectOfType<datacarrier>();
        if(datacarrier.onlinestate)
        {
            datacarrier.findPlayers();
            giveOwnership();
            Debug.Log("owner ship assignnment is done");
        }
        showPlayer.startdatainput();
        availableType=datacarrier.availableType;
        colourpairlist=datacarrier.colourpairlist;
        numOfPlayerType=datacarrier.numOfPlayerType;
        players=datacarrier.players;
        aiPlayers=datacarrier.aiPlayer;
        for(int i=0;i<4;i++)
        {
            if(aiPlayers[i]==true)
            {
                playerAI[i]=gameObject.AddComponent<AI>();
                playerAI[i].playerIndex=i;
            }
        }

        
        activatePawns();
    }
    
    void giveOwnership()
    {
        for(int i=0;i<numOfPlayerType;i++)
        {
            for(int j=0;j<4;j++)
            {
                if(i+1==colourpairlist[j])
                {
                    giveOwnershipToPlayer(players[i],j);
                    break;
                }
            }
            
        }
    }

    void giveOwnershipToPlayer(Photon.Realtime.Player owner,int colorindex)
    {
        Photon.Pun.PhotonView targetView;
        for(int i=0;i<4;i++)
        {
            targetView=pawns[colorindex][i].GetComponent<Photon.Pun.PhotonView>();
            targetView.TransferOwnership(owner);
        }
    }
void storePawns()
{
        redOriginGm=GameObject.FindGameObjectWithTag("redOrigin");
        storeElements(ref redPawns,ref redOriginGm,4);
        greenOriginGm=GameObject.FindGameObjectWithTag("greenOrigin");
        storeElements(ref greenPawns,ref greenOriginGm,4);
        blueOriginGm=GameObject.FindGameObjectWithTag("blueOrigin");
        storeElements(ref bluePawns,ref blueOriginGm,4);
        yellowOriginGm=GameObject.FindGameObjectWithTag("yellowOrigin");
        storeElements(ref yellowPawns,ref yellowOriginGm,4);
        pawns=new GameObject[4][];
        pawns[0]=redPawns;
        pawns[1]=greenPawns;
        pawns[2]=yellowPawns;
        pawns[3]=bluePawns;
}

    void activatePawns()
    {
        bool[] pawntype=FindObjectOfType<datacarrier>().availableType;
        for(int i=0;i<4;i++)
        {
            if(pawntype[i]==false)
            {
                for(int j=0;j<4;j++)
                {
                    pawns[i][j].SetActive(false);
                }
            }
        }
    }
 void storeElements(ref GameObject[] arr,ref GameObject gm,int a)
    {
        arr=new GameObject[a];
        for(int i=0;i<a;i++)
        {
           arr[i]= gm.transform.GetChild(i).gameObject;
        }

    }
    public void moveEvaluator(int steps)
    {
        moves=steps;
        numOfActivePlayer=0;
        if(playerindex>3)
        {
            playerindex=0;
        }
        

        if(steps==6 )
        {
            allEligiblePlayer();
        }
        else
        {
            showEligiblePlayer();
        }
        if(aiPlayers[playerindex]==true)
        {
            addEligiblePlayerToAI();
        }

    }
    public void moveWhile6(int Types)
    {
        pawns[Types][0].GetComponent<movesensor>().activatePlayer();
    }

    void showEligiblePlayer()
    {
        
        for(int i=0;i<4;i++)
        {
            int ps=pawns[playerindex][i].GetComponent<player>().playerstate;
            if(ps>0 && ps<3)
            {
                pawns[playerindex][i].GetComponent<movesensor>().enabled=true;
            }
        }
        if(numOfActivePlayer==1 )
        {
            activePlayer.GetComponent<movesensor>().activatePlayer();
        }
        else if(numOfActivePlayer==0)
        {
            showPlayer.enabled=true;
            showPlayer.showPlayerImage();

        }
        
    }
    void allEligiblePlayer()
    {
        for(int i=0;i<4;i++)
        {
            pawns[playerindex][i].GetComponent<movesensor>().enabled=true;
            //pawns[playerindex][i].GetComponent<movesensor>().mouseClick=true;
            //pawns[playerindex][i].GetComponent<BoxCollider2D>().enabled=true;
        }
    }

    public void addEligiblePlayerToAI()
    {
        int count=0;
        for(int i=0;i<4;i++)
        {
            if(pawns[playerindex][i].GetComponent<movesensor>().enabled==true)
            {
                playerAI[playerindex].playerAvailable[count]=pawns[playerindex][i];
                count++;
            }
            
        }
        
        playerAI[playerindex].activeplayernum=count;
        if(count<2)
        return;
        playerAI[playerindex].steps=moves;
        disablePlayers();
        Invoke("decide",0.5f);
    }
    void decide()
    {
        playerAI[playerindex].Aidecide();
    }

    public void disablePlayers()
    {
        for(int i=0;i<4;i++)
        {
            pawns[playerindex][i].GetComponent<movesensor>().enabled=false;
            //pawns[playerindex][i].GetComponent<movesensor>().mouseClick=false;
            //pawns[playerindex][i].GetComponent<BoxCollider2D>().enabled=false;
        }
    }

    public void showkillmenu(int numberOfBotton,int[] bottonType,GameObject place)
    {
        if(numberOfBotton==2)
        {
            bottons[bottonType[0]].GetComponent<RectTransform>().anchoredPosition=new Vector2(-75,0);
            bottons[bottonType[0]].SetActive(true);
            bottons[bottonType[1]].GetComponent<RectTransform>().anchoredPosition=new Vector2(75,0);
            bottons[bottonType[1]].SetActive(true);
        }
        else
        {
            bottons[bottonType[0]].GetComponent<RectTransform>().anchoredPosition=new Vector2(-100,0);
            bottons[bottonType[0]].SetActive(true);
            bottons[bottonType[1]].GetComponent<RectTransform>().anchoredPosition=new Vector2(100,0);
            bottons[bottonType[1]].SetActive(true);
            bottons[bottonType[2]].GetComponent<RectTransform>().anchoredPosition=new Vector2(0,0);
            bottons[bottonType[2]].SetActive(true);
        }
        placeholder=place;
        killMenu.SetActive(true);
    }

    public void closekillmenu(int type)
    {
        for(int i=0;i<4;i++)
        {
            bottons[i].SetActive(false);
        }
        killMenu.SetActive(false);
        placeholder.GetComponent<placeholders>().killPlayer(type);
    }

    public bool goalAchieved(int type,Vector3 playerpos)
    {
        playerEligibilty[type]--;
        //Debug.Log(type);
        playerAtHome[type]++;
        if(playerAtHome[type]==4)
        {
            audioSource.PlayOneShot(allInGoalSound);
            return addToRankLiist(type);
        }
        else
        {
            audioSource.PlayOneShot(enterGoalSound);
        }
        return false;
    }

    bool addToRankLiist(int type)
    {
        winners++;
        winnerMenu.transform.GetChild(type).GetComponent<RectTransform>().anchoredPosition3D=new Vector3(0,45-((winners-1)*70),0);
        winnerMenu.transform.GetChild(type).transform.GetChild(0).GetComponent<Text>().text=place[winners-1];
        showPlayer.availableType[type]=false;
        if(winners==numOfPlayerType-1)
        {
            gameOver();
            return true;
        }
        return false;
    }

    void gameOver()
    {
        audioSource.PlayOneShot(gameOverSound);
        winnerMenu.SetActive(true);
    }



}
