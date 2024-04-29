using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Build.Content;
using UnityEngine;

public class ShipSelector : MonoBehaviour
{
    [SerializeField] private new Camera camera;
    [SerializeField] private GameObject gameManager;
    [SerializeField] private GameObject[] selected;

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
                if (hit.collider.gameObject.tag == gameManager.GetComponent<GameManager>().GetEnemyTeam())
                {
                    if(selected != null)
                    {
                        foreach (var item in selected)
                        {
                            foreach (laser l in item.GetComponentsInChildren<laser>())
                            {
                                l.SetTarget(hit.collider.gameObject);
                            }
                        }
                    }
                    return;
                }
                if (hit.collider.gameObject.tag == gameManager.GetComponent<GameManager>().GetPlayerTeam() && hit.collider.gameObject.GetComponent<Movement>() != null)
                {
                    if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
                    {
                        GameObject[] old = ((GameObject[]) selected.Clone());
                        selected = new GameObject[selected.Length + 1];
                        for(int i = 0; i < selected.Length - 1; i++)
                        {
                            selected[i] = old[i];
                        }
                        selected[selected.Length - 1] = hit.collider.gameObject;
                        
                        hit.collider.gameObject.GetComponent<Movement>().Activate();
                        hit.collider.gameObject.transform.Find("range").gameObject.SetActive(true);
                    }
                    else
                    {
                        if (selected != null)
                        {
                            foreach (var item in selected)
                            {
                                item.GetComponent<Movement>().DeActivate();
                                item.transform.Find("range").gameObject.SetActive(false);
                            }
                        }
                        selected = new GameObject[] {hit.collider.gameObject};
                        selected[0].GetComponent<Movement>().Activate();
                        hit.collider.gameObject.transform.Find("range").gameObject.SetActive(true);
                    }
                    Refresh();

                }
                else
                {
                    foreach (var item in selected)
                    {
                        item.GetComponent<Movement>().DeActivate();
                        item.transform.Find("range").gameObject.SetActive(false);
                    }
                    selected = null;
                    Refresh();
                }
            }
            else
            {
                foreach (var item in selected)
                {
                    item.GetComponent<Movement>().DeActivate();
                    item.transform.Find("range").gameObject.SetActive(false);
                }
                selected = null;
                Refresh();
            }
        }
    }

    public void Refresh()
    {

        if (selected != null && selected.Length == 1)
        {
            gameManager.GetComponent<GameManager>().UpdateShipInfo(selected[0]);
        }
        else
        {
            gameManager.GetComponent<GameManager>().UpdateShipInfo(null);
        }
    }
}
