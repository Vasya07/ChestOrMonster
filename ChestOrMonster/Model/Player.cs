using ChestOrMonster.Interface;
using ChestOrMonster.Model.Item;

namespace ChestOrMonster.Model;

public class Player : BaseEntity
{
    public IWeapon Weapon { get; private set; }
    public IArmor Armor { get; private set; }
    
    public override string Name { get; }
    public override double Hp { get; protected set; } = _maxHp;
    public override double Atk => Weapon.Damage;
    public override double Def => Armor.Def;
    public override DamageType AttackType =>
        Weapon is Bow ? DamageType.Pure : DamageType.Usual;
    public override StatusEffect Effect { get; protected set; }
    
    private static double _maxHp = 100;
    private static double _dodgeChance = 0.4;

    public Player(string name)
    {
        Name = name;
        Weapon = new Weapon("Кулаки", 2);
        Armor = new Armor("Майка", 1);
        Hp = _maxHp;
        Effect = StatusEffect.None;
    }

    public override DamageInfo Attack()
    {
        if (Weapon is IRangedWeapon ranged)
        {
            if (_random.NextDouble() > ranged.Accuracy)
            {
                return new DamageInfo(0, AttackType);
            }
        }
        return new DamageInfo(Weapon.Damage, AttackType);
    }

    public void UseItem(IBaseItem item)
    {
        switch (item)
        {
            case Armor armor:
                Armor = armor;
                break;
            case Bow weapon:
                Weapon = weapon;
                break;
            case Weapon weapon:
                Weapon = weapon;
                break;
            case HealingPotion healingPotion:
                Hp = _maxHp;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(item), item, null);
        }
    }
    
    public bool Dodge()
    {
        if (_random.NextDouble() < _dodgeChance)
        {
            return true;
        }
        return false;
    }
}