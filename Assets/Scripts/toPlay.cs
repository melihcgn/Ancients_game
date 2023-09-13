using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class toPlay : MonoBehaviour
{
    // Start is called before the first frame update
    public string sceneName; // The name of the scene you want to load.
    
    public void LoadScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}
