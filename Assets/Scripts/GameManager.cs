using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject shipInfo;
    // Start is called before the first frame update
    void Start()
    {
        UpdateShipInfo(null);
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
            shipInfo.SetActive(false);
        }
        else
        {
            shipInfo.SetActive(true);
            ShipInfo info = ship.GetComponent<ShipInfo>();
            shipInfo.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = ship.name;
            shipInfo.transform.GetChild(2).gameObject.GetComponent<Slider>().value = info.health;
            shipInfo.transform.GetChild(2).gameObject.GetComponent<Slider>().maxValue = info.maxHealth;
            shipInfo.transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>().text = "" + info.shipClass;
            TextMeshProUGUI status = shipInfo.transform.GetChild(4).gameObject.GetComponent<TextMeshProUGUI>();
            status.text = "";
            for (int i = 0; i < info.weapons.Length; i++)
            {
                status.text += info.weaponNames[i] + ": " + info.weapons[i] + "\n";
            }
        }
    }
}
