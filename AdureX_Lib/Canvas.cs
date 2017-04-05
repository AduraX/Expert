using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Timers;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace AdureX_Lib
{
    public partial class Canvas : UserControl
    {        
        Color dc = Color.Red; float dcw = 1.0f; Color fc = Color.Green; float fcw = 2.0f; Color ec = Color.Blue;
        float fcw_f0 = 2.0f; Color fc_f0 = Color.Red; float fcw_fx = 2.0f; Color fc_f1 = Color.Blue; Color fc_f2 = Color.Green ;
        Color fc_f3 = Color.Violet; Color fc_f4 = Color.Brown; Color fc_f5 = Color.BlueViolet ; Color fc_f6 = Color.Gold ; Color fc_f7 = Color.GreenYellow;
        int Nf = 0; int uy = 0; Pen q; Pen fq;//int Py = 0; int Sy = 0;
        float Xrange; float Yrange; float MaxX = 1; float MinX = 0; float MaxY = 1; float MinY = 0; float refPoint = 0; float Mouse_Down = 0; float Mouse_Up = 0;
        float X_1unit = 0; float Y_1unit = 0.0f; float My = 0; float tmx; float tmy; float X0 = 0.0f; float Y0 = 0.0f;
        string hold = "yes"; string sRefPoint = "no"; string Headnote = ""; string Mouse_Move = "no"; 
        DateTime date = DateTime.Now;

        public Canvas()
        {
            InitializeComponent();
            this.printDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument1_PrintPage);
            pnGraph.ContextMenuStrip = cmCanvas;
        }

        private bool isInt = true;
        private bool isRt = true;
        private bool symbol = false;
        private bool fit_ = false;
        private String xLabel = "xLabel";
        private String yLabel = "yLabel";
        private string dName = "";
        private string pName = "";
        private string def_1st = " ";
        private float fact = 1;
        private float mx = 0;
        private int maxPt = 100;
        private int[] xdata;
        private float[] ydata;
        private float[] edata;
        private int[] combNo;
        private int startPt_f0 = 0;
        private int stopPt_f0 = 100;
        private int[] xdata_f0;
        private float[] ydata_f0;
        private int[] xdata_f1;
        private float[] ydata_f1;
        private int[] xdata_f2;
        private float[] ydata_f2;
        private int[] xdata_f3;
        private float[] ydata_f3;
        private int[] xdata_f4;
        private float[] ydata_f4;
        private int[] xdata_f5;
        private float[] ydata_f5;
        private int[] xdata_f6;
        private float[]ydata_f6;
        private int[] xdata_f7;
        private float[] ydata_f7;
        private int fmaxPt = 100;
        private int[] fxdata;
        private float[] fydata;
        private float dfMaxX = 0.0f;
        private float dfMinX = 0.0f;
        private float dfMaxY = 0.0f;
        private float dfMinY = 0.0f;

        void Mouse_Position(object sender, MouseEventArgs h)
        {
            Mx = MinX + (h.X) * (MaxX - MinX) / pnGraph.Width;
            My = MinY + (pnGraph.Height - 1 - h.Y) * (MaxY - MinY) / pnGraph.Height;
            if (isRt == true) txCursorPosition.Text = "[ time = " + Math.Round(fact * Mx).ToString() + "  (Chn = " + Math.Round(Mx).ToString() + ");   Counts = " + My.ToString("N4") + " ]";
            else txCursorPosition.Text = "[ Freq = " + Math.Round(Mx).ToString() + " ;   Counts = " + My.ToString("N4") + " ]";
        }
        int hxM = 0; int hyD = 0; int hDn = 0; int hyM = 0; int hxD = 0;  int sign = 0; int signx = 0;
        private void pnGraph_MouseMove(object sender, MouseEventArgs h)
        {
            Graphics Gml = pnGraph.CreateGraphics(); Pen Pml = new Pen(Color.Gray, 1.5f);
            if (pnGraph.Cursor == Cursors.SizeWE)
            {                
                if (Mouse_Move == "yes")
                {                    
                    if (h.X - hxM > 0) sign = 1;
                    else if (h.X - hxM < 0) sign = -1;
                    if (sign != signx) hDn += 15;
                    Gml.DrawLine(Pml, h.X, hyD - hDn, h.X, hyD + hDn);
                    hxM = h.X;
                    signx = sign;
                }                
            }
            if (pnGraph.Cursor == Cursors.SizeNS)
            {
                if (Mouse_Move == "yes")
                {
                    if (h.Y - hyM > 0) sign = 1;
                    else if (h.Y - hyM < 0) sign = -1;
                    if (sign != signx) hDn += 15;
                    Gml.DrawLine(Pml, hxD - hDn, h.Y, hxD + hDn, h.Y);
                    hyM = h.Y;
                    signx = sign;
                }
            }
        }
        private void pnGraph_MouseDown(object sender, MouseEventArgs h)
        {
            Graphics Gml = pnGraph.CreateGraphics(); Pen Pml = new Pen(Color.Gray, 1.5f);
            Mouse_Position(sender,h);
            if (pnGraph.Cursor == Cursors.SizeWE)
            {
                Mouse_Down = (int)(Mx * fact);
                hxM = h.X; hyD = h.Y;
            }
            if (pnGraph.Cursor == Cursors.SizeNS)
            {
                Mouse_Down = My;
                hyM = h.Y; hxD = h.X;
            }
            Mouse_Move = "yes";
        }
        private void pnGraph_MouseUp(object sender, MouseEventArgs e)
        {
            Mouse_Move = "no";
            if (pnGraph.Cursor == Cursors.SizeWE)
            {
                Mouse_Position(sender, e);
                Mouse_Up = (int)(Mx * fact);
                if (Mouse_Up > Mouse_Down)
                {
                    ntxXmin.Text = Mouse_Down.ToString();
                    ntxXmax.Text = Mouse_Up.ToString();
                }
                else if (Mouse_Up < Mouse_Down)
                {
                    ntxXmin.Text = Mouse_Up.ToString();
                    ntxXmax.Text = Mouse_Down.ToString();
                }
                Replot();
                sign = 0; signx = 0; hDn = 0;
                pnGraph.Cursor = Cursors.Cross;
            }
            if (pnGraph.Cursor == Cursors.SizeNS)
            {
                Mouse_Position(sender, e);
                Mouse_Up = My;
                if (Mouse_Up > Mouse_Down)
                {
                    ntxYmin.Text = Mouse_Down.ToString();
                    ntxYmax.Text = Mouse_Up.ToString();
                }
                else if (Mouse_Up < Mouse_Down)
                {
                    ntxYmin.Text = Mouse_Up.ToString();
                    ntxYmax.Text = Mouse_Down.ToString();
                }
                Replot();
                pnGraph.Cursor = Cursors.Cross;
            }
        }
        private void cmYdrag_Click(object sender, EventArgs e)
        {
            pnGraph.Cursor = Cursors.SizeNS;
        }
        private void cmXdrag_Click(object sender, EventArgs e)
        {
            pnGraph.Cursor = Cursors.SizeWE;
        } 
        private void cmYmax_Click(object sender, EventArgs e)
        {
           ntxYmax.Text = My.ToString();
           Replot();
        }
        private void cmYmin_Click(object sender, EventArgs e)
        {
            ntxYmin.Text = My.ToString();
            Replot();
        }
        private void cmXmax_Click(object sender, EventArgs e)
        {
            ntxXmax.Text = ((int)(Mx*fact)).ToString();
            Replot();
        }
        private void cmXmin_Click(object sender, EventArgs e)
        {
            ntxXmin.Text = ((int)(Mx * fact)).ToString();
            Replot();
        }
        private void mnError_Click(object sender, EventArgs e)
        {
            colorDialog1.AllowFullOpen = false;
            colorDialog1.ShowHelp = true;
            colorDialog1.Color = ec;
            if (colorDialog1.ShowDialog() == DialogResult.OK) { ec = colorDialog1.Color; }
            Replot();
        }
        private void mnDpColour_Click(object sender, EventArgs e)
        {
            colorDialog1.AllowFullOpen = true;
            colorDialog1.ShowHelp = true;
            colorDialog1.Color = dc;
            if (colorDialog1.ShowDialog() == DialogResult.OK) { dc = colorDialog1.Color; }
            Replot();
        }
        private void mnFlColour_Click(object sender, EventArgs e)
        {
            colorDialog1.AllowFullOpen = true;
            colorDialog1.ShowHelp = true;
            colorDialog1.Color = fc;
            if (colorDialog1.ShowDialog() == DialogResult.OK) { fc = colorDialog1.Color; }
            Replot();
        }
        private void mcDpWidth_DropDownClosed(object sender, EventArgs e)
        {
            if (Symbol == false) dcw = float.Parse((string)mcDpWidth.SelectedItem);
            Replot();
        }
        private void mcFlWidth_DropDownClosed(object sender, EventArgs e)
        {
            fcw = float.Parse((string)mcFlWidth.SelectedItem);
            Replot();
        }
        private void mnDefault_Click(object sender, EventArgs e)
        {
            dc = Color.Red; dcw = 1.0f; ec = Color.Blue; fc = Color.Green; fcw = 2.0f;
            Replot();
        }
        private void cmColor_DropDownOpening(object sender, EventArgs e)
        {
            mcDpWidth.Text = dcw.ToString();
            mcFlWidth.Text = fcw.ToString();

        }
        private void txRefPoint_TextChanged(object sender, EventArgs e)
        {
            try 
            {  
                refPoint = float.Parse(txRefPoint.Text); 
                sRefPoint = "yes";
                Replot();
            }
            catch 
            {
                Replot();   sRefPoint = "no";
            }
        }
        private void cmCanvas_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            if (sRefPoint == "no") txRefPoint.Text = "Ref. Point";
        }         
        private void pnGraph_Paint(object sender, PaintEventArgs e)
        {
            if(hold != "yes")Replot();
        }

        public bool IsInt
        {
            get { return isInt; }
            set { isInt = value; }
        }
        public bool IsRt
        {
            get { return isRt; }
            set { isRt = value; }
        }
        public bool Symbol
        {
            get { return symbol; }
            set { symbol = value; }
        }
        public bool Fit_
        {
            get { return fit_; }
            set { fit_ = value; }
        }
        public String XLabel
        {
            get { return xLabel; }
            set { xLabel = value; }
        }
        public String YLabel
        {
            get { return yLabel; }
            set { yLabel = value; }
        }
        public string DName
        {
            get { return dName; }
            set {dName = value; }
        }
        public string PName
        {
            get { return pName; }
            set { pName = value; }
        }
        public string Def_1st
        {
            get { return def_1st; }
            set { def_1st = value; }
        }
       
        public float DfMaxX
        {
            get { return dfMaxX; }
            set { dfMaxX = value; }
        }
        public float DfMaxY
        {
            get { return dfMaxY; }
            set { dfMaxY = value; }
        }
        public float DfMinX
        {
            get { return dfMinX; }
            set { dfMinX = value; }
        }
        public float DfMinY
        {
            get { return dfMinY; }
            set { dfMinY = value; }
        }
        public float Mx
        {
            get { return mx; }
            set { mx = value; }
        }
        public float Fact
        {
            get { return fact; }
            set { fact = value; }
        }
        public float RefPoint
        {
            get { return refPoint; }
            set { refPoint = value; }
        }
        public int MaxPt
        {
            get { return maxPt; }
            set { maxPt = value; }
        }
        public int[] Xdata //raw data
        {
            get { return xdata; }
            set { xdata = value; }
        }
        public float[] Ydata
        {
            get { return ydata; }
            set { ydata = value; }
        }
        public float[] Edata //raw error data
        {
            get { return edata; }
            set { edata = value; }
        }
        public int[] CombNo  //raw error data
        {
            get { return combNo; }
            set { combNo = value; }
        }
        public int StartPt_f0
        {
            get { return startPt_f0; }
            set { startPt_f0 = value; }
        }
        public int StopPt_f0
        {
            get { return stopPt_f0; }
            set { stopPt_f0 = value; }
        }
        public int[] Xdata_f0 //Rt Fit CombSum
        {
            get { return xdata_f0; }
            set { xdata_f0 = value; }
        }
        public float[] Ydata_f0
        {
            get { return ydata_f0; }
            set { ydata_f0 = value; }
        }
        public int[] Xdata_f1 //Rt Fit Comb1
        {
            get { return xdata_f1; }
            set { xdata_f1 = value; }
        }
        public float[] Ydata_f1
        {
            get { return ydata_f1; }
            set { ydata_f1 = value; }
        }
        public int[] Xdata_f2 //Rt Fit Comb2
        {
            get { return xdata_f2; }
            set { xdata_f2 = value; }
        }
        public float[] Ydata_f2
        {
            get { return ydata_f2; }
            set { ydata_f2 = value; }
        }
        public int[] Xdata_f3 //Rt Fit Comb3
        {
            get { return xdata_f3; }
            set { xdata_f3 = value; }
        }
        public float[] Ydata_f3
        {
            get { return ydata_f3; }
            set { ydata_f3 = value; }
        }
        public int[] Xdata_f4 //Rt Fit Comb4
        {
            get { return xdata_f4; }
            set { xdata_f4 = value; }
        }
        public float[] Ydata_f4
        {
            get { return ydata_f4; }
            set { ydata_f4 = value; }
        }
        public int[] Xdata_f5 //Rt Fit Comb5
        {
            get { return xdata_f5; }
            set { xdata_f5 = value; }
        }
        public float[] Ydata_f5
        {
            get { return ydata_f5; }
            set { ydata_f5 = value; }
        }
        public int[] Xdata_f6 //Rt Fit Comb6
        {
            get { return xdata_f6; }
            set { xdata_f6 = value; }
        }
        public float[] Ydata_f6
        {
            get { return ydata_f6; }
            set { ydata_f6 = value; }
        }
        public int[] Xdata_f7 //Rt Fit Comb7
        {
            get { return xdata_f7; }
            set { xdata_f7 = value; }
        }
        public float[] Ydata_f7
        {
            get { return ydata_f7; }
            set { ydata_f7 = value; }
        }
        public int fMaxPt
        {
            get { return fmaxPt; }
            set { fmaxPt = value; }
        }
        public int[] fXdata // linear fit data
        {
            get { return fxdata; }
            set { fxdata = value; }
        }
        public float[] fYdata
        {
            get { return fydata; }
            set { fydata = value; }
        }        
        public void Replot()
        {
            def_1st = "No";
            GraphXY();
            if (Fit_ == true) Fit();
            if (uy != 0) Comb_Fit();
        }
        public void Plot()
        {
            def_1st = "yes";
            GraphXY();
            if (Fit_ == true) Fit();
            if ( uy != 0)Comb_Fit();
        }
        public void GraphXY()
        {            
            if (IsInt == true) { ntxYmax.Type = "int"; ntxYmin.Type = "int"; }
            else { ntxYmax.Type = "double"; ntxYmin.Type = "double"; }
            if (Def_1st == "yes")
            {
                ntxXmax.Text = Math.Round(fact*dfMaxX).ToString(); ntxXmin.Text = Math.Round(fact*dfMinX).ToString();
                ntxYmax.Text = dfMaxY.ToString(); ntxYmin.Text = dfMinY.ToString();
            }
           // textBox1.Text = dfMaxX.ToString();
            Graphics g = pnGraph.CreateGraphics(); Graphics g1 = this.CreateGraphics();
            g.Clear(Color.White); g1.Clear(Color.White);

            //---------Canvas graph drawing property creation---------
            g.PageUnit = GraphicsUnit.Pixel;// pen for the curve
            g1.PageUnit = GraphicsUnit.Pixel;
            Pen B = new Pen(Color.Black, 1.5f);// pen for the axes

            //-----Drawing of X-Y axes--------------------				 
            g.DrawLine(B, 0.0f, pnGraph.Height - 1.0f, pnGraph.Width, pnGraph.Height - 1.0f);//X axis   bottom         
            g.DrawLine(B, 0.0f, pnGraph.Height, 0.0f, 0.0f); // Y axis left
            g.DrawLine(B, 0.0f, 0.0f, pnGraph.Width, 0.0f);//X axis   Top         
            g.DrawLine(B, pnGraph.Width - 1.0f, pnGraph.Height, pnGraph.Width - 1.0f, 0.0f); // Y axis Right
            // Axes labels---------------------------------------------------------------------------------------------------------------------

            MinX = ntxXmin.NumValue/fact; //Channnel Minx
            MaxX = ntxXmax.NumValue / fact;
            MaxY = ntxYmax.NumValue; 
            MinY = ntxYmin.NumValue;

            //------ Real scale range-------------------------------------------
            Xrange = MaxX - MinX; Yrange = MaxY - MinY;//Channel range

            //------This Pixel Section---1unit on real scale = X_1unit pixels on Canvas scale--------------
            X_1unit = (pnGraph.Width) / Xrange; Y_1unit = (pnGraph.Height) / Yrange; //Channel scale

            
            float Xsc = fact * Xrange * 70 / pnGraph.Width; //time Xsca
            float Xsca = 0; 
            if (Xsc < 10 || 50 / Xsc > 2) Xsca = 10;
            else if (Xsc < 50 || 100 / Xsc > 2) Xsca = 50;
            else if (Xsc < 100 || 500 / Xsc > 2) Xsca = 100;
            else if (Xsc < 500 || 1000 / Xsc > 2) Xsca = 500;
            else if (Xsc < 1000 || 5000 / Xsc > 2) Xsca = 1000;
            else if (Xsc < 5000 || 10000 / Xsc > 2) Xsca = 5000;
            else if (Xsc < 10000 || 50000 / Xsc > 2) Xsca = 10000;
            else Xsca = 50000;
            float Xpx = Xsca * pnGraph.Width / Xrange / fact;  //pixel Xpx 
            float Rtx = (float)Math.IEEERemainder(ntxXmin.NumValue, Xsca); if (Rtx < 0) Rtx = Xsca + Rtx; //time scale            
            float Rpx = Rtx/fact* X_1unit;// pixel scale;
            float Rlx = (Xsca - Rtx)*X_1unit/fact;
            
            float Ysc = Yrange * 50 / pnGraph.Height; //real scales 
            if (Ysc < 1) Ysc = Ysc*10000 ;
            float Ysca = 0;
            if (Ysc < 10 || 50 / Ysc > 2) Ysca = 10;
            else if (Ysc < 50 || 100 / Ysc > 2) Ysca = 50;
            else if (Ysc < 100 || 500 / Ysc > 2) Ysca = 100;
            else if (Ysc < 500 || 1000 / Ysc > 2) Ysca = 500;
            else if (Ysc < 1000 || 5000 / Ysc > 2) Ysca = 1000;
            else if (Ysc < 5000 || 10000 / Ysc > 2) Ysca = 5000;
            else if (Ysc < 10000 || 50000 / Ysc > 2) Ysca = 10000;
            else Ysca = 50000;
            if (Yrange * 50 / pnGraph.Height < 1) Ysca = Ysca / 10000; 
            float Ypx = Ysca * pnGraph.Height / Yrange;  //pixel Xpx 
            float Rty = (float)Math.IEEERemainder(ntxYmin.NumValue, Ysca); if (Rty < 0) Rty = Ysca + Rty; //time scale            
            float Rpy = Rty* Y_1unit;// pixel scale;
            float Rly = (Ysca - Rty) * Y_1unit;
            
            Font drawFont = new Font("Microsoft Sans Serif", 11); Font drawFont0 = new Font("Microsoft Sans Serif", 12);
            SolidBrush drawBrush = new SolidBrush(Color.Black);
            StringFormat YdrawFormat = new StringFormat(); YdrawFormat.Alignment = StringAlignment.Center; // Y-axis label            
            g1.DrawString(xLabel, drawFont, drawBrush, pnGraph.Width / 2, 35 + pnGraph.Height);// X-axis label
            g1.TranslateTransform(0, pnGraph.Height/1.5f); g1.RotateTransform(270); 
            g1.DrawString(yLabel, drawFont, drawBrush,0,0);// 5, pnGraph.Height / 20, YdrawFormat);// Y-axis label
            g1.ResetTransform();
            g1.DrawString(Headnote, drawFont0, drawBrush, 65, 0); //Print headnote
            txCursorPosition.Location = new System.Drawing.Point(pnGraph.Width / 2 - 125, 0);
            //--------XY-axes Scale--------------------------------------------------------------------------------------------------------------------------------------------	            				  			 
            Font drawFont2 = new Font("Arial", 8);
            StringFormat YdrawFormat2 = new StringFormat(); StringFormat YdrawFormat2R = new StringFormat(); int ypt = 0;
            int ix = 0, jy = 0; float Xint = 0, Yint = 0; tmx = ntxXmin.NumValue + Xsca - Rtx; tmy = MinY + Ysca - Rty;
            for (int i = 0; i <= pnGraph.Width/Xpx; i += 1)
            {
                Xint += Xpx;
                g.DrawLine(B, Xint - Rpx, pnGraph.Height, Xint - Rpx, pnGraph.Height - 5);//X-Scale pixel
                ix++;
            }
            for (int j = 0; j <= pnGraph.Height/Ypx; j += 1)
            {
                Yint += Ypx;
                g.DrawLine(B, 0, pnGraph.Height - 1 - Yint + Rpy, 5, pnGraph.Height - 1 - Yint + Rpy);//Y-Scale pixel
                jy++;
            }
            
            for (int f = 0; f <= ix / 2; f++)
            {
                String XScaleLabel = ((int)(Xsca * f * 2 + tmx)).ToString(); float xfi = (62 + f * 2 * Xpx + Rlx); // XScaleLabel = (Xsc * f).ToString();
                g1.DrawString(XScaleLabel, drawFont2, drawBrush, xfi, pnGraph.Height + 20);//X-Scale Label
            }

            YdrawFormat2.FormatFlags = StringFormatFlags.DirectionRightToLeft; String YScaleLabel;
            for (int f = 0; f <= jy / 2; f++)
            {
                float yfi = (pnGraph.Height + 12.5f - f*2*Ypx - Rly); //String YScaleLabel = (Ysc * f).ToString();
                if (IsInt == true)
                {
                    YScaleLabel = (Ysca * f * 2 + tmy).ToString("0");
                    ypt = 70; g1.DrawString(YScaleLabel, drawFont2, drawBrush, ypt, yfi, YdrawFormat2);
                }
                else
                {
                    YScaleLabel = (Ysca * f * 2 + tmy).ToString("0.000");
                    ypt = 35; g1.DrawString(YScaleLabel, drawFont2, drawBrush, ypt, yfi);
                }
            }
//------------------------------------Origin---------------------------------------------------------------------------------------------------
            X0 = -MinX * X_1unit; Y0 = MaxY * Y_1unit;
//----------------------------------------ORIGIN x0 = 0.0f, y0 = 0.0f----------------------------------------------------------------------------   

            g1.Dispose(); B.Dispose(); g.Dispose();
            CurveXY();
            hold = "no";
        }
 // CurveXY function ***********************************************************
        void CurveXY()
        {
            Graphics g = pnGraph.CreateGraphics();
            //-----Origin-----------------------------
            g.TranslateTransform(X0, Y0);
            //---------ORIGIN x0 = 0.0f, y0 = 0.0f--------------
            q = new Pen(dc, dcw);
            Pen qx = new Pen(ec, 1.5f);// pen for x-axis error data point
            if (Symbol == false)
            {
                PointF[] Pt = new PointF[MaxPt];

                for (int i = 0; i <= MaxPt - 1; i++)
                {
                    PointF pc = new PointF(X_1unit * Xdata[i], -Y_1unit * Ydata[i]);
                    Pt.SetValue(pc, i);
                }
                g.DrawCurve(q, Pt);
            }
            else
            {
                float X, Y, E = 0.0f;
                for (int i = 0; i <= MaxPt - 1; i++)
                {
                    X = X_1unit * Xdata[i]; Y = -Y_1unit * Ydata[i]; E = -Y_1unit * Edata[i];
                    g.DrawLine(qx, X, Y - E, X, Y + E); // Vertical
                    g.DrawLine(q, X - 3.0f, Y, X + 3.0f, Y); // horizontal
                }
            }
            Pen Bl = new Pen(Color.Blue, 1.5f);// pen for the axes 
            if (sRefPoint == "yes") g.DrawLine(Bl, 0, -Y_1unit * RefPoint, pnGraph.Width, -Y_1unit * RefPoint);
        }
//+++++++++++++++++++++++++++++++  Combination Fits ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++        
        void Comb1_Fit() // Comb1 Fit
        {
            Graphics T = pnGraph.CreateGraphics();
            //-----Origin-----------------------------
            T.TranslateTransform(X0, Y0);
            //---------ORIGIN x0 = 0.0f, y0 = 0.0f--------------

            PointF[] Pt = new PointF[Nf];
            Pen Fpen = new Pen(fc_f1, fcw_fx);
            for (int i = 0; i <= Nf-1; i++)
            {
                PointF pc = new PointF(X_1unit * xdata_f1[i], -Y_1unit * ydata_f1[i]); Pt.SetValue(pc, i);
            }

            T.DrawCurve(Fpen, Pt);
            Fpen.Dispose(); T.Dispose();
        }
        void Comb2_Fit() // Comb2 Fit
        {
            Graphics T = pnGraph.CreateGraphics();
            //-----Origin-----------------------------
            T.TranslateTransform(X0, Y0);
            //---------ORIGIN x0 = 0.0f, y0 = 0.0f--------------

            PointF[] Pt = new PointF[Nf];
            Pen Fpen = new Pen(fc_f2, fcw_fx);
            for (int i = 0; i <= Nf - 1; i++)
            {
                PointF pc = new PointF(X_1unit * xdata_f2[i], -Y_1unit * ydata_f2[i]);
                Pt.SetValue(pc, i);
            }

            T.DrawCurve(Fpen, Pt);
            Fpen.Dispose(); T.Dispose();
        }
        void Comb3_Fit() // Comb3 Fit
        {
            Graphics T = pnGraph.CreateGraphics();
            //-----Origin-----------------------------
            T.TranslateTransform(X0, Y0);
            //---------ORIGIN x0 = 0.0f, y0 = 0.0f--------------

            PointF[] Pt = new PointF[Nf];
            Pen Fpen = new Pen(fc_f3, fcw_fx);
            for (int i = 0; i <= Nf - 1; i++)
            {
                PointF pc = new PointF(X_1unit * xdata_f3[i], -Y_1unit * ydata_f3[i]);
                Pt.SetValue(pc, i);
            }

            T.DrawCurve(Fpen, Pt);
            Fpen.Dispose(); T.Dispose();
        }
        void Comb4_Fit() // Comb4 Fit
        {
            Graphics T = pnGraph.CreateGraphics();
            //-----Origin-----------------------------
            T.TranslateTransform(X0, Y0);
            //---------ORIGIN x0 = 0.0f, y0 = 0.0f--------------

            PointF[] Pt = new PointF[Nf];
            Pen Fpen = new Pen(fc_f4, fcw_fx);
            for (int i = 0; i <= Nf - 1; i++)
            {
                PointF pc = new PointF(X_1unit * xdata_f4[i], -Y_1unit * ydata_f4[i]);
                Pt.SetValue(pc, i);
            }

            T.DrawCurve(Fpen, Pt);
            Fpen.Dispose(); T.Dispose();
        }
        void Comb5_Fit() // Comb5 Fit
        {
            Graphics T = pnGraph.CreateGraphics();
            //-----Origin-----------------------------
            T.TranslateTransform(X0, Y0);
            //---------ORIGIN x0 = 0.0f, y0 = 0.0f--------------

            PointF[] Pt = new PointF[Nf];
            Pen Fpen = new Pen(fc_f5, fcw_fx);
            for (int i = 0; i <= Nf - 1; i++)
            {
                PointF pc = new PointF(X_1unit * xdata_f5[i], -Y_1unit * ydata_f5[i]);
                Pt.SetValue(pc, i);
            }

            T.DrawCurve(Fpen, Pt);
            Fpen.Dispose(); T.Dispose();
        }
        void Comb6_Fit() // Comb6 Fit
        {
            Graphics T = pnGraph.CreateGraphics();
            //-----Origin-----------------------------
            T.TranslateTransform(X0, Y0);
            //---------ORIGIN x0 = 0.0f, y0 = 0.0f--------------

            PointF[] Pt = new PointF[Nf];
            Pen Fpen = new Pen(fc_f6, fcw_fx);
            for (int i = 0; i <= Nf - 1; i++)
            {
                PointF pc = new PointF(X_1unit * xdata_f6[i], -Y_1unit * ydata_f6[i]);
                Pt.SetValue(pc, i);
            }

            T.DrawCurve(Fpen, Pt);
            Fpen.Dispose(); T.Dispose();
        }
        void Comb7_Fit() // Comb7 Fit
        {
            Graphics T = pnGraph.CreateGraphics();
            //-----Origin-----------------------------
            T.TranslateTransform(X0, Y0);
            //---------ORIGIN x0 = 0.0f, y0 = 0.0f--------------

            PointF[] Pt = new PointF[Nf];
            Pen Fpen = new Pen(fc_f7, fcw_fx);
            for (int i = 0; i <= Nf - 1; i++)
            {
                PointF pc = new PointF(X_1unit * xdata_f7[i], -Y_1unit * ydata_f7[i]);
                Pt.SetValue(pc, i);
            }

            T.DrawCurve(Fpen, Pt);
            Fpen.Dispose(); T.Dispose();
        }
        void CombSum_Fit() // CombSum Fit 
        {
            Graphics T = pnGraph.CreateGraphics();
            //-----Origin-----------------------------
            T.TranslateTransform(X0, Y0);
            //---------ORIGIN x0 = 0.0f, y0 = 0.0f--------------

            PointF[] Pt = new PointF[Nf];
            Pen Fpen = new Pen(fc_f0, fcw_f0);
            for (int i = 0; i <= Nf-1; i++)
            {
                PointF pc = new PointF(X_1unit * xdata_f0[i], -Y_1unit * Ydata_f0[i]);
                Pt.SetValue(pc, i);
            }

            T.DrawCurve(Fpen, Pt);
            Fpen.Dispose(); T.Dispose();
        }
        public void Comb_Fit()
        {
            Nf = stopPt_f0 - startPt_f0;
            xdata_f0 = new int[Nf]; ydata_f0 = new float[Nf];
            for (int i = 0; i <= Nf-1; i++) { xdata_f0[i] = xdata_f1[i]; ydata_f0[i] = 0; }

            if (combNo[1 - 1] == 1) { Comb1_Fit(); for (int i = 0; i <= Nf - 1; i++)ydata_f0[i] += ydata_f1[i]; }
            if (combNo[2 - 1] == 2) { Comb2_Fit(); for (int i = 0; i <= Nf - 1; i++)ydata_f0[i] += ydata_f2[i]; }
            if (combNo[3 - 1] == 3) { Comb3_Fit(); for (int i = 0; i <= Nf - 1; i++)ydata_f0[i] += ydata_f3[i]; }
            if (combNo[4 - 1] == 4) { Comb4_Fit(); for (int i = 0; i <= Nf - 1; i++)ydata_f0[i] += ydata_f4[i]; }
            if (combNo[5 - 1] == 5) { Comb5_Fit(); for (int i = 0; i <= Nf - 1; i++)ydata_f0[i] += ydata_f5[i]; }
            if (combNo[6 - 1] == 6) { Comb6_Fit(); for (int i = 0; i <= Nf - 1; i++)ydata_f0[i] += ydata_f6[i]; }
            if (combNo[7 - 1] == 7) { Comb7_Fit(); for (int i = 0; i <= Nf - 1; i++)ydata_f0[i] += ydata_f7[i]; }

            CombSum_Fit();
            uy = 11;
        }
        public void Fit()
        {
            Graphics T = pnGraph.CreateGraphics();
            //-----Origin-----------------------------
            T.TranslateTransform(X0, Y0);
            //---------ORIGIN x0 = 0.0f, y0 = 0.0f--------------

            PointF[] Pt = new PointF[fMaxPt];
            fq = new Pen(fc, fcw);
            for (int i = 0; i <= fMaxPt - 1; i++)
            {
                PointF pc = new PointF(X_1unit * fxdata[i], -Y_1unit * fydata[i]);
                Pt.SetValue(pc, i);
            }

            T.DrawCurve(fq, Pt);
            fq.Dispose(); T.Dispose();
        }//Fit function for data reduction ***********************
 //Printing graphs=============================================================================================================================
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern long BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, int dwRop);
        private Bitmap Image2;
        private void CaptureScreen()
        {
            Graphics g3 = this.CreateGraphics();
            Size s = this.Size;
            Image2 = new Bitmap(s.Width, s.Height, g3);
            Graphics m4g = Graphics.FromImage(Image2);
            IntPtr Hdc1 = g3.GetHdc();
            IntPtr Hdc2 = m4g.GetHdc();
            BitBlt(Hdc2, 0, 0, this.ClientRectangle.Width, this.ClientRectangle.Height, Hdc1, 0, 0, 13369376);
            g3.ReleaseHdc(Hdc1);
            m4g.ReleaseHdc(Hdc2);
        }
        public void print()
        {
            Headnote = "**Expert**Date: " + date + "**DataFile: " + dName + "**ParFile: " + pName + "**";
            
            ntxYmax.Visible = false;
            ntxYmin.Visible = false;
            ntxXmax.Visible = false;
            ntxXmin.Visible = false;
            txCursorPosition.Visible = false;
            Replot();
            CaptureScreen();
            if (printDialog1.ShowDialog() == DialogResult.OK) printDocument1.Print();

            Headnote = "";
            ntxYmax.Visible = true;
            ntxYmin.Visible = true;
            ntxXmax.Visible = true;
            ntxXmin.Visible = true;
            txCursorPosition.Visible = true;
            Replot();
        }
        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(Image2, 50, 50, 750-2, 600);
        }     
        /*public void PrintPanel(Panel MyPanel)
        {
            Bitmap MyChartPanel = new Bitmap(MyPanel.Width, MyPanel.Height);
            MyPanel.DrawToBitmap(MyChartPanel, new Rectangle(0, 0, MyPanel.Width, MyPanel.Height));

            PrintDialog MyPrintDialog = new PrintDialog();

            if (MyPrintDialog.ShowDialog() == DialogResult.OK)
            {
                System.Drawing.Printing.PrinterSettings values;
                values = MyPrintDialog.PrinterSettings;
                MyPrintDialog.Document = MyPrintDocument;
                MyPrintDocument.PrintController = new System.Drawing.Printing.StandardPrintController();
                MyPrintDocument.Print();
            }

            MyPrintDocument.Dispose();
        }*/
        public static double RoundToNearest(double Amount, double RoundTo)
        {
            double ExcessAmount = Amount % RoundTo;
            if (ExcessAmount < (RoundTo / 2))
            {
                Amount -= ExcessAmount;
            }
            else
            {
                Amount += (RoundTo - ExcessAmount);
            }

            return Amount;
        }

        private void Canvas_Load(object sender, EventArgs e)
        {
            this.BackColor = Color.White;
            txCursorPosition.Text = "[ time = " + "xx" + "  (Chn = " + "xx" + ");   Counts = " + "xx" + " ]";
        }

        

        
    }
}
//##################################################################################################
