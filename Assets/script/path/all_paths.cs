using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class all_paths : MonoBehaviour
{
    public Transform[] commonPathpoints;
    public Transform[] RedPathpoint;
    public Transform[] BluePathpoint;
    public Transform[] GreenPathpoint;
    public Transform[] YellowPathpoint;
    public Transform[][] safePath;
    public Transform[] redOrigin;
    public Transform[] greenOrigin;
    public Transform[] blueOrigin;
    public Transform[] yellowOrigin;

    GameObject redOriginGm;
    GameObject greenOriginGm;
    GameObject yellowOriginGm;
    GameObject blueOriginGm;

    void Awake()
    {
        safePath=new Transform[4][];
        safePath[0]=RedPathpoint;
        safePath[1]=GreenPathpoint;
        safePath[2]=YellowPathpoint;
        safePath[3]=BluePathpoint;
        redOriginGm=GameObject.FindGameObjectWithTag("redOrigin");
        storeElements(ref redOrigin,ref redOriginGm,4);
        greenOriginGm=GameObject.FindGameObjectWithTag("greenOrigin");
        storeElements(ref greenOrigin,ref greenOriginGm,4);
        blueOriginGm=GameObject.FindGameObjectWithTag("blueOrigin");
        storeElements(ref blueOrigin,ref blueOriginGm,4);
        yellowOriginGm=GameObject.FindGameObjectWithTag("yellowOrigin");
        storeElements(ref yellowOrigin,ref yellowOriginGm,4);
    }

    void storeElements(ref Transform[] arr,ref GameObject gm,int a)
    {
        arr=new Transform[a];
        for(int i=0;i<a;i++)
        {
           arr[i]= gm.transform.GetChild(i);
        }

    }
   
}
