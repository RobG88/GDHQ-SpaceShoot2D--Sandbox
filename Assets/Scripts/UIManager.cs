using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField] Text _scoreText;
    [SerializeField] Image _livesRemainingImage;
    [SerializeField] Text _livesRemainingText;
    [SerializeField] Sprite[] _livesSprites;
    [SerializeField] GameObject[] _shieldBonus;
    [SerializeField] Text _gameOverText;
    [SerializeField] Text _restartGameText;
    [SerializeField] Slider _ammoSlider;
    [SerializeField] Gradient _ammoBarGradient;
    [SerializeField] Image _ammoBarFill;
    [SerializeField] Slider _thrustersSlider;

    bool _isGameOver = false;

    public override void Init()
    {
        base.Init();
        //Debug.Log("UIManager has been initialized");
    }

    void Start()
    {
        _scoreText.text = "---";
        _gameOverText.gameObject.SetActive(false);
        _restartGameText.gameObject.SetActive(false);
        _isGameOver = false;
    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = playerScore.ToString("#,#");
    }

    public void UpdatePlayerLives(int livesRemaining)
    {
        _livesRemainingImage.sprite = _livesSprites[livesRemaining];
        _livesRemainingText.text = "LIVES = " + livesRemaining;

        if (livesRemaining == 0)
        {
            DisplayGameOver();
        }
    }

    public void UpdateShieldBonusUI(int shieldBonus)
    {
        if (shieldBonus > 0)
        {
            _shieldBonus[shieldBonus-1].SetActive(true);
        }
        else if (shieldBonus == 0)
        {
            foreach (var shield in _shieldBonus)
            {
                shield.SetActive(false);
            }
        }
    }

    public void DisplayGameOver()
    {
        _gameOverText.gameObject.SetActive(true);
        _restartGameText.gameObject.SetActive(true);
        StartCoroutine(GameOverColorChange());
    }

    IEnumerator GameOverColorChange()
    {
        var gameoverText = _gameOverText.GetComponent<Text>();

        for (int i = 60; i >= 0; i--)
        {
            gameoverText.color = Color.red;
            if (i % 2 == 0)
            {
                gameoverText.color = Color.blue;
            }
            if (i % 3 == 0)
            {
                gameoverText.color = Color.white;
            }
            yield return new WaitForSeconds(.1f);
        }
        gameoverText.color = Color.red;
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); // Scene 0 = 'MainMen'
    }

    public void SetMaxAmmo(int ammo)
    {
        _ammoSlider.maxValue = ammo;
        _ammoSlider.value = ammo;

        _ammoBarFill.color = _ammoBarGradient.Evaluate(1f);
    }
    public void SetAmmo(int ammo)
    {
        _ammoSlider.value = ammo;
        _ammoBarFill.color = _ammoBarGradient.Evaluate(_ammoSlider.normalizedValue);
    }

    public void SetMaxThrusters(float thrusters)
    {
        _thrustersSlider.maxValue = thrusters;
        _thrustersSlider.value = thrusters;

        //_ammoBarFill.color = _ammoBarGradient.Evaluate(1f);
    }
    public void SetThrusters(float thrusters)
    {
        _thrustersSlider.value = thrusters;
        //_ammoBarFill.color = _ammoBarGradient.Evaluate(_ammoSlider.normalizedValue);
    }
}
