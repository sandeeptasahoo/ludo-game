using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class showPlayer : MonoBehaviour
{
    public int playerIndex=-1;
    public AudioClip changePlayer;
    public Sprite[] playerNumber;
    public Image playerDisplay;
    public float timer;
    bool large;
    bool small;
    public Vector2 rate;
    float time;
    rollingdice rollingdice;
    player_piece player_Piece;
    RectTransform rectTransform;
    AudioSource audioSource;
    public bool[] availableType;
    bool availabiltyStatement;
    public GameObject dicePosition;
    int[] colourpairlist;
    datacarrier datacarrier;
    
    Photon.Realtime.Player[] players; 
    bool online;

    void Awake()
    {
        rollingdice=FindObjectOfType<rollingdice>();
        player_Piece=FindObjectOfType<player_piece>();
        rectTransform=GetComponent<RectTransform>();
        audioSource=GetComponent<AudioSource>();
        
        
        

    }


    void Start()
    {
       
    }

    public void startdatainput()
    {
        datacarrier=FindObjectOfType<datacarrier>();
        availableType=datacarrier.availableType;
        colourpairlist=datacarrier.colourpairlist;
        online=datacarrier.onlinestate;
        players=datacarrier.players;
        if(datacarrier.onlinestate)
        {
            rollingdice.changeOwnership(players[0]);
        }
        removeDicePlace();
        showPlayerImage();
    }
    void removeDicePlace()
    {
        //Debug.Log(availableType.Length);
        for(int i=0;i<4;i++)
        {
            if(availableType[i]==false)
            {
                dicePosition.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
    public void showPlayerImage()
    {
        for(int i=0;i<4;i++)
        {
            playerIndex++;
            if(playerIndex>3)
            {
               playerIndex=0;
            }
            if(availableType[playerIndex])
            {   
                availabiltyStatement=true;
                break;
                
            }
            else
            {
                availabiltyStatement=false;
            }
        }
        
        if(!availabiltyStatement)
        {
            Debug.Log("No Player Available");
        }
        player_Piece.playerindex=playerIndex;
       // Debug.Log("player index "+playerIndex+" player number "+ (colourpairlist[playerIndex]-1) );
        playerDisplay.sprite=playerNumber[colourpairlist[playerIndex]-1];
        playerDisplay.enabled=true;
        this.enabled=true;
        audioSource.PlayOneShot(changePlayer);
        large=true;
        //Invoke("endDisplay",timer);
    }

    void Update()
    {
        if(large)
        {
            makeLarge();
        }
        else if(small)
        {
            makeSmall();

        }
    }

    void makeLarge()
    {
        if(rectTransform.sizeDelta.x<400)
        {
            rectTransform.sizeDelta+=rate;
        }
        else
        {
            large=false;
            small=true;
        }
    }

    void makeSmall()
    {
        if(rectTransform.sizeDelta.x>200)
        {
            rectTransform.sizeDelta-=rate;
        }
        else
        {
            small=false;
            endDisplay();
        }
    }

    public void endDisplay()
    {
        rollingdice.enabled=true;
        if(online)
        {
            rollingdice.changeOwnership(players[colourpairlist[playerIndex]-1]);
        }
        rollingdice.diceRoll(true);
        playerDisplay.enabled=false;
        this.enabled=false;
    }

    


}
