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
        GameObject infoPanel = cPanels.infoPanel;
        List<string> infosList = new List<string>();
        int pListCount = playersList.Count;

        //checking the charmed players first
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
        


        for (int i = 0; i < pListCount; i++) // killing players
        {

            Players thePlayer = playersList[i];


            //checking for Alaz Han
            if(thePlayer.alazPower == true)
            {
                foreach(string plName in thePlayer.visitors )
                {
                    Players willKilled = playersList.FirstOrDefault(player => player.pname == plName); 
                    willKilled.changeDead(true); 
                }
            }

            // giving the output to SIGUN GEYIK
            if (thePlayer.action == "lookout")
            {
                GameObject infoPartPanel = playerPanels.FirstOrDefault(panel => panel.name == thePlayer.pname);
                TextMeshProUGUI infoText = infoPartPanel.transform.Find("passButton").Find("infoText").GetComponent<TextMeshProUGUI>();

                if (thePlayer.visitedPlayer != "")
                {
                    Players visitedBySigun = playersList.FirstOrDefault(obj => obj.pname == thePlayer.visitedPlayer);
                    infoText.text = thePlayer.visitedPlayer + "'s visited by";
                    foreach (string plName in visitedBySigun.visitors)
                    {
                        if (plName != thePlayer.pname)
                        {
                            infoText.text = infoText.text + ", " + plName;

                        }

                    }
                }
                else
                {
                    infoText.text = "";
                }


            }
            else
            {
                GameObject infoPartPanel = playerPanels.FirstOrDefault(panel => panel.name == thePlayer.pname);
                TextMeshProUGUI infoText = infoPartPanel.transform.Find("passButton").Find("infoText").GetComponent<TextMeshProUGUI>();
                infoText.text = "";
            }

            // checking he is dead or not
            if (thePlayer.dead == true && thePlayer.protectd == false && thePlayer.rescued == false)

            {
                string killer = "";
                string thePlayerName = thePlayer.pname;
                string deadInfo;
                if (playersList.Contains(thePlayer))
                {
                    List<string> vsNames = thePlayer.visitors;
                    killPlayers.Add(thePlayer);
                    for (int k = 0; k < vsNames.Count; k++)
                    {
                        Players visitor = playersList.FirstOrDefault(obj => obj.pname == vsNames[k]);
                        if (visitor.action == "kill")
                        {
                            killer = visitor.role;
                        }
                    }
                    deadInfo = thePlayerName + " is killed by " + killer + "!!";
                    infosList.Add(deadInfo);
                    GameObject deadPlayer = panels.FirstOrDefault(obj => obj.name == thePlayer.pname);
                    GameObject deadPlayerTransition = panels.FirstOrDefault(obj => obj.name == thePlayer.pname + " transition");
                    deadPlayers.Add(thePlayer);
                    deadPanels.Add(deadPlayer);
                    deadPanels.Add(deadPlayerTransition);
                }
                

                for (int m = 0; m < panels.Count; m++)
                {
                    if (panels[m].name == thePlayerName)
                    {
                        panels.RemoveAt(m - 1);
                        panels.RemoveAt(m - 1);
                    }
                }
                for (int k = 0; k < playerPanels.Count; k++)
                {
                    if (playerPanels[k].name == thePlayerName)
                    {
                        playerPanels.RemoveAt(k - 1);
                        playerPanels.RemoveAt(k - 1);
                    }
                }

                //inactivating the player node from the voting 
                for (int k = 1; k < playerPanels.Count; k = k + 2)
                {
                    GameObject otherPlayer = playerPanels[k];
                    string parentPlayerName = otherPlayer.name;
                    Players parentPlayer = playersList.FirstOrDefault(obj => obj.pname == parentPlayerName);
                    string parentPlayerRole = parentPlayer.role;
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
                    List<string> VisitorsNames = thePlayer.visitors;
                    for (int j = 0; j < VisitorsNames.Count; j++)
                    {
                        Players visitor = playersList.FirstOrDefault(obj => obj.pname == VisitorsNames[j]);

                        if (visitor.action == "protect" || visitor.action == "kill")
                        {
                            
                            deadPlayers.Add(visitor);
                            killPlayers.Add(visitor);
                            GameObject deadPlayer = panels.FirstOrDefault(obj => obj.name == visitor.pname);
                            GameObject deadPlayerTransition = panels.FirstOrDefault(obj => obj.name == visitor.pname + " transition");
                            deadPanels.Add(deadPlayer);
                            deadPanels.Add(deadPlayerTransition);
                            int deadPlayerTRIndex = panels.IndexOf(deadPlayerTransition);
                            Debug.Log("deadPlayerTRIndex: " + deadPlayerTRIndex + " , panelCount: " + panels.Count);
                            panels.RemoveAt(deadPlayerTRIndex);
                            panels.RemoveAt(deadPlayerTRIndex);
                            for (int k = 0; k < playerPanels.Count; k++)
                            {
                                if (playerPanels[k].name == visitor.pname)
                                {
                                    playerPanels.RemoveAt(k - 1);
                                    playerPanels.RemoveAt(k - 1);
                                }
                            }
                            if (visitor.action == "protect" )
                            {
                                string deadInfo = visitor.pname + " is killed while protecting!!";
                                infosList.Add(deadInfo);
                            }
                            else 
                            {
                                string deadInfo = visitor.pname + " is killed by BÜRKÜT!!";
                                infosList.Add(deadInfo);
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
        
        for (int i = 0; i < pListCount; i++)
        {
            playersList[i].visitedPlayer = "";
        }
        //Debug.Log("DENİTORUZZ HOCAM");


        for (int p = 0; p < killPlayers.Count; p++) // clearing killedplayers from the playerslist
        {
            playersList.Remove(killPlayers[p]);
        }
        killPlayers.Clear();


        // ending game phase
        bool endGame = true;
        for (int j = 0; j < playersList.Count; j++)
        {

            if (playersList[j].role == "TEPEGOZ" || playersList[j].role == "SU IYESI" || playersList[j].role == "GULYABANI")
            {
                endGame = false;
            }
        }
        string endMessage;
        if (endGame) 
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
        
        
        // INFO PANEL PART
        // Get the 'Names' GameObject
        GameObject namesObject = infoPanel.transform.Find("Names").gameObject;
        Transform namesTransform = namesObject.transform;

        // Loop through each child and destroy them
        foreach (Transform child in namesTransform)
        {
            GameObject.Destroy(child.gameObject);
        }
        RectTransform rectParentTransform = namesObject.GetComponent<RectTransform>();
        float topYPosition = rectParentTransform.rect.height * 0.5f; 

        float verticalSpacing = 100.0f;
        // Access the Text components of children in the 'Names' GameObject
        for (int i = 0; i < infosList.Count; i++)
        {
            GameObject newTextObject = new GameObject("InfoText" + i);
            GameObject starObject = new GameObject("StarText" + i);
            
            newTextObject.transform.SetParent(namesObject.transform); 
            starObject.transform.SetParent(namesObject.transform); 
            
            TextMeshProUGUI newTextComponent = newTextObject.AddComponent<TextMeshProUGUI>();
            TextMeshProUGUI starText = starObject.AddComponent<TextMeshProUGUI>();
            
            newTextComponent.text = infosList[i];
            newTextComponent.fontSize = 96;
            newTextComponent.alignment = TextAlignmentOptions.Center;

            starText.text = "***************";
            starText.fontSize = 96;
            starText.alignment = TextAlignmentOptions.Center;

            RectTransform newTextRectTransform = newTextObject.GetComponent<RectTransform>();
            RectTransform starRectTransform = starObject.GetComponent<RectTransform>();
            
            if (newTextRectTransform != null && starRectTransform != null)
            {
                newTextRectTransform.sizeDelta = new Vector2(namesObject.GetComponent<RectTransform>().rect.width, newTextRectTransform.rect.height);
                starRectTransform.sizeDelta = new Vector2(namesObject.GetComponent<RectTransform>().rect.width, starRectTransform.rect.height);

                newTextRectTransform.pivot = new Vector2(0.5f, 1f); // Set pivot to center-top
                starRectTransform.pivot = new Vector2(0.5f, 0f); // Set pivot to center-bottom
                
                newTextObject.transform.localPosition = new Vector3(0f, topYPosition - 2*i * verticalSpacing, 0f);
                starObject.transform.localPosition = new Vector3(0f, topYPosition - ((2*i+1) * verticalSpacing) - 60f, 0f); // Adjust the offset as needed
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
                panels.RemoveAt(m-1);
                panels.RemoveAt(m-1);
                break;
            }
        }

        for (int m = 0; m < playerPanels.Count; m++)
        {
            if (playerPanels[m].name == playerWillKilled)
            {
                playerPanels.RemoveAt(m-1);
                playerPanels.RemoveAt(m-1);
                break;
            }
        }
        

        // inactivating the player node who is killed by Vote
        for (int k = 1; k < playerPanels.Count; k = k + 2)
        {

            //GameObject otherPlayer = panels.FirstOrDefault(obj => obj.name == playersList[k].pname);
            GameObject otherPlayerPanel = playerPanels[k];
            // finding role of the parent player for UMAY
            string parentPlayerName = otherPlayerPanel.name;
            Players parentPlayer = playersList.FirstOrDefault(obj => obj.pname == parentPlayerName);
            Transform otherPlayerTransform = otherPlayerPanel.transform;

            GameObject profilesObject = otherPlayerTransform.Find("Names").gameObject;
            if (profilesObject != null)
            {
                if (parentPlayer != null)
                {   
                    string parentPlayerRole = parentPlayer.role;
                    FindObjectWithSpecificTextInChildren(profilesObject.transform, playerWillKilled, parentPlayerRole);
                }
                
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


    }

}