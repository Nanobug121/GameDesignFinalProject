using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Movement : MonoBehaviour
{
    [SerializeField] private new Camera camera;
    [SerializeField] private Material hologramMaterial;
    private NavMeshAgent agent;

    private GameObject hologram;
    private Vector3 oldPoint;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hologram == null)
                {
                    agent.SetDestination(hit.point);
                    AddHologram(hit.point);
                }
                else
                {
                    RotateHologram(hit.point);
                }
            }
        }
        else
        {
            if (hologram != null)
            {
                Destroy(hologram);
            }
        }
        if (hologram != null)
        {
            //TODO: lerp rotation
        }
    }

    void AddHologram(Vector3 point)
    {
        hologram = Instantiate(gameObject, new Vector3(point.x, transform.position.y, point.z), Quaternion.identity);
        Destroy(hologram.GetComponent<Movement>());
        Destroy(hologram.GetComponent<NavMeshAgent>());
        hologram.GetComponent<Renderer>().material = hologramMaterial;
        oldPoint = point;
    }

    void RotateHologram(Vector3 newPoint)
    {
        hologram.transform.Rotate(new Vector3(0, (newPoint.x - oldPoint.x) * 40, 0));
        oldPoint = newPoint;
    }
}
