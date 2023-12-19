using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using playerNS;

namespace playerNS{
    [System.Serializable]
    public class Players
{
    public string pname;
    public string role;
    public bool charmed;
    public bool protectd; // Note: 'protected' is a valid keyword, but 'protectd' might be a typo
    public bool rescued;
    public bool shot;
    public bool dead;
    public bool silenced;
    public bool revealed;
    public bool markedSu;
    public bool kortigesed;
    public bool alazPower;
    public string visitedPlayer;
    public string action; 
    
    public List<string> visitors;

    // Default constructor
    public Players()
    {
        pname = "";
        role = "";
        charmed = false;
        protectd = false;
        rescued = false;
        dead = false;
        shot = false;
        silenced = false;
        revealed = false;
        markedSu = false;
        kortigesed = false;
        alazPower= false;
        visitedPlayer = "";
        action = "";
        visitors = new List<string>();
    }

    // Parameterized constructor
    public Players(string pn, string r, bool ch, bool pr, bool re, bool de, bool sh, bool si, bool rev, bool ms, bool kt, string vs, string act)
    {
        pname = pn;
        role = r;
        charmed = ch;
        protectd = pr;
        rescued = re;
        dead = de;
        shot = sh;
        silenced = si;
        revealed = rev;
        markedSu = ms;
        kortigesed = kt;
        visitedPlayer = vs;
        action = act;
        visitors = new List<string>();
    }

    public void printPlayer()
    {
        Debug.Log("name: " + this.pname);
        Debug.Log("role: " + this.role);
        Debug.Log("charmed: " + this.charmed);
        Debug.Log("protectd: " + this.protectd);
        Debug.Log("rescued: " + this.rescued);
        Debug.Log("dead: " + this.dead);
    }
    public void changeDead(bool d)
    {
        this.dead = d;
    }
}

}



public class GameManager : MonoBehaviour
{
    private List<string> goodRoles;
    private List<string> badRoles;
    private List<string> neutralRoles;
    private List<string> availableRoles;
    private string tempRole;
    public List<Players> playerSit;

    // Start is called before the first frame update
    void Start()
    {
        goodRoles = new List<string>();
        goodRoles.Add("Ulgen"); //+
        goodRoles.Add("MERGEN"); //+
        goodRoles.Add("KIZAGAN"); //+
        goodRoles.Add("UMAY"); 
        goodRoles.Add("BURKUT"); //+
        goodRoles.Add("OD ANA");
        goodRoles.Add("AKBUGA"); //+
        goodRoles.Add("AI TOYON"); //+
        goodRoles.Add("AYZIT"); //+
        goodRoles.Add("ALAZ HAN"); //+
        goodRoles.Add("SIGUN GEYIK");
        goodRoles.Add("YILDIZ HAN");

        badRoles = new List<string>();
        badRoles.Add("TEPEGOZ");
        badRoles.Add("DEMIRKIYNAK");





        neutralRoles = new List<string>();
        neutralRoles.Add("SU IYESI"); 
        neutralRoles.Add("KORMOS");
        neutralRoles.Add("ARCURA");
        neutralRoles.Add("AZMIC");
        neutralRoles.Add("GULYABANI");
        neutralRoles.Add("KORTIGES");
        


        
    }
    public void AccessTextListFromAnotherScript()
    {
        // Find the GameObject with the takingPNames script (you can also use other methods to find it)
        GameObject takingPNamesObject = GameObject.Find("scriptMoves2"); // Replace "NameListObject" with the actual GameObject name
        createPanels createPlanezz = takingPNamesObject.GetComponent<createPanels>();

        // Check if the script component exists on the GameObject
        if (takingPNamesObject != null)
        {
            takingPNames takingPNamesScript = takingPNamesObject.GetComponent<takingPNames>();

            if (takingPNamesScript != null)
            {
                // Now you can access the textList from the takingPNames script
                List<string> textList = takingPNamesScript.textList;
                int textListLength = textList.Count; 

                
                if (textListLength >= 5 && textListLength <= 9)
                {
                    
                    Debug.Log("okeyy, you can start.");
                    // Create a Random object
                    System.Random random = new System.Random();
                    int roundedGood = Mathf.CeilToInt((textListLength+2)/2.0f);
                    Debug.Log("roundedGood " + roundedGood);
                    int numberGood = goodRoles.Count;
                    availableRoles = new List<string>();
                    PrintList(goodRoles);

                    tempRole = goodRoles[3];
                    RemoveElementAtIndex(3, goodRoles);
                    availableRoles.Add(tempRole);
                    numberGood = goodRoles.Count;

                    for (int i = 0; i < roundedGood-1; i++)
                    {
                        int randomNumber = random.Next(0, numberGood);  // Generates a number from 0 to 8 (inclusive)
                        Debug.Log(randomNumber);
                        tempRole = goodRoles[randomNumber];
                        RemoveElementAtIndex(randomNumber, goodRoles);
                        availableRoles.Add(tempRole);
                        numberGood = goodRoles.Count;
                    }
                    int roundedBad = Mathf.FloorToInt((textListLength/4.0f));
                    int numberBad = badRoles.Count;
                    Debug.Log("roundedBad " + roundedBad);
                    tempRole = badRoles[0];
                    availableRoles.Add(tempRole);
                    for (int i = 0; i < roundedBad-1; i++)
                    {
                        int randomNumber = random.Next(0, numberBad);  // Generates a number from 0 to 8 (inclusive)
                        tempRole = badRoles[randomNumber];
                        RemoveElementAtIndex(randomNumber, badRoles);
                        availableRoles.Add(tempRole);
                        numberBad = badRoles.Count;
                    }
                    int numberNeutral = neutralRoles.Count;
                    if (textListLength > 5)
                    {
                        int randomNumber = random.Next(0, numberNeutral);  // Generates a number from 0 to 8 (inclusive)
                        tempRole = neutralRoles[randomNumber];
                        RemoveElementAtIndex(randomNumber, neutralRoles);
                        availableRoles.Add(tempRole);
                    }
                    PrintList(availableRoles);
                    ShuffleRoles();
                    playerSit = new List<Players>();
                    for (int i = 0; i < textListLength; i++)
                    {
                        string role = GetNextRole();
                        Players player = new Players(textList[i], role, false, false, false, false, false, false, false, false, false, "", "");
                        playerSit.Add(player);
                        //player.printPlayer();
                    }

                    createPlanezz.SetupPlayerPanels(playerSit);
                    createPlanezz.changeToStart();
                    
                    
        // Generate a random number between 0 and 8
                    
        // Print the random number
                }
                else
                {
                    Debug.Log("NEIN AMK NEIN!!");
                }

                

                // Use textList as needed
                Debug.Log("Accessed textList from another script. Count: " + textList.Count);
            }
            else
            {
                Debug.LogError("The takingPNames script component was not found on the GameObject.");
            }
        }
        else
        {
            Debug.LogError("The GameObject with the takingPNames script was not found.");
        }

        
    }
    void RemoveElementAtIndex(int index, List<string> x)
    {
        if (index >= 0 && index < x.Count)
        {
            x.RemoveAt(index);
        }
        else
        {
            Debug.LogWarning("Invalid index. Element not removed.");
        }
    }
    private void ShuffleRoles()
    {
        System.Random rand = new System.Random();
        int n = availableRoles.Count;
        for (int i = n - 1; i > 0; i--)
        {
            int j = rand.Next(0, i + 1);
            string temp = availableRoles[i];
            availableRoles[i] = availableRoles[j];
            availableRoles[j] = temp;
        }
    }

    // Get the next role from the shuffled list
    private string GetNextRole()
    {
        string role = availableRoles[0];
        availableRoles.RemoveAt(0);
        return role;
    }

    void PrintList(List<string> r)
    {
        Debug.Log("Printing List:");
        foreach (string item in r)
        {
            Debug.Log(item);
        }
    }

    

    // INSTANTIATE PART

}