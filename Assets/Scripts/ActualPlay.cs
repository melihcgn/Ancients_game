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
    public List<Players> deadPlayers = new List<Players>();
    List<Players> killPlayers = new List<Players>();
    List<GameObject> deadPanels = new List<GameObject>();
    GameObject lastPlayer;
    public void afterPlayerAction()
    {

        //Debug.Log("bakalım 2 defamı");
        cPanels = GetComponent<createPanels>();

        List<GameObject> panels = cPanels.panels;
        List<GameObject> playerPanels = cPanels.playerPanels;
        GameObject votingPanel = cPanels.votingPanelTo;
        List<Players> playersList = cPanels.ps;
        lastPlayer = playerPanels[playerPanels.Count - 1];
        List<GameObject> umayButtons = cPanels.umayButtons;


        int pListCount = playersList.Count;

        for (int i = 0; i < pListCount; i++)
        {
            Players thePlayer = playersList[i];
            if (thePlayer.charmed == true)
            {
                if (thePlayer.visitedPlayer != "" && thePlayer.action != "")
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
                    else if (thePlayer.action == "retrieve")
                    {
                        deniedPlayer.dead = true;

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

        //retrieving the player 
        int dListCount = deadPlayers.Count;

        for (int p = 0; p < dListCount; p++)
        {
            if (deadPlayers[p].dead == false)
            {
                Debug.Log("deadPlayers: " + deadPlayers[p].pname);
                string playerName = deadPlayers[p].pname;
                string rname = deadPlayers[p].role;
                RetrieveThePlayer(playerName, rname);
            }
        }
        pListCount = playersList.Count;
        for (int i = 0; i < pListCount; i++)
        {

            Players thePlayer = playersList[i];


            //checking for Alaz Han
            if(thePlayer.alazPower == true)
            {
                foreach(string playerName in thePlayer.visitors )
                {
                    Players willKilled = playersList.FirstOrDefault(player => player.pname == playerName);
                    willKilled.changeDead(true); 
                }
            }


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
                    deadPanels.Add(panels[2 * i + 1]);
                }


                for (int m = 0; m < panels.Count; m++)
                {
                    if (panels[m].name == thePlayerName)
                    {
                        panels.RemoveAt(m - 1);
                        panels.RemoveAt(m - 1);
                        playerPanels.RemoveAt(m - 1);
                        playerPanels.RemoveAt(m - 1);
                    }
                }


                for (int k = 1; k < playerPanels.Count; k = k + 2)
                {

                    //GameObject otherPlayer = panels.FirstOrDefault(obj => obj.name == playersList[k].pname);
                    GameObject otherPlayer = playerPanels[k];
                    string parentPlayerName = otherPlayer.name;
                    Debug.Log("otherPlayer: " + otherPlayer.name);
                    Players parentPlayer = playersList.FirstOrDefault(obj => obj.pname == parentPlayerName);
                    Debug.Log("parentPlayer111: " + parentPlayer.role);
                    string parentPlayerRole = parentPlayer.role;
                    Debug.Log("parentPlayer: " + parentPlayerRole);
                    Transform otherPlayerTransform = otherPlayer.transform;
                    GameObject profilesObject = otherPlayerTransform.Find("Names").gameObject;
                    if (profilesObject != null)
                    {
                        FindObjectWithSpecificTextInChildren(profilesObject.transform, thePlayerName, parentPlayerRole);
                    }
                    else
                    {
                        Debug.Log("caba caba abaa");
                    }


                }
                //deleting the player node from voting page
                Transform votablePlayerTf = votingPanel.transform;
                GameObject votableObjects = votablePlayerTf.Find("Names").gameObject;
                FindObjectWithSpecificTextInChildren(votableObjects.transform, thePlayerName, thePlayerName);
                GameObject thePlayerPanel = playerPanels[playerPanels.Count - 1];
                Button passButton = thePlayerPanel.transform.Find("passButton").GetComponent<Button>();
                Button pickButton = thePlayerPanel.transform.Find("pickButton").GetComponent<Button>();
                ActualPlay Aplay = GetComponent<ActualPlay>();
                Debug.Log("lastPlayer: " +lastPlayer.name + "thePlayerPanel: " +thePlayerPanel.name);
                if (Aplay != null && lastPlayer.name != thePlayerPanel.name)
                {   
                    Debug.Log("DURUUDUDURUR");
                    lastPlayer = playerPanels[playerPanels.Count - 1];
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
                            deadPanels.Add(playerPanels[2 * (k - count)]);
                            deadPanels.Add(playerPanels[2 * (k - count) + 1]);
                            panels.RemoveAt(2 * (k - count));
                            panels.RemoveAt(2 * (k - count));
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


                else if (thePlayer.revealed == true)
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
        //Debug.Log("DENİTORUZZ HOCAM");
        for (int p = 0; p < killPlayers.Count; p++)
        {
            playersList.Remove(killPlayers[p]);
        }
        killPlayers.Clear();
        bool endGame = true;
        for (int j = 0; j < playersList.Count; j++)
        {

            if (playersList[j].role == "TEPEGOZ" || playersList[j].role == "SU IYESI" || playersList[j].role == "GULYABANI")
            {
                endGame = false;
            }
        }
        string endMessage;
        if (endGame) // ending game phase
        {
            endMessage = "Town wins";
            cPanels.createEndPanel(endMessage);
        }
        else
        {
            if (playersList.Count == 2)
            {
                for (int j = 0; j < playersList.Count; j++)
                {
                    Debug.Log("playersList[j].role = " + playersList[j].role);
                    if (playersList[j].role == "TEPEGOZ")
                    {
                        //Tepegöz wins
                        endMessage = "Tepegöz wins";
                        cPanels.createEndPanel(endMessage);
                        endGame = true;
                    }
                    else if (playersList[j].role == "SU IYESI")
                    {
                        endMessage = "Su Iyesi wins";
                        cPanels.createEndPanel(endMessage);
                        endGame = true;
                        // Su iyesi wins 
                    }
                    else if (playersList[j].role == "GULYABANI")
                    {
                        endMessage = "Gulyabani wins";
                        cPanels.createEndPanel(endMessage);

                        endGame = true;
                        // Gulyabani wins
                    }
                }
            }
        }
        

        // deleting the visitor for the newcomers
        for (int j = 0; j < playersList.Count; j++)
        {
            playersList[j].visitors.Clear();
        }
        
    }


    public void killVotedPlayer(string playerWillKilled)
    {
        cPanels = GetComponent<createPanels>();

        List<GameObject> panels = cPanels.panels;
        List<GameObject> playerPanels = cPanels.playerPanels;
        GameObject votingPanel = cPanels.votingPanelTo;
        List<Players> playersList = cPanels.ps;
        lastPlayer = playerPanels[playerPanels.Count - 1];




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
                deadPanels.Add(panels[m]);
                deadPanels.Add(panels[m - 1]);
                panels.RemoveAt(m);
                panels.RemoveAt(m);
                break;
            }
        }
        for (int k = 1; k < panels.Count; k = k + 2)
        {

            //GameObject otherPlayer = panels.FirstOrDefault(obj => obj.name == playersList[k].pname);
            GameObject otherPlayer = panels[k];
            // finding role of the parent player for UMAY
            string parentPlayerName = otherPlayer.name;
            Players parentPlayer = playersList.FirstOrDefault(obj => obj.pname == parentPlayerName);
            string parentPlayerRole = parentPlayer.role;
            Transform otherPlayerTransform = otherPlayer.transform;

            GameObject profilesObject = otherPlayerTransform.Find("Names").gameObject;
            if (profilesObject != null)
            {
                FindObjectWithSpecificTextInChildren(profilesObject.transform, playerWillKilled, parentPlayerRole);
            }
            else
            {
                Debug.Log("caba caba abaa");
            }


        }



        Transform votablePlayerTf = votingPanel.transform;
        GameObject votableObjects = votablePlayerTf.Find("Names").gameObject;
        FindObjectWithSpecificTextInChildren(votableObjects.transform, playerWillKilled, playerWillKilled);

        Button passButton = playerPanels[playerPanels.Count - 1].transform.Find("passButton").GetComponent<Button>();
        Button pickButton = playerPanels[playerPanels.Count - 1].transform.Find("pickButton").GetComponent<Button>();
        ActualPlay Aplay = GetComponent<ActualPlay>();
        if (Aplay != null && lastPlayer != playerPanels[playerPanels.Count - 1])
        {
            lastPlayer = playerPanels[playerPanels.Count - 1];
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
    void FindObjectWithSpecificTextInChildren(Transform parentTransform, string nameToDelete, string roleName)
    {

        Button buttonToDelete = null;
        int buttonIndexToDelete = -1;

        for (int i = 0; i < parentTransform.childCount; i++)
        {
            Transform child = parentTransform.GetChild(i);
            Button buttonComponent = child.GetComponent<Button>();
            TextMeshProUGUI textComponent = child.GetComponentInChildren<TextMeshProUGUI>();

            if (buttonComponent != null && textComponent != null && textComponent.text == nameToDelete)
            {
                buttonToDelete = buttonComponent;
                if (roleName == "UMAY")
                {
                    buttonToDelete.gameObject.SetActive(true);
                }
                else
                {
                    buttonToDelete.gameObject.SetActive(false);
                }


                buttonIndexToDelete = i;
                break;
            }
        }

    }

    void RetrieveThePlayer(string nameToRetrieve, string roleName)
    {
        Transform parentTransform;
        int dPanelCount = deadPanels.Count;
        int dListCount = deadPlayers.Count;
        List<GameObject> panels = cPanels.panels;
        List<GameObject> playerPanels = cPanels.playerPanels;
        List<Players> playersList = cPanels.ps;
        GameObject RetrievedPlayerPanel;
        GameObject votingPanel = cPanels.votingPanelTo;
        Players playerRetrieved;
        
        
        Players umayPlayer = playersList.FirstOrDefault(obj => obj.role == "UMAY");

        Button buttonToRetrieve = null;
        int buttonIndexToRetrieve = -1;
        for (int t = 1; t < playerPanels.Count; t = t + 2)
        {
            GameObject otherPlayer = playerPanels[t];

            Transform otherPlayerTransform = otherPlayer.transform;
            parentTransform = otherPlayerTransform.Find("Names").gameObject.transform;
            for (int i = 0; i < parentTransform.childCount; i++)
            {
                Transform child = parentTransform.GetChild(i);
                Button buttonComponent = child.GetComponent<Button>();
                TextMeshProUGUI textComponent = child.GetComponentInChildren<TextMeshProUGUI>();

                if (buttonComponent != null && textComponent != null && textComponent.text == nameToRetrieve)
                {
                    buttonToRetrieve = buttonComponent;
                    if (umayPlayer.pname == otherPlayer.name)
                    {
                        Debug.Log("Umayy geldi: ");
                        buttonToRetrieve.gameObject.SetActive(false);
                    }
                    else
                    {
                        buttonToRetrieve.gameObject.SetActive(true);
                    }


                    buttonIndexToRetrieve = i;
                    break;
                }
            }

        }

        // adding to voting panel
        GameObject votableAgain = votingPanel;

        Transform votableAgainTransform = votableAgain.transform;
        parentTransform = votableAgainTransform.Find("Names").gameObject.transform;
        for (int i = 0; i < parentTransform.childCount; i++)
        {
            Transform child = parentTransform.GetChild(i);
            Button buttonComponent = child.GetComponent<Button>();
            TextMeshProUGUI textComponent = child.GetComponentInChildren<TextMeshProUGUI>();

            if (buttonComponent != null && textComponent != null && textComponent.text == nameToRetrieve)
            {
                buttonToRetrieve = buttonComponent;
                if (umayPlayer.pname == votableAgain.name)
                {
                    Debug.Log("Umayy geldi: ");
                    buttonToRetrieve.gameObject.SetActive(false);
                }
                else
                {
                    buttonToRetrieve.gameObject.SetActive(true);
                }


                buttonIndexToRetrieve = i;
                break;
            }
        }

        // finding role of the parent player for UMAY

        for (int j = 0; j < dListCount; j++)
        {
            if (deadPlayers[j].dead == false)
            {
                int insertionIndex = playerPanels.Count - 3;
                RetrievedPlayerPanel = deadPanels[2 * j];
                playerRetrieved = deadPlayers[j];


                playerPanels.Add(deadPanels[2 * j]);
                playerPanels.Add(deadPanels[2 * j +1]);
                panels.Insert(insertionIndex, deadPanels[2 * j]);
                panels.Insert(insertionIndex, deadPanels[2 * j +1]);
                
                playersList.Add(playerRetrieved);
                deadPlayers.RemoveAt(j);
                deadPanels.RemoveAt(2 * j);
                deadPanels.RemoveAt(2 * j);
                
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