using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using playerNS;
using TMPro;
using UnityEngine.UI;
public class createPanels : MonoBehaviour
{
    private GameManager gameManager;
    public GameObject playerPanelPrefab;
    public Vector3 spawnPosition;
    public Transform canvas;
    public GameObject scriptAl;
    public GameObject openingpanel;
    private TextMeshProUGUI text1;
    private TextMeshProUGUI text2;
    public List<GameObject> panels = new List<GameObject>();
    public void Start()
    {
        GameObject gameManagerObject = GameObject.Find("scriptMoves2");

        if (gameManagerObject != null)
        {
            gameManager = gameManagerObject.GetComponent<GameManager>();

            if (gameManager != null)
            {
                List<Players> players = gameManager.playerSit;

                foreach (Players player in players)
                {
                    Debug.Log("Player name: " + player.pname);
                }

                SetupPlayerPanels(players);
            }
            else
            {
                Debug.LogError("GameManager component not found.");
            }
        }
        else
        {
            Debug.LogError("GameManager object not found.");
        }
    }

    public void SetupPlayerPanels(List<Players> playersList)
    {
        if (playerPanelPrefab == null)
        {
            Debug.LogError("Player panel prefab is null.");
            return;
        }

        foreach (var player in playersList)

        {
            if (playerPanelPrefab != null)
            {
                GameObject playerPlane = Instantiate(playerPanelPrefab, spawnPosition, Quaternion.identity, canvas);
                playerPlane.name= player.pname;
                
                playerPlane.SetActive(true);
                panels.Add(playerPlane);
                playerPlane.SetActive(false);
                Debug.Log("gorko2 " + panels.Count);
                if (playerPlane != null)
                {
                    setPlanes setPlaneScript = playerPanelPrefab.GetComponent<setPlanes>();
                    text1 = playerPlane.transform.Find("nameText").GetComponent<TextMeshProUGUI>();
                    text2 = playerPlane.transform.Find("roleText").GetComponent<TextMeshProUGUI>();
                    Button passButton =  playerPlane.transform.Find("passButton").GetComponent<Button>();
                    panelManager  pManager = GetComponent<panelManager >();
                    passButton.onClick.AddListener(pManager.passingPages);
                    text1.text = "Name: " +  player.pname;
                    text2.text = "Role: " +  player.role;
                    if (setPlaneScript != null)
                    {
                        //setPlaneScript.SetPlayer(player);
                        TextMeshProUGUI[] textComponents = playerPlane.GetComponentsInChildren<TextMeshProUGUI>();

                    foreach (TextMeshProUGUI textComponent in textComponents)
                    {
                        if (textComponent.gameObject.name == "nameText")
                        {
                            // Accessing the "nameText" TMP Text's text property for debugging
                            Debug.Log("TMP Text (nameText) value: " + textComponent.text);
                        }
                        else if (textComponent.gameObject.name == "roleName")
                        {
                            // Accessing the "roleName" TMP Text's text property for debugging
                            Debug.Log("TMP Text (roleName) value: " + textComponent.text);
                        }
                    }

                    }
                    else
                    {
                        Debug.LogError("setPlanes component not found on player panel.");
                    }
                }
                else
                {
                    Debug.LogError("Player panel instantiation failed.");
                }
            }
            else
            {
                Debug.LogError("Player panel prefab is null.");
            }
        }

        
    }
    public void changeToStart()
    {
        GameObject closingpanel = GameObject.Find("introPanel");
        closingpanel.SetActive(false);
        openingpanel.SetActive(true);
    }
    
}
