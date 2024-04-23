using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject shipInfo;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string GetPlayerTeam()
    {
        return "Roman";
    }

    public string GetEnemyTeam()
    {
        return "Shang";
    }

    public void UpdateShipInfo(GameObject ship)
    {
        if (ship == null)
        {
            shipInfo.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "";
        }
        else
        {
            shipInfo.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = ship.name;
        }
    }
}
