using System.Collections;
using UnityEngine;

public class ArcanePulseSpell : BaseSpell
{
    public ArcanePulseSpell(SpellCaster owner, BaseSpellInfo baseSpellInfo) : base(owner, baseSpellInfo) {}

    public override IEnumerator Cast(Transform where, Vector3 target, Hittable.Team team, int damage, float speed, string trajectory, bool piercing) {
        last_cast = Time.time;
        this.team = team;
        ProjectileInfo p = baseSpellInfo.projectile;
        int N = (int) RPN.Eval(baseSpellInfo.N, null);
        
        for (int i = 0; i < N; i++) {
            GameManager.Instance.projectileManager.CreateProjectile(p.sprite, p.trajectory, where.position, RotateVector(360f/N*i), RPN.Eval(p.speed, null), piercing, OnHit((int) RPN.Eval(baseSpellInfo.damage.amount, null)), team, RPN.Eval(p.lifetime, null));
        }
        yield return new WaitForEndOfFrame();
    }

    public Vector3 RotateVector(float a) {
        return Quaternion.AngleAxis(a, Vector3.forward) * new Vector3(1, 0, 0);
    }
}
