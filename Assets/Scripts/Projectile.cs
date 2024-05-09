using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    public string targetTag;
    public float damage;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag.Equals(targetTag))
        {
            collision.gameObject.GetComponent<Health>().Hit(damage);
            Destroy(gameObject);
        }
    }
}
