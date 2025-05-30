using System.Collections;
using UnityEngine;

public class ArcaneSpraySpell : BaseSpell
{
    public ArcaneSpraySpell(SpellCaster owner, BaseSpellInfo baseSpellInfo) : base(owner, baseSpellInfo) {}

    public override IEnumerator Cast(Transform where, Vector3 target, Hittable.Team team, int damage, float speed, string trajectory, bool piercing) {
        last_cast = Time.time;
        this.team = team;
        ProjectileInfo p = baseSpellInfo.projectile;
        int N = (int) RPN.Eval(baseSpellInfo.N, null);
        float angle = RPN.Eval(baseSpellInfo.spray, null);
        for (int i = 0; i < N; i++) {
            GameManager.Instance.projectileManager.CreateProjectile(p.sprite, trajectory, where.position, RotateVector((Random.value*2-1)*angle, target - where.position), RPN.Eval(p.speed, null), piercing, OnHit(damage), team, RPN.Eval(p.lifetime, null));
        }
        yield return new WaitForEndOfFrame();
    }

    public Vector3 RotateVector(float a, Vector3 v) {
        return Quaternion.AngleAxis(a, Vector3.forward) * v;
    }
}
