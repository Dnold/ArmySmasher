using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Soldier soldier;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Obstacle")
        {
            Destroy(gameObject);
        }
        Soldier tempSoldier = collision.GetComponent<Soldier>();
        if (tempSoldier != null && tempSoldier.id != soldier.id && tempSoldier.player != soldier.player)
        {

            DoDamage(collision.GetComponent<Soldier>());
            Destroy(gameObject);
        }

    }
    public virtual void DoDamage(Soldier sol)
    {
        sol.SubHealth(soldier.damage);
    }
    public Soldier[] GetEveryEnemyInRadius(float radius)
    {
        Soldier[] allSoldiersInRadius = Physics2D.OverlapCircleAll(transform.position, radius, soldier.soldier)
              .Select(e => e.GetComponent<Soldier>())
              .ToArray();


        return allSoldiersInRadius
                .Where(e => e.player != soldier.player)
                .ToArray();
    }
}
