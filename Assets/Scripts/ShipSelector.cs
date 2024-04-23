using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;

public class ShipSelector : MonoBehaviour
{
    [SerializeField] private new Camera camera;
    [SerializeField] private GameObject gameManager;
    private GameObject selected;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (selected != null)
                {
                    selected.GetComponent<Movement>().DeActivate();
                }
                if (hit.collider.gameObject.tag == gameManager.GetComponent<GameManager>().GetEnemyTeam())
                {
                    if(selected != null && selected.tag == gameManager.GetComponent<GameManager>().GetPlayerTeam())
                    {
                        foreach(laser l in selected.GetComponentsInChildren<laser>())
                        {
                            l.SetTarget(hit.collider.gameObject);
                        }
                    }
                    selected.GetComponent<Movement>().Activate();
                    return;
                }
                if (hit.collider.gameObject.tag == gameManager.GetComponent<GameManager>().GetPlayerTeam())
                {
                selected = hit.collider.gameObject;
                    gameManager.GetComponent<GameManager>().UpdateShipInfo(selected);

                    if (selected.GetComponent<Movement>() == null)
                    {
                        selected = null;
                        gameManager.GetComponent<GameManager>().UpdateShipInfo(selected);
                        return;
                    }
                    selected.GetComponent<Movement>().Activate();
                }
                else
                {
                    selected = null;
                    gameManager.GetComponent<GameManager>().UpdateShipInfo(selected);
                }
            }
            else
            {
                selected = null;
                gameManager.GetComponent<GameManager>().UpdateShipInfo(selected);
            }
        }
    }
}
