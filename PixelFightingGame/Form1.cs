using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PixelFightingGame
{
    public partial class Form1 : Form
    {
        private GameManager _game;
        private ToolTip _statToolTip;
        private Label lblAISelection;

        private Panel pnlPreview;
        private PictureBox pbPlayerPreview, pbAIPreview;
        private Label lblPlayerStats, lblAIStats;
        private Button btnProceedToStage;
        private Button btnLockSelection;

        private PictureBox pbPlayerIconStage, pbAIIconStage;
        private PictureBox pbPlayerIconBattle, pbAIIconBattle;

        private Label lblPlayerNameStage, lblAINameStage, lblVS;

        private int _playerDrawX, _aiDrawX;
        private int _basePlayerX, _baseAIX;

        private Guid _previewedCharacterId = Guid.Empty;

        public Form1()
        {
            InitializeComponent();

            this.AutoScaleMode = AutoScaleMode.None;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.ClientSize = new Size(1258, 685);

            pnlSelectScreen.Dock = DockStyle.Fill;
            pnlStageScreen.Dock = DockStyle.Fill;
            pnlBattleScreen.Dock = DockStyle.Fill;

            this.DoubleBuffered = true;

            SetupAISelectionLabel();
            SetupPreviewPanel();
            SetupStageLayout();
            SetupBattleLayout();

            EnableDoubleBuffering(playerHealthPanel);
            EnableDoubleBuffering(aiHealthPanel);
            if (playerStaminaPanel != null) EnableDoubleBuffering(playerStaminaPanel);
            if (aiStaminaPanel != null) EnableDoubleBuffering(aiStaminaPanel);
            EnableDoubleBuffering(pnlBattleScreen);

            btnStartFight.Click += btnStartFight_Click;
            btnAttack.Click += btnAttack_Click;
            btnBlock.Click += btnBlock_Click;
            btnSpecial.Click += btnSpecial_Click;
            gameLoopTimer.Tick += gameLoopTimer_Tick;

            _statToolTip = new ToolTip();
            _statToolTip.InitialDelay = 200;
            _statToolTip.AutoPopDelay = 10000;
            _statToolTip.ReshowDelay = 100;

            _game = new GameManager(gameLoopTimer);
            _game.OnLogMessage += UpdateCombatLog;

            ShowScreen(pnlSelectScreen);
            PopulateSelectionGrid();
        }

        private void SetupAISelectionLabel()
        {
            lblAISelection = new Label();
            lblAISelection.AutoSize = false;
            lblAISelection.Size = new Size(700, 120);
            lblAISelection.Location = new Point((pnlSelectScreen.Width - 700) / 2, (pnlSelectScreen.Height - 120) / 2);
            lblAISelection.Anchor = AnchorStyles.None;
            lblAISelection.TextAlign = ContentAlignment.MiddleCenter;
            lblAISelection.Font = new Font("Consolas", 26, FontStyle.Bold);
            lblAISelection.ForeColor = Color.Yellow;
            lblAISelection.BackColor = Color.Black;
            lblAISelection.BorderStyle = BorderStyle.Fixed3D;
            lblAISelection.Visible = false;
            pnlSelectScreen.Controls.Add(lblAISelection);
            lblAISelection.BringToFront();
        }

        private void SetupPreviewPanel()
        {
            pnlPreview = new Panel
            {
                Size = new Size(pnlSelectScreen.Width, 250),
                Location = new Point(0, pnlSelectScreen.Height - 250),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                BackColor = Color.FromArgb(40, 40, 40),
                BorderStyle = BorderStyle.FixedSingle
            };

            pbPlayerPreview = new PictureBox
            {
                Size = new Size(150, 150),
                Location = new Point(50, 20),
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.DimGray
            };

            lblPlayerStats = new Label
            {
                Size = new Size(300, 150),
                Location = new Point(210, 20),
                ForeColor = Color.White,
                Font = new Font("Consolas", 11, FontStyle.Regular),
                Text = "Select a character..."
            };

            pbAIPreview = new PictureBox
            {
                Size = new Size(150, 150),
                Location = new Point(pnlPreview.Width - 200, 20),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.DimGray
            };

            lblAIStats = new Label
            {
                Size = new Size(300, 150),
                Location = new Point(pnlPreview.Width - 520, 20),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                ForeColor = Color.White,
                Font = new Font("Consolas", 11, FontStyle.Regular),
                Text = "Waiting for player...",
                TextAlign = ContentAlignment.TopRight
            };

            btnLockSelection = new Button
            {
                Text = "LOCK IN",
                Size = new Size(200, 60),
                Location = new Point((pnlPreview.Width - 200) / 2, 130),
                Anchor = AnchorStyles.Top,
                Font = new Font("Consolas", 16, FontStyle.Bold),
                BackColor = Color.LimeGreen,
                Visible = false,
                FlatStyle = FlatStyle.Flat,
                TabStop = false
            };
            btnLockSelection.FlatAppearance.BorderSize = 0;
            btnLockSelection.Click += BtnLockSelection_Click;

            btnProceedToStage = new Button
            {
                Text = "PROCEED TO STAGE",
                Size = new Size(200, 60),
                Location = new Point((pnlPreview.Width - 200) / 2, 130),
                Anchor = AnchorStyles.Top,
                Font = new Font("Consolas", 14, FontStyle.Bold),
                BackColor = Color.Gold,
                Visible = false,
                FlatStyle = FlatStyle.Flat,
                TabStop = false
            };
            btnProceedToStage.FlatAppearance.BorderSize = 0;
            btnProceedToStage.Click += BtnProceedToStage_Click;

            pnlPreview.Controls.Add(pbPlayerPreview);
            pnlPreview.Controls.Add(lblPlayerStats);
            pnlPreview.Controls.Add(pbAIPreview);
            pnlPreview.Controls.Add(lblAIStats);
            pnlPreview.Controls.Add(btnLockSelection);
            pnlPreview.Controls.Add(btnProceedToStage);

            pnlSelectScreen.Controls.Add(pnlPreview);
            pnlPreview.BringToFront();
        }

        private void ResetSelectionScreen()
        {
            _game.CurrentState = GameState.SelectionState;
            _previewedCharacterId = Guid.Empty;

            pbPlayerPreview.Image = null;
            pbAIPreview.Image = null;
            lblPlayerStats.Text = "Select a character...";
            lblAIStats.Text = "Waiting for player...";
            lblAISelection.Visible = false;

            btnLockSelection.Visible = false;
            btnProceedToStage.Visible = false;
            btnProceedToStage.Enabled = true;

            selectionGrid.Enabled = true;

            foreach (Control ctrl in selectionGrid.Controls)
            {
                if (ctrl is Panel p) p.BorderStyle = BorderStyle.None;
            }
        }

        private void SetupStageLayout()
        {
            int centerX = pnlStageScreen.Width / 2;

            pnlStageScreen.BackgroundImageLayout = ImageLayout.Stretch;

            lblVS = new Label { Text = "VS", Font = new Font("Consolas", 48, FontStyle.Bold | FontStyle.Italic), ForeColor = Color.Red, AutoSize = true, BackColor = Color.Transparent };
            pnlStageScreen.Controls.Add(lblVS);
            lblVS.Location = new Point(centerX - 45, 120);

            pbPlayerIconStage = new PictureBox { Size = new Size(180, 180), SizeMode = PictureBoxSizeMode.StretchImage, BackColor = Color.DimGray, BorderStyle = BorderStyle.FixedSingle };
            pbPlayerIconStage.Location = new Point(centerX - 300 - 180, 80);
            pnlStageScreen.Controls.Add(pbPlayerIconStage);

            lblPlayerNameStage = new Label { Size = new Size(180, 40), Font = new Font("Consolas", 20, FontStyle.Bold), ForeColor = Color.White, TextAlign = ContentAlignment.MiddleCenter, BackColor = Color.Transparent };
            lblPlayerNameStage.Location = new Point(pbPlayerIconStage.Left, pbPlayerIconStage.Bottom + 10);
            pnlStageScreen.Controls.Add(lblPlayerNameStage);

            pbAIIconStage = new PictureBox { Size = new Size(180, 180), SizeMode = PictureBoxSizeMode.StretchImage, BackColor = Color.DimGray, BorderStyle = BorderStyle.FixedSingle };
            pbAIIconStage.Location = new Point(centerX + 300, 80);
            pnlStageScreen.Controls.Add(pbAIIconStage);

            lblAINameStage = new Label { Size = new Size(180, 40), Font = new Font("Consolas", 20, FontStyle.Bold), ForeColor = Color.White, TextAlign = ContentAlignment.MiddleCenter, BackColor = Color.Transparent };
            lblAINameStage.Location = new Point(pbAIIconStage.Left, pbAIIconStage.Bottom + 10);
            pnlStageScreen.Controls.Add(lblAINameStage);

            lblStageReveal.AutoSize = false;
            lblStageReveal.Size = new Size(pnlStageScreen.Width, 60);
            lblStageReveal.Location = new Point(0, pnlStageScreen.Height - 200);
            lblStageReveal.TextAlign = ContentAlignment.MiddleCenter;
            lblStageReveal.Font = new Font("Consolas", 28, FontStyle.Bold);
            lblStageReveal.ForeColor = Color.Gold;
            lblStageReveal.BackColor = Color.Transparent;

            btnStartFight.Size = new Size(350, 70);
            btnStartFight.Location = new Point(centerX - 175, pnlStageScreen.Height - 120);
            btnStartFight.Font = new Font("Consolas", 24, FontStyle.Bold);
            btnStartFight.Enabled = false;
            btnStartFight.FlatStyle = FlatStyle.Flat;
            btnStartFight.TabStop = false;
            btnStartFight.BackColor = Color.White;
            btnStartFight.FlatAppearance.BorderSize = 0;

            lblStageReveal.BringToFront();
            lblVS.BringToFront();
            lblPlayerNameStage.BringToFront();
            lblAINameStage.BringToFront();
            btnStartFight.BringToFront();
        }

        private void SetupBattleLayout()
        {
            int margin = 30;

            pbPlayerIconBattle = new PictureBox { Size = new Size(80, 80), Location = new Point(margin, margin), SizeMode = PictureBoxSizeMode.StretchImage, BackColor = Color.DimGray, BorderStyle = BorderStyle.FixedSingle };
            pnlBattleScreen.Controls.Add(pbPlayerIconBattle);

            playerHealthPanel.Size = new Size(400, 45);
            playerHealthPanel.Location = new Point(pbPlayerIconBattle.Right + 15, margin);

            playerStaminaPanel.Size = new Size(400, 20);
            playerStaminaPanel.Location = new Point(pbPlayerIconBattle.Right + 15, playerHealthPanel.Bottom + 5);

            pbAIIconBattle = new PictureBox { Size = new Size(80, 80), Location = new Point(pnlBattleScreen.Width - margin - 80, margin), SizeMode = PictureBoxSizeMode.StretchImage, BackColor = Color.DimGray, BorderStyle = BorderStyle.FixedSingle };
            pbAIIconBattle.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pnlBattleScreen.Controls.Add(pbAIIconBattle);

            aiHealthPanel.Size = new Size(400, 45);
            aiHealthPanel.Location = new Point(pbAIIconBattle.Left - 15 - 400, margin);
            aiHealthPanel.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            aiStaminaPanel.Size = new Size(400, 20);
            aiStaminaPanel.Location = new Point(pbAIIconBattle.Left - 15 - 400, aiHealthPanel.Bottom + 5);
            aiStaminaPanel.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            combatLogRtb.Size = new Size(600, 160);
            combatLogRtb.Location = new Point((pnlBattleScreen.Width - 600) / 2, pnlBattleScreen.Height - 190);
            combatLogRtb.Anchor = AnchorStyles.Bottom;

            int btnWidth = 180;
            int btnHeight = 45;
            int btnX = combatLogRtb.Left - btnWidth - 70;
            int btnStartY = combatLogRtb.Top + 5;

            btnAttack.Size = new Size(btnWidth, btnHeight);
            btnAttack.Location = new Point(btnX, btnStartY);
            btnAttack.Font = new Font("Consolas", 14, FontStyle.Bold);
            btnAttack.FlatStyle = FlatStyle.Flat; btnAttack.TabStop = false; btnAttack.FlatAppearance.BorderSize = 1; btnAttack.BackColor = Color.LightGray;

            btnBlock.Size = new Size(btnWidth, btnHeight);
            btnBlock.Location = new Point(btnX, btnStartY + btnHeight + 10);
            btnBlock.Font = new Font("Consolas", 14, FontStyle.Bold);
            btnBlock.FlatStyle = FlatStyle.Flat; btnBlock.TabStop = false; btnBlock.FlatAppearance.BorderSize = 1; btnBlock.BackColor = Color.LightGray;

            btnSpecial.Size = new Size(btnWidth, btnHeight);
            btnSpecial.Location = new Point(btnX, btnStartY + (btnHeight * 2) + 20);
            btnSpecial.Font = new Font("Consolas", 14, FontStyle.Bold);
            btnSpecial.FlatStyle = FlatStyle.Flat; btnSpecial.TabStop = false; btnSpecial.FlatAppearance.BorderSize = 1; btnSpecial.BackColor = Color.LightGray;

            pnlBattleScreen.Controls.Remove(playerSpritePb);
            pnlBattleScreen.Controls.Remove(aiSpritePb);

            _basePlayerX = pnlBattleScreen.Width / 2 - 350;
            _baseAIX = pnlBattleScreen.Width / 2 + 50;
            _playerDrawX = _basePlayerX;
            _aiDrawX = _baseAIX;

            pnlBattleScreen.Paint -= PnlBattleScreen_Paint;
            pnlBattleScreen.Paint += PnlBattleScreen_Paint;
        }

        private void PnlBattleScreen_Paint(object sender, PaintEventArgs e)
        {
            if (_game == null || _game.CurrentArena == null || !pnlBattleScreen.Visible) return;

            Rectangle playerRect = new Rectangle(_playerDrawX, 220, 300, 300);
            Rectangle aiRect = new Rectangle(_aiDrawX, 220, 300, 300);

            bool playerIsActive = _game.ActiveFighter == _game.PlayerFighter;

            if (playerIsActive)
            {
                DrawFighterPose(e.Graphics, aiRect, _game.AIFighter);
                DrawFighterPose(e.Graphics, playerRect, _game.PlayerFighter);
            }
            else
            {
                DrawFighterPose(e.Graphics, playerRect, _game.PlayerFighter);
                DrawFighterPose(e.Graphics, aiRect, _game.AIFighter);
            }
        }

        private void UpdateCombatLog(string message, Color color)
        {
            combatLogRtb.SelectionStart = combatLogRtb.TextLength;
            combatLogRtb.SelectionLength = 0;
            combatLogRtb.SelectionColor = color;
            combatLogRtb.AppendText(message + "\n");
            combatLogRtb.ScrollToCaret();
        }

        private void EnableDoubleBuffering(Control control)
        {
            typeof(Control).InvokeMember("DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.NonPublic | BindingFlags.Instance,
                null, control, new object[] { true });
        }

        private void ShowScreen(Panel screenToShow)
        {
            pnlSelectScreen.Visible = false;
            pnlStageScreen.Visible = false;
            pnlBattleScreen.Visible = false;
            screenToShow.Visible = true;
            screenToShow.BringToFront();
        }

        private void PopulateSelectionGrid()
        {
            selectionGrid.Size = new Size(pnlSelectScreen.Width, pnlSelectScreen.Height - 250);
            selectionGrid.Location = new Point(0, 0);
            selectionGrid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            selectionGrid.Controls.Clear();

            foreach (Character c in _game.CharacterRoster)
            {
                Panel card = new Panel { Size = new Size(140, 160), BackColor = Color.DimGray, Margin = new Padding(10), Cursor = Cursors.Hand, Tag = c.CharacterID };
                PictureBox icon = new PictureBox { Size = new Size(80, 80), Location = new Point(30, 10), SizeMode = PictureBoxSizeMode.StretchImage, BackColor = Color.White, Cursor = Cursors.Hand, Tag = c.CharacterID };
                icon.Image = LoadStaticImage(c.IconPath);

                Label nameLbl = new Label { Text = c.CharacterName, Location = new Point(0, 105), Size = new Size(140, 30), TextAlign = ContentAlignment.MiddleCenter, ForeColor = Color.White, Font = new Font("Consolas", 14, FontStyle.Bold), Cursor = Cursors.Hand, Tag = c.CharacterID };

                EventHandler clickHandler = (s, e) => CharacterCard_Click(c.CharacterID);
                card.Click += clickHandler;
                icon.Click += clickHandler;
                nameLbl.Click += clickHandler;

                card.Controls.Add(icon);
                card.Controls.Add(nameLbl);

                if (c.FighterType == Archetype.Striker) card.BackColor = Color.LightCoral;
                else if (c.FighterType == Archetype.Tank) card.BackColor = Color.LightSkyBlue;
                else card.BackColor = Color.LightGreen;

                _statToolTip.SetToolTip(card, $"{c.CharacterBio}\nHP: {c.MaxHealth} | SP: {c.Stamina}");
                selectionGrid.Controls.Add(card);
            }
        }

        private void CharacterCard_Click(Guid charId)
        {
            if (_game.CurrentState != GameState.SelectionState) return;

            _previewedCharacterId = charId;
            Character selected = _game.CharacterRoster.Find(c => c.CharacterID == charId);

            pbPlayerPreview.Image = LoadStaticImage($"{selected.SpritePath.Replace(".png", "")}_idle.png");

            lblPlayerStats.Text = $"NAME: {selected.CharacterName}\nTYPE: {selected.FighterType}\nELEMENT: {selected.Element}\nHP: {selected.MaxHealth} | SP: {selected.Stamina}\nATK: {selected.AttackPower} | DEF: {selected.DefenseGrade * 100}%\nW: {selected.WinCount} | L: {selected.LossCount}";

            foreach (Control ctrl in selectionGrid.Controls)
            {
                if (ctrl is Panel p)
                {
                    if ((Guid)p.Tag == charId) p.BorderStyle = BorderStyle.Fixed3D;
                    else p.BorderStyle = BorderStyle.None;
                }
            }

            btnProceedToStage.Visible = false;
            btnLockSelection.Visible = true;
            btnLockSelection.BringToFront();
        }

        private async void BtnLockSelection_Click(object sender, EventArgs e)
        {
            if (_previewedCharacterId == Guid.Empty) return;

            selectionGrid.Enabled = false;
            btnLockSelection.Visible = false;

            Character selected = _game.CharacterRoster.Find(c => c.CharacterID == _previewedCharacterId);

            if (selected is Striker) _game.PlayerFighter = new Striker(selected.CharacterName, selected.Element, selected.SpritePath, selected.IconPath, selected.SpecialMoveName);
            else if (selected is Tank) _game.PlayerFighter = new Tank(selected.CharacterName, selected.Element, selected.SpritePath, selected.IconPath, selected.SpecialMoveName);
            else _game.PlayerFighter = new Tactician(selected.CharacterName, selected.Element, selected.SpritePath, selected.IconPath, selected.SpecialMoveName);

            _game.PlayerFighter.CharacterID = selected.CharacterID;

            // --- FIX: Copy player's win/loss stats to the clone ---
            _game.PlayerFighter.WinCount = selected.WinCount;
            _game.PlayerFighter.LossCount = selected.LossCount;

            lblAISelection.BringToFront();
            lblAISelection.Visible = true;
            lblAISelection.Text = "AI is choosing a fighter...";
            await Task.Delay(1000);

            Random rng = new Random();
            for (int i = 0; i < 15; i++)
            {
                Character temp = _game.CharacterRoster[rng.Next(_game.CharacterRoster.Count)];
                lblAISelection.Text = $"{_game.PlayerFighter.CharacterName} VS {temp.CharacterName}";
                await Task.Delay(100);
            }

            Character finalEnemy = _game.GetRandomEnemy(_previewedCharacterId);
            if (finalEnemy is Striker) _game.AIFighter = new Striker(finalEnemy.CharacterName, finalEnemy.Element, finalEnemy.SpritePath, finalEnemy.IconPath, finalEnemy.SpecialMoveName);
            else if (finalEnemy is Tank) _game.AIFighter = new Tank(finalEnemy.CharacterName, finalEnemy.Element, finalEnemy.SpritePath, finalEnemy.IconPath, finalEnemy.SpecialMoveName);
            else _game.AIFighter = new Tactician(finalEnemy.CharacterName, finalEnemy.Element, finalEnemy.SpritePath, finalEnemy.IconPath, finalEnemy.SpecialMoveName);

            _game.AIFighter.CharacterID = finalEnemy.CharacterID;

            // --- FIX: Copy AI's win/loss stats to the clone ---
            _game.AIFighter.WinCount = finalEnemy.WinCount;
            _game.AIFighter.LossCount = finalEnemy.LossCount;

            Image aiPreviewImage = LoadStaticImage($"{_game.AIFighter.SpritePath.Replace(".png", "")}_idle.png");
            if (aiPreviewImage != null)
            {
                Bitmap flippedPreview = new Bitmap(aiPreviewImage);
                flippedPreview.RotateFlip(RotateFlipType.RotateNoneFlipX);
                pbAIPreview.Image = flippedPreview;
            }

            lblAIStats.Text = $"NAME: {_game.AIFighter.CharacterName}\nTYPE: {_game.AIFighter.FighterType}\nELEMENT: {_game.AIFighter.Element}\nHP: {_game.AIFighter.MaxHealth} | SP: {_game.AIFighter.Stamina}\nATK: {_game.AIFighter.AttackPower} | DEF: {_game.AIFighter.DefenseGrade * 100}%\nW: {_game.AIFighter.WinCount} | L: {_game.AIFighter.LossCount}";

            lblAISelection.Text = $"AI chose {_game.AIFighter.CharacterName}!";
            await Task.Delay(1200);
            lblAISelection.Visible = false;

            btnProceedToStage.Visible = true;
            btnProceedToStage.BringToFront();
        }

        private async void BtnProceedToStage_Click(object sender, EventArgs e)
        {
            btnProceedToStage.Enabled = false;
            btnStartFight.Enabled = false;
            ShowScreen(pnlStageScreen);

            pbPlayerIconStage.Image = LoadStaticImage(_game.PlayerFighter.IconPath);
            lblPlayerNameStage.Text = _game.PlayerFighter.CharacterName;

            Image aiStageIcon = LoadStaticImage(_game.AIFighter.IconPath);
            if (aiStageIcon != null)
            {
                Bitmap flippedStage = new Bitmap(aiStageIcon);
                flippedStage.RotateFlip(RotateFlipType.RotateNoneFlipX);
                pbAIIconStage.Image = flippedStage;
            }
            lblAINameStage.Text = _game.AIFighter.CharacterName;

            lblStageReveal.Text = "Rolling for Stage...";

            Random rng = new Random();
            for (int i = 0; i < 30; i++)
            {
                Stage tempStage = _game.StageRoster[rng.Next(_game.StageRoster.Count)];
                lblStageReveal.Text = $"Stage: {tempStage.StageName}";

                pnlStageScreen.BackgroundImage = LoadStaticImage(tempStage.ImagePath);

                await Task.Delay(100);
            }

            _game.CurrentArena = _game.StageRoster[rng.Next(_game.StageRoster.Count)];

            lblStageReveal.Text = $"Stage: {_game.CurrentArena.StageName}";
            pnlStageScreen.BackgroundImage = LoadStaticImage(_game.CurrentArena.ImagePath);

            if (_game.PlayerFighter.BaseSpeed >= _game.AIFighter.BaseSpeed)
            {
                _game.CurrentState = GameState.PlayerTurn;
                _game.ActiveFighter = _game.PlayerFighter;
            }
            else
            {
                _game.CurrentState = GameState.AITurn;
                _game.ActiveFighter = _game.AIFighter;
            }

            btnStartFight.Enabled = true;
        }

        private void btnStartFight_Click(object sender, EventArgs e)
        {
            if (_game.CurrentArena != null && !string.IsNullOrEmpty(_game.CurrentArena.ImagePath))
            {
                pnlBattleScreen.BackgroundImage = LoadStaticImage(_game.CurrentArena.ImagePath);
                pnlBattleScreen.BackgroundImageLayout = ImageLayout.Stretch;
            }

            pnlBattleScreen.BackColor = _game.CurrentArena.StageColor;

            LoadSprite(_game.PlayerFighter, false);
            LoadSprite(_game.AIFighter, true);

            pbPlayerIconBattle.Image = LoadStaticImage(_game.PlayerFighter.IconPath);

            Image aiBattleIcon = LoadStaticImage(_game.AIFighter.IconPath);
            if (aiBattleIcon != null)
            {
                Bitmap flippedBattle = new Bitmap(aiBattleIcon);
                flippedBattle.RotateFlip(RotateFlipType.RotateNoneFlipX);
                pbAIIconBattle.Image = flippedBattle;
            }

            ShowScreen(pnlBattleScreen);
            combatLogRtb.Clear();
            _game.LogMessage($"THE FIGHT BEGINS IN THE {_game.CurrentArena.StageName.ToUpper()}!", Color.Orange);
            _game.AnnounceStageBoosts();
            if (_game.CurrentState == GameState.PlayerTurn)
                _game.LogMessage($"{_game.PlayerFighter.CharacterName} is faster and takes the initiative!", Color.LimeGreen);
            else
                _game.LogMessage($"{_game.AIFighter.CharacterName} is faster and takes the initiative!", Color.Red);
            gameLoopTimer.Start();
        }

        private void LoadSprite(Character fighter, bool flipImage)
        {
            fighter.CurrentPose = "Idle";
            string basePath = fighter.SpritePath.Replace(".png", "");

            fighter.IdleFrame = LoadStaticImage($"{basePath}_idle.png");
            fighter.AttackFrame = LoadStaticImage($"{basePath}_attack.png");
            fighter.BlockFrame = LoadStaticImage($"{basePath}_block.png");
            fighter.SpecialFrame = LoadStaticImage($"{basePath}_special.png");
            fighter.HurtFrame = LoadStaticImage($"{basePath}_hurt.png");
            fighter.DefeatFrame = LoadStaticImage($"{basePath}_defeat.png");
            if (fighter.IdleFrame == null) fighter.IdleFrame = fighter.BlockFrame;

            if (flipImage)
            {
                fighter.IdleFrame?.RotateFlip(RotateFlipType.RotateNoneFlipX);
                fighter.AttackFrame?.RotateFlip(RotateFlipType.RotateNoneFlipX);
                fighter.BlockFrame?.RotateFlip(RotateFlipType.RotateNoneFlipX);
                fighter.SpecialFrame?.RotateFlip(RotateFlipType.RotateNoneFlipX);
                fighter.HurtFrame?.RotateFlip(RotateFlipType.RotateNoneFlipX);
                fighter.DefeatFrame?.RotateFlip(RotateFlipType.RotateNoneFlipX);
            }
        }

        private Image LoadStaticImage(string path)
        {
            if (string.IsNullOrEmpty(path)) return null;

            // This formats your existing paths (e.g., "Assets/kenji.png") 
            // into the format the embedded .exe needs: "PixelFightingGame.Assets.kenji.png"
            string resourceName = "PixelFightingGame." + path.Replace("/", ".").Replace("\\", ".");

            // Fetch the image straight from the compiled .exe file!
            using (System.IO.Stream stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            {
                if (stream != null)
                {
                    return Image.FromStream(stream);
                }
            }

            return null;
        }

        private async Task PlayCombatAnimation(bool isPlayer, bool isSpecial = false)
        {
            Character attackFighter = isPlayer ? _game.PlayerFighter : _game.AIFighter;
            Character targetFighter = isPlayer ? _game.AIFighter : _game.PlayerFighter;

            attackFighter.CurrentPose = isSpecial ? "Special" : "Attack";
            pnlBattleScreen.Invalidate();

            int originalX = isPlayer ? _basePlayerX : _baseAIX;
            int targetX = isPlayer ? _baseAIX - 150 : _basePlayerX + 150;

            int steps = 6;
            int stepDistance = (targetX - originalX) / steps;

            for (int i = 0; i < steps; i++)
            {
                if (isPlayer) _playerDrawX += stepDistance;
                else _aiDrawX += stepDistance;

                pnlBattleScreen.Invalidate();
                await Task.Delay(15);
            }

            if (isPlayer) _playerDrawX = targetX;
            else _aiDrawX = targetX;
            pnlBattleScreen.Invalidate();

            await Task.Delay(50);

            if (targetFighter.IsBlocking) targetFighter.CurrentPose = "Block";
            else targetFighter.CurrentPose = "Hurt";

            pnlBattleScreen.Invalidate();
            await Task.Delay(250);

            targetFighter.CurrentPose = "Idle";
            pnlBattleScreen.Invalidate();

            for (int i = 0; i < steps; i++)
            {
                if (isPlayer) _playerDrawX -= stepDistance;
                else _aiDrawX -= stepDistance;

                pnlBattleScreen.Invalidate();
                await Task.Delay(15);
            }

            if (isPlayer) _playerDrawX = originalX;
            else _aiDrawX = originalX;

            attackFighter.CurrentPose = "Idle";
            pnlBattleScreen.Invalidate();

            await Task.Delay(100);
        }

        private async Task PlayBlockAnimation(bool isPlayer)
        {
            Character blockFighter = isPlayer ? _game.PlayerFighter : _game.AIFighter;
            blockFighter.CurrentPose = "Block";

            pnlBattleScreen.Invalidate();
            await Task.Delay(400);
            pnlBattleScreen.Invalidate();
        }

        private async void btnAttack_Click(object sender, EventArgs e)
        {
            if (_game.CurrentState == GameState.PlayerTurn && _game.PlayerFighter.CurrentStamina >= 10)
            {
                _game.CurrentState = GameState.AnimationState;
                await PlayCombatAnimation(true, false);
                _game.PlayerFighter.IsBlocking = false;
                _game.ExecuteStandardAttack(_game.PlayerFighter, _game.AIFighter);
                _game.FinalizeTurn();
            }
        }

        private async void btnBlock_Click(object sender, EventArgs e)
        {
            if (_game.CurrentState == GameState.PlayerTurn)
            {
                _game.CurrentState = GameState.AnimationState;
                await PlayBlockAnimation(true);
                _game.PlayerFighter.CurrentStamina = Math.Min((int)_game.PlayerFighter.Stamina, _game.PlayerFighter.CurrentStamina + 30);
                _game.PlayerFighter.IsBlocking = true;
                _game.LogMessage("You take a defensive stance and recover stamina! (BLOCKING)", Color.CornflowerBlue);
                _game.FinalizeTurn();
            }
        }

        private async void btnSpecial_Click(object sender, EventArgs e)
        {
            if (_game.CurrentState == GameState.PlayerTurn && _game.PlayerFighter.CurrentStamina >= 50)
            {
                _game.CurrentState = GameState.AnimationState;
                await Task.Delay(300);
                await PlayCombatAnimation(true, true);
                _game.PlayerFighter.IsBlocking = false;
                _game.ExecuteSpecialAttack(_game.PlayerFighter, _game.AIFighter);
                _game.FinalizeTurn();
            }
        }

        private async void gameLoopTimer_Tick(object sender, EventArgs e)
        {
            playerHealthPanel.Invalidate();
            aiHealthPanel.Invalidate();
            if (playerStaminaPanel != null) playerStaminaPanel.Invalidate();
            if (aiStaminaPanel != null) aiStaminaPanel.Invalidate();
            pnlBattleScreen.Invalidate();

            UpdateButtonState(btnAttack, "Attack", 10);
            UpdateButtonState(btnBlock, "Block", 0);
            UpdateButtonState(btnSpecial, "Special", 50);

            if (_game.CurrentState == GameState.PlayerTurn && _game.PlayerFighter.IsStunned)
            {
                _game.CurrentState = GameState.AnimationState;
                _game.LogMessage("You are STUNNED and must skip your turn!", Color.Cyan);
                _game.PlayerFighter.IsStunned = false;
                await Task.Delay(1000);
                _game.FinalizeTurn();
            }
            else if (_game.CurrentState == GameState.AITurn)
            {
                gameLoopTimer.Stop();
                _game.CurrentState = GameState.AnimationState;
                await Task.Delay(600);
                AIAction action = _game.DecideAITurn();
                if (action == AIAction.Attack || action == AIAction.Special)
                {
                    bool isSpecial = (action == AIAction.Special);
                    await PlayCombatAnimation(false, isSpecial);
                }
                else if (action == AIAction.Block)
                {
                    await PlayBlockAnimation(false);
                }
                _game.CurrentState = GameState.AITurn;
                _game.ExecuteAITurn(action);
                gameLoopTimer.Start();
            }
            else if (_game.CurrentState == GameState.ResolutionState)
            {
                _game.CheckCombatResolution();
                if (_game.PlayerFighter.HealthPoints <= 0 || _game.AIFighter.HealthPoints <= 0)
                {
                    if (_game.PlayerFighter.HealthPoints <= 0) _game.PlayerFighter.CurrentPose = "Defeat";
                    if (_game.AIFighter.HealthPoints <= 0) _game.AIFighter.CurrentPose = "Defeat";

                    pnlBattleScreen.Invalidate();
                    gameLoopTimer.Stop();
                    await Task.Delay(1000);

                    string winnerMessage = _game.PlayerFighter.HealthPoints > 0 ? "VICTORY! You won the match!" : "DEFEAT! You were destroyed!";
                    MessageBox.Show(winnerMessage, "Match Over", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    combatLogRtb.Clear();
                    PopulateSelectionGrid();
                    ResetSelectionScreen();
                    ShowScreen(pnlSelectScreen);
                }
            }
        }

        private void UpdateButtonState(Button btn, string baseText, int cost)
        {
            if (_game.CurrentState != GameState.PlayerTurn)
            {
                btn.Text = "Wait...";
                btn.Enabled = false;
                btn.BackColor = Color.LightGray;
            }
            else if (_game.PlayerFighter.CurrentStamina < cost)
            {
                btn.Text = "Too Tired!";
                btn.Enabled = false;
                btn.BackColor = Color.LightGray;
            }
            else
            {
                btn.Text = cost > 0 ? $"{baseText} ({cost} SP)" : baseText;
                btn.Enabled = true;
                btn.BackColor = Color.White;
                btn.UseVisualStyleBackColor = false;
            }
        }

        private void playerHealthPanel_Paint(object sender, PaintEventArgs e)
        {
            if (_game == null || _game.PlayerFighter == null) return;
            DrawCustomHealthBar(e.Graphics, playerHealthPanel.ClientRectangle, _game.PlayerFighter);
        }

        private void aiHealthPanel_Paint(object sender, PaintEventArgs e)
        {
            if (_game == null || _game.AIFighter == null) return;
            DrawCustomHealthBar(e.Graphics, aiHealthPanel.ClientRectangle, _game.AIFighter);
        }

        private void playerStaminaPanel_Paint(object sender, PaintEventArgs e)
        {
            if (_game == null || _game.PlayerFighter == null) return;
            DrawCustomStaminaBar(e.Graphics, playerStaminaPanel.ClientRectangle, _game.PlayerFighter);
        }

        private void aiStaminaPanel_Paint(object sender, PaintEventArgs e)
        {
            if (_game == null || _game.AIFighter == null) return;
            DrawCustomStaminaBar(e.Graphics, aiStaminaPanel.ClientRectangle, _game.AIFighter);
        }

        private void DrawFighterPose(Graphics g, Rectangle destRect, Character fighter)
        {
            if (fighter == null) return;
            if (fighter.IsBlocking && fighter.CurrentPose != "Defeat") fighter.CurrentPose = "Block";

            Image frameToDraw = null;
            switch (fighter.CurrentPose)
            {
                case "Idle": frameToDraw = fighter.IdleFrame; break;
                case "Attack": frameToDraw = fighter.AttackFrame; break;
                case "Block": frameToDraw = fighter.BlockFrame; break;
                case "Special": frameToDraw = fighter.SpecialFrame; break;
                case "Hurt": frameToDraw = fighter.HurtFrame; break;
                case "Defeat": frameToDraw = fighter.DefeatFrame; break;
                default: frameToDraw = fighter.IdleFrame; break;
            }
            if (frameToDraw == null) frameToDraw = fighter.IdleFrame;
            if (frameToDraw == null) return;

            float ratio = Math.Min((float)destRect.Width / frameToDraw.Width, (float)destRect.Height / frameToDraw.Height);
            int drawWidth = (int)(frameToDraw.Width * ratio);
            int drawHeight = (int)(frameToDraw.Height * ratio);
            int drawX = destRect.X + (destRect.Width - drawWidth) / 2;
            int drawY = destRect.Y + (destRect.Height - drawHeight) / 2;

            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            g.PixelOffsetMode = PixelOffsetMode.Half;

            g.DrawImage(frameToDraw, drawX, drawY, drawWidth, drawHeight);
        }

        private void DrawCustomHealthBar(Graphics g, Rectangle bounds, Character fighter)
        {
            float hpRatio = Math.Max(0, fighter.HealthPoints / fighter.MaxHealth);
            g.FillRectangle(Brushes.DarkGray, bounds);
            Brush healthBrush = Brushes.LimeGreen;
            if (hpRatio <= 0.5f) healthBrush = Brushes.Gold;
            if (hpRatio <= 0.2f) healthBrush = Brushes.Crimson;
            Rectangle healthFill = new Rectangle(bounds.X, bounds.Y, (int)(bounds.Width * hpRatio), bounds.Height);
            g.FillRectangle(healthBrush, healthFill);
            string shortName = fighter.CharacterName.Contains("(") ? fighter.CharacterName.Substring(0, fighter.CharacterName.IndexOf("(")).Trim() : fighter.CharacterName;
            string overlayText = $"{shortName}: {fighter.HealthPoints:0} / {fighter.MaxHealth:0}";
            using (Font f = new Font("Consolas", 12, FontStyle.Bold))
            {
                g.DrawString(overlayText, f, Brushes.Black, 5, 5);
                g.DrawString(overlayText, f, Brushes.White, 4, 4);
            }
        }

        private void DrawCustomStaminaBar(Graphics g, Rectangle bounds, Character fighter)
        {
            float stamRatio = Math.Max(0, (float)fighter.CurrentStamina / fighter.Stamina);
            g.FillRectangle(Brushes.DarkGray, bounds);
            Rectangle stamFill = new Rectangle(bounds.X, bounds.Y, (int)(bounds.Width * stamRatio), bounds.Height);
            g.FillRectangle(Brushes.DeepSkyBlue, stamFill);
            string overlayText = $"SP: {fighter.CurrentStamina:0} / {fighter.Stamina:0}";
            using (Font f = new Font("Consolas", 10, FontStyle.Bold))
            {
                g.DrawString(overlayText, f, Brushes.Black, 3, 2);
                g.DrawString(overlayText, f, Brushes.White, 2, 1);
            }
        }

        private void playerSpritePb_Click(object sender, EventArgs e) { }
    }
}