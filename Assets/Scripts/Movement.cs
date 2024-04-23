using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Movement : MonoBehaviour
{
    [SerializeField] private new Camera camera;
    [SerializeField] private GameObject hologramPrefab;
    private NavMeshAgent agent;

    private GameObject hologram;
    private Vector3 targetAngle;

    private bool active = false;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            if (Input.GetMouseButton(1))
            {
                Ray ray = camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hologram == null)
                    {
                        AddHologram(hit.point);
                    }
                    else
                    {
                        if (transform.position == hologram.transform.position)
                        {
                            hologram.SetActive(false);
                        }
                        RotateHologram(hit.point);
                    }
                }
            }
            else
            {
                if (hologram != null)
                {
                    agent.SetDestination(hologram.transform.position);
                    Destroy(hologram);
                }
                else
                {
                    transform.Rotate(new Vector3(0, Mathf.Clamp(Mathf.DeltaAngle(transform.rotation.eulerAngles.y, targetAngle.y), -0.1f, 0.1f), 0));
                }
            }
        }
    }

    void AddHologram(Vector3 point)
    {
        hologram = Instantiate(hologramPrefab, new Vector3(point.x, transform.position.y, point.z), Quaternion.identity);
        //Destroy(hologram.GetComponent<Movement>());
        //Destroy(hologram.GetComponent<NavMeshAgent>());
        //hologram.GetComponent<Renderer>().material = hologramMaterial;
    }

    void RotateHologram(Vector3 newPoint)
    {
        //hologram.transform.Rotate(new Vector3(0, (newPoint.x - oldPoint.x) * 40, 0));
        hologram.transform.LookAt(newPoint);
        hologram.transform.Rotate(new Vector3(-hologram.transform.rotation.eulerAngles.x, 0, -hologram.transform.rotation.eulerAngles.z));
        targetAngle = hologram.transform.rotation.eulerAngles;
    }

    public void Activate()
    {
        active = true;
    }

    public void DeActivate()
    {
        active = false;
    }
}
