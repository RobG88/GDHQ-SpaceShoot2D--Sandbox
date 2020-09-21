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
    [SerializeField] Text _bonusLife_text;

    [SerializeField] Text _gameOverText;
    [SerializeField] Text _restartGameText;

    [SerializeField] GameObject _optionsMenu;
    [SerializeField] GameObject _pausePanel;

    [SerializeField] GameObject _SFXIcon;
    [SerializeField] GameObject _redSFXIcon;
    [SerializeField] GameObject _BGMIcon;
    [SerializeField] GameObject _redBGMIcon;

    private bool ShieldBonusActivated;

    private bool _isGameOver = false;

    WaitForSeconds BonusLifePause = new WaitForSeconds(.25f);

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
        if (shieldBonus > 0 && shieldBonus < 4)
        {
            _shieldBonus[shieldBonus - 1].SetActive(true);
            if (shieldBonus == 3)
            {
                ShieldBonusActivated = true;
                _bonusLife_text.gameObject.SetActive(true);
                StartCoroutine(BonusLifeMessage());
            }
        }
        else if (shieldBonus == 0)
        {
            ShieldBonusActivated = false;
            _bonusLife_text.gameObject.SetActive(false);
            foreach (var shield in _shieldBonus)
            {
                shield.SetActive(false);
            }
        }
        else if (shieldBonus > 3)
        {
            // TODO:
            // Display MAX SHIELD PROTECTION
            // Blink
            // Paladin "Max Shield Protection reached"
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
        SceneManager.LoadScene("MainMenu"); // Scene 0 = 'MainMenu'
    }

    public void OptionsMenu()
    {
        _pausePanel.SetActive(false);
        _optionsMenu.SetActive(true);
    }

    public void OptionsBackButton()
    {
        _optionsMenu.SetActive(false);
        _pausePanel.SetActive(true);
    }

    IEnumerator BonusLifeMessage()
    {
        Color colorBlue;
        Color colorSilver;

        ColorUtility.TryParseHtmlString("#0d00ff", out colorBlue);
        ColorUtility.TryParseHtmlString("#a6a6a6", out colorSilver);

        while (ShieldBonusActivated)
        {
            _bonusLife_text.color = colorBlue;
            yield return BonusLifePause;
            _bonusLife_text.color = colorSilver;
            yield return BonusLifePause;
        }
    }
    public void EnableDisableSFXIcon(bool isDisable)
    {
        _redSFXIcon.GetComponent<Image>().enabled = isDisable;
        _SFXIcon.GetComponent<Image>().enabled = !isDisable;
    }

    public void EnableDisableBGMIcon(bool isDisable)
    {
        _redBGMIcon.GetComponent<Image>().enabled = isDisable;
        _BGMIcon.GetComponent<Image>().enabled = !isDisable;
    }
}
