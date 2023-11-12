using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using playerNS;
using System.Linq;
using TMPro;
using UnityEngine.UI;
public class ActualPlay : MonoBehaviour
{
    private createPanels cPanels;
    List<Players> deadPlayers = new List<Players>();
    List<Players> killPlayers = new List<Players>();
    List<GameObject> deadPanels = new List<GameObject>();
    public void afterPlayerAction()
    {
        
        
        cPanels = GetComponent<createPanels>();

        List<GameObject> panels = cPanels.panels;
        List<GameObject> playerPanels = cPanels.playerPanels;

        List<Players> playersList = cPanels.ps;

        

        int pListCount = playersList.Count;

        for (int i = 0; i < pListCount; i++)
        {
            Players thePlayer = playersList[i];
            if (thePlayer.charmed == true)
            {
                if ( thePlayer.visitedPlayer!= "" && thePlayer.action != "")
                {

                    Players deniedPlayer = playersList.FirstOrDefault(obj => obj.pname == thePlayer.visitedPlayer);
                    if (thePlayer.action == "kill")
                    {
                        deniedPlayer.dead = false;
                    }
                    else if (thePlayer.action == "protect")
                    {
                        deniedPlayer.protectd = false;
                    }
                    else if (thePlayer.action == "rescue")
                    {
                        deniedPlayer.rescued = false;
                    }
                    else if (thePlayer.action == "reveal")
                    {
                        if (thePlayer.role == "AI TOYON")
                        {
                            thePlayer.revealed = false;
                        }
                        else if (thePlayer.role == "AI TOYON")
                        {
                            deniedPlayer.revealed = false;

                        }
                    }
                    else if (thePlayer.action == "charm")
                    {
                        deniedPlayer.charmed = false;
                    }
                    else
                    {

                    }
                }
                Debug.Log(thePlayer.pname + " is charmed!!");
                thePlayer.charmed = false;
            }

        }

        for (int i = 0; i < pListCount; i++)
        {
            Debug.Log(  "worry about ya!!");
            Players thePlayer = playersList[i];
            // checking he is dead or not
            if (thePlayer.dead == true && thePlayer.protectd == false && thePlayer.rescued == false)
        
            {
                
                string thePlayerName = thePlayer.pname;
                if (playersList.Contains(thePlayer))
                {
                    killPlayers.Add(thePlayer);
                    Debug.Log("Removed: " + thePlayerName);
                     Debug.Log(thePlayerName + " is dead!!");
                    GameObject deadPlayer = panels.FirstOrDefault(obj => obj.name == thePlayer.pname);
                    deadPlayers.Add(thePlayer);
                    deadPanels.Add(panels[2 * i]);
                    deadPanels.Add(panels[2 * i +1]);
                }
               
                
                for (int m = 0; m < panels.Count; m++)
                {
                    if (panels[m].name == thePlayerName)
                    {
                        panels.RemoveAt(m);
                        panels.RemoveAt(m-1);
                    }
                }


                for (int k = 1; k < panels.Count; k = k + 2)
                {

                        //GameObject otherPlayer = panels.FirstOrDefault(obj => obj.name == playersList[k].pname);
                    GameObject otherPlayer = panels[k];
                    Transform otherPlayerTransform = otherPlayer.transform;
                    GameObject profilesObject = otherPlayerTransform.Find("Names").gameObject;
                    if (profilesObject != null)
                    {
                        FindObjectWithSpecificTextInChildren(profilesObject.transform, thePlayerName);
                    }
                    else
                    {
                        Debug.Log("caba caba abaa");
                    }
                    

                }
                Button passButton = playerPanels[playerPanels.Count -1].transform.Find("passButton").GetComponent<Button>();
                Button pickButton = playerPanels[playerPanels.Count -1].transform.Find("pickButton").GetComponent<Button>();
                ActualPlay Aplay = GetComponent<ActualPlay>();
                if (Aplay != null)
                {
                    pickButton.onClick.AddListener(Aplay.afterPlayerAction);
                    passButton.onClick.AddListener(Aplay.afterPlayerAction);
                }
                else
                {
                    Debug.Log("WOKEGEEE!");
                }
            }
            
            else
            {
                
                if (thePlayer.dead == true && thePlayer.protectd == true)
                {
                    int count = 0;
                    for (int k = 0; k < pListCount; k++)
                    {
                        Players visitingPlayer = playersList[k];
                        if (visitingPlayer.visitedPlayer == thePlayer.pname)
                        {
                            deadPlayers.Add(visitingPlayer);
                            killPlayers.Add(visitingPlayer);
                            deadPanels.Add(panels[2 * (k-count)]);
                            deadPanels.Add(panels[2 * (k-count) +1]);
                            panels.RemoveAt(2 * (k-count));
                            panels.RemoveAt(2 * (k-count) +1);
                            count++;
                            if (count == 2)
                            {
                                break;
                            }
                        } 
                    }
                    thePlayer.dead = false;
                    thePlayer.protectd = false;
                    Debug.Log(thePlayer.pname + " is protected!");
                }

                else if (thePlayer.dead == true && thePlayer.rescued == true)
                {
                    thePlayer.dead = false;
                    thePlayer.protectd = false;
                    Debug.Log(thePlayer.pname + " is rescued!");
                }

                
                else if(thePlayer.revealed == true)
                {
                    if (thePlayer.role == "AI TOYON")
                        {
                            Debug.Log(thePlayer.pname + " revealed himself!! His role is " + thePlayer.role);
                        }
                        else 
                        {
                           Debug.Log(thePlayer.pname + " is revealed!! His role is " + thePlayer.role);

                        }
                    
                }
            }

            
        }    

        for (int p = 0; p < killPlayers.Count; p++)
        {
            playersList.Remove(killPlayers[p]);
        }   
        killPlayers.Clear();
    }


    public void killVotedPlayer(string playerWillKilled)
    {
        cPanels = GetComponent<createPanels>();

        List<GameObject> panels = cPanels.panels;
        List<GameObject> playerPanels = cPanels.playerPanels;

        List<Players> playersList = cPanels.ps;
        int pListCount = playersList.Count;

        Players thePlayer = playersList.FirstOrDefault(obj => obj.pname == playerWillKilled);
        GameObject thePlayerPanel = panels.FirstOrDefault(obj => obj.name == playerWillKilled);
        killPlayers.Add(thePlayer);
        Debug.Log("Removed: " + playerWillKilled);
        Debug.Log(playerWillKilled + " is dead!!");
                    
        deadPlayers.Add(thePlayer);
        
        for (int m = 0; m < panels.Count; m++)
                {
                    if (panels[m].name == playerWillKilled)
                    {
                        deadPanels.Add(panels[2 * m]);
                        deadPanels.Add(panels[2 * m +1]);
                        panels.RemoveAt(m);
                        panels.RemoveAt(m-1);
                        break;
                    }
                }
        for (int k = 1; k < panels.Count; k = k + 2)
                {

                        //GameObject otherPlayer = panels.FirstOrDefault(obj => obj.name == playersList[k].pname);
                    GameObject otherPlayer = panels[k];
                    Transform otherPlayerTransform = otherPlayer.transform;
                    GameObject profilesObject = otherPlayerTransform.Find("Names").gameObject;
                    if (profilesObject != null)
                    {
                        FindObjectWithSpecificTextInChildren(profilesObject.transform, playerWillKilled);
                    }
                    else
                    {
                        Debug.Log("caba caba abaa");
                    }
                    

                }
                Button passButton = playerPanels[playerPanels.Count -1].transform.Find("passButton").GetComponent<Button>();
                Button pickButton = playerPanels[playerPanels.Count -1].transform.Find("pickButton").GetComponent<Button>();
                ActualPlay Aplay = GetComponent<ActualPlay>();
                if (Aplay != null)
                {
                    pickButton.onClick.AddListener(Aplay.afterPlayerAction);
                    passButton.onClick.AddListener(Aplay.afterPlayerAction);
                }
                else
                {
                    Debug.Log("WOKEGEEE!");
                }
        

        for (int p = 0; p < killPlayers.Count; p++)
        {
            playersList.Remove(killPlayers[p]);
        }   
        killPlayers.Clear();
    }
    void FindObjectWithSpecificTextInChildren(Transform parentTransform, string nameToDelete)
    {

        Button buttonToDelete = null;
        int buttonIndexToDelete = -1;

        for (int i = 0; i < parentTransform.childCount; i++)
        {
            Debug.Log("Countof the buttons: " + i);
            Transform child = parentTransform.GetChild(i);
            Button buttonComponent = child.GetComponent<Button>();
            TextMeshProUGUI textComponent = child.GetComponentInChildren<TextMeshProUGUI>();

            if (buttonComponent != null && textComponent != null && textComponent.text == nameToDelete)
            {
                buttonToDelete = buttonComponent;
                Debug.Log("Name of the Button: " + buttonToDelete);
                Destroy(buttonToDelete.gameObject);

                buttonIndexToDelete = i;
                break;
            }
        }
        /*
        while (x < 10)
        {
            if (buttonToDelete != null && buttonIndexToDelete >= 0)
            {
                if (buttonIndexToDelete < parentTransform.childCount - 1)
                {
                    // Swap the button with the next one
                    Button nextButton = parentTransform.GetChild(buttonIndexToDelete + 1).GetComponent<Button>();
                    TextMeshProUGUI textComponent = nextButton.GetComponentInChildren<TextMeshProUGUI>();
                    Debug.Log("yayayayay gorko : " + textComponent.text);
                    Transform buttonParent = buttonToDelete.transform.parent;
                    buttonToDelete.transform.SetParent(nextButton.transform.parent);
                    nextButton.transform.SetParent(buttonParent);
                    buttonIndexToDelete++;  
                }
                else
                {
                    // The buttonToDelete is the last one, delete it
                    Destroy(buttonToDelete.gameObject);
                    break;
                }
            }
            else
            {
                // If the buttonToDelete is not found, break out of the loop
                break;
            }
            x++;
        }*/
    }
}