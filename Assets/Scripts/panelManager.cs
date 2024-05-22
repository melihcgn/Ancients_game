using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using playerNS;
using UnityEngine.SceneManagement;
public class panelManager : MonoBehaviour
{
    public GameObject startGamePanel;
    private createPanels createpanels;
    public List<GameObject> panels;
    private int count;
    private int prevPanelCount;
    public void Start()
    {
        prevPanelCount= 0;
        count = 0;
        GameObject gameManagerObject = GameObject.Find("scriptMoves2");
        if (gameManagerObject != null)
        {
            createpanels = gameManagerObject.GetComponent<createPanels>();
            

        }
        else{
          
        }

    }


    public void beginningOf()
    {
        List<GameObject> panels = createpanels.panels;

        if (panels != null && count < panels.Count)
        {
            ClosePanel("startGame");
            panels[count].SetActive(true);
            count++;
        }
        else
        {
            Debug.LogWarning("Invalid panels list or count value. Check your initialization.");
        }
    }

    public void passingPages()
    {
        
        List<GameObject> panels = createpanels.panels;
        //Debug.Log("count: " + count);
        //Debug.Log("panelcount: " + panels.Count);
        //Debug.Log("prevcount: " + prevPanelCount);
        if (prevPanelCount != 0 && prevPanelCount > panels.Count)
        {
            count = count + panels.Count - prevPanelCount;
        }
        else if(prevPanelCount != 0 && prevPanelCount < panels.Count){
            count = count + panels.Count - prevPanelCount;
        }
        else
        {
            
        }
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
        prevPanelCount = panels.Count;
    }

    public void toHomePage()
    {
        SceneManager.LoadScene("Intro");
    }


    public void OpenPanel(string panelName)
    {
        GameObject panel = GameObject.Find(panelName);
        Debug.Log("panelname: " + panelName + "deneme");
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

    public void toStartGame()
    {
        startGamePanel.SetActive(true);;
    }
}