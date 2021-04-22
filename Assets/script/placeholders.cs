using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class placeholders : MonoBehaviour
{
    // Start is called before the first frame update
    int playerNumber=-1;
    int[] playerType=new int[4];
    GameObject[] players=new GameObject[16];
    int killAvailable;
    int[] killtype=new int[3];
    public bool safePosition;
    AudioSource audioSource;
    AudioClip safePlaceSound;
    AudioClip killSound;

    player_piece player_Piece;
    bool ai;
    

    void Awake()
    {
        player_Piece=FindObjectOfType<player_piece>();
        audioSource=gameObject.AddComponent<AudioSource>();
        safePlaceSound=Resources.Load("safePlaceSound") as AudioClip;
        killSound=Resources.Load("killSound") as AudioClip;
    }
    public void removePlayer(GameObject player,int playerIndex)
    {
        int i=0;
        for(i=0;i<=playerNumber;i++)
        {
            if(players[i]==player)
            {
                break;
            }
        }
        while(i<playerNumber)
        {
            players[i]=players[i+1];
            i++;
        }
        playerNumber--;
        playerType[playerIndex]--;
        player.transform.localScale=new Vector3(0.5f,0.5f,1);
        if(playerNumber<0)
        {
            return;
        }
        placePlayer();
    }

    public bool addplace(GameObject player,int playerIndex)
    {
        playerType[playerIndex]++;
        playerNumber++;
        players[playerNumber]=player;
        /*Debug.Log(playerNumber);
        if(players[playerNumber]==player)
        {
            Debug.Log("added successfully");
        }*/
        if(safePosition && !player.GetComponent<player>().checkStartPosition())
        {
            audioSource.PlayOneShot(safePlaceSound);
        }
        if(playerNumber<1)
        {
            return false;
        }
        placePlayer();
        return checkKill(playerIndex);
        
    }

    void placePlayer()
    {
        float size=Mathf.Clamp((0.5f-(0.02f*playerNumber)),0.35f,0.5f);
        Vector3 scale=new Vector3(size,size,1);
        for(int i=0;i<=playerNumber;i++)
        {
            players[i].transform.localScale=scale;
        }
        if(playerNumber==0)
        {
            players[0].transform.position=transform.position;
            return;
        }
        Vector3 initialPos=transform.position-new Vector3(0.1f,0,0);
        players[0].transform.position=initialPos;
        float gap=0.2f/(playerNumber);

        for(int i=1;i<=playerNumber;i++)
        {
            initialPos.x+=gap;
            players[i].transform.position=initialPos;
        }
        //Debug.Log("resized");
    }
    public bool checkKillAi(int playerIndex)
    {
        ai=true;
        if(safePosition)
        {
            return false;
        }
        for(int i=0;i<4;i++)
        {
            if(i!=playerIndex && playerType[i]==1)
            {
                return true;
            }

        }
        return false;
    }

    bool checkKill(int playerIndex)
    {
        if(safePosition)
        {
            return false;
        }
        killAvailable=0;
        for(int i=0;i<4;i++)
        {
            if(i!=playerIndex && playerType[i]==1)
            {
                killAvailable++;
                killtype[killAvailable-1]=i;
            }

        }
        //Debug.Log("kills Avaolable "+killAvailable);
        if(killAvailable==0)
        {
            ai=false;
            return false;
        }
        else if(killAvailable>1 )
        {
            if(ai==false)
            player_Piece.showkillmenu(killAvailable,killtype,gameObject);
            else
            killPlayer(killtype[Random.Range(0,killAvailable)]);
            ai=false;
            return true;
        }
        else
        {
            killPlayer(killtype[0]);
            ai=false;
            return true;
            
        }
        
    }

    public void killPlayer(int type)
    {
        GameObject deadplayer=players[0];
        for(int i=0;i<=playerNumber;i++)
        {
            if(players[i].GetComponent<player>().playerIndex==type)
            {
                deadplayer=players[i];
                break;
            }
        }
        removePlayer(deadplayer,type);
        audioSource.PlayOneShot(killSound);
        deadplayer.GetComponent<player>().playerBackToOrigin();
       // Debug.Log("player killed");
    }

   
}
