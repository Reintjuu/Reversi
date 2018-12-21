namespace Reversi
{
    partial class Screen
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.newGameButton = new System.Windows.Forms.Button();
			this.hintButton = new System.Windows.Forms.Button();
			this.gameStatsLabel = new System.Windows.Forms.Label();
			this.canvas = new System.Windows.Forms.PictureBox();
			this.passButton = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.canvas)).BeginInit();
			this.SuspendLayout();
			// 
			// newGameButton
			// 
			this.newGameButton.Location = new System.Drawing.Point(12, 9);
			this.newGameButton.Name = "newGameButton";
			this.newGameButton.Size = new System.Drawing.Size(75, 23);
			this.newGameButton.TabIndex = 0;
			this.newGameButton.Text = "New Game";
			this.newGameButton.UseVisualStyleBackColor = true;
			this.newGameButton.Click += new System.EventHandler(this.NewGame);
			// 
			// hintButton
			// 
			this.hintButton.Location = new System.Drawing.Point(93, 9);
			this.hintButton.Name = "hintButton";
			this.hintButton.Size = new System.Drawing.Size(75, 23);
			this.hintButton.TabIndex = 1;
			this.hintButton.Text = "Hide Hints";
			this.hintButton.UseVisualStyleBackColor = true;
			this.hintButton.Click += new System.EventHandler(this.HintButton_Click);
			// 
			// gameStatsLabel
			// 
			this.gameStatsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.gameStatsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.gameStatsLabel.Location = new System.Drawing.Point(255, 9);
			this.gameStatsLabel.Name = "gameStatsLabel";
			this.gameStatsLabel.Size = new System.Drawing.Size(317, 40);
			this.gameStatsLabel.TabIndex = 3;
			this.gameStatsLabel.Text = "Black: 0\r\nWhite: 0\r\n";
			this.gameStatsLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// canvas
			// 
			this.canvas.Location = new System.Drawing.Point(0, 70);
			this.canvas.Name = "canvas";
			this.canvas.Size = new System.Drawing.Size(584, 491);
			this.canvas.TabIndex = 4;
			this.canvas.TabStop = false;
			// 
			// passButton
			// 
			this.passButton.Enabled = false;
			this.passButton.Location = new System.Drawing.Point(174, 9);
			this.passButton.Name = "passButton";
			this.passButton.Size = new System.Drawing.Size(75, 23);
			this.passButton.TabIndex = 5;
			this.passButton.Text = "Pass Turn";
			this.passButton.UseVisualStyleBackColor = true;
			this.passButton.Click += new System.EventHandler(this.PassButton_Click);
			// 
			// Screen
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(584, 561);
			this.Controls.Add(this.passButton);
			this.Controls.Add(this.canvas);
			this.Controls.Add(this.hintButton);
			this.Controls.Add(this.newGameButton);
			this.Controls.Add(this.gameStatsLabel);
			this.Name = "Screen";
			this.Text = "Reversi";
			((System.ComponentModel.ISupportInitialize)(this.canvas)).EndInit();
			this.ResumeLayout(false);

        }

		#endregion

		private System.Windows.Forms.Button newGameButton;
		private System.Windows.Forms.Button hintButton;
		private System.Windows.Forms.Label gameStatsLabel;
		private System.Windows.Forms.PictureBox canvas;
		private System.Windows.Forms.Button passButton;
	}
}

