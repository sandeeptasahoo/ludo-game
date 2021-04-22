using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class waitingpanel : MonoBehaviour
{
   public GameObject bottons;
   public Text search;
   public networkconnector networkconnector;
   int dotcounter;
    void OnEnable()
    {
        bottons.SetActive(false);
        search.text="Searching";
        dotcounter=0;
        dotAdder();
    }


    void Update()
    {
        if(Application.platform==RuntimePlatform.Android)
        {
            if(Input.GetKey(KeyCode.Escape))
            {
               this.gameObject.SetActive(false);
               networkconnector.disconnectFromNetwork();

            }
        }
    }
    void dotAdder()
    {
        if(dotcounter>5)
        {
            search.text="Searching";
            dotcounter=0;
        }
        search.text+=".";
        dotcounter++;
        Invoke("dotAdder",0.5f);
    }
}
