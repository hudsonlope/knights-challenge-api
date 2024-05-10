using System;
using System.Collections.Generic;

namespace Knights.Challenge.Domain.Entities
{
    public class KnightEntity : BaseEntity
    {
        private DateTime _birthday = new DateTime();

        public string Name { get; set; }
        public string Nickname { get; set; }
        public DateTime Birthday
        {
            get => _birthday;
            set => _birthday = value.Date;
        }
        public List<Weapon> Weapons { get; set; }
        public bool Deleted { get; set; }
        public int Age => CalculateAge();
        public int Attack => CalculateAttack();
        public long Experience => CalculateExperience();
        public Attributes Attributes { get; set; }
        public string KeyAttribute { get; set; }

        private int CalculateAge()
        {
            var today = DateTime.Today;
            var age = today.Year - Birthday.Year;
            if (Birthday > today.AddYears(-age))
                age--;
            return age;
        }

        private int CalculateAttack()
        {
            var keyAttributeModifier = GetAttributeModifier(Attributes.GetValueByKey(KeyAttribute));
            var weaponModifier = CalculateWeaponModifier();
            return 10 + keyAttributeModifier + weaponModifier;
        }

        private long CalculateExperience()
        {
            if (Age < 7)
                return 0;
            return (long)Math.Floor((Age - 7) * Math.Pow(22, 1.45));
        }

        private int GetAttributeModifier(int value)
        {
            if (value >= 0 && value <= 8) return -2;
            if (value >= 9 && value <= 10) return -1;
            if (value >= 11 && value <= 12) return 0;
            if (value >= 13 && value <= 15) return 1;
            if (value >= 16 && value <= 18) return 2;
            if (value >= 19 && value <= 20) return 3;
            return 0;
        }

        private int CalculateWeaponModifier()
        {
            var weaponModifier = 0;
            foreach (var weapon in Weapons)
            {
                weaponModifier += weapon.Mod;
            }
            return weaponModifier;
        }
    }

    public class Weapon
    {
        public string Name { get; set; }
        public int Mod { get; set; }
        public string Attr { get; set; }
        public bool Equipped { get; set; }
    }

    public class Attributes
    {
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Constitution { get; set; }
        public int Intelligence { get; set; }
        public int Wisdom { get; set; }
        public int Charisma { get; set; }

        public int GetValueByKey(string key)
        {
            return key switch
            {
                "strength" => Strength,
                "dexterity" => Dexterity,
                "constitution" => Constitution,
                "intelligence" => Intelligence,
                "wisdom" => Wisdom,
                "charisma" => Charisma,
                _ => 0,
            };
        }
    }
}
