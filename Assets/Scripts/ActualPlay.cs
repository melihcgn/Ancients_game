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
    List<GameObject> deadPanels = new List<GameObject>();
    public void afterPlayerAction()
    {
        
        
        cPanels = GetComponent<createPanels>();

        List<GameObject> panels = cPanels.panels;

        List<Players> playersList = cPanels.ps;

        

        for (int i = 0; i < playersList.Count; i++)
        {
            Debug.Log(  "worry about ya!!");
            Players thePlayer = playersList[i];
            // checking he is dead or not
            if (thePlayer.dead == true && thePlayer.protectd == false && thePlayer.rescued == false)
            {
                
                string thePlayerName = thePlayer.pname;
                if (playersList.Contains(thePlayer))
                {
                    playersList.Remove(thePlayer);
                    Debug.Log("Removed: " + thePlayerName);
                }
                Debug.Log(thePlayerName + " is dead!!");
                GameObject deadPlayer = panels.FirstOrDefault(obj => obj.name == thePlayer.pname);
                deadPlayers.Add(thePlayer);
                deadPanels.Add(panels[2 * i]);
                deadPanels.Add(panels[2 * i +1]);
                
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
                Button passButton = panels[panels.Count -3].transform.Find("passButton").GetComponent<Button>();
                Button pickButton = panels[panels.Count -3].transform.Find("pickButton").GetComponent<Button>();
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
                 if (thePlayer.charmed == true)
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