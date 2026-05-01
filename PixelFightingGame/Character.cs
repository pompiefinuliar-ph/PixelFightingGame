using System;

namespace PixelFightingGame
{
    public enum Archetype { Striker, Tank, Tactician }
    public enum ElementType { Fire, Water, Earth, Wind, Lightning, None }

    public abstract class Character
    {
        public Guid CharacterID { get; set; }
        public string CharacterName { get; set; }
        public float HealthPoints { get; set; }
        public float MaxHealth { get; set; }
        public float Stamina { get; set; }
        public int AttackPower { get; set; }
        public float DefenseGrade { get; set; }
        public int Agility { get; set; }
        public float CriticalHitRate { get; set; }
        public string SpecialMoveName { get; set; }
        public int SpecialMoveDamage { get; set; }
        public int WinCount { get; set; }
        public int LossCount { get; set; }
        public string CharacterBio { get; set; }

        public bool IsStunned { get; set; }
        public bool IsBlocking { get; set; }
        public ElementType Element { get; set; }
        public int BaseSpeed { get; set; }
        public int Accuracy { get; set; }
        public string SpritePath { get; set; }
        public string IconPath { get; set; } // NEW: Path to the profile icon

        public int CurrentStamina { get; set; }
        public Archetype FighterType { get; set; }

        public string CurrentPose { get; set; } = "Idle";
        public System.Drawing.Image IdleFrame { get; set; }
        public System.Drawing.Image AttackFrame { get; set; }
        public System.Drawing.Image BlockFrame { get; set; }
        public System.Drawing.Image SpecialFrame { get; set; }
        public System.Drawing.Image HurtFrame { get; set; }
        public System.Drawing.Image DefeatFrame { get; set; }

        protected Character(string name, Archetype type, ElementType element, string spritePath, string iconPath, string specialMove)
        {
            this.CharacterID = Guid.NewGuid();
            this.CharacterName = name;
            this.FighterType = type;
            this.Element = element;
            this.SpritePath = spritePath;
            this.IconPath = iconPath; // Set the icon path
            this.SpecialMoveName = specialMove;
            this.WinCount = 0;
            this.LossCount = 0;
            this.IsStunned = false;
            this.IsBlocking = false;
        }

        public float CalculateDamageTo(Character target, Stage currentStage, out bool isCritical, out bool targetEvaded, out float elementalMultiplier, out bool isStageBoosted)
        {
            System.Random rnd = new System.Random();
            isCritical = false;
            targetEvaded = false;
            elementalMultiplier = 1.0f;
            isStageBoosted = false;

            if (rnd.Next(1, 101) > (this.Accuracy - target.Agility))
            {
                targetEvaded = true;
                return 0f;
            }

            float criticalMultiplier = 1.0f;
            if (rnd.NextDouble() < this.CriticalHitRate)
            {
                isCritical = true;
                criticalMultiplier = 1.5f;
            }

            if (this.Element == ElementType.Water && target.Element == ElementType.Fire) elementalMultiplier = 1.5f;
            else if (this.Element == ElementType.Fire && target.Element == ElementType.Wind) elementalMultiplier = 1.5f;
            else if (this.Element == ElementType.Wind && target.Element == ElementType.Earth) elementalMultiplier = 1.5f;
            else if (this.Element == ElementType.Earth && target.Element == ElementType.Lightning) elementalMultiplier = 1.5f;
            else if (this.Element == ElementType.Lightning && target.Element == ElementType.Water) elementalMultiplier = 1.5f;

            float stageBoost = 1.0f;
            if (currentStage != null && this.Element == currentStage.BoostedElement)
            {
                stageBoost = currentStage.BoostMultiplier;
                isStageBoosted = true;
            }

            float baseDamage = this.AttackPower * elementalMultiplier * stageBoost;
            float finalDefense = target.DefenseGrade;
            if (target.IsBlocking) { finalDefense = System.Math.Min(0.99f, finalDefense + 0.3f); }

            float damageReduction = 1.0f - finalDefense;
            float finalDamage = baseDamage * damageReduction * criticalMultiplier;

            return System.Math.Max(1, finalDamage);
        }
    }

    public class Striker : Character
    {
        public Striker(string name, ElementType element, string spritePath, string iconPath, string specialMove) : base(name, Archetype.Striker, element, spritePath, iconPath, specialMove)
        {
            MaxHealth = 80f; Stamina = 100f; AttackPower = 20;
            DefenseGrade = 0.1f; Agility = 15; CriticalHitRate = 0.20f;
            BaseSpeed = 8; Accuracy = 95;
            CharacterBio = "A fast, glass-cannon fighter.";
            HealthPoints = MaxHealth; CurrentStamina = (int)Stamina;
            SpecialMoveDamage = 35;
        }
    }

    public class Tank : Character
    {
        public Tank(string name, ElementType element, string spritePath, string iconPath, string specialMove) : base(name, Archetype.Tank, element, spritePath, iconPath, specialMove)
        {
            MaxHealth = 150f; Stamina = 80f; AttackPower = 12;
            DefenseGrade = 0.4f; Agility = 5; CriticalHitRate = 0.05f;
            BaseSpeed = 3; Accuracy = 85;
            CharacterBio = "A slow, heavily armored protector.";
            HealthPoints = MaxHealth; CurrentStamina = (int)Stamina;
            SpecialMoveDamage = 25;
        }
    }

    public class Tactician : Character
    {
        public Tactician(string name, ElementType element, string spritePath, string iconPath, string specialMove) : base(name, Archetype.Tactician, element, spritePath, iconPath, specialMove)
        {
            MaxHealth = 100f; Stamina = 150f; AttackPower = 15;
            DefenseGrade = 0.2f; Agility = 10; CriticalHitRate = 0.10f;
            BaseSpeed = 6; Accuracy = 100;
            CharacterBio = "A master of elements. Specials can STUN.";
            HealthPoints = MaxHealth; CurrentStamina = (int)Stamina;
            SpecialMoveDamage = 28;
        }
    }
}