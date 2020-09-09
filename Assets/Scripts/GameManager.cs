using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] bool _isGameOver = false;
    [SerializeField] bool _gamePaused = false;
    [SerializeField] GameObject _pausePanel;

    bool _isAsteroidDestroyed = false;
    bool _EnemyInvasion = false;

    SpawnManager _spawnManager;

    public override void Init()
    {
        base.Init();
        _gamePaused = false;
        Time.timeScale = 1;
        //Debug.Log("GameManager has been initialized");
    }

    void Start()
    {
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();

        if (_spawnManager == null)
        {
            Debug.LogError("GAMEMANAGER: Start *** ERROR: SpawnManager is Null!");
        }
    }

    private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.I)) && _isGameOver)
        {
            SceneManager.LoadScene(1); // Reload current game scene "Shooter2020"
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }

        if (Input.GetKeyDown(KeyCode.P) && !_isGameOver)
        {
            PauseGame();
        }

        if (_isAsteroidDestroyed && !_EnemyInvasion)
        {
            _EnemyInvasion = true;
            _spawnManager.Spawn();
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PauseGame()
    {
        if (!_gamePaused)
        {
            _gamePaused = true;
            _pausePanel.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            _gamePaused = false;
            _pausePanel.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void GameOver()
    {
        _isGameOver = true;
    }

    public void DestroyedAsteroid()
    {
        _isAsteroidDestroyed = true;
    }
}