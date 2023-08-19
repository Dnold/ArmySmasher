using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngineInternal;

public class Fireball : Projectile
{
    public float damageRange;
    public int minimalDamage;

    public GameObject ExplosionSFX;
    public override void DoDamage(Soldier sol)
    {
        StartCoroutine(SpawnExplosionSFX());
        Soldier[] soldiers = GetEveryEnemyInRadius(damageRange);
        if (soldiers.Length > 0)
        {
            foreach (Soldier s in soldiers)
            {
                float distance = Vector2.Distance(transform.position, s.transform.position);
                int damage = GetDamage(distance, minimalDamage, soldier.damage, 0.4f, damageRange);
                s.SubHealth(damage);

            }
        }
    }
    int GetDamage(float bulletDistance, int minDamage, int maxDamage, float dropOffStart, float dropOffEnd)
    {
        // Use the min/max damage if we're outside the drop off range
        if (bulletDistance <= dropOffStart) return (int)maxDamage;
        if (bulletDistance >= dropOffEnd) return (int)minDamage;

        // Where in the drop off range is the distance we are interested in?
        // We want this in the range of 0 at the start to 1 at the end so we can use Lerp
        float dropOffRange = dropOffEnd - dropOffStart;

        float distanceNormalised = (bulletDistance - dropOffStart) / dropOffRange;

        return (int)Mathf.Lerp(maxDamage, minDamage, distanceNormalised);
    }
    IEnumerator SpawnExplosionSFX()
    {
        GameObject sfx = Instantiate(ExplosionSFX, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(1f);
        Destroy(sfx);

    }

}
