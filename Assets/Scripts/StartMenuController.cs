using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    public GameObject startButton;
    public GameObject planetSelector;
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
}
