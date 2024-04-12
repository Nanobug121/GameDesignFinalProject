using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laser : MonoBehaviour
{
    private GameObject tracking;
    private Vector3 targetRotation;

    // Start is called before the first frame update
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
            Quaternion oldRotation = transform.rotation;
            transform.LookAt(tracking.transform.position);
            targetRotation = transform.rotation.eulerAngles;
            transform.rotation = oldRotation;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (tracking != null) { 
        transform.Rotate(new Vector3(0, Mathf.Clamp(Mathf.DeltaAngle(transform.rotation.eulerAngles.y, targetRotation.y), -0.1f, 0.1f), 0));
            if (Mathf.Abs(Mathf.Clamp(Mathf.DeltaAngle(transform.rotation.eulerAngles.y, targetRotation.y), -0.1f, 0.1f)) < .01f)
            {
                Destroy(tracking);
                tracking = null;
            }
        }
        else
        {
            Start();
        }
    }
}
