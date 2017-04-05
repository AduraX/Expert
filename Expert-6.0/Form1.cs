using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Timers;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Expert
{
    public partial class Form1 : Form
    {
        Splash sp = new Splash();
        AKK Akk = new AKK();
        Par Comm = new Par();
        
        public Form1()
        {
            InitializeComponent();

            rtData.ContextMenuStrip = cmModifyData; 
            Thread th = new Thread(new ThreadStart(DoSplash));
            th.Start();
            Thread.Sleep(2000);
            th.Abort();
            this.TopMost = true; 
        }
        private void DoSplash()
        {                  
          sp.ShowDialog();
        }        
        private void Form1_Shown(object sender, EventArgs e)
        {
          this.TopMost = false;
        }
        private void tbAkk_Click(object sender, EventArgs e)
        {
          Akk.FormClosed += new FormClosedEventHandler(Akk_FormClosed);
          Akk.Show();
        }
        private void Akk_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Close();
        }

        int nx = 0; int ni = 0; int iy = 0; int nc = 0; int es = 0; int n = 0; 
        double Bg = 0.0; double EBg2 = 0.0; double ln2 = 0.69314718055994530941723212145818;
        double[,] Spt = new double[4100, 13]; double[,] ESpt = new double[4100, 13];
        int[] ColmX = new int[100000]; float[] ColmY = new float[100000]; float[] defRt;
        float cut = 10.0f; float cc = 0.0f; float Calibx = 0;
        int[] tzero; int[] XDaF = new int[4100]; float[,] YDaF = new float[4100, 13];
        string[] speck = new string[12]; string[,] FitR; string[] Ang; string[] inv; string[] RtString;
        string ParName;
        int TZ_X = 0; float TZ_Y = 0.0f;

        private void Form1_Load(object sender, EventArgs e)
        {            
            pgDataGrid.SelectedObject = Comm;
            cbPlotSpec.Enabled = false;
            chLog.Enabled = false;
            btStart.Enabled = false;
            btStop.Enabled = false;
            chScatters.Enabled = false;
            rtData.BringToFront();
            btReplot.SendToBack();
            btDefault.SendToBack();
            btRemoveBKG.Enabled = false;
            lbDumUD.Visible = false;
            lbDum.Visible = false;
            tbRt.Enabled = false;
            modifyData.Visible = false;
            FitR = new string[12, 4];
            for (int a = 0; a <= 3; a++) { for (int k = 0; k <= 11; k++) { FitR[k, a] = "0.00"; } }

            pnCanvas.MaxPt = 10;            
            pnCanvas.IsRt = true;
            pnCanvas.Xdata = new int[pnCanvas.MaxPt]; pnCanvas.Ydata = new float[pnCanvas.MaxPt]; pnCanvas.Edata = new float[pnCanvas.MaxPt];
            for (int i = 0; i <= pnCanvas.MaxPt - 1; i++)
            {
                pnCanvas.Xdata[i] = 0; pnCanvas.Ydata[i] = 0; pnCanvas.Edata[i] = 0;
            }
// =============== R(t) Fit ================================ R(t) Fit ================================ R(t) Fit =================
            pnCanvas.CombNo = new int[7] { 0, 0, 0, 0, 0, 0, 0 };
            //AddRoot(sender, e);
        }
        void Ok() //----------------------------OK--------------- Ok -------------------OK-------------------------
        {
            cbPlotSpec.Items.Clear();
            cbPlotSpec.Items.Add("Spec data");
            es = Comm.N0_Spectra - 1;
            for (int a = 0; a <= es; a++)
            {
                string items;
                if (a <= 8) items = "Spec " + (a + 1).ToString("") + "  [xxxx]";
                else items = "Spek " + (a + 1).ToString("") + "  [xxxx]";
                cbPlotSpec.Items.Insert(a, items);
                speck[a] = items;
            }
            cbPlotSpec.SelectedIndex = es + 1;
        }
        //"""""""""""""""""" Process Ftn  """""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""
        void Process(float[,] Sp1)
        {
            //int ad = 1; // Just to additional row 
            tzero = new int[12] { Comm.TimeZero1, Comm.TimeZero2, Comm.TimeZero3, Comm.TimeZero4, Comm.TimeZero5, Comm.TimeZero6, Comm.TimeZero7, Comm.TimeZero8, Comm.TimeZero9, Comm.TimeZero10, Comm.TimeZero11, Comm.TimeZero12 };
            inv = new string[12] { Comm.Invert1, Comm.Invert2, Comm.Invert3, Comm.Invert4, Comm.Invert5, Comm.Invert6, Comm.Invert7, Comm.Invert8, Comm.Invert9, Comm.Inverts10, Comm.Inverts11, Comm.Inverts12 };
            Ang = new string[12] { Comm.Angle1, Comm.Angle2, Comm.Angle3, Comm.Angle4, Comm.Angle5, Comm.Angle6, Comm.Angle7, Comm.Angle8, Comm.Angle9, Comm.Angles10, Comm.Angles11, Comm.Angles12 };
            float[] Spt0 = new float[nc + 1]; // Array of y points for inverse modification
            float[,] Spt1 = new float[nc + 1, 13]; // Y multi-D array for all the 12 spectra + 1 spectrum
            float[] Spt00 = new float[nc / 4 + 2]; // Array for 1st 1/4 Y points to identify the max Y for time  zero
            int d = cbPlotSpec.SelectedIndex + 1; // d = spectrum number
            int u = 0; // u = new time-zero channel just to add/minus 2 channels to ensure that time-zero is included
            int v = 0; // v = inverse flag
            if (inv[d - 1] == "Yes") { v = -1; u = tzero[d - 1] + 2; }
            else { v = 1; u = tzero[d - 1] - 2; }

            for (int p = 0; p <= nc; p++)
            {
                Spt0[p] = ColmY[u + v * p];
            }
            // ############ Making spectrum data to start from time-zero or use Max y point as the time-zero ##################
            int TZ_MaxX = 0; float TZ_MaxY = 0.0f; // Final Time-Zero used are TZ_X and TZ_Y
            for (int p = 0; p <= nc / 4; p++)
            {
                if (Spt0[p] > TZ_MaxY) { TZ_MaxX = u + p + 1; TZ_MaxY = Spt0[p]; }
                Spt00[p] = Spt0[p];
            }

            if (tb_cbTimeZero.SelectedIndex == 0) { TZ_X = tzero[d - 1]; TZ_Y = ColmY[TZ_X]; }
            else { TZ_X = TZ_MaxX; TZ_Y = TZ_MaxY; }

            for (int t = 0; t <= nc / 4; ++t)
            {
                if (Spt0[t] == TZ_Y)
                {
                    for (int p = 0; p <= nc - 1 - t; p++)
                    {
                        Spt1[p, d] = Spt0[p + t];
                    }
                }
            }
            // &&&&&&&&&&&&&&&&&&& Smoothing of data &&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&
            int chnx = Comm.NoChn4Add;
            if (Math.IEEERemainder(Comm.NoChn4Add, 2) == 0) chnx = Comm.NoChn4Add - 1;
            else chnx = Comm.NoChn4Add;
            if (chnx < 1) chnx = 1;
            Comm.NoChn4Add = chnx;
            pgDataGrid.Refresh();
            nx = (int)(chnx / 2 - 0.1);
            float[] Rts = new float[nc + chnx + 2];
            float schn1 = 0, schn2 = 0; int ut = 0;
            if (nx > 1)
            {
                for (int j = 0; j <= nc - 1; j++) Rts[j] = Spt1[j, d];

                for (int j = 0; j <= nc - 1; j++)
                {
                    schn1 = 0; schn2 = 0;
                    for (int us = 0; us <= nx - 1; us++)
                    {
                        ut = j - 1 - us;
                        if (ut < 0) { ut = 0; schn1 += Rts[ut]; }
                    }
                    for (int us = 0; us <= nx; us++) schn2 += Rts[j + us];
                    Spt1[j, d] = schn1 + schn2;
                }

            }
            //@@@@@@@@@@@@@@@@@@@@@@@@@@@ averaging by adding x channels together @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
            ni = Comm.NoChn4Ave;
            int i = 0; float sy = 0.0f; iy = 0;
            for (int t = 0; t <= nc - 1; t++)
            {
                sy = 0.0f; i = 1;
                for (i = 1; i <= ni; i++)
                {

                    if (t > nc - 1) { i--; break; }
                    sy += Spt1[t, d];
                    t++;
                }
                Sp1[iy, d] = sy / (i - 1); // divide by ni is irrelevant in R(t) calculation
                YDaF[iy, d - 1] = Sp1[iy, d];
                XDaF[iy] = t - ni;
                iy++; t--;
            }
            //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@/                
        }
        // ********************* LinearFit Function ******************************************************************************************
        void LinearFit(int Sn, int En, int[] Xi, float[] Yi)
        {
            cc = (float)Comm.CalibConst;// Calibration Constant
            int ca = ni; // No of channnels added for averaging
            double t = Comm.Half_life; // Halflife
            double sF2 = 0.0; double sF2y = 0.0; double sF22y = 0.0; double sY = 0.0;
            double N0 = 0.0; double D = 0.0; double Lambda = ln2 / t;
            int N = En - Sn; pnCanvas.fYdata = new float[N]; pnCanvas.fXdata = new int[N]; //double B = 0.0;

            if (lbDumUD.Text == "NoBA")
            {

                for (int s = Sn; s <= En - 1; s++)
                {
                    sF2 += Math.Exp(-Lambda * cc * Xi[s]);
                    sF2y += Math.Exp(-Lambda * cc * Xi[s]) / Yi[s];
                    sF22y += (Math.Exp(-Lambda * cc * Xi[s]) * Math.Exp(-Lambda * cc * Xi[s])) / Yi[s];
                    sY += 1 / Yi[s];
                }

                D = sY * sF22y - sF2y * sF2y;
                Bg = (N * sF22y - sF2y * sF2) / D; EBg2 = sF22y / D;
                N0 = (sY * sF2 - N * sF2y) / D;

                udBackground.Text = Bg.ToString("");
                udAmplitude.Text = N0.ToString("");
            }
            else
            {
                if (lbDumUD.Text == "Amp") N0 = (double)udAmplitude.Value;
                if (lbDumUD.Text == "Bkg") { Bg = (double)udBackground.Value; }
                N0 = (double)udAmplitude.Value;
            }

            for (int z = Sn; z <= En - 1; z++)
            {
                pnCanvas.fXdata[z - Sn] = Xi[z];
                if (chLog.Checked == false) pnCanvas.fYdata[z - Sn] = (float)(N0 * Math.Exp(-Lambda * cc * Xi[z]) + Bg);
                else pnCanvas.fYdata[z - Sn] = (float)(Math.Log((N0 * Math.Exp(-Lambda * cc * Xi[z]) + Bg), Math.E));
            }

            pnCanvas.fMaxPt = N;  //pnCanvas.Fit();
        }
        private void tbRawData_Click(object sender, EventArgs e)
        {
            pnCanvas.Symbol = true;// pnCanvas.Fit_ = false;
            chScatters.Enabled = true;
            Ok();
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "ADFA Binary files(*.dat)|*.dat|ANU files(*.asc)|*.asc|Bonn files(*.txt)|*.txt|User-defined files(*.txt)|*.txt";
            this.openFileDialog1.Title = "Load PAC Files";
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK) { }
            else return;

            //==========================================================================================

            if (openFileDialog1.FilterIndex == 1)//AdFA binary files
            {
                n = 0;
                using (BinaryReader b = new BinaryReader(File.Open(openFileDialog1.FileName, FileMode.Open)))
                {
                    int lng = (int)b.BaseStream.Length;
                    int pos = 0;
                    while (pos < lng)
                    {
                        ColmY[n] = float.Parse(b.ReadInt32().ToString());
                        ColmX[n] = n;
                        n++;
                        pos += sizeof(int);
                    }
                }
            }
            if (openFileDialog1.FilterIndex == 2)//ANU ascii files
            {
                FileStream fs = new FileStream(openFileDialog1.FileName, FileMode.Open);
                StreamReader sr = new StreamReader(fs);
                n = int.Parse(sr.ReadLine());
                for (int k = 0; k <= n - 1; k++)
                {
                    ColmY[k] = float.Parse(sr.ReadLine());
                    ColmX[k] = k;
                }
                sr.Dispose();
                fs.Dispose();
            }
            if (openFileDialog1.FilterIndex == 3)//Bonn txt files
            {
                FileStream fs = new FileStream(openFileDialog1.FileName, FileMode.Open);
                StreamReader sr = new StreamReader(fs);
                string subrow = "Dummy"; n = 0;
                string row = "Dummy"; int k = 0;
                while (n < 70000)
                {
                    row = sr.ReadLine();
                    if (row == null) break;
                    if (n < 10) { k = 2; subrow = row.Substring(k); }
                    if (n >= 10 && n < 100) { k = 3; subrow = row.Substring(k); }
                    if (n >= 100 && n < 1000) { k = 4; subrow = row.Substring(k); }
                    if (n >= 1000 && n < 10000) { k = 5; subrow = row.Substring(k); }
                    if (n >= 10000 && n < 100000) { k = 6; subrow = row.Substring(k); }
                    ColmY[n] = float.Parse(subrow);
                    ColmX[n] = n;
                    n++;
                }
                sr.Dispose();
                fs.Dispose();
            }
            if (openFileDialog1.FilterIndex == 4)//User-defined text files
            {
                FileStream fs = new FileStream(openFileDialog1.FileName, FileMode.Open);
                StreamReader sr = new StreamReader(fs);
                string subrow = "Dummy"; n = 0;
                while (n < 70000)
                {
                    subrow = sr.ReadLine();
                    if (subrow == null) break;
                    ColmY[n] = float.Parse(subrow);
                    ColmX[n] = n;
                    n++;
                }
                sr.Dispose();
                fs.Dispose();
            }
            //===================================================================================================== 
            cbPlotSpec.Enabled = true; chScatters.Enabled = true;
            pnCanvas.Symbol = false;

            cbPlotSpec.SelectedIndex = cbPlotSpec.Items.Count - 1;
            LoadFn();
            pnCanvas.DName = Path.GetFileName(openFileDialog1.FileName);
            tcPar.SelectTab(tpRedtn);
            tcMain.SelectTab(tpRt);
            this.toolStripStatusLabel1.Text = ">>>> loaded raw data file : " + pnCanvas.DName + " >>>> ";
        }
        //~~~ LoadFn ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        void LoadFn()
        {
            pnCanvas.IsInt = true;
            pnCanvas.Fact = (float)Comm.CalibConst;
            pnCanvas.Fit_ = false;
            string[] PtStr = new string[n + 1];
            lbSpectraTittle.Text = "Spectrum of raw data";
            for (int k = 0; k <= n - 1; k++) { PtStr[k + 1] = ""; }
            for (int k = 0; k <= n - 1; k++) { PtStr[k + 1] = "   " + k.ToString("") + "\t\t" + ColmY[k].ToString(""); }
            PtStr[0] = "   Channels" + "\t" + "Counts";
            this.rtData.Lines = PtStr;
            pnCanvas.MaxPt = n;
            pnCanvas.Xdata = new int[pnCanvas.MaxPt]; pnCanvas.Ydata = new float[pnCanvas.MaxPt]; pnCanvas.Edata = new float[pnCanvas.MaxPt];
            for (int i = 0; i <= pnCanvas.MaxPt - 1; i++)
            {
                pnCanvas.Xdata[i] = 0; pnCanvas.Ydata[i] = 0; pnCanvas.Edata[i] = 0;
            }

            for (int i = 0; i <= pnCanvas.MaxPt - 1; i++)
            {
                pnCanvas.Xdata[i] = ColmX[i]; pnCanvas.Ydata[i] = ColmY[i]; pnCanvas.Edata[i] = (float)Math.Pow(ColmY[i], 0.5);
            }
            
            pnCanvas.DfMaxX = n; pnCanvas.DfMinX = 0;
            pnCanvas.DfMaxY = ColmY.Max(); pnCanvas.DfMinY = ColmY.Min();            
            pnCanvas.XLabel = "Delay time(ns)"; pnCanvas.YLabel = "Counts";
            pnCanvas.Plot();
        }
        private void tbRt_Data_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "R(t) files(*.nnr)|*.nnr|User-defined files(*.txt)|*.txt";
            this.openFileDialog1.Title = "Load PAC R(t) Files";
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK) { }
            else return;

            FileStream FSo = new FileStream(this.openFileDialog1.FileName, FileMode.Open);
            StreamReader SRo = new StreamReader(FSo);

            char[] sep = new char[] { ' ', '\t', ',' };
                string[] Cx = new string[12];
                string linex = SRo.ReadLine();
                string[] splitx = linex.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                int jx = 0;
                foreach (string l in splitx)
                {
                    Cx[jx] = l;
                    jx++;
                }
                pnCanvas.Fact = float.Parse(Cx[1]); 
            iy = int.Parse(SRo.ReadLine()); // read value
            string[] Cl = new string[3]; pnCanvas.Xdata = new int[iy]; pnCanvas.Ydata = new float[iy]; pnCanvas.Edata = new float[iy]; defRt = new float[iy];
            for (int k = 0; k <= iy - 1; k++)
            {
                string line = SRo.ReadLine(); if (line == null) break;
                string[] split = line.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                int j = 0;

                foreach (string l in split)
                {
                    Cl[j] = l;
                    j++;
                }
                pnCanvas.Xdata[k] = (int)(float.Parse(Cl[0]) / pnCanvas.Fact);
                pnCanvas.Ydata[k] = float.Parse(Cl[1]);
                pnCanvas.Edata[k] = float.Parse(Cl[2]);
            }
            SRo.Close();

            for (int k = 0; k <= iy - 1; k++) defRt[k] = pnCanvas.Ydata[k];

            chScatters.Enabled = false;
            Rt_Data();
            pnCanvas.DName = Path.GetFileName(openFileDialog1.FileName);
            tcMain.SelectTab(tpRt);
            Calibx =float.Parse(Cx[0]); 
            toolSlb1.Text = " Opened parameter file: Par file not needed ";
            this.toolStripStatusLabel1.Text = ">>>> loaded R(t)file: " + pnCanvas.DName + " >>>> ";
        }
        void Rt_Data()
        {
            pnCanvas.Fact = (float)Comm.CalibConst;
            pnCanvas.IsInt = false;
            pnCanvas.Symbol = true;
            string[] RtString = new string[iy + 1];
            RtString[0] = "Chn" + "\t" + "Time" + "\t" + "R(t)" + "\t" + "Error[R(t)]";
            for (int j = 0; j <= iy - 1; j++)
            {
                RtString[j + 1] = j.ToString() + "\t" + (pnCanvas.Xdata[j] * Comm.CalibConst).ToString("N3") + "\t" + pnCanvas.Ydata[j].ToString("N4") + "\t" + pnCanvas.Edata[j].ToString("N4");
            }
            rtData.Lines = RtString;
            lbSpectraTittle.Text = "Spectrum of R(t)";
            
            pnCanvas.DfMinY = -0.1f; pnCanvas.DfMaxY = 0.1f;
            //-pnCanvas.DfMinY = pnCanvas.Ydata.Min(); pnCanvas.DfMaxY = pnCanvas.Ydata.Max();
            pnCanvas.DfMinX = pnCanvas.Xdata.Min(); pnCanvas.DfMaxX = pnCanvas.Xdata.Max();
            
            pnCanvas.MaxPt = iy;
            pnCanvas.XLabel = "Delay time(ns)"; pnCanvas.YLabel = "Ratio Value [R(t)]";
            pnCanvas.Symbol = true;
            pnCanvas.Plot();
            udAmplitude.Enabled = false; udBackground.Enabled = false; 
            chLog.Enabled = false; chAll.Enabled = false; btRemoveBKG.Enabled = false; cbPlotSpec.Enabled = false;
            tbRt.Visible = false;
        }

        bool doSel = true;
        private void cbPlotSpec_DropDownClosed(object sender, EventArgs e)
        {
            lbDumUD.Text = "NoBA"; 
            chScatters.Enabled = true;
            pnCanvas.Symbol = false;
            if (doSel == true) { tb_cbTimeZero.SelectedIndex = 0; doSel = false; }

            if (cbPlotSpec.SelectedIndex == cbPlotSpec.Items.Count - 1)
            {
                tb_cbTimeZero.Enabled = false;
                Plot();
                tb_lbTimeZero.Text = "[Chn = ***; Counts = ***.**]";
            }
            else
            {
                tb_cbTimeZero.Enabled = true;
                Plot();
                tb_lbTimeZero.Text = "[Chn = " + TZ_X + "; Counts = " + TZ_Y + "]";
            }
        }
        private void tb_cbTimeZero_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (doSel == false) Plot();
            tb_lbTimeZero.Text = "[Chn = " + TZ_X + "; Counts = " + TZ_Y + "]";
        }
        //==========================Plot function====================Plot=================================================
        void Plot()
        {
            if (cbPlotSpec.SelectedIndex == cbPlotSpec.Items.Count - 1)
            {
                chLog.Enabled = false;
                btRemoveBKG.Enabled = false;
                LoadFn();
            }
            else
            {
                pnCanvas.IsInt = false;
                pnCanvas.Fact = (float)Comm.CalibConst;
                pnCanvas.Fit_ = true;
                chLog.Enabled = true;
                ntxStartFit.Enabled = true; ntxStopFit.Enabled = true; btStart.Enabled = true; btStop.Enabled = true;
                chAll.Enabled = true; btRemoveBKG.Enabled = true; //tbRt.Enabled = true;
                udAmplitude.Enabled = true; udBackground.Enabled = true;

                int k = cbPlotSpec.SelectedIndex + 1;
                int Sn = 0; int En = 0;
                nc = Comm.Chn_per_spectrum;
                float[,] Sp1 = new float[nc, 13];
                Process(Sp1);

                pnCanvas.Xdata = new int[iy]; pnCanvas.Ydata = new float[iy]; pnCanvas.Edata = new float[iy];
                int[] Xdata2 = new int[iy]; float[] Ydata2 = new float[iy];
                double Err = 0.0; double Errlog = 0.0; string SpecTittle = "";
                for (int i = 0; i <= iy - 1; i++)
                {
                    pnCanvas.Xdata[i] = 0; Xdata2[i] = 0; pnCanvas.Ydata[i] = 0; Ydata2[i] = 0; pnCanvas.Edata[i] = 0;
                }
                for (int i = 0; i <= iy - 1; i++)
                {
                    pnCanvas.Xdata[i] = XDaF[i]; Xdata2[i] = XDaF[i];
                    if (Sp1[i, k] < cut) Sp1[i, k] = cut;
                    Ydata2[i] = Sp1[i, k]; Err = Math.Pow(Sp1[i, k], 0.5); Errlog = Err / Sp1[i, k];
                    if (chLog.Checked == false) { pnCanvas.Ydata[i] = Sp1[i, k]; pnCanvas.Edata[i] = (float)Err; SpecTittle = "Linear"; }
                    else { pnCanvas.Ydata[i] = (float)Math.Log(Sp1[i, k], Math.E); pnCanvas.Edata[i] = (float)Errlog; SpecTittle = "Logrithm"; }
                }

                Sn = (int)(int.Parse(ntxStartFit.Text) / ni); En = (int)(int.Parse(ntxStopFit.Text) / ni);
                LinearFit(Sn, En, Xdata2, Ydata2);

                pnCanvas.DfMinX = 0; pnCanvas.DfMaxX = pnCanvas.Xdata[iy - 1];
                pnCanvas.DfMinY = 0; pnCanvas.DfMaxY = pnCanvas.Ydata.Max() + pnCanvas.Edata[0];
                pnCanvas.MaxPt = iy;
                pnCanvas.XLabel = "Delay time(ns)"; pnCanvas.YLabel = "Ratio Value [R(t)]";
                pnCanvas.Plot();

                string[] RtString = new string[iy + 1];
                RtString[0] = " Time" + "\t" + " Counts" + "\t" + " Count-Error";
                lbSpectraTittle.Text = "Spectrum " + k.ToString() + " ( " + SpecTittle + " )";
                for (int j = 0; j <= iy - 1; j++)
                {
                    if (chLog.Checked == false) RtString[j + 1] = " " + (pnCanvas.Xdata[j] * Comm.CalibConst).ToString("N3") + "\t" + pnCanvas.Ydata[j].ToString("N0") + "\t" + pnCanvas.Edata[j].ToString("N2");
                    else RtString[j + 1] = " " + (pnCanvas.Xdata[j] * Comm.CalibConst).ToString("N3") + "\t" + pnCanvas.Ydata[j].ToString("N3") + "\t" + pnCanvas.Edata[j].ToString("N5");
                }
                rtData.Lines = RtString;
                btRemoveBKG.Enabled = true;
            }
        }
        //MMMMMMMMMMMMMMMMMMMMMMMMMMMMM Adjustments of events MMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMM              
        private void udBackground_ValueChanged(object sender, EventArgs e)
        {
            lbDumUD.Text = "Bkg";
            udBackground.Increment = int.Parse(ntxBackground.Text);
            Plot();
        }
        private void udAmplitude_ValueChanged(object sender, EventArgs e)
        {
            lbDumUD.Text = "Amp";
            udAmplitude.Increment = int.Parse(ntxAmplitude.Text);
            Plot();
        }
        private void btReplot_Click(object sender, EventArgs e)
        {
            pnCanvas.Fact = (float)Comm.CalibConst;
            pnCanvas.Replot();
        }
        private void btDefault_Click(object sender, EventArgs e)
        {
            pnCanvas.Fact = (float)Comm.CalibConst;
            pnCanvas.Plot();
        }
        
        private void chLog_CheckedChanged(object sender, EventArgs e)
        {
            pnCanvas.Def_1st = "yes";
            Plot();
        }
        private void chScatters_CheckedChanged(object sender, EventArgs e)
        {
            if (chScatters.Checked == false) pnCanvas.Symbol = false;
            else
            {
                if (cbPlotSpec.SelectedIndex == cbPlotSpec.Items.Count - 1)pnCanvas.Symbol = false;
                else pnCanvas.Symbol = true;
            }
            Plot();
        }
        private void btStart_Click(object sender, EventArgs e)
        {
            ntxStartFit.Text = ((int)pnCanvas.Mx).ToString();
            cbPlotSpec_DropDownClosed(sender, e);
        }
        private void btStop_Click(object sender, EventArgs e)
        {
            ntxStopFit.Text = ((int)pnCanvas.Mx).ToString();
            cbPlotSpec_DropDownClosed(sender, e);
        }

        // InputBox -Rt InputBox QQQQQQQQQ- InputBox -QQQQQQQQQQQQQ
        int valueX = 0; float valueY = 0; int flag;
        public static DialogResult InputBoXY(ref int valueX, ref float valueY, ref int flag)
        {
            Form form = new Form();
            ComboBox cbFlag = new ComboBox();
            Label lbAng1 = new Label();
            Label lbAng2 = new Label();
            TextBox txAng1 = new TextBox();
            TextBox txAng2 = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.Text = " R(t) Modification";
            cbFlag.Text = "Select action on data points";
            cbFlag.Items.Add("  Shift all data points");
            cbFlag.Items.Add("      Move a data point");
            cbFlag.Items.Add("Restore original data points");
            lbAng1.Text = "Type the Rx: ";
            lbAng2.Text = "Type the Rt: ";
            txAng1.Text = "0";
            txAng2.Text = "0";

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            cbFlag.SetBounds(5, 15, 175, 20);
            lbAng1.SetBounds(5, 50, 150, 13); txAng1.SetBounds(113, 50, 70, 20);
            lbAng2.SetBounds(5, 80, 150, 13); txAng2.SetBounds(113, 80, 70, 20);
            buttonOk.SetBounds(20, 115, 75, 23); buttonCancel.SetBounds(115, 115, 75, 23);

            lbAng1.AutoSize = true;
            lbAng2.AutoSize = true;
            txAng1.Anchor = txAng1.Anchor | AnchorStyles.Right;
            txAng2.Anchor = txAng2.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(200, 150);
            form.Controls.AddRange(new Control[] { cbFlag, lbAng1, lbAng2, txAng1, txAng2, buttonOk, buttonCancel });
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;
            // cbFlag.DropDownClosed  += new System.EventHandler(cbFlag.DropDownClosed_DropDownClosed);
            //private void cbFlag_DropDownClosed(object sender, EventArgs e)            {            }

            DialogResult dialogResult = form.ShowDialog();
            if (form.DialogResult == DialogResult.OK)
            {
                try
                {
                    flag = cbFlag.SelectedIndex;
                    valueX = int.Parse(txAng1.Text);
                    valueY = float.Parse(txAng2.Text);
                }
                catch (Exception e)
                {
                    MessageBox.Show(String.Concat(e.Message, "  Type in a real number"), "Error!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    form.ShowDialog();
                }
            }
            return dialogResult;
        }
        private void modifyData_Click(object sender, EventArgs e)
        {
            if (InputBoXY(ref valueX, ref valueY, ref flag) == DialogResult.OK)
            {
                if (flag == 0)
                {
                    for (int k = 0; k <= iy - 1; k++) pnCanvas.Ydata[k] = pnCanvas.Ydata[k] + valueY;
                    btReplot_Click(sender, e);
                }
                else if (flag == 1)
                {
                    pnCanvas.Ydata[valueX] = valueY;
                    btReplot_Click(sender, e);
                }
                else if (flag == 2)
                {
                    for (int k = 0; k <= iy - 1; k++) pnCanvas.Ydata[k] = defRt[k];
                    btReplot_Click(sender, e);
                }
                else
                {
                    MessageBox.Show("  You have not selected an action", "Warning!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    modifyData_Click(sender, e);
                }

                RtString = new string[iy + 1];
                RtString[0] = "Chn" + "\t" + "Time" + "\t" + "R(t)" + "\t" + "Error[R(t)]";
                for (int j = 0; j <= iy - 1; j++)
                {
                    RtString[j + 1] = j.ToString() + "\t" + (pnCanvas.Xdata[j] * pnCanvas.Fact).ToString("N3") + "\t" + pnCanvas.Ydata[j].ToString("N4") + "\t" + pnCanvas.Edata[j].ToString("N4");
                }
                rtData.Lines = RtString;
            }
        }
        private void cmModifyData_Opening(object sender, CancelEventArgs e)
        {
            modifyData.Visible = true;
            //if (ntxAmplitude.Text == "000") modifyData.Visible = true;
            //else modifyData.Visible = false;
        }
//MMMMMMMMMMMMMMMMMMMMMMMMMMMMM End of Adjust events MMMMMM Start of Rt computation MMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMM        
        private void btRemoveBKG_Click(object sender, EventArgs e)
        {
            string subitem; string items;
            pnCanvas.Symbol = false;
            if (chAll.Checked == true)
            {
                for (int t = 0; t <= es; t++)
                {
                    FitR[t, 0] = ntxStartFit.Text; FitR[t, 1] = ntxStopFit.Text; FitR[t, 2] = udAmplitude.Value.ToString(); FitR[t, 3] = udBackground.Value.ToString();
                    cbPlotSpec.Items.RemoveAt(t);
                    if (Ang[t] == "NoFit") subitem = "  [NoFit]";
                    else if (Ang[t] == "Θ1") subitem = "  [ Θ1 ]";
                    else subitem = "  [ Θ2=180 ]";
                    if (t <= 8) items = "Spec " + (t + 1).ToString("") + subitem;
                    else items = "Spek " + (t + 1).ToString("") + subitem;
                    cbPlotSpec.Items.Insert(t, items);
                    lbDumUD.Text = "NoBA";
                    cbPlotSpec.SelectedIndex = t;
                    Plot();
                    for (int i = 0; i <= iy - 1; i++)
                    {
                        Spt[i, t] = YDaF[i, t] - (double)udBackground.Value;
                        ESpt[i, t] = (Math.Pow(YDaF[i, t], 0.5)) / (Comm.NoChn4Ave * Spt[i, t]);
                    }
                }
            }
            else
            {
                int a = cbPlotSpec.SelectedIndex;
                FitR[a, 0] = ntxStartFit.Text; FitR[a, 1] = ntxStopFit.Text; FitR[a, 2] = udAmplitude.Value.ToString(); FitR[a, 3] = udBackground.Value.ToString();
                cbPlotSpec.Items.RemoveAt(a);
                if (Ang[a] == "NoFit") subitem = "  [NoFit]";
                else if (Ang[a] == "Θ1") subitem = "  [ Θ1 ]";
                else subitem = "  [ Θ2=180 ]";
                if (a <= 8) items = "Spec " + (a + 1).ToString("") + subitem;
                else items = "Spek " + (a + 1).ToString("") + subitem;
                cbPlotSpec.Items.Insert(a, items);
                cbPlotSpec.SelectedIndex = a;
                for (int t = 0; t <= iy - 1; t++)
                {
                    Spt[t, a] = YDaF[t, a] - (double)udBackground.Value;
                    ESpt[t, a] = (Math.Pow(YDaF[t, a], 0.5) + EBg2) / (Comm.NoChn4Ave * Spt[t, a]);
                }
            }
            tbRt.Enabled = true; tbRt.Visible = true; 
        }
        private void tbRt_Click(object sender, EventArgs e)
        {
            pnCanvas.IsInt = false;
            if (chAll.Checked == false)
            {
                for (int k = 0; k <= es; k++)
                {
                    string items;
                    if (k <= 8) items = "Spec " + (k + 1).ToString("") + "  [xxxx]";
                    else items = "Spek " + (k + 1).ToString("") + "  [xxxx]";
                    cbPlotSpec.SelectedIndex = k;
                    if (cbPlotSpec.SelectedItem.ToString() == items)
                    {
                        MessageBox.Show((" You have not removed the background of spectrum " + (k + 1).ToString("")), " Error!! ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        cbPlotSpec_DropDownClosed(sender, e);
                        cbPlotSpec.Focus();
                        return;
                    }
                }
            }

            int[] A = new int[8] { 12, 12, 12, 12, 12, 12, 12, 12 }; int[] B = new int[4] { 12, 12, 12, 12 };
            string[] Ang = new string[12] { Comm.Angle1, Comm.Angle2, Comm.Angle3, Comm.Angle4, Comm.Angle5, Comm.Angle6, Comm.Angle7, Comm.Angle8, Comm.Angle9, Comm.Angles10, Comm.Angles11, Comm.Angles12 };
            double[,] Esp = new double[nc, 13];
            int a1 = 0, a2 = 0; double p1 = 0.0, p2 = 0.0;
            for (int i = 0; i <= 11; i++)
            {
                if (Ang[i] == "Θ1") { A[a1] = i; a1++; }
                if (Ang[i] == "Θ2=180") { B[a2] = i; a2++; }
            }
            for (int i = 0; i <= nc - 1; i++)
            {
                Spt[i, 12] = 1;
                ESpt[i, 12] = 0.0;
            }
            p1 = 1.0 / a1; p2 = 1.0 / a2;
            double[] RtΘ1 = new double[iy]; double[] ERtΘ1 = new double[iy];
            double[] RtΘ2 = new double[iy]; double[] ERtΘ2 = new double[iy];
            pnCanvas.Xdata = new int[iy]; pnCanvas.Ydata = new float[iy]; pnCanvas.Edata = new float[iy];
            for (int i = 0; i <= iy - 1; i++)
            {
                RtΘ1[i] = Math.Pow((Spt[i, A[0]] * Spt[i, A[1]] * Spt[i, A[2]] * Spt[i, A[3]] * Spt[i, A[4]] * Spt[i, A[5]] * Spt[i, A[6]] * Spt[i, A[7]]), p1);// N(90,t)
                RtΘ2[i] = Math.Pow((Spt[i, B[0]] * Spt[i, B[1]] * Spt[i, B[2]] * Spt[i, B[3]]), p2); // N(180,t)

                ERtΘ1[i] = RtΘ1[i] * p1 * (ESpt[i, A[0]] + ESpt[i, A[1]] + ESpt[i, A[2]] + ESpt[i, A[3]] + ESpt[i, A[4]] + ESpt[i, A[5]] + ESpt[i, A[6]] + ESpt[i, A[7]]);// Error in N(90,t)
                ERtΘ2[i] = RtΘ2[i] * p2 * (ESpt[i, B[0]] + ESpt[i, B[1]] + ESpt[i, B[2]] + ESpt[i, B[3]]);// Error in N(180,t) 
            }
            for (int j = 0; j <= iy - 1; j++)
            {
                double Up = RtΘ2[j] - RtΘ1[j]; double UpEr = ERtΘ2[j] + ERtΘ1[j];
                double Dwn = RtΘ2[j] + 2.0 * RtΘ1[j]; double DwnEr = ERtΘ2[j] + 2 * ERtΘ1[j];
                pnCanvas.Ydata[j] = (float)(2.0 * (Up / Dwn));
                pnCanvas.Edata[j] = (float)(pnCanvas.Ydata[j] * (UpEr / Up + DwnEr / Dwn));//x.xxf ko si mbe
                pnCanvas.Xdata[j] = XDaF[j];
                float SmNf = 1E-7f;
                if (float.IsNaN(pnCanvas.Ydata[j]) == true) pnCanvas.Ydata[j] = SmNf;
                if (float.IsNaN(pnCanvas.Edata[j]) == true) pnCanvas.Edata[j] = SmNf;
            }

            RtFtn();
            udAmplitude.Enabled = false; udBackground.Enabled = false; chScatters.Enabled = false;
            chLog.Enabled = false; chAll.Enabled = false; btRemoveBKG.Enabled = false;
            tbRt.Visible = false; 
            // =============== R(t) Fit ================================ R(t) Fit ================================ R(t) Fit =================
            Calibx = (float)Comm.CalibConst * Comm.NoChn4Ave;
            this.toolStripStatusLabel1.Text = ">>>> loaded raw data file : " + pnCanvas.DName + " >>>> ";
        }
        void RtFtn()
        {
            pnCanvas.Fact = (float)Comm.CalibConst;
            defRt = new float[iy];
            for (int k = 0; k <= iy - 1; k++) defRt[k] = pnCanvas.Ydata[k];

            RtString = new string[iy + 1];
            RtString[0] = "Chn" + "\t" + "Time" + "\t" + "R(t)" + "\t" + "Error[R(t)]";
            for (int j = 0; j <= iy - 1; j++)
            {
              RtString[j + 1] = j.ToString() + "\t" + (pnCanvas.Xdata[j] * Comm.CalibConst).ToString("N3") + "\t" + pnCanvas.Ydata[j].ToString("N4") + "\t" + pnCanvas.Edata[j].ToString("N4");
            }
            rtData.Lines = RtString;
            lbSpectraTittle.Text = "Spectrum of R(t)";

            pnCanvas.DfMinY = -0.1f; pnCanvas.DfMaxY = 0.1f;
            //-pnCanvas.DfMinY = pnCanvas.Ydata.Min(); pnCanvas.DfMaxY = pnCanvas.Ydata.Max();
            pnCanvas.DfMinX = pnCanvas.Xdata.Min(); pnCanvas.DfMaxX = pnCanvas.Xdata.Max();

            pnCanvas.Symbol = true;
            pnCanvas.MaxPt = iy;
            pnCanvas.XLabel = "Delay time(ns)"; pnCanvas.YLabel = "Ratio Value [R(t)]";
            pnCanvas.Plot();
        }
        private void tsSave_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Title = " Save Parameter file.";
            saveFileDialog1.Filter = "Parameter file(*.par)|*.par";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileStream fsop = new FileStream(saveFileDialog1.FileName, FileMode.Create, FileAccess.Write);
                StreamWriter swop = new StreamWriter(fsop);

                swop.WriteLine(Comm.CalibConst);
                swop.WriteLine(Comm.Chn_per_spectrum);
                swop.WriteLine(Comm.Half_life);
                swop.WriteLine(Comm.N0_Spectra);
                swop.WriteLine(Comm.NoChn4Ave);
                swop.WriteLine(Comm.NoChn4Add);
                swop.WriteLine(ntxStartFit.Text);
                swop.WriteLine(ntxStopFit.Text);


                swop.WriteLine(Comm.Angle1);
                swop.WriteLine(Comm.Angle2);
                swop.WriteLine(Comm.Angle3);
                swop.WriteLine(Comm.Angle4);
                swop.WriteLine(Comm.Angle5);
                swop.WriteLine(Comm.Angle6);
                swop.WriteLine(Comm.Angle7);
                swop.WriteLine(Comm.Angle8);
                swop.WriteLine(Comm.Angle9);
                swop.WriteLine(Comm.Angles10);
                swop.WriteLine(Comm.Angles11);
                swop.WriteLine(Comm.Angles12);

                swop.WriteLine(Comm.Invert1);
                swop.WriteLine(Comm.Invert2);
                swop.WriteLine(Comm.Invert3);
                swop.WriteLine(Comm.Invert4);
                swop.WriteLine(Comm.Invert5);
                swop.WriteLine(Comm.Invert6);
                swop.WriteLine(Comm.Invert7);
                swop.WriteLine(Comm.Invert8);
                swop.WriteLine(Comm.Invert9);
                swop.WriteLine(Comm.Inverts10);
                swop.WriteLine(Comm.Inverts11);
                swop.WriteLine(Comm.Inverts12);

                swop.WriteLine(Comm.TimeZero1);
                swop.WriteLine(Comm.TimeZero2);
                swop.WriteLine(Comm.TimeZero3);
                swop.WriteLine(Comm.TimeZero4);
                swop.WriteLine(Comm.TimeZero5);
                swop.WriteLine(Comm.TimeZero6);
                swop.WriteLine(Comm.TimeZero7);
                swop.WriteLine(Comm.TimeZero8);
                swop.WriteLine(Comm.TimeZero9);
                swop.WriteLine(Comm.TimeZero10);
                swop.WriteLine(Comm.TimeZero11);
                swop.WriteLine(Comm.TimeZero12);
                swop.Flush();
                swop.Close();
            }
            else return;

        }
        private void tsOpen_Click(object sender, EventArgs e)
        {
            openFileDialog2.Filter = "Parameter file(*.par)|*.par";
            this.openFileDialog2.Title = "Open parameter file.";
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                ParName = openFileDialog2.FileName;
                ParLoad(sender, e);
            }
            else return;

            for (int k = 0; k <= 11; k++)
            {
                for (int t = 0; t <= 4100 - 1; t++)
                {
                    Spt[t, k] = 1.0;
                    ESpt[t, k] = 0.0;
                }
            }
            tcPar.SelectTab(tpPropertyGrid);

            tbRawData.Enabled = true;
        }
        void ParLoad(object sender, EventArgs e)
        {
            FileStream fs = new FileStream(ParName, FileMode.Open);
            StreamReader sr = new StreamReader(fs);

            Comm.CalibConst = double.Parse(sr.ReadLine());
            Comm.Chn_per_spectrum = int.Parse(sr.ReadLine());
            Comm.Half_life = double.Parse(sr.ReadLine());
            Comm.N0_Spectra = int.Parse(sr.ReadLine());
            Comm.NoChn4Ave = int.Parse(sr.ReadLine());
            Comm.NoChn4Add = int.Parse(sr.ReadLine());
            ntxStartFit.Text = sr.ReadLine();
            ntxStopFit.Text = sr.ReadLine();

            Comm.Angle1 = sr.ReadLine();
            Comm.Angle2 = sr.ReadLine();
            Comm.Angle3 = sr.ReadLine();
            Comm.Angle4 = sr.ReadLine();
            Comm.Angle5 = sr.ReadLine();
            Comm.Angle6 = sr.ReadLine();
            Comm.Angle7 = sr.ReadLine();
            Comm.Angle8 = sr.ReadLine();
            Comm.Angle9 = sr.ReadLine();
            Comm.Angles10 = sr.ReadLine();
            Comm.Angles11 = sr.ReadLine();
            Comm.Angles12 = sr.ReadLine();

            Comm.Invert1 = sr.ReadLine();
            Comm.Invert2 = sr.ReadLine();
            Comm.Invert3 = sr.ReadLine();
            Comm.Invert4 = sr.ReadLine();
            Comm.Invert5 = sr.ReadLine();
            Comm.Invert6 = sr.ReadLine();
            Comm.Invert7 = sr.ReadLine();
            Comm.Invert8 = sr.ReadLine();
            Comm.Invert9 = sr.ReadLine();
            Comm.Inverts10 = sr.ReadLine();
            Comm.Inverts11 = sr.ReadLine();
            Comm.Inverts12 = sr.ReadLine();

            Comm.TimeZero1 = int.Parse(sr.ReadLine());
            Comm.TimeZero2 = int.Parse(sr.ReadLine());
            Comm.TimeZero3 = int.Parse(sr.ReadLine());
            Comm.TimeZero4 = int.Parse(sr.ReadLine());
            Comm.TimeZero5 = int.Parse(sr.ReadLine());
            Comm.TimeZero6 = int.Parse(sr.ReadLine());
            Comm.TimeZero7 = int.Parse(sr.ReadLine());
            Comm.TimeZero8 = int.Parse(sr.ReadLine());
            Comm.TimeZero9 = int.Parse(sr.ReadLine());
            Comm.TimeZero10 = int.Parse(sr.ReadLine());
            Comm.TimeZero11 = int.Parse(sr.ReadLine());
            Comm.TimeZero12 = int.Parse(sr.ReadLine());
           
            fs.Dispose();
            Ok();
            pgDataGrid.Refresh();
            pnCanvas.PName = Path.GetFileName(ParName);
            toolSlb1.Text = " Opened parameter file : " + pnCanvas.PName;
        }
        //-------Data Grid------------------------------------------------------------------------------       
        private void tcPar_Selected(object sender, TabControlEventArgs e)
        {
            pgDataGrid.Refresh();
            Ok();
        }
        //~~~~~~~~~~~~~~~~~~~~Redrawing after deactivated~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~        
        private void Form1_Resize(object sender, EventArgs e)
        {
            splitContainer1.SplitterDistance = 214;
            pnCanvas.Replot();
        }
        private void tbPrint_Click(object sender, EventArgs e)
        {
            pnCanvas.print();
        }
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~Output Files Color Menus ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ 
        private void mnNightmare_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Title = " Export data for Nighmare Software";
            saveFileDialog1.Filter = "R(t) file(*.nnr)|*.nnr";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileStream fsop = new FileStream(saveFileDialog1.FileName, FileMode.Create, FileAccess.Write);
                StreamWriter swop = new StreamWriter(fsop);
                DateTime date = DateTime.Now;
                string FnAdd = (Comm.CalibConst * Comm.NoChn4Ave).ToString("###0.###") + ", " + Comm.CalibConst.ToString("###0.###") + ", " + Comm.NoChn4Ave.ToString() + ", " + Comm.NoChn4Add.ToString() +  
                    " (Final Calibration-Constant, Calibration-Constant, No-chn-Averged, No-Chn-Added); Date: " + date.ToString("dd/MM/yy");
                swop.WriteLine(FnAdd);
                swop.WriteLine(iy.ToString());
                for (int k = 0; k <= iy - 1; k++)
                {
                    swop.WriteLine((pnCanvas.Xdata[k] * Comm.CalibConst).ToString("###0.###") + "\t" + pnCanvas.Ydata[k].ToString("0.0000000###") + "\t" + pnCanvas.Edata[k].ToString("0.######"));
                }
                swop.Flush();
                swop.Close();
            }
        }
        private void mnRawData_Click(object sender, EventArgs e)//export raw data
        {
            saveFileDialog1.Title = " Export Raw data ";
            saveFileDialog1.Filter = "Raw data file(*.dat)|*.dat";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileStream fsop = new FileStream(saveFileDialog1.FileName, FileMode.Create, FileAccess.Write);
                StreamWriter swop = new StreamWriter(fsop);
                for (int i = 0; i <= n - 1; i++)
                {
                    swop.WriteLine(((int)(ColmX[i] * Comm.CalibConst)).ToString() + "\t" + ColmY[i].ToString());
                }
                swop.Flush();
                swop.Close();
            }
        }
        private void tbHelp_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("Expert Manual.pdf");
        }
    }
}
