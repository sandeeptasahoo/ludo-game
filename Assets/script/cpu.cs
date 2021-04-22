using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cpu : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject redOriginGm;
    GameObject greenOriginGm;
    GameObject yellowOriginGm;
    GameObject blueOriginGm;
    GameObject[] redPawns;
    GameObject[] greenPawns;
    GameObject[] yellowPawns;
    GameObject[] bluePawns;
    GameObject[][] pawns;
    void Awake()
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

     void storeElements(ref GameObject[] arr,ref GameObject gm,int a)
    {
        arr=new GameObject[a];
        for(int i=0;i<a;i++)
        {
           arr[i]= gm.transform.GetChild(i).gameObject;
        }

    }

    
}
