using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : Singleton<SceneController>
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadGame(){
        SceneManager.LoadScene("Game");
    }
    public void LoadGameOver(){
        SceneManager.LoadScene("GameOver");
    }
    public void LoadMenu(){
        SceneManager.LoadScene("Menu");
    }
    public void LoadTutorial(){
        SceneManager.LoadScene("Tutorial");
    }

    public void QuitGame(){
        Application.Quit();
    }

    
}
