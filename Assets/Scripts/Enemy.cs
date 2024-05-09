using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

    }

    // Update is called once per frame
    void Update()
    {
        if(GetComponent<ShipInfo>().weapons[0] == ShipInfo.WeaponState.idle)
        {
            GameObject tracking = null;
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(gameManager.GetPlayerTeam());
            foreach (GameObject enemy in enemies)
            {
                if (tracking == null)
                {
                    tracking = enemy;
                }
                else if (Vector3.Distance(transform.position, enemy.transform.position) < Vector3.Distance(transform.position, tracking.transform.position))
                {
                    tracking = enemy;
                }
            }
            if (tracking != null)
            {
                GetComponent<Movement>().GoToEnemy(tracking);
            }
        }
    }
}
