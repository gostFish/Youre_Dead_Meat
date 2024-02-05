using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public Item self;
    public Texture icon;
    public int itemType;

    private Transform floatingIcon;
    private Transform itemPool;
    private SpriteRenderer img;

    //[SerializeField] private GameObject widget;

    private void Awake()
    {
        self = this;

        if(transform.childCount > 0)
        {
            floatingIcon = transform.GetChild(0).transform;
            img = floatingIcon.GetComponent<SpriteRenderer>();
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

   private void FixedUpdate()
    {
        if(this.isActiveAndEnabled)
        {
            floatingIcon.position = transform.position + new Vector3(0, Mathf.Sin(Time.fixedTime * Mathf.PI * 0.5f) * 1.5f,0);
        }
    }

    public void SetItemPool(Transform newPool)
    {
        itemPool = newPool;
    }

    public void SetItemIcon(Sprite newSprite)
    {
        img.sprite = newSprite;
    }

    public void ItemFinished()
    {
        transform.parent = itemPool.transform;
        gameObject.SetActive(false);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.layer == 9)
        {
            ItemFinished();
        }        
    }

    public IEnumerator SetExplosionTimer(float explosionForce, float explosionTime)
    {
        yield return new WaitForSeconds(explosionTime);
    }


    //Some code I copied from the internet
    /*public void AddExplosionForce()
    {
        Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, explosionRadius, 8);
        foreach (Collider2D obj in objects)
        {
            Vector2 dir = obj.transform.position - transform.position;
            obj.GetComponent<Rigidbody2D>().AddForce(dir * explosionForce);
            Debug.Log("Should have applied force to " + obj.ToString() + " with force (" + dir + " * " + explosionForce + ") ");
        }
        Collider2D[] players = Physics2D.OverlapCircleAll(transform.position, explosionRadius, 7);
        foreach (Collider2D obj in players)
        {
            Vector2 dir = obj.transform.position - transform.position;
            if(obj.GetComponent<Rigidbody2D>() != null)
            {
                obj.GetComponent<Rigidbody2D>().AddForce(dir * explosionForce);
            }
        }

    }*/

    /*private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }*/

}
