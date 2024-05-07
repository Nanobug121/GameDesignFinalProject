using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningField : MonoBehaviour
{
    [SerializeField] private int timeToCapture;
    private float captureTime;
    [SerializeField] private float points;
    [SerializeField] private float range;
    private GameManager.Team team;
    private GameManager gameManager;
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

        gameManager.AddPoints(team, points * Time.deltaTime);
    }
}
