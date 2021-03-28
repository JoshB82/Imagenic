
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
            this.addCubeButton = new System.Windows.Forms.Button();
            this.addConeButton = new System.Windows.Forms.Button();
            this.addCylinderButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox
            // 
            this.pictureBox.Location = new System.Drawing.Point(11, 70);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(872, 369);
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel1.Controls.Add(this.addCylinderButton);
            this.panel1.Controls.Add(this.addConeButton);
            this.panel1.Controls.Add(this.addCubeButton);
            this.panel1.Controls.Add(this.viewAllButton);
            this.panel1.Controls.Add(this.noSceneObjectsValueLabel);
            this.panel1.Controls.Add(this.noSceneObjectsTextLabel);
            this.panel1.Location = new System.Drawing.Point(11, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(872, 52);
            this.panel1.TabIndex = 1;
            // 
            // viewAllButton
            // 
            this.viewAllButton.Location = new System.Drawing.Point(670, 13);
            this.viewAllButton.Name = "viewAllButton";
            this.viewAllButton.Size = new System.Drawing.Size(94, 29);
            this.viewAllButton.TabIndex = 2;
            this.viewAllButton.Text = "View All";
            this.viewAllButton.UseVisualStyleBackColor = true;
            this.viewAllButton.Click += new System.EventHandler(this.viewAllButton_Click);
            // 
            // noSceneObjectsValueLabel
            // 
            this.noSceneObjectsValueLabel.AutoSize = true;
            this.noSceneObjectsValueLabel.Location = new System.Drawing.Point(165, 17);
            this.noSceneObjectsValueLabel.Name = "noSceneObjectsValueLabel";
            this.noSceneObjectsValueLabel.Size = new System.Drawing.Size(17, 20);
            this.noSceneObjectsValueLabel.TabIndex = 1;
            this.noSceneObjectsValueLabel.Text = "0";
            // 
            // noSceneObjectsTextLabel
            // 
            this.noSceneObjectsTextLabel.AutoSize = true;
            this.noSceneObjectsTextLabel.Location = new System.Drawing.Point(11, 17);
            this.noSceneObjectsTextLabel.Name = "noSceneObjectsTextLabel";
            this.noSceneObjectsTextLabel.Size = new System.Drawing.Size(146, 20);
            this.noSceneObjectsTextLabel.TabIndex = 0;
            this.noSceneObjectsTextLabel.Text = "No. of SceneObjects:";
            // 
            // addCubeButton
            // 
            this.addCubeButton.Location = new System.Drawing.Point(197, 12);
            this.addCubeButton.Name = "addCubeButton";
            this.addCubeButton.Size = new System.Drawing.Size(94, 29);
            this.addCubeButton.TabIndex = 3;
            this.addCubeButton.Text = "Add Cube";
            this.addCubeButton.UseVisualStyleBackColor = true;
            this.addCubeButton.Click += new System.EventHandler(this.addCubeButton_Click);
            // 
            // addConeButton
            // 
            this.addConeButton.Location = new System.Drawing.Point(310, 12);
            this.addConeButton.Name = "addConeButton";
            this.addConeButton.Size = new System.Drawing.Size(94, 29);
            this.addConeButton.TabIndex = 4;
            this.addConeButton.Text = "Add Cone";
            this.addConeButton.UseVisualStyleBackColor = true;
            this.addConeButton.Click += new System.EventHandler(this.addConeButton_Click);
            // 
            // addCylinderButton
            // 
            this.addCylinderButton.Location = new System.Drawing.Point(425, 12);
            this.addCylinderButton.Name = "addCylinderButton";
            this.addCylinderButton.Size = new System.Drawing.Size(114, 29);
            this.addCylinderButton.TabIndex = 5;
            this.addCylinderButton.Text = "Add Cylinder";
            this.addCylinderButton.UseVisualStyleBackColor = true;
            this.addCylinderButton.Click += new System.EventHandler(this.addCylinderButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(895, 451);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pictureBox);
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
        private System.Windows.Forms.Button addCylinderButton;
        private System.Windows.Forms.Button addConeButton;
        private System.Windows.Forms.Button addCubeButton;
    }
}

