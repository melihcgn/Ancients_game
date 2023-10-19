using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using playerNS;
using TMPro;
using UnityEngine.UI;
using System.Linq;


public class createPanels : MonoBehaviour
{
    private GameManager gameManager;
    public GameObject playerPanelPrefab;
    public GameObject transferPanelPrefab;
    public Vector3 spawnPosition;
    public Transform canvas;
    public GameObject scriptAl;
    public GameObject openingpanel;
    private TextMeshProUGUI pNameText1, pNameText2 , tNameText1, pRoleText1, pRoleText2;
    public List<GameObject> panels = new List<GameObject>();

    public GameObject circleButtonPrefab;

    // for clickable ptofile buttons
    public Color selectedColor = Color.yellow;  // Set selectedColor to yellow
    public Color defaultColor = Color.gray;

    private List<Button> buttons = new List<Button>();
    public List<Players> ps = new List<Players>();

    public Button selectedButton; 

    
    public void Start()
    {
        GameObject gameManagerObject = GameObject.Find("scriptMoves2");

        if (gameManagerObject != null)
        {
            gameManager = gameManagerObject.GetComponent<GameManager>();

            if (gameManager != null)
            {
                List<Players> players = gameManager.playerSit;
                ps = players;

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
                GameObject transferPlane = Instantiate(transferPanelPrefab, spawnPosition, Quaternion.identity, canvas);

                GameObject playerPlane = Instantiate(playerPanelPrefab, spawnPosition, Quaternion.identity, canvas);
                Transform names = playerPlane.transform.Find("Names");

                // Calculate the width of a single button
                float buttonWidth = circleButtonPrefab.GetComponent<RectTransform>().rect.width;
                // Set the maximum buttons per row
                int maxButtonsPerRow = 3;

                // Calculate the spacing between buttons to fit them in a row
                float spacing = buttonWidth * maxButtonsPerRow / 3 * 2;
                int pCount = playersList.Count;

                int dummy = 0;

                for (int i = 0; i < pCount; i++)
                {
                    if (player.pname != playersList[i].pname)
                    {
                        // Calculate row and column indices for the button
                        int row = (i + dummy) / maxButtonsPerRow;
                        int col = (i + dummy) % maxButtonsPerRow;

                        // Calculate the position with appropriate spacing
                        Vector3 buttonPosition = new Vector3(
                            names.position.x + (col) * buttonWidth * 2 - spacing,
                            names.position.y - (row - 1) * buttonWidth * 2,
                            names.position.z);

                        // Instantiate the button at the calculated position
                        GameObject circleButtonInstance = Instantiate(circleButtonPrefab, buttonPosition, Quaternion.identity, names);
                        TextMeshProUGUI tmpText = circleButtonInstance.GetComponentInChildren<TextMeshProUGUI>();
                        tmpText.text = playersList[i].pname;
                        Button circleButton = circleButtonInstance.GetComponent<Button>();
                        circleButton.onClick.AddListener(() => OnButtonClick(ref circleButton, playersList));
                        //making clickable
                    }
                    else
                    {
                        dummy--;
                    }

                }

                string playerName = player.pname;
                playerPlane.name = playerName;
                string roleName = player.role;
                playerPlane.SetActive(true);
                transferPlane.SetActive(true);
                panels.Add(transferPlane);
                panels.Add(playerPlane);
                transferPlane.SetActive(false);
                playerPlane.SetActive(false);
                Debug.Log("gorko2 " + panels.Count);

                
                if (playerPlane != null)
                {
                    setPlanes setPlaneScript = playerPanelPrefab.GetComponent<setPlanes>();
                    pNameText1 = playerPlane.transform.Find("nameText").GetComponent<TextMeshProUGUI>();
                    pRoleText2 = playerPlane.transform.Find("roleText").GetComponent<TextMeshProUGUI>();
                    Button passButton = playerPlane.transform.Find("passButton").GetComponent<Button>();
                    Button pickButton = playerPlane.transform.Find("pickButton").GetComponent<Button>();
                    panelManager pManager = GetComponent<panelManager>();
                    passButton.onClick.AddListener(pManager.passingPages);
                    pickButton.onClick.AddListener(() => changeStatus(player.role));
                    pNameText1.text = "Name: " + player.pname;
                    pRoleText2.text = "Role: " + player.role;
                    if (player.pname == playersList[playersList.Count - 1].pname)
                    {
                        ActualPlay Aplay = GetComponent<ActualPlay>();
                        pickButton.onClick.AddListener(Aplay.afterPlayerAction);
                        passButton.onClick.AddListener(Aplay.afterPlayerAction);
                    }
                    else
                    {
                        Debug.Log(player.pname+" daha olmadı " +playersList[playersList.Count - 1].pname);
                    }
                    tNameText1 = transferPlane.transform.Find("nameText").GetComponent<TextMeshProUGUI>();
                    tNameText1.text = player.pname;
                    Button seeButton = transferPlane.transform.Find("seeButton").GetComponent<Button>();
                    seeButton.onClick.AddListener(pManager.passingPages);
                    TextMeshProUGUI buttonText = pickButton.GetComponentInChildren<TextMeshProUGUI>();
                    if (roleName == "AI TOYON"  || roleName == "KIZAGAN" || roleName == "OD ANA" )
                    {
                        Debug.Log("valla girdi: " + roleName );
                        buttonText.text = "Announce";
                    }
                    else
                    {
                     //Debug.Log("valla girMEdi: " + playerName );

                    }

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


    public void changeStatus(string ExeRole)
    {
        if(selectedButton != null){
            TextMeshProUGUI buttonText = selectedButton.GetComponentInChildren<TextMeshProUGUI>();
            Debug.Log("sdfsdf=  " + ps[0].pname + " DSFDS " + ExeRole);
            Players playerCell = new Players();
             playerCell = ps.FirstOrDefault(players => players.pname == buttonText.text);

            Players choosingCell = ps.FirstOrDefault(players => players.role == ExeRole);
            Debug.Log("resmin var şuan elimde: " + choosingCell.pname);
            if (choosingCell.role == "MERGEN")
            {
                playerCell.changeDead(true) ;
            }
            else if (choosingCell.role == "Ulgen")
            {
                playerCell.changeDead(true) ;
            }
            else if (choosingCell.role == "KIZAGAN")
            {
                playerCell.changeDead(true) ;
            }
            else if (choosingCell.role == "UMAY")
            {
                playerCell.changeDead(true) ;
            }
            else if (choosingCell.role == "BURKUT")
            {
                playerCell.changeDead(true) ;
            }
            else if (choosingCell.role == "OD ANA")
            {
                playerCell.changeDead(true) ;
            }
            else if (choosingCell.role == "AKBUGA")
            {
                playerCell.changeDead(true) ;
            }
            else if (choosingCell.role == "AI TOYON")
            {
                playerCell.changeDead(true) ;
            }
            else if (choosingCell.role == "AYZIT")
            {
                playerCell.changeDead(true) ;
            }
            else if (choosingCell.role == "ALAZ HAN")
            {
                playerCell.changeDead(true) ;
            }
            else if (choosingCell.role == "SIGUN GEYIK")
            {
                playerCell.changeDead(true) ;
            }
            else if (choosingCell.role == "YILDIZ HAN")
            {
                playerCell.changeDead(true) ;
            }
            else if (choosingCell.role == "TEPEGOZ")
            {
                playerCell.changeDead(true) ;
            }
            else if (choosingCell.role == "DEMIRKIYNAK")
            {
                playerCell.changeDead(true) ;
            }
            else if (choosingCell.role == "SU IYESI")
            {
                playerCell.changeDead(true) ;
            }
            else if (choosingCell.role == "KORMOS")
            {
                playerCell.changeDead(true) ;
            }
            else if (choosingCell.role == "ARCURA")
            {
                playerCell.changeDead(true) ;
            }
            else if (choosingCell.role == "AZMIC")
            {
                playerCell.changeDead(true) ;
            }
            else if (choosingCell.role == "GULYABANI")
            {
                playerCell.changeDead(true) ;
            }
            else if (choosingCell.role == "KORTIGES")
            {
                playerCell.changeDead(true) ;
            }
            else
            {
                Debug.Log("OLMADIA Q " );
            }
            Debug.Log("resmin mi var şuan elimde: " + playerCell.pname + " - " + playerCell.dead );
        }
        else
        {
             Debug.Log("SIKINTI VAAR");
        }
        
    }   

    private void OnButtonClick( ref Button clickedButton, List<Players> ps)
    {
        selectedButton = clickedButton;
        TextMeshProUGUI buttonText = selectedButton.GetComponentInChildren<TextMeshProUGUI>();
        Debug.Log("DUR BAKIMMM : " + buttonText.text);
    }
}
