using System.Collections;
using System.Collections.Generic;
using System.Threading;
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

    void Start()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
        foreach(GameObject enemy in enemies)
        {
            if(tracking == null)
            {
                tracking = enemy;
            }
            else if(Vector3.Distance(transform.position, enemy.transform.position) < Vector3.Distance(transform.position, tracking.transform.position))
            {
                tracking = enemy;
            }
        }
        if (tracking != null)
        {
            //TODO: use quat fromto
            Quaternion oldRotation = transform.rotation;
            transform.LookAt(tracking.transform.position);
            targetRotation = transform.rotation.eulerAngles;
            transform.rotation = oldRotation;
        }
    }

    void Update()
    {
        //WARNING: DO NOT TOUCH -- IT WORKS
        if (tracking != null) { 
        transform.Rotate(new Vector3(0, Mathf.Clamp(Mathf.DeltaAngle(transform.rotation.eulerAngles.y, targetRotation.y), -0.1f, 0.1f), 0));
            if (Mathf.Abs(Mathf.Clamp(Mathf.DeltaAngle(transform.rotation.eulerAngles.y, targetRotation.y), -0.1f, 0.1f)) < .01f)
            {
                
                float delta = Vector3.Distance(-(barrels[0].transform.GetChild(0).GetChild(0).position - barrels[0].transform.GetChild(0).position).normalized, (transform.position - tracking.transform.position).normalized);
                barrels[0].transform.RotateAround(barrels[0].transform.GetChild(0).position - barrels[0].transform.position, 0.5f * Time.deltaTime);
                barrels[1].transform.RotateAround(barrels[1].transform.GetChild(0).position - barrels[1].transform.position, -0.5f * Time.deltaTime);
                if(delta < Vector3.Distance(-(barrels[0].transform.GetChild(0).GetChild(0).position - barrels[0].transform.GetChild(0).position).normalized, (transform.position - tracking.transform.position).normalized))
                {
                    barrels[0].transform.RotateAround(barrels[0].transform.GetChild(0).position - barrels[0].transform.position, -1 * Time.deltaTime);
                    barrels[1].transform.RotateAround(barrels[1].transform.GetChild(0).position - barrels[1].transform.position, 1 * Time.deltaTime);

                    if (delta < Vector3.Distance(-(barrels[0].transform.GetChild(0).GetChild(0).position - barrels[0].transform.GetChild(0).position).normalized, (transform.position - tracking.transform.position).normalized))
                    {
                        //Destroy(tracking);
                        Shoot();
                        tracking = null;
                    }
                }
            }
        }
        else
        {
            if(Time.time > shotLast + cooldown)
            Start();
        }
    }

    void Shoot()
    {
        Instantiate(projectile, transform.position, Quaternion.Euler(targetRotation));
        shotLast = Time.time;
    }
}
