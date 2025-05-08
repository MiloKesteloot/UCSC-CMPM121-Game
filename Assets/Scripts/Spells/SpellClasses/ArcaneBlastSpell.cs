using UnityEngine;

public class ArcaneBlastSpell : BaseSpell
{
    public ArcaneBlastSpell(SpellCaster owner, BaseSpellInfo baseSpellInfo) : base(owner, baseSpellInfo) {}

    public override void OnHit(Hittable other, Vector3 impact)
    {
        base.OnHit(other, impact);

        ProjectileInfo p = baseSpellInfo.secondaryProjectile;
        int N = (int) RPN.Eval(baseSpellInfo.N, null);
        for (int i = 0; i < N; i++) {
            GameManager.Instance.projectileManager.CreateProjectile(p.sprite, p.trajectory, impact, RotateVector(360f/N*i), RPN.Eval(p.speed, null), base.OnHit, RPN.Eval(p.lifetime, null));
        }
        // GameManager.Instance.projectileManager.CreateProjectile(0, "straight", where, target - where, 15f, OnHit);
    }

    public Vector3 RotateVector(float a) {
        return Quaternion.AngleAxis(a, Vector3.forward) * new Vector3(1, 0, 0);
    }
}
