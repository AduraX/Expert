namespace AdureX_Lib
{
    partial class Canvas
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pnGraph = new System.Windows.Forms.Panel();
            this.cmCanvas = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmResizeDrag = new System.Windows.Forms.ToolStripMenuItem();
            this.cmXdrag = new System.Windows.Forms.ToolStripMenuItem();
            this.cmYdrag = new System.Windows.Forms.ToolStripMenuItem();
            this.cmResize = new System.Windows.Forms.ToolStripMenuItem();
            this.cmXmax = new System.Windows.Forms.ToolStripMenuItem();
            this.cmXmin = new System.Windows.Forms.ToolStripMenuItem();
            this.cmYmax = new System.Windows.Forms.ToolStripMenuItem();
            this.cmYmin = new System.Windows.Forms.ToolStripMenuItem();
            this.cmColor = new System.Windows.Forms.ToolStripMenuItem();
            this.dataPointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnDpColour = new System.Windows.Forms.ToolStripMenuItem();
            this.mcDpWidth = new System.Windows.Forms.ToolStripComboBox();
            this.mnError = new System.Windows.Forms.ToolStripMenuItem();
            this.fitLineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnFlColour = new System.Windows.Forms.ToolStripMenuItem();
            this.mcFlWidth = new System.Windows.Forms.ToolStripComboBox();
            this.mnDefault = new System.Windows.Forms.ToolStripMenuItem();
            this.txRefPoint = new System.Windows.Forms.ToolStripTextBox();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.printDialog1 = new System.Windows.Forms.PrintDialog();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.txCursorPosition = new System.Windows.Forms.TextBox();
            this.ntxXmax = new AdureX_Lib.NumTextBox();
            this.ntxXmin = new AdureX_Lib.NumTextBox();
            this.ntxYmin = new AdureX_Lib.NumTextBox();
            this.ntxYmax = new AdureX_Lib.NumTextBox();
            this.cmCanvas.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnGraph
            // 
            this.pnGraph.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pnGraph.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnGraph.BackColor = System.Drawing.Color.White;
            this.pnGraph.Cursor = System.Windows.Forms.Cursors.Cross;
            this.pnGraph.Location = new System.Drawing.Point(70, 20);
            this.pnGraph.Name = "pnGraph";
            this.pnGraph.Size = new System.Drawing.Size(800, 600);
            this.pnGraph.TabIndex = 1;
            this.pnGraph.Paint += new System.Windows.Forms.PaintEventHandler(this.pnGraph_Paint);
            this.pnGraph.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pnGraph_MouseMove);
            this.pnGraph.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnGraph_MouseDown);
            this.pnGraph.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pnGraph_MouseUp);
            // 
            // cmCanvas
            // 
            this.cmCanvas.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmResizeDrag,
            this.cmResize,
            this.cmColor,
            this.txRefPoint});
            this.cmCanvas.Name = "cmCanvas";
            this.cmCanvas.Size = new System.Drawing.Size(161, 95);
            this.cmCanvas.Closed += new System.Windows.Forms.ToolStripDropDownClosedEventHandler(this.cmCanvas_Closed);
            // 
            // cmResizeDrag
            // 
            this.cmResizeDrag.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmXdrag,
            this.cmYdrag});
            this.cmResizeDrag.Name = "cmResizeDrag";
            this.cmResizeDrag.Size = new System.Drawing.Size(160, 22);
            this.cmResizeDrag.Text = "ResizeDrag";
            // 
            // cmXdrag
            // 
            this.cmXdrag.Name = "cmXdrag";
            this.cmXdrag.Size = new System.Drawing.Size(119, 22);
            this.cmXdrag.Text = "Along X";
            this.cmXdrag.Click += new System.EventHandler(this.cmXdrag_Click);
            // 
            // cmYdrag
            // 
            this.cmYdrag.Name = "cmYdrag";
            this.cmYdrag.Size = new System.Drawing.Size(119, 22);
            this.cmYdrag.Text = "Along Y ";
            this.cmYdrag.Click += new System.EventHandler(this.cmYdrag_Click);
            // 
            // cmResize
            // 
            this.cmResize.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmXmax,
            this.cmXmin,
            this.cmYmax,
            this.cmYmin});
            this.cmResize.Name = "cmResize";
            this.cmResize.Size = new System.Drawing.Size(160, 22);
            this.cmResize.Text = "Resize";
            // 
            // cmXmax
            // 
            this.cmXmax.Name = "cmXmax";
            this.cmXmax.Size = new System.Drawing.Size(108, 22);
            this.cmXmax.Text = "X-Max";
            this.cmXmax.Click += new System.EventHandler(this.cmXmax_Click);
            // 
            // cmXmin
            // 
            this.cmXmin.Name = "cmXmin";
            this.cmXmin.Size = new System.Drawing.Size(108, 22);
            this.cmXmin.Text = "X-Min";
            this.cmXmin.Click += new System.EventHandler(this.cmXmin_Click);
            // 
            // cmYmax
            // 
            this.cmYmax.Name = "cmYmax";
            this.cmYmax.Size = new System.Drawing.Size(108, 22);
            this.cmYmax.Text = "Y-Max";
            this.cmYmax.Click += new System.EventHandler(this.cmYmax_Click);
            // 
            // cmYmin
            // 
            this.cmYmin.Name = "cmYmin";
            this.cmYmin.Size = new System.Drawing.Size(108, 22);
            this.cmYmin.Text = "Y-Min";
            this.cmYmin.Click += new System.EventHandler(this.cmYmin_Click);
            // 
            // cmColor
            // 
            this.cmColor.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dataPointToolStripMenuItem,
            this.mnError,
            this.fitLineToolStripMenuItem,
            this.mnDefault});
            this.cmColor.Name = "cmColor";
            this.cmColor.Size = new System.Drawing.Size(160, 22);
            this.cmColor.Text = "Color";
            this.cmColor.DropDownOpening += new System.EventHandler(this.cmColor_DropDownOpening);
            // 
            // dataPointToolStripMenuItem
            // 
            this.dataPointToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnDpColour,
            this.mcDpWidth});
            this.dataPointToolStripMenuItem.Name = "dataPointToolStripMenuItem";
            this.dataPointToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.dataPointToolStripMenuItem.Text = "Data point";
            // 
            // mnDpColour
            // 
            this.mnDpColour.Name = "mnDpColour";
            this.mnDpColour.Size = new System.Drawing.Size(181, 22);
            this.mnDpColour.Text = "Color";
            this.mnDpColour.Click += new System.EventHandler(this.mnDpColour_Click);
            // 
            // mcDpWidth
            // 
            this.mcDpWidth.Items.AddRange(new object[] {
            "0.5",
            "1.0",
            "1.5",
            "2.0",
            "2.5",
            "3.0",
            "3.5",
            "4.0"});
            this.mcDpWidth.Name = "mcDpWidth";
            this.mcDpWidth.Size = new System.Drawing.Size(121, 23);
            this.mcDpWidth.DropDownClosed += new System.EventHandler(this.mcDpWidth_DropDownClosed);
            // 
            // mnError
            // 
            this.mnError.Name = "mnError";
            this.mnError.Size = new System.Drawing.Size(129, 22);
            this.mnError.Text = "Error";
            this.mnError.Click += new System.EventHandler(this.mnError_Click);
            // 
            // fitLineToolStripMenuItem
            // 
            this.fitLineToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnFlColour,
            this.mcFlWidth});
            this.fitLineToolStripMenuItem.Name = "fitLineToolStripMenuItem";
            this.fitLineToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.fitLineToolStripMenuItem.Text = "Fit line";
            // 
            // mnFlColour
            // 
            this.mnFlColour.Name = "mnFlColour";
            this.mnFlColour.Size = new System.Drawing.Size(181, 22);
            this.mnFlColour.Text = "Color";
            this.mnFlColour.Click += new System.EventHandler(this.mnFlColour_Click);
            // 
            // mcFlWidth
            // 
            this.mcFlWidth.Items.AddRange(new object[] {
            "0.5",
            "1.0",
            "1.5",
            "2.0",
            "2.5",
            "3.0",
            "3.5",
            "4.0"});
            this.mcFlWidth.Name = "mcFlWidth";
            this.mcFlWidth.Size = new System.Drawing.Size(121, 23);
            this.mcFlWidth.DropDownClosed += new System.EventHandler(this.mcFlWidth_DropDownClosed);
            // 
            // mnDefault
            // 
            this.mnDefault.Name = "mnDefault";
            this.mnDefault.Size = new System.Drawing.Size(129, 22);
            this.mnDefault.Text = "Default";
            this.mnDefault.Click += new System.EventHandler(this.mnDefault_Click);
            // 
            // txRefPoint
            // 
            this.txRefPoint.Name = "txRefPoint";
            this.txRefPoint.Size = new System.Drawing.Size(100, 23);
            this.txRefPoint.Text = "Ref. Point";
            this.txRefPoint.TextChanged += new System.EventHandler(this.txRefPoint_TextChanged);
            // 
            // printDialog1
            // 
            this.printDialog1.Document = this.printDocument1;
            this.printDialog1.UseEXDialog = true;
            // 
            // printDocument1
            // 
            this.printDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument1_PrintPage);
            // 
            // txCursorPosition
            // 
            this.txCursorPosition.BackColor = System.Drawing.Color.White;
            this.txCursorPosition.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txCursorPosition.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txCursorPosition.ForeColor = System.Drawing.Color.Green;
            this.txCursorPosition.Location = new System.Drawing.Point(297, 0);
            this.txCursorPosition.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.txCursorPosition.MinimumSize = new System.Drawing.Size(2, 20);
            this.txCursorPosition.Name = "txCursorPosition";
            this.txCursorPosition.ReadOnly = true;
            this.txCursorPosition.Size = new System.Drawing.Size(390, 17);
            this.txCursorPosition.TabIndex = 7;
            this.txCursorPosition.Text = "Cursor Position";
            this.txCursorPosition.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ntxXmax
            // 
            this.ntxXmax.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ntxXmax.Location = new System.Drawing.Point(837, 635);
            this.ntxXmax.Name = "ntxXmax";
            this.ntxXmax.Size = new System.Drawing.Size(35, 20);
            this.ntxXmax.TabIndex = 6;
            this.ntxXmax.Text = "10";
            this.ntxXmax.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.ntxXmax.Type = "int";
            // 
            // ntxXmin
            // 
            this.ntxXmin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ntxXmin.Location = new System.Drawing.Point(61, 635);
            this.ntxXmin.Name = "ntxXmin";
            this.ntxXmin.Size = new System.Drawing.Size(35, 20);
            this.ntxXmin.TabIndex = 0;
            this.ntxXmin.Text = "0";
            this.ntxXmin.Type = "int";
            // 
            // ntxYmin
            // 
            this.ntxYmin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ntxYmin.Location = new System.Drawing.Point(1, 607);
            this.ntxYmin.Name = "ntxYmin";
            this.ntxYmin.Size = new System.Drawing.Size(35, 20);
            this.ntxYmin.TabIndex = 0;
            this.ntxYmin.Text = "0";
            this.ntxYmin.Type = "double";
            // 
            // ntxYmax
            // 
            this.ntxYmax.Location = new System.Drawing.Point(1, 14);
            this.ntxYmax.Name = "ntxYmax";
            this.ntxYmax.Size = new System.Drawing.Size(35, 20);
            this.ntxYmax.TabIndex = 0;
            this.ntxYmax.Text = "10";
            this.ntxYmax.Type = "double";
            // 
            // Canvas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Silver;
            this.Controls.Add(this.ntxXmax);
            this.Controls.Add(this.ntxXmin);
            this.Controls.Add(this.ntxYmin);
            this.Controls.Add(this.ntxYmax);
            this.Controls.Add(this.pnGraph);
            this.Controls.Add(this.txCursorPosition);
            this.Name = "Canvas";
            this.Size = new System.Drawing.Size(873, 656);
            this.Load += new System.EventHandler(this.Canvas_Load);
            this.cmCanvas.ResumeLayout(false);
            this.cmCanvas.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnGraph;
        private System.Windows.Forms.ContextMenuStrip cmCanvas;
        private System.Windows.Forms.ToolStripMenuItem cmResize;
        private System.Windows.Forms.ToolStripMenuItem cmColor;
        private System.Windows.Forms.ToolStripMenuItem cmYmax;
        private System.Windows.Forms.ToolStripMenuItem cmYmin;
        private System.Windows.Forms.ToolStripMenuItem cmXmax;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.ToolStripMenuItem cmXmin;
        private System.Windows.Forms.ToolStripMenuItem dataPointToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnDpColour;
        private System.Windows.Forms.ToolStripComboBox mcDpWidth;
        private System.Windows.Forms.ToolStripMenuItem mnError;
        private System.Windows.Forms.ToolStripMenuItem fitLineToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnFlColour;
        private System.Windows.Forms.ToolStripComboBox mcFlWidth;
        private System.Windows.Forms.ToolStripMenuItem mnDefault;
        private System.Windows.Forms.PrintDialog printDialog1;
        private System.Drawing.Printing.PrintDocument printDocument1;
        private System.Windows.Forms.ToolStripTextBox txRefPoint;
        private NumTextBox ntxYmax;
        private NumTextBox ntxYmin;
        private NumTextBox ntxXmin;
        private NumTextBox ntxXmax;
        private System.Windows.Forms.TextBox txCursorPosition;
        private System.Windows.Forms.ToolStripMenuItem cmResizeDrag;
        private System.Windows.Forms.ToolStripMenuItem cmYdrag;
        private System.Windows.Forms.ToolStripMenuItem cmXdrag;
    }
}