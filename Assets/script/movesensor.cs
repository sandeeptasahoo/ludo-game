using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class movesensor : MonoBehaviourPun
{
    player_piece player_Piece;
    player player;
    bool large;
    bool small;
    Vector3 rate=new Vector3(0.5f,0.5f,0);
    Vector3 originalSize;
    public bool mouseClick;
    bool online;
    


    void Awake()
    {
        player_Piece=FindObjectOfType<player_piece>();
        player=GetComponent<player>();
        online=FindObjectOfType<datacarrier>().onlinestate;
       
    }
    void OnEnable()
    {
        
        player_Piece.numOfActivePlayer++;
        
        if(player.playerstate>1 )
        {
            if(player.playerstate==3 || !player.evaluateMove(player_Piece.moves))
            {
                this.enabled=false;
                return;
            }
            
        }
        mouseClick=true;
        originalSize=transform.localScale;
        large=true;
       // Debug.Log("active player "+player_Piece.numOfActivePlayer);
        player_Piece.activePlayer=this.gameObject;
        
    }

    void OnDisable()
    {
        mouseClick=false;
        small=false;
        large=false;
        player_Piece.numOfActivePlayer--;
        transform.localScale=originalSize;
    }
    // Update is called once per frame
    void Update()
    {
        if(large)
        {
            makeLarge();
        }
        if(small)
        {
            makeSmall();
        }
        
    }

    void makeLarge()
    {
        if(transform.localScale.x<0.6f)
        {
            transform.localScale+=rate*Time.deltaTime;
        }
        else
        {
            small=true;
            large=false;
        }
    }

    void makeSmall()
    {
        if(transform.localScale.x>0.4f)
        {
            transform.localScale-=rate*Time.deltaTime;
        }
        else
        {
            small=false;
            large=true;
        }
    }

    void OnMouseDown()
    {
        if(online)
        {
            if(mouseClick && base.photonView.IsMine)
            {
                activatePlayer();
            }
        }
        else
        {
            if(mouseClick)
            activatePlayer();
        }
    }
    

    public void activatePlayer()
    {
        player.moves=player_Piece.moves;
        player.enabled=true;
        player_Piece.disablePlayers();
        player.movePlayer();
    }
}
