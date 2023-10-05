using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using playerNS;
using TMPro;
public class setPlanes : MonoBehaviour
{
    public TextMeshProUGUI playerNameText;
    public TextMeshProUGUI playerRoleText;
    public Renderer planeRenderer;

    private Players player;

    public void SetPlayer(Players player)
    {
        this.player = player;
        UpdatePlayerInfo();
    }

    private void UpdatePlayerInfo()
    {
        if (player != null)
        {
            Debug.Log("pname: " + player.pname + " role: " +player.role);
            playerNameText.text = "Name: " + player.pname;
            playerRoleText.text = "Role: " + player.role;

        }
    }
}
