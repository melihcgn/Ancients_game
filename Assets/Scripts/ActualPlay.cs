using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using playerNS;
public class ActualPlay : MonoBehaviour
{
    public GameObject scriptMoves;
    private createPanels cPanels;
    public void afterPlayerAction()
    {
        
        cPanels = scriptMoves.GetComponent<createPanels>();
        List<Players> playersList = cPanels.ps;

        for (int i = 0; i < playersList.Count; i++)
        {
            if (playersList[i].dead == true)
            {
                Debug.Log(playersList[i].pname + " is dead!!");
            }
            else
            {
                Debug.Log(playersList[i].pname + " is not dead. Nothing to worry :)");
            }
        }       
    }
}
