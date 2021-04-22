using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    public int playerIndex;
    public int startingPosition;
    public int homeEntryPosition;
    public int playerstate;//-1=kill,0=intial,1=running,2=safe,3=home
    public int moves;
    all_paths all_Paths;
    player_piece player_Piece;
    showPlayer showPlayer;
    public int currentPlaceIndex;
    public int targetPlaceIndex;
    Transform target;
    rollingdice rollingdice;
    public int targetIndex;
    Vector3 origin;
    bool kill=false;
    AudioSource audioSource;
    AudioClip moveSound;

    void Awake()
    {
        currentPlaceIndex=startingPosition-1;
        all_Paths=FindObjectOfType<all_paths>();
        player_Piece=FindObjectOfType<player_piece>();
        showPlayer=FindObjectOfType<showPlayer>();
        rollingdice=FindObjectOfType<rollingdice>();
        audioSource=gameObject.AddComponent<AudioSource>();
        moveSound=Resources.Load("PlayerMoveSound") as AudioClip;
        audioSource.volume=0.5f;
        origin=transform.position;
    }

    void Update()
    {
        
    }

    public void movePlayer()
    {
        kill=false;
        if(playerstate==1)
            {
                all_Paths.commonPathpoints[currentPlaceIndex].gameObject.GetComponent<placeholders>().removePlayer(gameObject,playerIndex);
            }
        else if(playerstate>1) 
            {
                all_Paths.safePath[playerIndex][currentPlaceIndex].gameObject.GetComponent<placeholders>().removePlayer(gameObject,playerIndex);
            }
        if(moves==6 && playerstate==0)
        {
            player_Piece.playerEligibilty[playerIndex]++;
            playerstate++;
            targetPlaceIndex=startingPosition;
            currentPlaceIndex=startingPosition-1;
            target=all_Paths.commonPathpoints[startingPosition];
            targetIndex=targetPlaceIndex;
            moveStep(0.5f);
        }
        else if(playerstate==1 && currentPlaceIndex!=homeEntryPosition)
        {
            targetPlaceIndex=currentPlaceIndex+moves;
            if(targetPlaceIndex>51)
            {
                targetPlaceIndex-=52;
            }
            targetIndex=currentPlaceIndex+1;
            if(targetIndex>51)
            {
                targetIndex-=52;
            }
            target=all_Paths.commonPathpoints[targetIndex];
            moveStep(0.5f);
        }
        else 
        {
            if(currentPlaceIndex==homeEntryPosition)
            {
                playerstate++;
                currentPlaceIndex=-1;
            }
            targetPlaceIndex=currentPlaceIndex+moves;
            if(targetPlaceIndex==5)
            {
                playerstate++;
            }
            targetIndex=currentPlaceIndex+1;
            target=all_Paths.safePath[playerIndex][targetIndex];
            moveStep(0.5f);
        }
    }

    void moveStep(float timelaps)
    {
        changePosition();
        currentPlaceIndex=targetIndex;
        Invoke("endMove",timelaps);
    }
    void changePosition()
    {
        transform.position=target.position;
        audioSource.PlayOneShot(moveSound);
    }
    

    
    void endMove()
    {
        if(currentPlaceIndex==targetPlaceIndex)
        {
            if(playerstate==3)
            {
                bool win=player_Piece.goalAchieved(playerIndex,transform.position);
                
                if(win)
                {
                    return;
                }
                rollingdice.repeated6=0;
                rollingdice.diceRoll(false);
                return;
            }
            else if(playerstate==1)
            {
                kill=all_Paths.commonPathpoints[currentPlaceIndex].gameObject.GetComponent<placeholders>().addplace(gameObject,playerIndex);
            }
            else if(playerstate==2)
            {
                kill=all_Paths.safePath[playerIndex][currentPlaceIndex].gameObject.GetComponent<placeholders>().addplace(gameObject,playerIndex);
            }
            else
            {
                currentPlaceIndex=-1;
                playerstate=0;
                transform.position=origin;
                return;
            }
            if(!kill)
            {
                if(moves==6)
                {
                    rollingdice.diceRoll(false);
                }
                else
                {
                    showPlayer.showPlayerImage();
                }
            }
            else
            {
                rollingdice.repeated6=0;
                rollingdice.diceRoll(false);
            }
            
            this.enabled=false;
            
        }
        else if(playerstate==1)
        {
            if(currentPlaceIndex>51)
            {
                currentPlaceIndex-=52;
            }
            if(currentPlaceIndex==homeEntryPosition)
            {
                changeRoot();
                moveStep(0.5f);
                return;
            }
            targetIndex=currentPlaceIndex+1;
            if(targetIndex>51)
            {
                targetIndex-=52;
            }
            target=all_Paths.commonPathpoints[targetIndex];
            moveStep(0.5f);
            //move=true;
        }
        else if(playerstate==-1)
        {
            targetIndex=currentPlaceIndex-1;
            if(targetIndex<0)
            {
                targetIndex=51;
            }
            target=all_Paths.commonPathpoints[targetIndex];
            moveStep(0.1f);
        }
        else 
        {
            targetIndex=currentPlaceIndex+1;
            target=all_Paths.safePath[playerIndex][targetIndex];
            moveStep(0.5f);

        }
    }

    void changeRoot()
    {
        playerstate++;
        targetIndex=0;
        if(targetPlaceIndex-currentPlaceIndex<0)
        {
            targetPlaceIndex=targetPlaceIndex-currentPlaceIndex+51;
        }
        else
        {
            targetPlaceIndex=targetPlaceIndex-currentPlaceIndex-1;
        }
        target=all_Paths.safePath[playerIndex][targetIndex];
    }

    public bool evaluateMove(int move)
    {
        if(currentPlaceIndex+move>5)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void playerBackToOrigin()
    {
        targetPlaceIndex=startingPosition;
        targetIndex=currentPlaceIndex-1;
        if(targetIndex<0)
        {
            targetIndex=51;
        }
        target=all_Paths.commonPathpoints[targetIndex];
        player_Piece.playerEligibilty[playerIndex]--;
        playerstate=-1;
        moveStep(0.1f);

    }

    public bool checkStartPosition()
    {
        if(currentPlaceIndex==startingPosition && playerstate==1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

   
}
