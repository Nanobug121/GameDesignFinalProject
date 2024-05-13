using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public AudioClip destroySound;

    private GameObject gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Hit(float damage)
    {
        health -= damage;
        if (health < 0)
        {
            if(destroySound != null)
            {
                gameManager.GetComponent<GameManager>().PlaySound(destroySound);
            }
            Destroy(gameObject);
        }
        gameManager.GetComponent<ShipSelector>().Refresh();
    }
}
