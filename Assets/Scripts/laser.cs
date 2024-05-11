using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

//the functionality i want is only included in an old deprecated function
//i'm sorry
#pragma warning disable CS0618

public class laser : MonoBehaviour
{
    private GameObject tracking;
    private Vector3 targetRotation;
    private float shotLast;
    [SerializeField] private float cooldown = 1;
    [SerializeField] private GameObject[] barrels;
    [SerializeField] private GameObject projectile;
    [SerializeField] private float damage = 1;
    private GameObject gameManager;
    public string targetTag;

    private ShipInfo.WeaponState weaponState = ShipInfo.WeaponState.idle;

    public ShipInfo.WeaponState state
    {
        get
        {
            return weaponState;
        }
        private set
        {
            if (value != weaponState)
            {
                weaponState = value;
                gameManager.GetComponent<ShipSelector>().Refresh();
            }
        }
    }


    private void Awake()
    {
        gameManager = GameObject.Find("Game Manager");
    }

    void Start()
    {
        targetTag = transform.parent.parent.GetComponent<ShipInfo>().enemyTeam.ToString();
    }

    void Update()
    {
        if (tracking != null)
        {
            if (Time.time > shotLast + cooldown)
            {
                ReTrack();
                if (tracking == null) return;
                state = ShipInfo.WeaponState.aiming;
                //transform.Rotate(new Vector3(0, 0, Mathf.Clamp(Mathf.DeltaAngle(transform.rotation.eulerAngles.z, targetRotation.z), -0.1f, 0.1f)));
                //if (Mathf.Abs(Mathf.Clamp(Mathf.DeltaAngle(transform.rotation.eulerAngles.z, targetRotation.z), -0.1f, 0.1f)) < .01f)


                //{

                //    float delta = Vector3.Distance(-(barrels[0].transform.GetChild(0).GetChild(0).position - barrels[0].transform.GetChild(0).position).normalized, (transform.position - tracking.transform.position).normalized);
                //    barrels[0].transform.RotateAround(barrels[0].transform.GetChild(0).position - barrels[0].transform.position, 0.5f * Time.deltaTime);
                //    barrels[1].transform.RotateAround(barrels[1].transform.GetChild(0).position - barrels[1].transform.position, -0.5f * Time.deltaTime);
                //    if (delta < Vector3.Distance(-(barrels[0].transform.GetChild(0).GetChild(0).position - barrels[0].transform.GetChild(0).position).normalized, (transform.position - tracking.transform.position).normalized))
                //    {
                //        barrels[0].transform.RotateAround(barrels[0].transform.GetChild(0).position - barrels[0].transform.position, -1 * Time.deltaTime);
                //        barrels[1].transform.RotateAround(barrels[2].transform.GetChild(0).position - barrels[2].transform.position, 1 * Time.deltaTime);

                //        if (delta < Vector3.Distance(-(barrels[0].transform.GetChild(0).GetChild(0).position - barrels[0].transform.GetChild(0).position).normalized, (transform.position - tracking.transform.position).normalized))
                //        {
                            //Destroy(tracking);
                            Shoot();
                            //tracking = null;
                //        }
                //    }
                //}
            }
        }
        else
        {
            if (Time.time > shotLast + cooldown)
            {
                GameObject[] enemies = GameObject.FindGameObjectsWithTag(targetTag);
                foreach (GameObject enemy in enemies)
                {
                    if (Vector3.Distance(transform.position, enemy.transform.position) < transform.parent.parent.gameObject.GetComponent<ShipInfo>().range)
                    {
                        tracking = enemy;
                        return;
                    }

                }
                state = ShipInfo.WeaponState.idle;
            }
            //Start();
        }
    }

    void Shoot()
    {
        foreach (GameObject barrel in barrels)
        {
            GameObject pr = Instantiate(projectile, barrel.transform.GetChild(0).position, Quaternion.Euler(targetRotation));
            pr.GetComponent<Projectile>().targetTag = targetTag;
            pr.GetComponent<Projectile>().damage = damage;
        }
        shotLast = Time.time;
        state = ShipInfo.WeaponState.firing;
    }

    public void SetTarget(GameObject newTarget)
    {
        if (Time.time > shotLast + cooldown)
        {
            if (Vector3.Distance(transform.position, newTarget.transform.position) < transform.parent.parent.GetComponent<ShipInfo>().range)
            {
                tracking = newTarget;
            }
            else
            {
                transform.parent.GetComponent<Movement>().GoToEnemy(newTarget);
                tracking = null;
            }

            //ReTrack();
        }
    }

    void ReTrack()
    {
        if (transform.parent.parent.GetComponent<ShipInfo>() != null)
        {
            if (Vector3.Distance(transform.position, tracking.transform.position) > transform.parent.parent.GetComponent<ShipInfo>().range)
            {
                tracking = null;
            }
            if (tracking != null)
            {
                Quaternion oldRotation = transform.rotation;
                transform.LookAt(tracking.transform.position);
                targetRotation = transform.rotation.eulerAngles;
                transform.rotation = oldRotation;
            }
        }
    }
}
