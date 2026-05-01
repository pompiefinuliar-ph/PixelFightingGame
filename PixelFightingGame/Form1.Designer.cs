namespace PixelFightingGame
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pnlSelectScreen = new System.Windows.Forms.Panel();
            this.selectionGrid = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlStageScreen = new System.Windows.Forms.Panel();
            this.lblStageReveal = new System.Windows.Forms.Label();
            this.btnStartFight = new System.Windows.Forms.Button();
            this.pnlBattleScreen = new System.Windows.Forms.Panel();
            this.aiStaminaPanel = new System.Windows.Forms.Panel();
            this.playerStaminaPanel = new System.Windows.Forms.Panel();
            this.combatLogRtb = new System.Windows.Forms.RichTextBox();
            this.aiSpritePb = new System.Windows.Forms.PictureBox();
            this.playerSpritePb = new System.Windows.Forms.PictureBox();
            this.aiHealthPanel = new System.Windows.Forms.Panel();
            this.playerHealthPanel = new System.Windows.Forms.Panel();
            this.btnSpecial = new System.Windows.Forms.Button();
            this.btnBlock = new System.Windows.Forms.Button();
            this.btnAttack = new System.Windows.Forms.Button();
            this.gameLoopTimer = new System.Windows.Forms.Timer(this.components);
            this.pnlSelectScreen.SuspendLayout();
            this.pnlStageScreen.SuspendLayout();
            this.pnlBattleScreen.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.aiSpritePb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.playerSpritePb)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlSelectScreen
            // 
            this.pnlSelectScreen.BackColor = System.Drawing.Color.DimGray;
            this.pnlSelectScreen.Controls.Add(this.selectionGrid);
            this.pnlSelectScreen.Location = new System.Drawing.Point(8, 8);
            this.pnlSelectScreen.Margin = new System.Windows.Forms.Padding(2);
            this.pnlSelectScreen.Name = "pnlSelectScreen";
            this.pnlSelectScreen.Size = new System.Drawing.Size(823, 430);
            this.pnlSelectScreen.TabIndex = 0;
            // 
            // selectionGrid
            // 
            this.selectionGrid.BackColor = System.Drawing.Color.DarkOrange;
            this.selectionGrid.Location = new System.Drawing.Point(2, 2);
            this.selectionGrid.Margin = new System.Windows.Forms.Padding(2);
            this.selectionGrid.Name = "selectionGrid";
            this.selectionGrid.Size = new System.Drawing.Size(819, 426);
            this.selectionGrid.TabIndex = 0;
            // 
            // pnlStageScreen
            // 
            this.pnlStageScreen.BackColor = System.Drawing.Color.DarkSlateBlue;
            this.pnlStageScreen.Controls.Add(this.lblStageReveal);
            this.pnlStageScreen.Controls.Add(this.btnStartFight);
            this.pnlStageScreen.Font = new System.Drawing.Font("Microsoft Sans Serif", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlStageScreen.Location = new System.Drawing.Point(8, 8);
            this.pnlStageScreen.Margin = new System.Windows.Forms.Padding(2);
            this.pnlStageScreen.Name = "pnlStageScreen";
            this.pnlStageScreen.Size = new System.Drawing.Size(823, 430);
            this.pnlStageScreen.TabIndex = 1;
            // 
            // lblStageReveal
            // 
            this.lblStageReveal.Font = new System.Drawing.Font("Microsoft Sans Serif", 26F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStageReveal.ForeColor = System.Drawing.Color.White;
            this.lblStageReveal.Location = new System.Drawing.Point(0, 65);
            this.lblStageReveal.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblStageReveal.Name = "lblStageReveal";
            this.lblStageReveal.Size = new System.Drawing.Size(823, 227);
            this.lblStageReveal.TabIndex = 2;
            this.lblStageReveal.Text = "VS";
            this.lblStageReveal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnStartFight
            // 
            this.btnStartFight.Location = new System.Drawing.Point(289, 314);
            this.btnStartFight.Margin = new System.Windows.Forms.Padding(2);
            this.btnStartFight.Name = "btnStartFight";
            this.btnStartFight.Size = new System.Drawing.Size(251, 75);
            this.btnStartFight.TabIndex = 1;
            this.btnStartFight.Text = "FIGHT!";
            this.btnStartFight.UseVisualStyleBackColor = true;
            // 
            // pnlBattleScreen
            // 
            this.pnlBattleScreen.BackColor = System.Drawing.Color.DarkRed;
            this.pnlBattleScreen.Controls.Add(this.aiStaminaPanel);
            this.pnlBattleScreen.Controls.Add(this.playerStaminaPanel);
            this.pnlBattleScreen.Controls.Add(this.combatLogRtb);
            this.pnlBattleScreen.Controls.Add(this.aiSpritePb);
            this.pnlBattleScreen.Controls.Add(this.playerSpritePb);
            this.pnlBattleScreen.Controls.Add(this.aiHealthPanel);
            this.pnlBattleScreen.Controls.Add(this.playerHealthPanel);
            this.pnlBattleScreen.Controls.Add(this.btnSpecial);
            this.pnlBattleScreen.Controls.Add(this.btnBlock);
            this.pnlBattleScreen.Controls.Add(this.btnAttack);
            this.pnlBattleScreen.Location = new System.Drawing.Point(8, 8);
            this.pnlBattleScreen.Margin = new System.Windows.Forms.Padding(2);
            this.pnlBattleScreen.Name = "pnlBattleScreen";
            this.pnlBattleScreen.Size = new System.Drawing.Size(823, 430);
            this.pnlBattleScreen.TabIndex = 4;
            // 
            // aiStaminaPanel
            // 
            this.aiStaminaPanel.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.aiStaminaPanel.Location = new System.Drawing.Point(515, 73);
            this.aiStaminaPanel.Margin = new System.Windows.Forms.Padding(2);
            this.aiStaminaPanel.Name = "aiStaminaPanel";
            this.aiStaminaPanel.Size = new System.Drawing.Size(239, 18);
            this.aiStaminaPanel.TabIndex = 9;
            this.aiStaminaPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.aiStaminaPanel_Paint);
            // 
            // playerStaminaPanel
            // 
            this.playerStaminaPanel.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.playerStaminaPanel.Location = new System.Drawing.Point(65, 73);
            this.playerStaminaPanel.Margin = new System.Windows.Forms.Padding(2);
            this.playerStaminaPanel.Name = "playerStaminaPanel";
            this.playerStaminaPanel.Size = new System.Drawing.Size(239, 18);
            this.playerStaminaPanel.TabIndex = 8;
            this.playerStaminaPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.playerStaminaPanel_Paint);
            // 
            // combatLogRtb
            // 
            this.combatLogRtb.BackColor = System.Drawing.Color.Black;
            this.combatLogRtb.Location = new System.Drawing.Point(246, 307);
            this.combatLogRtb.Margin = new System.Windows.Forms.Padding(2);
            this.combatLogRtb.Name = "combatLogRtb";
            this.combatLogRtb.Size = new System.Drawing.Size(331, 110);
            this.combatLogRtb.TabIndex = 7;
            this.combatLogRtb.Text = "";
            // 
            // aiSpritePb
            // 
            this.aiSpritePb.BackColor = System.Drawing.Color.White;
            this.aiSpritePb.Location = new System.Drawing.Point(539, 118);
            this.aiSpritePb.Margin = new System.Windows.Forms.Padding(2);
            this.aiSpritePb.Name = "aiSpritePb";
            this.aiSpritePb.Size = new System.Drawing.Size(193, 156);
            this.aiSpritePb.TabIndex = 6;
            this.aiSpritePb.TabStop = false;
            // 
            // playerSpritePb
            // 
            this.playerSpritePb.BackColor = System.Drawing.Color.White;
            this.playerSpritePb.Location = new System.Drawing.Point(89, 118);
            this.playerSpritePb.Margin = new System.Windows.Forms.Padding(2);
            this.playerSpritePb.Name = "playerSpritePb";
            this.playerSpritePb.Size = new System.Drawing.Size(193, 156);
            this.playerSpritePb.TabIndex = 5;
            this.playerSpritePb.TabStop = false;
            this.playerSpritePb.Click += new System.EventHandler(this.playerSpritePb_Click);
            // 
            // aiHealthPanel
            // 
            this.aiHealthPanel.BackColor = System.Drawing.Color.White;
            this.aiHealthPanel.Location = new System.Drawing.Point(515, 27);
            this.aiHealthPanel.Margin = new System.Windows.Forms.Padding(2);
            this.aiHealthPanel.Name = "aiHealthPanel";
            this.aiHealthPanel.Size = new System.Drawing.Size(239, 42);
            this.aiHealthPanel.TabIndex = 4;
            this.aiHealthPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.aiHealthPanel_Paint);
            // 
            // playerHealthPanel
            // 
            this.playerHealthPanel.BackColor = System.Drawing.Color.White;
            this.playerHealthPanel.Location = new System.Drawing.Point(65, 27);
            this.playerHealthPanel.Margin = new System.Windows.Forms.Padding(2);
            this.playerHealthPanel.Name = "playerHealthPanel";
            this.playerHealthPanel.Size = new System.Drawing.Size(239, 42);
            this.playerHealthPanel.TabIndex = 3;
            this.playerHealthPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.playerHealthPanel_Paint);
            // 
            // btnSpecial
            // 
            this.btnSpecial.Location = new System.Drawing.Point(65, 375);
            this.btnSpecial.Margin = new System.Windows.Forms.Padding(2);
            this.btnSpecial.Name = "btnSpecial";
            this.btnSpecial.Size = new System.Drawing.Size(102, 23);
            this.btnSpecial.TabIndex = 2;
            this.btnSpecial.Text = "btnSpecial";
            this.btnSpecial.UseVisualStyleBackColor = true;
            // 
            // btnBlock
            // 
            this.btnBlock.Location = new System.Drawing.Point(65, 342);
            this.btnBlock.Margin = new System.Windows.Forms.Padding(2);
            this.btnBlock.Name = "btnBlock";
            this.btnBlock.Size = new System.Drawing.Size(102, 23);
            this.btnBlock.TabIndex = 1;
            this.btnBlock.Text = "btnBlock";
            this.btnBlock.UseVisualStyleBackColor = true;
            // 
            // btnAttack
            // 
            this.btnAttack.Location = new System.Drawing.Point(65, 307);
            this.btnAttack.Margin = new System.Windows.Forms.Padding(2);
            this.btnAttack.Name = "btnAttack";
            this.btnAttack.Size = new System.Drawing.Size(102, 23);
            this.btnAttack.TabIndex = 0;
            this.btnAttack.Text = "btnAttack";
            this.btnAttack.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(839, 445);
            this.Controls.Add(this.pnlBattleScreen);
            this.Controls.Add(this.pnlStageScreen);
            this.Controls.Add(this.pnlSelectScreen);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Pixel Fighting Game";
            this.pnlSelectScreen.ResumeLayout(false);
            this.pnlStageScreen.ResumeLayout(false);
            this.pnlBattleScreen.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.aiSpritePb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.playerSpritePb)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlSelectScreen;
        private System.Windows.Forms.Panel pnlStageScreen;
        private System.Windows.Forms.Panel pnlBattleScreen;
        private System.Windows.Forms.RichTextBox combatLogRtb;
        private System.Windows.Forms.PictureBox aiSpritePb;
        private System.Windows.Forms.PictureBox playerSpritePb;
        private System.Windows.Forms.Panel aiHealthPanel;
        private System.Windows.Forms.Panel playerHealthPanel;
        private System.Windows.Forms.Button btnSpecial;
        private System.Windows.Forms.Button btnBlock;
        private System.Windows.Forms.Button btnAttack;
        private System.Windows.Forms.FlowLayoutPanel selectionGrid;
        private System.Windows.Forms.Timer gameLoopTimer;
        private System.Windows.Forms.Button btnStartFight;
        private System.Windows.Forms.Label lblStageReveal;
        private System.Windows.Forms.Panel aiStaminaPanel;
        private System.Windows.Forms.Panel playerStaminaPanel;
    }
}