using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Movement : MonoBehaviour
{
    public float turningSpeed = 10;
    public new Camera camera;
    [SerializeField] private GameObject hologramPrefab;
    private NavMeshAgent agent;

    private GameObject hologram;
    private Vector3 targetAngle;

    public GameManager gameManager;

    private bool active = false;
    private bool stopping = false;
    private GameObject nextEnemy;
    public GameObject particle;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        agent = GetComponent<NavMeshAgent>();
        camera = gameManager.camera;
    }

    // Update is called once per frame
    void Update()
    {
        if(agent.velocity.magnitude > 0.01f)
        {
            particle.SetActive(true);
        }
        else
        {
            particle.SetActive(false);
        }
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
                agent.stoppingDistance = 0;
                if (stopping || agent.isStopped)
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
                        //var holo = AddHologram(hit.point);
                        //gameManager.AddHologram(holo);

                        hologram = Instantiate(hologramPrefab, new Vector3(hit.point.x, transform.position.y, hit.point.z), Quaternion.identity);
                        gameManager.AddToQueue(hologram);
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
                    transform.Rotate(new Vector3(0, Mathf.Clamp(Mathf.DeltaAngle(transform.rotation.eulerAngles.y, targetAngle.y), -turningSpeed * Time.deltaTime, turningSpeed * Time.deltaTime), 0));
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
                Vector3 offset = Vector3.zero;
                var b = hologram.GetComponent<Collider>().bounds;
                float spacing = 1;
                b.Expand(spacing);
                while (b.Intersects(bounds))
                {
                    offset += new Vector3(Random.value * 0.2f - 0.1f, 0, Random.value * 0.2f - 0.1f);

                    b = hologram.GetComponent<Collider>().bounds;
                    b.Expand(spacing);
                    b.center += offset;
                }
                hologram.transform.Translate(offset);
            }
            foreach (var bounds in gameManager.GetBounds())
            {
                var b = hologram.GetComponent<Collider>().bounds;
                b.center = hologram.transform.position;
                float spacing = 1;
                b.Expand(spacing);
                if (b.Intersects(bounds))
                {
                    Destroy(hologram);
                    return AddHologram(point);
                }
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
        hologram.transform.LookAt(-(newPoint - hologram.transform.position) + hologram.transform.position);
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

    public void GoToEnemy(GameObject enemy)
    {
        nextEnemy = enemy;
        agent.stoppingDistance = GetComponent<ShipInfo>().range - 1;
        agent.SetDestination(enemy.transform.position);
    }
}
