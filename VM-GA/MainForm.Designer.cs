namespace VM_GA
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.runBtn = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pxInputLabel = new System.Windows.Forms.Label();
            this.pmInputLabel = new System.Windows.Forms.Label();
            this.pxInput = new System.Windows.Forms.TextBox();
            this.pmInput = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.generationsCount = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.chromosomesCount = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.chromosomesCount);
            this.splitContainer1.Panel2.Controls.Add(this.generationsCount);
            this.splitContainer1.Panel2.Controls.Add(this.pmInput);
            this.splitContainer1.Panel2.Controls.Add(this.pxInput);
            this.splitContainer1.Panel2.Controls.Add(this.label3);
            this.splitContainer1.Panel2.Controls.Add(this.label2);
            this.splitContainer1.Panel2.Controls.Add(this.pmInputLabel);
            this.splitContainer1.Panel2.Controls.Add(this.pxInputLabel);
            this.splitContainer1.Panel2.Controls.Add(this.cancelBtn);
            this.splitContainer1.Panel2.Controls.Add(this.runBtn);
            this.splitContainer1.Panel2.Controls.Add(this.richTextBox1);
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Size = new System.Drawing.Size(1029, 815);
            this.splitContainer1.SplitterDistance = 642;
            this.splitContainer1.SplitterWidth = 7;
            this.splitContainer1.TabIndex = 0;
            this.splitContainer1.Text = "splitContainer1";
            // 
            // cancelBtn
            // 
            this.cancelBtn.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.cancelBtn.Location = new System.Drawing.Point(202, 726);
            this.cancelBtn.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(164, 74);
            this.cancelBtn.TabIndex = 3;
            this.cancelBtn.Text = "Cancel";
            this.cancelBtn.UseVisualStyleBackColor = true;
            this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
            // 
            // runBtn
            // 
            this.runBtn.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.runBtn.Location = new System.Drawing.Point(17, 726);
            this.runBtn.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.runBtn.Name = "runBtn";
            this.runBtn.Size = new System.Drawing.Size(164, 74);
            this.runBtn.TabIndex = 3;
            this.runBtn.Text = "Run";
            this.runBtn.UseVisualStyleBackColor = true;
            this.runBtn.Click += new System.EventHandler(this.runBtn_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox1.Location = new System.Drawing.Point(21, 87);
            this.richTextBox1.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(345, 388);
            this.richTextBox1.TabIndex = 2;
            this.richTextBox1.Text = "";
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(152, 44);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 37);
            this.label1.TabIndex = 0;
            this.label1.Text = "Result";
            // 
            // pxInputLabel
            // 
            this.pxInputLabel.AutoSize = true;
            this.pxInputLabel.Location = new System.Drawing.Point(21, 504);
            this.pxInputLabel.Name = "pxInputLabel";
            this.pxInputLabel.Size = new System.Drawing.Size(47, 37);
            this.pxInputLabel.TabIndex = 4;
            this.pxInputLabel.Text = "PX";
            // 
            // pmInputLabel
            // 
            this.pmInputLabel.AutoSize = true;
            this.pmInputLabel.Location = new System.Drawing.Point(21, 559);
            this.pmInputLabel.Name = "pmInputLabel";
            this.pmInputLabel.Size = new System.Drawing.Size(56, 37);
            this.pmInputLabel.TabIndex = 4;
            this.pmInputLabel.Text = "PM";
            // 
            // pxInput
            // 
            this.pxInput.Location = new System.Drawing.Point(85, 504);
            this.pxInput.Name = "pxInput";
            this.pxInput.Size = new System.Drawing.Size(127, 43);
            this.pxInput.TabIndex = 5;
            this.pxInput.TextChanged += new System.EventHandler(this.pxInput_TextChanged);
            // 
            // pmInput
            // 
            this.pmInput.Location = new System.Drawing.Point(85, 556);
            this.pmInput.Name = "pmInput";
            this.pmInput.Size = new System.Drawing.Size(127, 43);
            this.pmInput.TabIndex = 5;
            this.pmInput.TextChanged += new System.EventHandler(this.pmInput_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 613);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(160, 37);
            this.label2.TabIndex = 4;
            this.label2.Text = "Generations";
            // 
            // generationsCount
            // 
            this.generationsCount.Location = new System.Drawing.Point(187, 607);
            this.generationsCount.Name = "generationsCount";
            this.generationsCount.Size = new System.Drawing.Size(127, 43);
            this.generationsCount.TabIndex = 5;
            this.generationsCount.TextChanged += new System.EventHandler(this.generationsCount_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 663);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(179, 37);
            this.label3.TabIndex = 4;
            this.label3.Text = "Chomosomes";
            // 
            // chromosomesCount
            // 
            this.chromosomesCount.Location = new System.Drawing.Point(206, 663);
            this.chromosomesCount.Name = "chromosomesCount";
            this.chromosomesCount.Size = new System.Drawing.Size(127, 43);
            this.chromosomesCount.TabIndex = 5;
            this.chromosomesCount.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 30F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1029, 815);
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.Button runBtn;
        private System.Windows.Forms.TextBox pmInput;
        private System.Windows.Forms.TextBox pxInput;
        private System.Windows.Forms.Label pmInputLabel;
        private System.Windows.Forms.Label pxInputLabel;
        private System.Windows.Forms.TextBox generationsCount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox chromosomesCount;
        private System.Windows.Forms.Label label3;
    }
}

