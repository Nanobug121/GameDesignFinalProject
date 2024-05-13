using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    public GameObject startButton;
    public GameObject planetSelector;
    public GameObject controls;
    public GameObject back;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        //Debug.Log("start");
        startButton.SetActive(false);
        planetSelector.SetActive(true);
    }
    public void ChoosePlanet()
    {
        SceneManager.LoadScene(1);
    }

    public void ShowControls()
    {
        startButton.SetActive(false);
        controls.SetActive(true);
        back.SetActive(true);
    }
    public void HideControls()
    {
        startButton.SetActive(true);
        controls.SetActive(false);
        back.SetActive(false);
    }
}
