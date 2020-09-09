using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Change PowerUpCountDownBar position or style
/// currently the Bar sits on top of screen/player
/// </summary>
public class PowerUpCountDown : MonoBehaviour
{
    Image timerBar;
    [SerializeField]
    float maxTime = 5.0f;
    [SerializeField]
    float timeLeft;
    [SerializeField]
    GameObject timesUpText;
    bool resetCountDown = false;
    [SerializeField]
    Text text;
    [SerializeField]
    Color color;

    [SerializeField]
    int timeLeft100;
    [SerializeField]
    int timeLeftpercent;

    void Start()
    {
        text = timesUpText.GetComponent<Text>();
        timesUpText.SetActive(false);
        timerBar = GetComponent<Image>();
        timeLeft = maxTime;
    }

    void Update()
    {
        if (resetCountDown)
        {
            ResetCountDown();
        }
        else
        {
            DisplayCountDownBar();
        }
    }

    private void DisplayCountDownBar()
    {
        if (timeLeft >= 0 && !resetCountDown)
        {
            timesUpText.SetActive(true);
            timeLeft -= Time.deltaTime;
            timerBar.fillAmount = timeLeft / maxTime;
            timeLeftpercent = (int)(timeLeft) * 100;
            timeLeft100 = timeLeftpercent * 100;
            if ((timeLeft / maxTime) < 0.30f)
            {
                if ((timeLeft * 100.0f) % 3 == 0)
                {
                    text.color = Color.red;
                }
                else
                {
                    text.color = Color.white;
                }
            }
        }

        else
        {
            //timesUpText.SetActive(false);
            // Time.timeScale = 0;
            resetCountDown = true;
        }
    }

    private void ResetCountDown()
    {
        if (resetCountDown)
        {
            timeLeft = maxTime;
            timerBar.fillAmount = 1.0f;
            timesUpText.SetActive(false);
            resetCountDown = false;
            gameObject.SetActive(false);
        }
    }

    void StartPowerUpCountDownBar()
    {

    }
}