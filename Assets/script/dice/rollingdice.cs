using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class rollingdice : MonoBehaviourPun,IPunOwnershipCallbacks
{
    [SerializeField ]public Sprite[] facenumber;
    [SerializeField ]public Transform[] dicePosition;
    //public AudioClip rollingSound;
    [SerializeField ]public AudioClip faceUpSound;
    //public AudioClip switchPlaceSound;
    [SerializeField ]int dicePositionIndex=-1;
    [SerializeField ]Animator anim;
    [SerializeField ]SpriteRenderer fdice;
    [SerializeField ]public int value;
   // public GameObject gamemanager;
    [SerializeField ]player_piece player_Piece;
    [SerializeField ]BoxCollider2D col;
    [SerializeField ]showPlayer show;
    [SerializeField ]public int repeated6;
    [SerializeField ]public bool randomDice;
    [SerializeField ]AudioSource audioSource;
    [SerializeField ]Photon.Realtime.Player requestFromPlayer;
    datacarrier datacarrier;
    bool online;
    bool[] aiAvailable;
    

    void Awake()
    {
        anim=GetComponent<Animator>();
        fdice=GetComponent<SpriteRenderer>();
        player_Piece=FindObjectOfType<player_piece>();
        show=FindObjectOfType<showPlayer>();
        col=GetComponent<BoxCollider2D>();
        audioSource=GetComponent<AudioSource>();
        datacarrier=FindObjectOfType<datacarrier>();
        online=datacarrier.onlinestate;
        aiAvailable=datacarrier.aiPlayer;
    }

    public void diceRoll(bool replace)
    {
        //Debug.Log(repeated6);
        if(replace)
        {
            placeDice();
        }
        //fdice.enabled=false;
        anim.enabled=true;
        col.enabled=true;
        anim.Play("ludo_animationanim");
        if(aiAvailable[dicePositionIndex])
        {
            Invoke("diceOut",0.5f);
        }
        /*audioSource.clip=rollingSound;
        audioSource.loop=true;
        audioSource.Play();
        */
        //audioSource.PlayOneShot(rollingSound);
    }

    void placeDice()
    {
        dicePositionIndex=show.playerIndex;
        transform.position=dicePosition[dicePositionIndex].position;
        //audioSource.PlayOneShot(switchPlaceSound);
    }
    
    void Update()
    {
        
        
        
    }

    void diceOut()
    {
        if(randomDice)
        {
            value=Random.Range(1,7);
        }
        //value=Random.Range(5,7);
        fdice.sprite=facenumber[value-1];
        //manager.moveplace=value;
        anim.enabled=false;
        //audioSource.Stop();
        audioSource.PlayOneShot(faceUpSound);
        audioSource.loop=false;
        
        col.enabled=false;
        //fdice.enabled=true;
        //manager.stateIndex++;
        if(value==6)
        {
            repeated6++;
        }
        else
        {
            repeated6=0;
        }
        //Debug.Log(repeated6);
        if(repeated6==3)
        {
            //Debug.Log("showimage");
            show.showPlayerImage();
            repeated6=0;
            
        }
        else
        {
            //Debug.Log("move evaluator");
            player_Piece.moveEvaluator(value);
        }
        this.enabled=false;

    }
    void OnMouseDown()
    { 
        if(online)
        {  if(base.photonView.IsMine)
            {
                diceOut();
            }
        }
        else
        {
            diceOut();
        }
    }

    public void changeOwnership(Photon.Realtime.Player player)
    {
        base.photonView.TransferOwnership(player);
    }

    public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
    {
        if(targetView!=base.photonView)
        {
            return;
        }
        if(requestingPlayer==requestFromPlayer)
        {
            targetView.TransferOwnership(requestingPlayer);
        }
    }

    public void OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
    {
        Debug.Log("Ownership changed from "+previousOwner+"to "+requestFromPlayer);
    }

    public void OnOwnershipTransferFailed(PhotonView targetView, Player senderOfFailedRequest)
    {
        Debug.Log("owner transfer failed");
    }

}
