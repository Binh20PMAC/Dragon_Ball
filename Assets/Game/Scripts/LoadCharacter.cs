using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadCharacter : MonoBehaviour
{
    public GameObject[] characterPrefabs;
    public GameObject[] mapPrefabs;
    public Transform P1;
    public Transform P2;
    public Material skyboxMaterial;

    void Start()
    {  
        //LoadCharacter1
        int selectedCharacter1 = PlayerPrefs.GetInt("selectedCharacter1");
        GameObject clone1 = Instantiate(characterPrefabs[selectedCharacter1], P1.transform.position, Quaternion.Euler(0f, 90f, 0f));
        clone1.name = "Player";
        clone1.layer = LayerMask.NameToLayer("Player");
        int selectedCharacter2 = PlayerPrefs.GetInt("selectedCharacter2");
        GameObject clone2 = Instantiate(characterPrefabs[selectedCharacter2], P2.transform.position, Quaternion.Euler(0f, 90f, 0f));
        clone2.name = "Enemy";
        clone2.layer = LayerMask.NameToLayer("Enemy");
        Instantiate(mapPrefabs[PlayerPrefs.GetInt("selectedMap")]);
        RenderSettings.skybox = skyboxMaterial;
    }
}
