using ChestOrMonster.Interface;

namespace ChestOrMonster.Model.Enemy;

public class AntiFlag : BaseEntity
{
    public override string Name { get; }
    public override double Hp { get; protected set; }
    public override double Atk { get; }
    public override double Def { get; }
    public override DamageType AttackType { get; }
    public override StatusEffect Effect { get; protected set; }
    protected double StoredDamage { get; set; } = 0;
    protected virtual double ReflectRatio { get; } = 0.5;
    protected virtual double MaxStoredDamage { get; } = 15;
    protected virtual double CritRate { get; }
    protected virtual double SelfDamageRatio { get; } = 0.5;
    protected virtual double MaxSelfDamage { get; } = 3;

    public AntiFlag()
    {
        Name = "АнтиФлаг";
        Hp = 12;
        Atk = 6;
        Def = 1;
        AttackType = DamageType.Usual;
        Effect = StatusEffect.None;
        CritRate = 0.1;
    }

    public override DamageInfo TakeDamage(DamageInfo damage)
    {
        DamageInfo actual = base.TakeDamage(damage);
        if (actual.Type != DamageType.Pure && actual.Amount > 0)
        {
            StoredDamage = Math.Min(
                StoredDamage + actual.Amount * ReflectRatio,
                MaxStoredDamage);
        }

        return actual;
    }

    public override DamageInfo Attack()
    {
        double reflectedDamage = StoredDamage;
        StoredDamage = 0;

        double damage = Atk + reflectedDamage;

        if (_random.NextDouble() < CritRate)
        {
            damage += 5.0;
        }
        // АнтиФлаг тратит силы на разворот урона — теряет 50% отражённого, но не больше 3, чтобы враг не умер слишком быстро
        Hp = Math.Max(0, Hp - Math.Min(reflectedDamage * SelfDamageRatio, MaxSelfDamage));

        return new DamageInfo(damage, AttackType);
    }
}