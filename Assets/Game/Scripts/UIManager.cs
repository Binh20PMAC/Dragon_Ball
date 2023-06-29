using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    public Image player_health_UI;
    public Image player_energy_UI;
    public Image enemy_health_UI;
    public Image enemy_energy_UI;
    public Gradient gradient;
    private float player_energy = 15f;
    private float enemy_energy = 15f;
    private float health = 0;
    private float health_enemy = 0;
    public int playerWins = 0;
    public int enemyWins = 0;
    public GameObject P1W1;
    public GameObject P1W2;
    public GameObject P2W1;
    public GameObject P2W2;
    public TMP_Text countdownText;
    public bool isCountdownFinished = false;
    public TMP_Text winMessageText;
    public GameObject winPanel;
    private void Start()
    {
        StartCoroutine(CountdownCoroutine());
    }
    void Awake()
    {
        player_health_UI = GameObject.FindWithTag("HealthUI").GetComponent<Image>();
        enemy_health_UI = GameObject.FindWithTag("EnemyHealthUI").GetComponent<Image>();
        player_energy_UI = GameObject.FindWithTag("EnergyUI").GetComponent<Image>();
        enemy_energy_UI = GameObject.FindWithTag("EnemyEnergyUI").GetComponent<Image>();
        DisplayEnergy(player_energy, true);
        DisplayEnergy(enemy_energy, false);
    }
    public void DisplayBO3()
    {
        if (playerWins == 1)
        {
            P1W1.SetActive(true);
        }
        else if (playerWins == 2)
        {
            P1W2.SetActive(true);
        }
        if (enemyWins == 1)
        {
            P2W1.SetActive(true);
        }
        else if (enemyWins == 2)
        {
            P2W2.SetActive(true);
        }
    }
    public void DisplayHealth(float value, bool isPlayer)
    {
        if (isPlayer)
        {
            if (health == 0)
                health = value;

            value /= health;
        }
        else
        {
            if (health_enemy == 0)
                health_enemy = value;

            value /= health_enemy;
        }
        if (value < 0f)
            value = 0f;
        if (isPlayer && player_health_UI != null)
        {
            player_health_UI.fillAmount = value;
        }
        else if (!isPlayer && enemy_health_UI != null)
        {
            enemy_health_UI.fillAmount = value;
        }
    }
    public void DisplayEnergy(float value, bool isPlayer)
    {
        value /= 100f;
        if (value < 0f)
            value = 0f;

        if (isPlayer && player_energy_UI != null)
        {
            player_energy_UI.fillAmount = value;
            player_energy_UI.color = gradient.Evaluate(value);
        }
        else if (!isPlayer && enemy_energy_UI != null)
        {
            enemy_energy_UI.fillAmount = value;
            enemy_energy_UI.color = gradient.Evaluate(value);
        }
    }
    public IEnumerator CountdownCoroutine()
    {
        int countdown = 3;
        isCountdownFinished = false;
        while (countdown > 0)
        {
            countdownText.text = countdown.ToString();
            yield return new WaitForSeconds(1f);
            countdown--;
        }

        countdownText.text = "Fight!";
        yield return new WaitForSeconds(1f);

        countdownText.gameObject.SetActive(false);
        isCountdownFinished = true;
    }
    public void RestartCountdown()
    {
        countdownText.gameObject.SetActive(true);
        StartCoroutine(CountdownCoroutine());
    }
    public void DisplayWinMessage(string message)
    {
        winMessageText.text = message;
        winPanel.SetActive(true);
    }
    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
        playerWins = 0;
        enemyWins = 0;
    }
}

