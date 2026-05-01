using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace PixelFightingGame
{
    public enum GameState { SelectionState, InitializeState, PlayerTurn, AITurn, AnimationState, ResolutionState }
    public enum AIAction { Attack, Special, Block, None }

    public class GameManager
    {
        public GameState CurrentState { get; set; }
        public Character PlayerFighter { get; set; }
        public Character AIFighter { get; set; }
        public Character ActiveFighter { get; set; }

        public List<Character> CharacterRoster { get; set; }
        public List<Stage> StageRoster { get; set; }
        public Stage CurrentArena { get; set; }

        public event Action<string, Color> OnLogMessage;

        private Timer _gameLoopTimer;
        private Random rng = new Random();
        private string saveFilePath = "fighter_stats.txt";

        public GameManager(Timer loop)
        {
            _gameLoopTimer = loop;
            CurrentState = GameState.SelectionState;
            CharacterRoster = new List<Character>();
            StageRoster = new List<Stage>();
            InitializeRoster();
            LoadStats();
            InitializeStages();
        }

        private void InitializeRoster()
        {
            CharacterRoster.Add(new Striker("Kenji", ElementType.Wind, "Assets/kenji.png", "Assets/kenji_icon.png", "Zephyr Strike"));
            CharacterRoster.Add(new Striker("Ryu", ElementType.Fire, "Assets/ryu.png", "Assets/ryu_icon.png", "Dragon's Breath"));
            CharacterRoster.Add(new Striker("Akira", ElementType.Lightning, "Assets/akira.png", "Assets/akira_icon.png", "Thunderclap Dash"));
            CharacterRoster.Add(new Striker("Sakura", ElementType.Water, "Assets/sakura.png", "Assets/sakura_icon.png", "Azure Ripple"));
            CharacterRoster.Add(new Tank("Goliath", ElementType.Earth, "Assets/goliath.png", "Assets/goliath_icon.png", "Tectonic Slam"));
            CharacterRoster.Add(new Tank("Ironclad", ElementType.Earth, "Assets/ironclad.png", "Assets/ironclad_icon.png", "Fortress Crash"));
            CharacterRoster.Add(new Tank("Sentinel", ElementType.None, "Assets/sentinel.png", "Assets/sentinel_icon.png", "Absolute Bastion"));
            CharacterRoster.Add(new Tactician("Gaia", ElementType.Earth, "Assets/gaia.png", "Assets/gaia_icon.png", "Nature's Wrath"));
            CharacterRoster.Add(new Tactician("Frostbyte", ElementType.Water, "Assets/frostbyte.png", "Assets/frostbyte_icon.png", "Zero-Point Freeze"));
            CharacterRoster.Add(new Tactician("Static", ElementType.Lightning, "Assets/static.png", "Assets/static_icon.png", "Ion Storm"));
        }

        private void InitializeStages()
        {
            // --- UPDATED: Added the image paths for the rolling stage preview ---
            StageRoster.Add(new Stage("Volcano Crater", ElementType.Fire, 1.25f, Color.DarkRed, "Assets/stage_volcano.png"));
            StageRoster.Add(new Stage("Ocean Ruins", ElementType.Water, 1.25f, Color.Navy, "Assets/stage_ocean.png"));
            StageRoster.Add(new Stage("Sky Temple", ElementType.Wind, 1.25f, Color.LightSkyBlue, "Assets/stage_sky.png"));
            StageRoster.Add(new Stage("Earthquake Fault", ElementType.Earth, 1.25f, Color.SaddleBrown, "Assets/stage_earth.png"));
            StageRoster.Add(new Stage("Power Plant", ElementType.Lightning, 1.25f, Color.DarkSlateBlue, "Assets/stage_power.png"));
        }

        public Character GetRandomEnemy(Guid playerID)
        {
            var availableEnemies = CharacterRoster.Where(c => c.CharacterID != playerID).ToList();
            return availableEnemies[rng.Next(availableEnemies.Count)];
        }

        public void AnnounceStageBoosts()
        {
            bool anyoneBoosted = false;
            if (PlayerFighter.Element == CurrentArena.BoostedElement)
            {
                LogMessage($"{PlayerFighter.CharacterName} feels the power of the {CurrentArena.StageName}! (DAMAGE BOOSTED)", Color.Gold);
                anyoneBoosted = true;
            }
            if (AIFighter.Element == CurrentArena.BoostedElement)
            {
                LogMessage($"{AIFighter.CharacterName} is empowered by the {CurrentArena.StageName}! (DAMAGE BOOSTED)", Color.Gold);
                anyoneBoosted = true;
            }
            if (!anyoneBoosted) LogMessage("The environment is neutral for both fighters.", Color.Gray);
        }

        public void LoadStats()
        {
            if (!File.Exists(saveFilePath)) return;
            try
            {
                string[] lines = File.ReadAllLines(saveFilePath);
                foreach (string line in lines)
                {
                    string[] parts = line.Split(',');
                    if (parts.Length == 3)
                    {
                        Character c = CharacterRoster.Find(x => x.CharacterName == parts[0]);
                        if (c != null) { c.WinCount = int.Parse(parts[1]); c.LossCount = int.Parse(parts[2]); }
                    }
                }
            }
            catch (Exception ex) { LogMessage("Error loading save data: " + ex.Message, Color.Red); }
        }

        public void SaveStats()
        {
            try
            {
                List<string> lines = new List<string>();
                foreach (Character c in CharacterRoster) lines.Add($"{c.CharacterName},{c.WinCount},{c.LossCount}");
                File.WriteAllLines(saveFilePath, lines);
            }
            catch (Exception ex) { LogMessage("Error saving data: " + ex.Message, Color.Red); }
        }

        public void LogMessage(string message, Color color) { OnLogMessage?.Invoke(message, color); }

        private string GetSpecialMoveFlavorText(Character attacker)
        {
            switch (attacker.Element)
            {
                case ElementType.Fire: return attacker.FighterType == Archetype.Striker ? "A blur of searing heat strikes with blinding speed!" : attacker.FighterType == Archetype.Tank ? "A massive eruption of molten lava consumes the area!" : "A swirling vortex of inferno traps the opponent!";
                case ElementType.Water: return attacker.FighterType == Archetype.Striker ? "A crashing wave of pressure slams into the target!" : attacker.FighterType == Archetype.Tank ? "The crushing weight of the deep ocean descends!" : "A freezing chill locks the enemy in a glacial prison!";
                case ElementType.Earth: return attacker.FighterType == Archetype.Striker ? "A sudden shatter of stone strikes with precision!" : attacker.FighterType == Archetype.Tank ? "The ground trembles as a mountain-sized blow falls!" : "Jagged stone pillars erupt from the earth!";
                case ElementType.Wind: return attacker.FighterType == Archetype.Striker ? "A whirlwind of blades slices through the air!" : attacker.FighterType == Archetype.Tank ? "A violent hurricane blast pushes the enemy back!" : "A void vacuum sucks the air out of the arena!";
                case ElementType.Lightning: return attacker.FighterType == Archetype.Striker ? "An electric dash connects in a flash of light!" : attacker.FighterType == Archetype.Tank ? "A deafening thunderclap shakes the very ground!" : "Arcing bolts of plasma chain through the target!";
                default: return "A powerful surge of energy hits the opponent!";
            }
        }

        public AIAction DecideAITurn()
        {
            if (AIFighter.IsStunned) return AIAction.None;
            float estStd = AIFighter.AttackPower * (1.0f - PlayerFighter.DefenseGrade);
            float estSpec = AIFighter.SpecialMoveDamage * (1.0f - PlayerFighter.DefenseGrade);
            if (PlayerFighter.HealthPoints <= estSpec && AIFighter.CurrentStamina >= 50) return AIAction.Special;
            if (PlayerFighter.HealthPoints <= estStd && AIFighter.CurrentStamina >= 10) return AIAction.Attack;
            if (AIFighter.HealthPoints < (AIFighter.MaxHealth * 0.3f) && AIFighter.CurrentStamina < 40) return AIAction.Block;
            if (PlayerFighter.IsBlocking) return (AIFighter.CurrentStamina < 60) ? AIAction.Block : AIAction.Attack;
            if (AIFighter.CurrentStamina >= 70) return AIAction.Special;
            if (AIFighter.CurrentStamina >= 20) return AIAction.Attack;
            return AIAction.Block;
        }

        public void ExecuteAITurn(AIAction action)
        {
            if (action != AIAction.Block) AIFighter.IsBlocking = false;
            switch (action)
            {
                case AIAction.Attack: ExecuteStandardAttack(AIFighter, PlayerFighter); break;
                case AIAction.Special: ExecuteSpecialAttack(AIFighter, PlayerFighter); break;
                case AIAction.Block: RecoverStaminaAndBlock(); break;
                case AIAction.None: LogMessage($"{AIFighter.CharacterName} is STUNNED and skips their turn.", Color.Cyan); AIFighter.IsStunned = false; break;
            }
            FinalizeTurn();
        }

        private void RecoverStaminaAndBlock()
        {
            AIFighter.CurrentStamina = Math.Min((int)AIFighter.Stamina, AIFighter.CurrentStamina + 30);
            AIFighter.IsBlocking = true;
            LogMessage($"{AIFighter.CharacterName} takes a defensive stance and recovers stamina!", Color.IndianRed);
        }

        public void ExecuteStandardAttack(Character attacker, Character target)
        {
            attacker.IsStunned = false;
            attacker.CurrentStamina -= 10;
            bool isCrit, isEvaded, isStageBoosted;
            float elementalMult;
            float damage = attacker.CalculateDamageTo(target, CurrentArena, out isCrit, out isEvaded, out elementalMult, out isStageBoosted);
            string logMsg = $"{attacker.CharacterName} attacks!";
            Color logColor = (attacker == PlayerFighter) ? Color.LimeGreen : Color.Red;
            if (isEvaded) { logMsg += $" MISS! {target.CharacterName} dodged."; logColor = Color.Gray; }
            else
            {
                if (isCrit) { logMsg += " CRITICAL HIT!"; logColor = Color.Yellow; }
                if (elementalMult > 1.0f) { logMsg += " It's super effective!"; logColor = Color.Cyan; }
                if (isStageBoosted) { logMsg += $" Damage Boost Active in {CurrentArena.StageName}!"; logColor = Color.Gold; }
                logMsg += $" Deals {damage:0} damage.";
                target.HealthPoints = Math.Max(0, target.HealthPoints - damage);
            }
            LogMessage(logMsg, logColor);
        }

        public void ExecuteSpecialAttack(Character attacker, Character target)
        {
            attacker.CurrentStamina -= 50;
            LogMessage($"{attacker.CharacterName} unleashes {attacker.SpecialMoveName}!", Color.Yellow);
            LogMessage(GetSpecialMoveFlavorText(attacker), Color.White);
            float damage = attacker.SpecialMoveDamage * (1.0f - target.DefenseGrade);
            target.HealthPoints = Math.Max(0, target.HealthPoints - damage);
            LogMessage($"The impact deals {damage:0} massive damage!", Color.Yellow);
            if (attacker is Tactician && rng.Next(1, 101) <= 30)
            {
                target.IsStunned = true;
                LogMessage($"{target.CharacterName} is STUNNED by the elemental blast!", Color.Cyan);
            }
        }

        public void FinalizeTurn() { this.CurrentState = GameState.ResolutionState; }

        public void CheckCombatResolution()
        {
            if (PlayerFighter.HealthPoints <= 0 || AIFighter.HealthPoints <= 0)
            {
                Character masterPlayer = CharacterRoster.Find(c => c.CharacterID == PlayerFighter.CharacterID);
                Character masterAI = CharacterRoster.Find(c => c.CharacterID == AIFighter.CharacterID);
                if (PlayerFighter.HealthPoints > 0) { masterPlayer.WinCount++; masterAI.LossCount++; }
                else { masterAI.WinCount++; masterPlayer.LossCount++; }
                SaveStats();
                return;
            }
            if (this.ActiveFighter == PlayerFighter) { this.ActiveFighter = AIFighter; this.CurrentState = GameState.AITurn; this.PlayerFighter.IsBlocking = false; }
            else { this.ActiveFighter = PlayerFighter; this.CurrentState = GameState.PlayerTurn; this.AIFighter.IsBlocking = false; }
        }
    }
}