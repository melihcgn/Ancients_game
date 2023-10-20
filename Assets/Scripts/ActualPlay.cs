using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using playerNS;
public class ActualPlay : MonoBehaviour
{
    private createPanels cPanels;
    public void afterPlayerAction()
    {
        
        cPanels = GetComponent<createPanels>();
        List<Players> playersList = cPanels.ps;

        for (int i = 0; i < playersList.Count; i++)
        {
            Debug.Log(  "worry about ya!!");
            Players thePlayer = playersList[i];
            if (thePlayer.dead == true && thePlayer.protectd == false && thePlayer.rescued == false)
            {
                Debug.Log(thePlayer.pname + " is dead!!");
            }
            else if(thePlayer.charmed == true)
            {
                Debug.Log(thePlayer.pname + " is charmed!!");
            }
            else if(thePlayer.revealed == true)
            {
                Debug.Log(thePlayer.pname + " is revealed!!");
            }
        }       
    }
}