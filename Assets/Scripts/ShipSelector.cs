using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
                if (hit.collider.transform.parent == null) return;
                if (hit.collider.transform.parent.gameObject.tag == gameManager.GetComponent<GameManager>().GetEnemyTeam())
                {
                    if (selected != null)
                    {
                        foreach (var item in selected)
                        {
                                foreach (laser l in item.GetComponentsInChildren<laser>())
                                {
                                    l.SetTarget(hit.collider.transform.parent.gameObject);
                                }
                        }
                    }
                    return;
                }
                if (hit.collider.transform.parent.gameObject.tag == gameManager.GetComponent<GameManager>().GetPlayerTeam() && hit.collider.transform.parent.gameObject.GetComponent<Movement>() != null)
                {
                    if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
                    {
                        //TODO: ensure value of selected
                        if (selected != null)
                        {
                            GameObject[] old = ((GameObject[])selected.Clone());
                            selected = new GameObject[selected.Length + 1];
                            for (int i = 0; i < selected.Length - 1; i++)
                            {
                                selected[i] = old[i];
                            }
                        }
                        else
                        {
                            selected = new GameObject[1];
                        }
                        selected[selected.Length - 1] = hit.collider.transform.parent.gameObject;

                        hit.collider.transform.parent.gameObject.GetComponent<Movement>().Activate();
                        hit.collider.transform.parent.gameObject.transform.Find("range").gameObject.SetActive(true);
                    }
                    else
                    {
                        if (selected != null)
                        {
                            foreach (var item in selected)
                            {
                                if (item != null)
                                {
                                    item.GetComponent<Movement>().DeActivate();
                                    item.transform.Find("range").gameObject.SetActive(false);
                                }
                            }
                        }
                        selected = new GameObject[] { hit.collider.transform.parent.gameObject };
                        selected[0].GetComponent<Movement>().Activate();
                        hit.collider.transform.parent.gameObject.transform.Find("range").gameObject.SetActive(true);
                    }
                    Refresh();

                }
                else
                {
                    if (selected != null)
                    {
                        foreach (var item in selected)
                        {
                            if (item != null)
                            {
                                item.GetComponent<Movement>().DeActivate();
                                item.transform.Find("range").gameObject.SetActive(false);
                            }
                        }
                    }
                    selected = null;
                    Refresh();
                }
            }
            else
            {
                if (selected != null) {
                foreach (var item in selected)
                {
                    if (item != null)
                    {
                        item.GetComponent<Movement>().DeActivate();
                        item.transform.Find("range").gameObject.SetActive(false);
                    }
                }
                selected = null;
            }
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
