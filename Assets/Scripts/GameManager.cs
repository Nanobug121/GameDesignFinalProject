using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System.Collections.Concurrent;
using System.Drawing;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject shipInfo;
    [SerializeField] GameObject points;
    public new Camera camera;
    Bounds[] holograms;
    ConcurrentQueue<GameObject> holoQueue;
    private float shangPoints;
    private float romanPoints;

    public enum Team
    {
        None,
        Roman,
        Shang
    }
    // Start is called before the first frame update
    void Start()
    {
        holoQueue = new ConcurrentQueue<GameObject>();
        UpdateShipInfo(null);
    }

    // Update is called once per frame
    void Update()
    {
        deleteBounds();
    }

    private void LateUpdate()
    {
        ComputeHolograms();
    }

    public string GetPlayerTeam()
    {
        return "Roman";
    }

    public string GetEnemyTeam()
    {
        return "Shang";
    }

    public void UpdateShipInfo(GameObject ship)
    {
        if (ship == null)
        {
            shipInfo.SetActive(false);
        }
        else
        {
            shipInfo.SetActive(true);
            ShipInfo info = ship.GetComponent<ShipInfo>();
            shipInfo.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = ship.name;
            shipInfo.transform.GetChild(2).gameObject.GetComponent<Slider>().value = info.health;
            shipInfo.transform.GetChild(2).gameObject.GetComponent<Slider>().maxValue = info.maxHealth;
            shipInfo.transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>().text = "" + info.shipClass;
            TextMeshProUGUI status = shipInfo.transform.GetChild(4).gameObject.GetComponent<TextMeshProUGUI>();
            status.text = "";
            for (int i = 0; i < info.weapons.Length; i++)
            {
                if (info.weapons[i] != ShipInfo.WeaponState.none)
                    status.text += info.weaponNames[i] + ": " + info.weapons[i] + "\n";
            }
        }
    }

    public void UpdatePoints()
    {
        points.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "" + ((int)romanPoints);
        points.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = "" + ((int)shangPoints);
    }

    public void AddToQueue(GameObject hologram)
    {
        holoQueue.Enqueue(hologram);
    }

    public void AddHologram(Bounds hologram)
    {
        if (holograms != null)
        {
            Bounds[] old = ((Bounds[])holograms.Clone());
            holograms = new Bounds[holograms.Length + 1];
            for (int i = 0; i < old.Length; i++)
            {
                holograms[i] = old[i];
            }
        }
        else
        {
            holograms = new Bounds[1];
        }
        holograms[holograms.Length - 1] = hologram;
    }

    public Bounds[] GetBounds()
    {
        return holograms;
    }

    public void deleteBounds()
    {
        holograms = null;
    }

    void SortQueue()
    {
        List<Bounds> bList = new List<Bounds>();
        List<GameObject> gList = new List<GameObject>();
        GameObject hologram;
        var a = holoQueue.ToArray();
        while (holoQueue.TryDequeue(out hologram))
        {
            bList.Add(hologram.GetComponent<Renderer>().bounds);
            gList.Add(hologram);
        }
        for (int i = 0; i < bList.Count; i++)
        {
            for (int k = 0; k < i; k++)
            {
                if (bList[i].extents.sqrMagnitude > bList[k].extents.sqrMagnitude)
                {
                    {
                        var temp = bList[i];
                        bList[i] = bList[k];
                        bList[k] = temp;
                    }
                    {
                        var temp = gList[i];
                        gList[i] = gList[k];
                        gList[k] = temp;
                    }
                    i--;
                    break;
                }
            }
        }
        foreach (var item in gList)
        {
            holoQueue.Enqueue(item);
        }
    }

    public void ComputeHolograms()
    {
        SortQueue();
        GameObject hologram;
        while (holoQueue.TryDequeue(out hologram))
        {
            AddHologram(ComputeHologram(hologram));
        }
    }

    public Bounds ComputeHologram(GameObject hologram)
    {
        if (GetBounds() != null)
        {
            Vector3 offset = Vector3.zero;
            foreach (var bounds in GetBounds())
            {
                var b = hologram.GetComponent<Renderer>().bounds;
                float spacing = 1;
                b.Expand(spacing);
                while (b.Intersects(bounds))
                {
                    offset += new Vector3(Random.value * 0.2f - 0.1f, 0, Random.value * 0.2f - 0.1f);

                    b = hologram.GetComponent<Renderer>().bounds;
                    b.Expand(spacing);
                    b.center += offset;
                }
            }
            foreach (var bounds in GetBounds())
            {
                var b = hologram.GetComponent<Renderer>().bounds;
                b.center += offset;
                float spacing = 1;
                b.Expand(spacing);
                if (b.Intersects(bounds))
                {
                    //hologram.transform.Translate(-offset);
                    //Debug.Log("intersecting");
                    return ComputeHologram(hologram);
                }
            }
            hologram.transform.Translate(offset);
        }
        return hologram.GetComponent<Renderer>().bounds;
    }

    public void AddPoints(Team team, float points)
    {
        if (team == Team.None) return;
        if (team == Team.Roman) romanPoints += points;
        if (team == Team.Shang) shangPoints += points;
        UpdatePoints();
    }
}
