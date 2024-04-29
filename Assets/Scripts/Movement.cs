using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Movement : MonoBehaviour
{
    [SerializeField] private new Camera camera;
    [SerializeField] private GameObject hologramPrefab;
    private NavMeshAgent agent;

    private GameObject hologram;
    private Vector3 targetAngle;

    public GameManager gameManager;

    private bool active = false;
    private bool stopping = false;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            if (stopping)
            {
                if (agent.velocity == Vector3.zero)
                {
                    agent.SetDestination(transform.position);
                stopping = false;
                agent.isStopped = false;
                }
            }

            if (Input.GetKey(KeyCode.Backspace))
            {
                if (agent.velocity == Vector3.zero)
                {
                    agent.SetDestination(transform.position);
                }
                if (hologram != null)
                {
                    Destroy(hologram);
                }
                stopping = true;
                agent.isStopped = true;
                targetAngle = transform.rotation.eulerAngles;
                return;
            }
            if (Input.GetMouseButton(1))
            {
                if (stopping)
                {
                    stopping = false;
                    agent.isStopped = false;
                }

                Ray ray = camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hologram == null)
                    {
                        var holo = AddHologram(hit.point);
                        gameManager.AddHologram(holo);
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

    Bounds AddHologram(Vector3 point)
    {
        hologram = Instantiate(hologramPrefab, new Vector3(point.x, transform.position.y, point.z), Quaternion.identity);
        //hologram = Instantiate(hologramPrefab, transform.position, Quaternion.identity);
        if (gameManager.GetBounds() != null)
        {
            foreach (var bounds in gameManager.GetBounds())
            {
                //TODO: loop over bounds already checked
                Vector3 offset = new Vector3(0, 0, 0);
                var b = hologram.GetComponent<Collider>().bounds;
                while (b.Intersects(bounds))
                {
                    offset += new Vector3(Random.value * 0.2f - 0.1f, 0, Random.value * 0.2f - 0.1f);

                    b = hologram.GetComponent<Collider>().bounds;
                    b.center += offset;
                }
                hologram.transform.Translate( offset);
            }
        }
        return hologram.GetComponent<Collider>().bounds;

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
