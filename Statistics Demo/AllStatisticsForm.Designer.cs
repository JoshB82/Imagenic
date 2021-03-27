
namespace Statistics_Demo
{
    partial class AllStatisticsForm
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
            this.listView = new System.Windows.Forms.ListView();
            this.IdHeader = new System.Windows.Forms.ColumnHeader();
            this.TypeHeader = new System.Windows.Forms.ColumnHeader();
            this.WorldOriginHeader = new System.Windows.Forms.ColumnHeader();
            this.WorldDirectionForwardHeader = new System.Windows.Forms.ColumnHeader();
            this.WorldDirectionUpHeader = new System.Windows.Forms.ColumnHeader();
            this.WorldDirectionRightHeader = new System.Windows.Forms.ColumnHeader();
            this.SuspendLayout();
            // 
            // listView
            // 
            this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.IdHeader,
            this.TypeHeader,
            this.WorldOriginHeader,
            this.WorldDirectionForwardHeader,
            this.WorldDirectionUpHeader,
            this.WorldDirectionRightHeader});
            this.listView.HideSelection = false;
            this.listView.Location = new System.Drawing.Point(12, 12);
            this.listView.Name = "listView";
            this.listView.Size = new System.Drawing.Size(676, 314);
            this.listView.TabIndex = 0;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = System.Windows.Forms.View.Details;
            // 
            // IdHeader
            // 
            this.IdHeader.Text = "Id";
            this.IdHeader.Width = 30;
            // 
            // TypeHeader
            // 
            this.TypeHeader.Text = "Type";
            this.TypeHeader.Width = 80;
            // 
            // WorldOriginHeader
            // 
            this.WorldOriginHeader.Text = "World Origin";
            this.WorldOriginHeader.Width = 120;
            // 
            // WorldDirectionForwardHeader
            // 
            this.WorldDirectionForwardHeader.Text = "World Direction Forward";
            this.WorldDirectionForwardHeader.Width = 120;
            // 
            // WorldDirectionUpHeader
            // 
            this.WorldDirectionUpHeader.Text = "WorldDirectionUp";
            this.WorldDirectionUpHeader.Width = 120;
            // 
            // WorldDirectionRightHeader
            // 
            this.WorldDirectionRightHeader.Text = "World Direction Right";
            this.WorldDirectionRightHeader.Width = 120;
            // 
            // AllStatisticsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 338);
            this.Controls.Add(this.listView);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "AllStatisticsForm";
            this.Text = "All Statistics";
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.ListView listView;
        private System.Windows.Forms.ColumnHeader IdHeader;
        private System.Windows.Forms.ColumnHeader TypeHeader;
        private System.Windows.Forms.ColumnHeader WorldOriginHeader;
        private System.Windows.Forms.ColumnHeader WorldDirectionForwardHeader;
        private System.Windows.Forms.ColumnHeader WorldDirectionUpHeader;
        private System.Windows.Forms.ColumnHeader WorldDirectionRightHeader;
    }
}