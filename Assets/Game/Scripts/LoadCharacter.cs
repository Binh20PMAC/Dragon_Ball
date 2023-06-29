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
    {   //Camera
        GameObject cameraObject = new GameObject("Camera");
        Camera cameraComponent = cameraObject.AddComponent<Camera>();
        GameObject cameraObject2 = new GameObject("Camera2");
        Camera cameraComponent2 = cameraObject2.AddComponent<Camera>();
        //LoadCharacter1
        int selectedCharacter1 = PlayerPrefs.GetInt("selectedCharacter1");
        GameObject clone1 = Instantiate(characterPrefabs[selectedCharacter1], P1.transform.position, Quaternion.Euler(0f, 90f, 0f));
        clone1.name = "Player";
        clone1.layer = LayerMask.NameToLayer("Player");
        //LoadCamera1
        cameraObject.transform.parent = clone1.transform;
        //cameraObject.transform.position = new Vector3(0f, 0f, 0f);
        CameraController cameraController = cameraObject.AddComponent<CameraController>();
        cameraObject.SetActive(false);
        //LoadCharacter2
        int selectedCharacter2 = PlayerPrefs.GetInt("selectedCharacter2");
        GameObject clone2 = Instantiate(characterPrefabs[selectedCharacter2], P2.transform.position, Quaternion.Euler(0f, 90f, 0f));
        clone2.name = "Enemy";
        clone2.layer = LayerMask.NameToLayer("Enemy");
        //LoadCamera2
        cameraObject2.transform.parent = clone2.transform;
        CameraController2 cameraController2 = cameraObject2.AddComponent<CameraController2>();
        cameraObject2.SetActive(false);
        //
        Instantiate(mapPrefabs[PlayerPrefs.GetInt("selectedMap")]);
        RenderSettings.skybox = skyboxMaterial;
    }
}
