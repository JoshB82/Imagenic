
namespace Statistics_Demo
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
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.viewAllButton = new System.Windows.Forms.Button();
            this.noSceneObjectsValueLabel = new System.Windows.Forms.Label();
            this.noSceneObjectsTextLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox
            // 
            this.pictureBox.Location = new System.Drawing.Point(0, 52);
            this.pictureBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(690, 194);
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel1.Controls.Add(this.viewAllButton);
            this.panel1.Controls.Add(this.noSceneObjectsValueLabel);
            this.panel1.Controls.Add(this.noSceneObjectsTextLabel);
            this.panel1.Location = new System.Drawing.Point(10, 9);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(679, 39);
            this.panel1.TabIndex = 1;
            // 
            // viewAllButton
            // 
            this.viewAllButton.Location = new System.Drawing.Point(586, 10);
            this.viewAllButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.viewAllButton.Name = "viewAllButton";
            this.viewAllButton.Size = new System.Drawing.Size(82, 22);
            this.viewAllButton.TabIndex = 2;
            this.viewAllButton.Text = "View All";
            this.viewAllButton.UseVisualStyleBackColor = true;
            this.viewAllButton.Click += new System.EventHandler(this.viewAllButton_Click);
            // 
            // noSceneObjectsValueLabel
            // 
            this.noSceneObjectsValueLabel.AutoSize = true;
            this.noSceneObjectsValueLabel.Location = new System.Drawing.Point(144, 13);
            this.noSceneObjectsValueLabel.Name = "noSceneObjectsValueLabel";
            this.noSceneObjectsValueLabel.Size = new System.Drawing.Size(13, 15);
            this.noSceneObjectsValueLabel.TabIndex = 1;
            this.noSceneObjectsValueLabel.Text = "0";
            // 
            // noSceneObjectsTextLabel
            // 
            this.noSceneObjectsTextLabel.AutoSize = true;
            this.noSceneObjectsTextLabel.Location = new System.Drawing.Point(10, 13);
            this.noSceneObjectsTextLabel.Name = "noSceneObjectsTextLabel";
            this.noSceneObjectsTextLabel.Size = new System.Drawing.Size(117, 15);
            this.noSceneObjectsTextLabel.TabIndex = 0;
            this.noSceneObjectsTextLabel.Text = "No. of SceneObjects:";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 338);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pictureBox);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "MainForm";
            this.Text = "Statistics Demo";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label noSceneObjectsTextLabel;
        private System.Windows.Forms.Label noSceneObjectsValueLabel;
        private System.Windows.Forms.Button viewAllButton;
    }
}

