using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    void Start()
    {
        //GameManager.Instance._gamePaused = false;
    }

    void Update()
    {

    }

    public void LoadGame()
    {
        SceneManager.LoadScene(1); // Start Game
        //SceneManager.LoadScene("Shooter2020");
    }
}
