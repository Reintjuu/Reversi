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
			this.rowsInput = new System.Windows.Forms.NumericUpDown();
			this.rowsLabel = new System.Windows.Forms.Label();
			this.columnsLabel = new System.Windows.Forms.Label();
			this.columnsInput = new System.Windows.Forms.NumericUpDown();
			((System.ComponentModel.ISupportInitialize)(this.canvas)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.rowsInput)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.columnsInput)).BeginInit();
			this.SuspendLayout();
			// 
			// newGameButton
			// 
			this.newGameButton.Location = new System.Drawing.Point(127, 9);
			this.newGameButton.Name = "newGameButton";
			this.newGameButton.Size = new System.Drawing.Size(75, 23);
			this.newGameButton.TabIndex = 0;
			this.newGameButton.Text = "New Game";
			this.newGameButton.UseVisualStyleBackColor = true;
			this.newGameButton.Click += new System.EventHandler(this.NewGame);
			// 
			// hintButton
			// 
			this.hintButton.Location = new System.Drawing.Point(208, 9);
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
			this.gameStatsLabel.Location = new System.Drawing.Point(183, 9);
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
			this.canvas.Size = new System.Drawing.Size(512, 512);
			this.canvas.TabIndex = 4;
			this.canvas.TabStop = false;
			// 
			// passButton
			// 
			this.passButton.Enabled = false;
			this.passButton.Location = new System.Drawing.Point(208, 31);
			this.passButton.Name = "passButton";
			this.passButton.Size = new System.Drawing.Size(75, 23);
			this.passButton.TabIndex = 5;
			this.passButton.Text = "Pass Turn";
			this.passButton.UseVisualStyleBackColor = true;
			this.passButton.Click += new System.EventHandler(this.PassButton_Click);
			// 
			// rowsInput
			// 
			this.rowsInput.Location = new System.Drawing.Point(87, 12);
			this.rowsInput.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
			this.rowsInput.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
			this.rowsInput.Name = "rowsInput";
			this.rowsInput.Size = new System.Drawing.Size(34, 20);
			this.rowsInput.TabIndex = 6;
			this.rowsInput.Value = new decimal(new int[] {
            6,
            0,
            0,
            0});
			// 
			// rowsLabel
			// 
			this.rowsLabel.AutoSize = true;
			this.rowsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rowsLabel.Location = new System.Drawing.Point(12, 11);
			this.rowsLabel.Name = "rowsLabel";
			this.rowsLabel.Size = new System.Drawing.Size(49, 20);
			this.rowsLabel.TabIndex = 7;
			this.rowsLabel.Text = "Rows";
			// 
			// columnsLabel
			// 
			this.columnsLabel.AutoSize = true;
			this.columnsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.columnsLabel.Location = new System.Drawing.Point(12, 31);
			this.columnsLabel.Name = "columnsLabel";
			this.columnsLabel.Size = new System.Drawing.Size(71, 20);
			this.columnsLabel.TabIndex = 8;
			this.columnsLabel.Text = "Columns";
			// 
			// columnsInput
			// 
			this.columnsInput.Location = new System.Drawing.Point(87, 31);
			this.columnsInput.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
			this.columnsInput.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
			this.columnsInput.Name = "columnsInput";
			this.columnsInput.Size = new System.Drawing.Size(34, 20);
			this.columnsInput.TabIndex = 9;
			this.columnsInput.Value = new decimal(new int[] {
            6,
            0,
            0,
            0});
			// 
			// Screen
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(512, 582);
			this.Controls.Add(this.columnsInput);
			this.Controls.Add(this.columnsLabel);
			this.Controls.Add(this.rowsLabel);
			this.Controls.Add(this.rowsInput);
			this.Controls.Add(this.passButton);
			this.Controls.Add(this.canvas);
			this.Controls.Add(this.hintButton);
			this.Controls.Add(this.newGameButton);
			this.Controls.Add(this.gameStatsLabel);
			this.MinimumSize = new System.Drawing.Size(528, 621);
			this.Name = "Screen";
			this.Text = "Reversi";
			((System.ComponentModel.ISupportInitialize)(this.canvas)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.rowsInput)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.columnsInput)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

		#endregion

		private System.Windows.Forms.Button newGameButton;
		private System.Windows.Forms.Button hintButton;
		private System.Windows.Forms.Label gameStatsLabel;
		private System.Windows.Forms.PictureBox canvas;
		private System.Windows.Forms.Button passButton;
		private System.Windows.Forms.NumericUpDown rowsInput;
		private System.Windows.Forms.Label rowsLabel;
		private System.Windows.Forms.Label columnsLabel;
		private System.Windows.Forms.NumericUpDown columnsInput;
	}
}

