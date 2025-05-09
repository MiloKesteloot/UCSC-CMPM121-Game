using System;
using UnityEngine;

public class ArcaneBlastSpell : BaseSpell
{
    public ArcaneBlastSpell(SpellCaster owner, BaseSpellInfo baseSpellInfo) : base(owner, baseSpellInfo) {}

    public override Action<Hittable,Vector3> OnHit(int damage) {
        void OnHit(Hittable other, Vector3 impact) {
            if (other.team != team) {
                other.Damage(new Damage(damage, Damage.Type.ARCANE)); // TODO make damageType accurate
            }

            ProjectileInfo p = baseSpellInfo.secondaryProjectile;
            int N = (int) RPN.Eval(baseSpellInfo.N, null);
            for (int i = 0; i < N; i++) {
                GameManager.Instance.projectileManager.CreateProjectile(p.sprite, p.trajectory, impact, RotateVector(360f/N*i), RPN.Eval(p.speed, null), base.OnHit((int) RPN.Eval(baseSpellInfo.secondaryDamage, null)), RPN.Eval(p.lifetime, null));
                // TODO should modifiers affect secondary bullets?
            }
        }
        return OnHit;
    }

    public Vector3 RotateVector(float a) {
        return Quaternion.AngleAxis(a, Vector3.forward) * new Vector3(1, 0, 0);
    }
}
