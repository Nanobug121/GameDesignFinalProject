using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShipInfo : MonoBehaviour
{
    public ShipClass shipClass;
    public float maxHealth { get { return gameObject.GetComponent<Health>().maxHealth; } }
    public float health { get { return gameObject.GetComponent<Health>().health; }}
    public float speed { get { return gameObject.GetComponent<NavMeshAgent>().speed; } }
    public WeaponState[] weapons;
    public string[] weaponNames;

    public enum ShipClass
    {
        big,
        smol
    }

    public enum WeaponState
    {
        firing,
        idle,
        damaged,
        aiming
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
