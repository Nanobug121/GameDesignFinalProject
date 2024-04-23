using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipInfo : MonoBehaviour
{
    public string name;
    public ShipClass shipClass;
    public float maxHealth;
    public float health;
    public float speed;
    public WeaponState[] weapons;

    public enum ShipClass
    {
        big,
        small
    }

    public enum WeaponState
    {
        firing,
        waiting,
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
