using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShipInfo : MonoBehaviour
{
    public ShipClass shipClass;

    public float range;
    public float maxHealth { get { return gameObject.GetComponent<Health>().maxHealth; } }
    public float health { get { return gameObject.GetComponent<Health>().health; } }
    public float speed { get { return gameObject.GetComponent<NavMeshAgent>().speed; } }
    public GameManager.Team team
    {
        get
        {
            if (gameObject.CompareTag("Roman")) return GameManager.Team.Roman;
            if (gameObject.CompareTag("Shang")) return GameManager.Team.Shang;
            return GameManager.Team.None;
        }
    }
    public GameManager.Team enemyTeam
    {
        get
        {
            if (gameObject.CompareTag("Roman")) return GameManager.Team.Shang;
            if (gameObject.CompareTag("Shang")) return GameManager.Team.Roman;
            return GameManager.Team.None;
        }
    }

    public WeaponState[] weapons
    {
        get
        {
            WeaponState[] ret = new WeaponState[transform.childCount];
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).GetComponent<laser>() != null)
                    ret[i] = transform.GetChild(i).GetComponent<laser>().state;
            }
            return ret;
        }
    }
    public string[] weaponNames
    {
        get
        {
            string[] ret = new string[transform.childCount];
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).GetComponent<laser>() != null)
                    ret[i] = transform.GetChild(i).name;
            }
            return ret;
        }
    }

    public enum ShipClass
    {
        big,
        smol
    }

    public enum WeaponState
    {
        none,
        firing,
        idle,
        damaged,
        aiming
    }

    // Start is called before the first frame update
    void Start()
    {
        var r = transform.Find("range");
        if (r != null)
            r.localScale = new Vector3(range, 0.1f, range);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
