using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class connecting : MonoBehaviour
{
    public GameObject bottons;
   public Text search;
   int dotcounter;
   public bool stopconnecting;
    void OnEnable()
    {
        bottons.SetActive(false);
        search.text="Connecting";
        stopconnecting=false;
        dotcounter=0;
        dotAdder();
    }

    

    void Update()
    {
        
    }
    void dotAdder()
    {
        if(dotcounter>5)
        {
            search.text="Connecting";
            dotcounter=0;
        }
        search.text+=".";
        dotcounter++;
        if(!stopconnecting)
        {
            Invoke("dotAdder",0.5f);
        }
    }
}
