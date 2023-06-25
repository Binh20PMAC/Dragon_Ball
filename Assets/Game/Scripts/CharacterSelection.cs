using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
    public GameObject[] characters1;
    public GameObject[] characters2;
    public GameObject[] map;
    public GameObject characterSelection;
    public GameObject mapSelection;
    public int selectedCharacter1 = 0;
    public int selectedCharacter2 = 0;
    public int selectedMap = 0;
    public Text characterNameTextP1;
    public Text characterNameTextP2;
    public Text mapName;
    private void Start()
    {
        characterNameTextP1.text = characters1[selectedCharacter1].name;
        characterNameTextP2.text = characters1[selectedCharacter2].name;
        mapName.text = map[selectedMap].name;
    }
    public void NextCharacter1()
    {
        characters1[selectedCharacter1].SetActive(false);
        selectedCharacter1 = (selectedCharacter1 + 1) % characters1.Length;
        characters1[selectedCharacter1].SetActive(true);
        characterNameTextP1.text = characters1[selectedCharacter1].name;
    }
    public void PreviousCharacter1()
    {
        characters1[selectedCharacter1].SetActive(false);
        selectedCharacter1--;
        if (selectedCharacter1 < 0)
        {
            selectedCharacter1 += characters1.Length;
        }
        characters1[selectedCharacter1].SetActive(true);
        characterNameTextP1.text = characters1[selectedCharacter1].name;

    }

    public void NextCharacter2()
    {
        characters2[selectedCharacter2].SetActive(false);
        selectedCharacter2 = (selectedCharacter2 + 1) % characters2.Length;
        characters2[selectedCharacter2].SetActive(true);
        characterNameTextP2.text = characters2[selectedCharacter2].name;
    }

    public void PreviousCharacter2()
    {
        characters2[selectedCharacter2].SetActive(false);
        selectedCharacter2--;
        if (selectedCharacter2 < 0)
        {
            selectedCharacter2 += characters2.Length;
        }
        characters2[selectedCharacter2].SetActive(true);
        characterNameTextP2.text = characters2[selectedCharacter2].name;
    }

    public void NextMap()
    {
        map[selectedMap].SetActive(false);
        selectedMap = (selectedMap + 1) % map.Length;
        mapName.text = map[selectedMap].name;
    }

    public void PreviousMap()
    {
        map[selectedMap].SetActive(false);
        selectedMap--;
        if (selectedMap < 0)
        {
            selectedMap += map.Length;
        }
        mapName.text = map[selectedMap].name;
    }

    public void StartGame()
    {
        PlayerPrefs.SetInt("selectedCharacter1", selectedCharacter1);
        PlayerPrefs.SetInt("selectedCharacter2", selectedCharacter2);
        PlayerPrefs.SetInt("selectedMap", selectedMap);
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
    public void SelectionMap()
    {
        gameObject.SetActive(false);
        characterSelection.SetActive(false);
        mapSelection.SetActive(true);
    }
}
