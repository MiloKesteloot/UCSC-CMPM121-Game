using System.Collections;
using UnityEngine;

public class ArcaneSpraySpell : BaseSpell
{
    public ArcaneSpraySpell(SpellCaster owner, BaseSpellInfo baseSpellInfo) : base(owner, baseSpellInfo) {}

    public override IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team) {
        this.team = team;
        ProjectileInfo p = baseSpellInfo.projectile;
        int N = (int) RPN.Eval(baseSpellInfo.N, null);
        float angle = RPN.Eval(baseSpellInfo.spray, null);
        for (int i = 0; i < N; i++) {
            GameManager.Instance.projectileManager.CreateProjectile(p.sprite, p.trajectory, where, RotateVector((Random.value*2-1)*angle, target - where), RPN.Eval(p.speed, null), OnHit, RPN.Eval(p.lifetime, null));
        }
        yield return new WaitForEndOfFrame();
    }

    public Vector3 RotateVector(float a, Vector3 v) {
        return Quaternion.AngleAxis(a, Vector3.forward) * v;
    }
}
