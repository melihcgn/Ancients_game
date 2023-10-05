using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using playerNS;
public class panelManager : MonoBehaviour
{
    private createPanels createpanels;
    public List<GameObject> panels;
    private int count;
    public void Start()
    {
        count = 0;
        GameObject gameManagerObject = GameObject.Find("scriptMoves2");
        if (gameManagerObject != null)
        {
            createpanels = gameManagerObject.GetComponent<createPanels>();
            

        }
        else{
            Debug.Log("gorko");
        }

    }


    public void beginningOf()
    {
        List<GameObject> panels = createpanels.panels;
        ClosePanel("startGame");

        Debug.Log("gorko234 " + panels.Count);
        panels[count].SetActive(true);
        count++;
    }

    public void passingPages()
    {
        List<GameObject> panels = createpanels.panels;
        if(count < panels.Count)
        {
            panels[count-1].SetActive(false);
            panels[count].SetActive(true);
            count++;
        }
        else
        {
            panels[count-1].SetActive(false);
            count = 0;
            panels[count].SetActive(true);
            count++;
        }
        
    }


    public void OpenPanel(string panelName)
    {
        GameObject panel = GameObject.Find(panelName);
        Debug.Log("panelname: " + panelName  + "deneme");
        if (panel != null)
        {
            panel.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Panel not found with name: " + panelName);
        }
    }

    public void ClosePanel(string panelName)
    {
        GameObject panel = GameObject.Find(panelName);
        if (panel != null)
        {
            panel.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Panel not found with name: " + panelName);
        }
    }
}