using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    public int killweight;
    public int goalWeight;
    public int PawnStartWeight;
    public int safetyweight;
   public int playerIndex;
   public GameObject[] playerAvailable;
   public int activeplayernum;
   public int steps;
   
   all_paths all_Paths;
   player_piece player_Piece;


    void Awake()
    {
        all_Paths =FindObjectOfType<all_paths>();
        player_Piece=FindObjectOfType<player_piece>();
        playerAvailable=new GameObject[4];
        killweight=Random.Range(0,6);
        goalWeight=Random.Range(0,6);
        PawnStartWeight=Random.Range(0,6);
        safetyweight=Random.Range(0,6);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Aidecide()
    {
        int perk;
        int maxperk=0;
        int player=0;
        for(int i=0;i<activeplayernum;i++)
        {
            perk=playerperks(i);
            if(perk>maxperk)
            {
                maxperk=perk;
                player=i;
            }
        }
        if(maxperk==0)
        {
            player=Random.Range(0,activeplayernum);
        }
        player_Piece.pawns[playerIndex][player].GetComponent<movesensor>().activatePlayer();

        
    }

    int playerperks(int playerindex)
    {
        int placeindex=playerAvailable[playerindex].GetComponent<player>().currentPlaceIndex;
        int playerState=playerAvailable[playerindex].GetComponent<player>().playerstate;
        int a=0;
        int b=0;
        int c=0;
        int safety=0;
        int totalPerks=0;
        if(playerState==0)
        {
            c=1;
        }
        else
        {
            a=killpossible(playerIndex,placeindex,playerState);
            safety=safecheck(placeindex,playerState);
            if(playerState==2)
            {
                b=goalPossible(playerindex,placeindex);
            }
        }
        totalPerks=((a*killweight)+(b*goalWeight)+(c*PawnStartWeight)+(safety*safetyweight));
        return totalPerks;
        
    }

    int safecheck(int placeindex,int playerstate)
    {
        bool possible;
        if(playerstate==1)
        {
            possible=all_Paths.commonPathpoints[placeindex].GetComponent<placeholders>().safePosition;
        }
        else 
        {
            possible=all_Paths.safePath[playerIndex][placeindex].GetComponent<placeholders>().safePosition;
        }
        if (possible)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }

    int goalPossible(int playerindex,int placeindex)
    {
        bool possible=playerAvailable[playerindex].GetComponent<player>().evaluateMove(steps);
        if(possible)
        return 1;
        else
        return 0;
    }

    int killpossible(int playerindex,int placeindex,int playerState)
    {
        bool possible;
        placeindex=Mathf.Clamp(placeindex+steps,0,51);
        
        if(playerState==1)
        {
            possible=all_Paths.commonPathpoints[placeindex].GetComponent<placeholders>().checkKillAi(playerindex);
        }
        else if(playerState==2)
        {
            possible=all_Paths.safePath[playerIndex][placeindex].GetComponent<placeholders>().checkKillAi(playerindex);
        }
        else
        {
            possible=false;
        }
        if(possible)
        return 1;
        else
        return 0;
    }
}
