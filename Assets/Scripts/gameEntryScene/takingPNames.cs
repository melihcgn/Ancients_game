using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class takingPNames : MonoBehaviour
{
    public TMP_InputField inputField;
    public TextMeshProUGUI nameListText;
    public List<string> textList = new List<string>();

    public void OnEnterButtonClick()
    {
        string inputText = inputField.text;

        // Check if the entered name already exists in the textList
        if (!string.IsNullOrEmpty(inputText) && !textList.Contains(inputText) )
        {
            textList.Add(inputText);
            inputField.text = string.Empty;

            Debug.Log("Input Text: " + inputText);

            UpdateNameListText(); // Update the displayed list
        }
        else
        {
            // Handle the case where the name already exists in the list
            Debug.Log("Name already exists in the list: " + inputText);
        }
    }

    private void UpdateNameListText()
    {
        // Create a formatted string to display the list of names
        string names = string.Join("\n", textList);

        // Update the TextMeshProUGUI element with the list of names
        nameListText.text = names;
    }


}
