using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ProjectileController : MonoBehaviour
{
    public float lifetime;
    public event Action<Hittable,Vector3> OnHit;
    public ProjectileMovement movement;
    public bool piercing;
    public List<object> collisions;
    public Hittable.Team team;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        collisions = new();
    }

    // Update is called once per frame
    void Update()
    {
        movement.Movement(transform);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("projectile")) return;
        if (collision.gameObject.CompareTag("unit"))
        {
            bool hit = false;

            var ec = collision.gameObject.GetComponent<EnemyController>();
            if (ec != null)
            {
                // if (collisions.Contains(ec)) return;
                OnHit(ec.hp, transform.position);
                // collisions.Add(ec);
                hit = true;

                Physics2D.IgnoreCollision(
                    ec.GetComponent<Collider2D>(), // Enemy's collider
                    GetComponent<Collider2D>() // Projectile's collider
                );
            }
            else
            {
                var pc = collision.gameObject.GetComponent<PlayerController>();
                if (pc != null)
                {
                    // if (collisions.Contains(pc)) return;
                    OnHit(pc.hp, transform.position);
                    // collisions.Add(pc);
                    hit = true;
                    
                    Physics2D.IgnoreCollision(
                        pc.GetComponent<Collider2D>(), // Enemy's collider
                        GetComponent<Collider2D>() // Projectile's collider
                    );
                }
            }
            if (hit && !piercing) {
                Destroy(gameObject);
            }
            return;
        }

        if (this.team == Hittable.Team.PLAYER)
        {
            EventBus.Instance.DoMiss();
        }

        Destroy(gameObject);
    }

    public void SetLifetime(float lifetime)
    {
        StartCoroutine(Expire(lifetime));
    }

    IEnumerator Expire(float lifetime)
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}