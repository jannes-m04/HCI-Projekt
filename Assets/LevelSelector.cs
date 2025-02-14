using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI; //automatically fill out level names
using UnityEngine.SceneManagement; //Used to navigate between scenes

public class LevelSelector : MonoBehaviour
{

    public int level;
    public TMP_Text levelText;
    
    // Start is called before the first frame update
    void Start()
    {
        levelText.text = level.ToString(); //texts will be replaced by the level number
    }
    
    // Replaced update with costume method
    public void OpenScene() {
        SceneManager.LoadScene("Level "+ level.ToString()); //checks the int and gets to correct level
    }
}
