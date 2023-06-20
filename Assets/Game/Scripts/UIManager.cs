using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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


    public TMP_Text countdownText;
    
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
    public void DisplayHealth(float value, bool isPlayer)
    {
        if (health == 0)
            health = value;

        value /= health;
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
        value /= 100f; // Chia giá tr? n?ng l??ng cho 100 ?? n?m trong kho?ng t? 0 -> 1
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

    private IEnumerator CountdownCoroutine()
    {
        int countdown = 3;

        while (countdown > 0)
        {
            countdownText.text = countdown.ToString();
            yield return new WaitForSeconds(1f);
            countdown--;
        }

        countdownText.text = "Fight!";
        yield return new WaitForSeconds(1f);

        // G?i ph??ng th?c b?t ??u tr?n ??u t?i ?ây

        countdownText.gameObject.SetActive(false); // T?t hi?n th? màn hình ??m ng??c
    }
}

