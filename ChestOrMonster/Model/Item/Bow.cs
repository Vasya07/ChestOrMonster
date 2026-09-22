using ChestOrMonster.Interface;

namespace ChestOrMonster.Model.Item;

public class Bow : IRangedWeapon
{
    public string Name { get; private set; }
    public double Damage { get; private set; }
    public double Accuracy { get; private set; }

    public Bow(string name, double damage, double accuracy)
    {
        Name = name;
        Damage = damage;
        Accuracy = accuracy;
    }
}