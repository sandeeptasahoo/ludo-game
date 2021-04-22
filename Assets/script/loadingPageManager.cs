using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
public class loadingPageManager : MonoBehaviour
{
    networkconnector networkconnector;
    void Start()
    {
        networkconnector=FindObjectOfType<networkconnector>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Application.platform==RuntimePlatform.Android)
        {
            if(Input.GetKey(KeyCode.Escape))
            {
                networkconnector.disconnectFromNetwork();
                
            }
        }
    }

    
}
