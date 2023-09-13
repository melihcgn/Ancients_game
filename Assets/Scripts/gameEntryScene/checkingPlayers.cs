using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkingPlayers : MonoBehaviour
{
    public void AccessTextListFromAnotherScript()
    {
        // Find the GameObject with the takingPNames script (you can also use other methods to find it)
        GameObject takingPNamesObject = GameObject.Find("scriptMoves2"); // Replace "NameListObject" with the actual GameObject name

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
}
