using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shipyard : MonoBehaviour
{
    [SerializeField] private int timeToCapture;
    private float captureTime;
    [SerializeField] private int ships;
    [SerializeField] private GameObject ship;
    [SerializeField] private float range;
    [SerializeField] private float cooldown;
    private GameManager.Team team;
    private GameManager gameManager;
    private float nextSpawn;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        int counter = 0;
        foreach (var go in GameObject.FindGameObjectsWithTag("Roman"))
        {
            if ((go.transform.position - transform.position).magnitude < range)
            {
                counter++;
            }
        }
        foreach (var go in GameObject.FindGameObjectsWithTag("Shang"))
        {
            if ((go.transform.position - transform.position).magnitude < range)
            {
                counter--;
            }
        }
        captureTime += counter * Time.deltaTime;
        if (captureTime >= timeToCapture)
        {
            captureTime = timeToCapture;
            team = GameManager.Team.Roman;
        }
        if (captureTime <= -timeToCapture)
        {
            captureTime = -timeToCapture;
            team = GameManager.Team.Shang;
        }

        if (team != GameManager.Team.None && nextSpawn < Time.time && ships-- > 0)
        {
            var go = Instantiate(ship, transform.position, Quaternion.identity);
            go.tag = team.ToString();
            go.GetComponent<Movement>().camera = gameManager.camera;
            nextSpawn = Time.time + cooldown;
        }

    }
}
