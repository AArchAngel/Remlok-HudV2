﻿
using UnityEngine;


public class HUDActivate : MonoBehaviour
{
    private void Update()
    {



        if (Input.GetKeyDown("h"))
        {

            GetComponent<Unity_Overlay>().enabled = false;
            Debug.Log("should be empire");
        }
        if (Input.GetKeyDown("j"))
        {

            GetComponent<Unity_Overlay>().enabled = true;
            Debug.Log("should be empire");
        }
    }
}
