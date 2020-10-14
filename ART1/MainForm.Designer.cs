namespace ART1
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.richTextBox2 = new System.Windows.Forms.RichTextBox();
            this.vInputsText = new System.Windows.Forms.RichTextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.itemsInputText = new System.Windows.Forms.RichTextBox();
            this.generateRandomBtn = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.betaInput = new System.Windows.Forms.NumericUpDown();
            this.roInput = new System.Windows.Forms.NumericUpDown();
            this.clientsCountInput = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.betaInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.roInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.clientsCountInput)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.richTextBox2);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(893, 359);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Variants";
            // 
            // richTextBox2
            // 
            this.richTextBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox2.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.richTextBox2.Location = new System.Drawing.Point(4, 26);
            this.richTextBox2.Margin = new System.Windows.Forms.Padding(4);
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.Size = new System.Drawing.Size(885, 329);
            this.richTextBox2.TabIndex = 0;
            this.richTextBox2.Text = "";
            // 
            // vInputsText
            // 
            this.vInputsText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vInputsText.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.vInputsText.Location = new System.Drawing.Point(4, 26);
            this.vInputsText.Margin = new System.Windows.Forms.Padding(4);
            this.vInputsText.Name = "vInputsText";
            this.vInputsText.Size = new System.Drawing.Size(412, 318);
            this.vInputsText.TabIndex = 0;
            this.vInputsText.Text = "";
            this.vInputsText.TextChanged += new System.EventHandler(this.vInputsText_TextChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.vInputsText);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(420, 348);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Input vectors";
            // 
            // itemsInputText
            // 
            this.itemsInputText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.itemsInputText.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.itemsInputText.Location = new System.Drawing.Point(4, 26);
            this.itemsInputText.Margin = new System.Windows.Forms.Padding(4);
            this.itemsInputText.Name = "itemsInputText";
            this.itemsInputText.Size = new System.Drawing.Size(460, 318);
            this.itemsInputText.TabIndex = 0;
            this.itemsInputText.Text = "";
            this.itemsInputText.TextChanged += new System.EventHandler(this.itemsInputText_TextChanged);
            // 
            // generateRandomBtn
            // 
            this.generateRandomBtn.Location = new System.Drawing.Point(15, 13);
            this.generateRandomBtn.Margin = new System.Windows.Forms.Padding(4);
            this.generateRandomBtn.Name = "generateRandomBtn";
            this.generateRandomBtn.Size = new System.Drawing.Size(206, 32);
            this.generateRandomBtn.TabIndex = 1;
            this.generateRandomBtn.Text = "Generate Random";
            this.generateRandomBtn.UseVisualStyleBackColor = true;
            this.generateRandomBtn.Click += new System.EventHandler(this.GenerateRandomClick);
            // 
            // panel1
            // 
            this.panel1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.panel1.Controls.Add(this.betaInput);
            this.panel1.Controls.Add(this.roInput);
            this.panel1.Controls.Add(this.clientsCountInput);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.generateRandomBtn);
            this.panel1.Location = new System.Drawing.Point(200, 744);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(567, 59);
            this.panel1.TabIndex = 2;
            // 
            // betaInput
            // 
            this.betaInput.DecimalPlaces = 1;
            this.betaInput.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.betaInput.Location = new System.Drawing.Point(508, 16);
            this.betaInput.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.betaInput.Name = "betaInput";
            this.betaInput.Size = new System.Drawing.Size(46, 29);
            this.betaInput.TabIndex = 3;
            this.betaInput.Value = new decimal(new int[] {
            8,
            0,
            0,
            65536});
            this.betaInput.ValueChanged += new System.EventHandler(this.betaInput_ValueChanged);
            // 
            // roInput
            // 
            this.roInput.DecimalPlaces = 1;
            this.roInput.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.roInput.Location = new System.Drawing.Point(394, 16);
            this.roInput.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.roInput.Name = "roInput";
            this.roInput.Size = new System.Drawing.Size(50, 29);
            this.roInput.TabIndex = 3;
            this.roInput.Value = new decimal(new int[] {
            3,
            0,
            0,
            65536});
            this.roInput.ValueChanged += new System.EventHandler(this.roInput_ValueChanged);
            // 
            // clientsCountInput
            // 
            this.clientsCountInput.Location = new System.Drawing.Point(307, 17);
            this.clientsCountInput.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.clientsCountInput.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.clientsCountInput.Name = "clientsCountInput";
            this.clientsCountInput.Size = new System.Drawing.Size(46, 29);
            this.clientsCountInput.TabIndex = 3;
            this.clientsCountInput.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.clientsCountInput.ValueChanged += new System.EventHandler(this.clientsCountInput_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(462, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 21);
            this.label3.TabIndex = 2;
            this.label3.Text = "Beta";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(369, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 21);
            this.label2.TabIndex = 2;
            this.label2.Text = "Ro";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(244, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 21);
            this.label1.TabIndex = 2;
            this.label1.Text = "Clients";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.itemsInputText);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox3.Size = new System.Drawing.Size(468, 348);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Items vector";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox3);
            this.splitContainer1.Size = new System.Drawing.Size(893, 348);
            this.splitContainer1.SplitterDistance = 420;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 5;
            this.splitContainer1.Text = "splitContainer1";
            // 
            // splitContainer2
            // 
            this.splitContainer2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer2.Location = new System.Drawing.Point(13, 13);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.splitContainer1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer2.Size = new System.Drawing.Size(893, 711);
            this.splitContainer2.SplitterDistance = 348;
            this.splitContainer2.TabIndex = 6;
            this.splitContainer2.Text = "splitContainer2";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(918, 816);
            this.Controls.Add(this.splitContainer2);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(500, 500);
            this.Name = "MainForm";
            this.Text = "ART1";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.betaInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.roInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.clientsCountInput)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RichTextBox itemsInputText;
        private System.Windows.Forms.Button generateRandomBtn;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RichTextBox richTextBox2;
        private System.Windows.Forms.RichTextBox vInputsText;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.NumericUpDown clientsCountInput;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown betaInput;
        private System.Windows.Forms.NumericUpDown roInput;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.SplitContainer splitContainer2;
    }
}

