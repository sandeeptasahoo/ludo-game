using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class playerselectionmanager : MonoBehaviour
{
    datacarrier datacarrier;
    public GameObject selectionMenu;
    public string[] playerTags; 
    public string[] playerQuestions;
    int playernum;
    int player;
    int playercolor;
    GameObject rightsymbol;
    GameObject text;
    public Text question;
    bool choice=false;
    
    void Awake()
    {
        
        rightsymbol=selectionMenu.transform.GetChild(1).GetChild(1).gameObject;
        
    }
    void  Start()
    {
        datacarrier=FindObjectOfType<datacarrier>();
        for(int i=0;i<4;i++)
        {
            datacarrier.availableType[i]=false;
        }
    }

    void Update()
    {
        if(Application.platform==RuntimePlatform.Android)
        {
            if(Input.GetKey(KeyCode.Escape))
            {
                SceneManager.LoadScene("MainMenu");
            }
        }
    }
    public void selectPlayerNumber(int num)
    {
        datacarrier.numOfPlayerType=num;
        playernum=num;
        selectionMenu.SetActive(true);
    }

    public void colourselect(int num)
    {
        rightsymbol.SetActive(false);
        rightsymbol=selectionMenu.transform.GetChild(num).GetChild(1).gameObject;
        rightsymbol.SetActive(true);
        playercolor=num-1;
        text=selectionMenu.transform.GetChild(num).GetChild(2).gameObject;
        choice=true;
        

    }

    public void submitcolor()
    {
        if(!choice)
        {
            return;
        }
        choice=false;
        
        datacarrier.availableType[playercolor]=true;
        rightsymbol.SetActive(false);
        text.GetComponent<Text>().text=playerTags[player];
        datacarrier.colourpairlist[playercolor]=player+1;
        player++;
        if(player+1>playernum)
        {
            SceneManager.LoadScene("ludo_game_scene");
            return;
        }
        question.text=playerQuestions[player];
    }
}
