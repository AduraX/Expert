using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Expert
{
    public partial class AKK : Form
    {
        public AKK()
        {
            InitializeComponent();
        }

        int intV = 0; string[] line; double c = 0.0; double C = 0.0; double d = 0.0; double e = 0.0;
        double t = 0; double tau1 = 0; double tau2 = 0; double x = 0.0; double H = 0.0; double r = 0.0;
        double Bmax(double h)
        {
            return (Math.Atan((r - e) / (h + t + d)));
        }
        public double F0E1(double B)
        {
            if (c == 0.0) C = 0.0;
            else C = (H * Math.Tan(B) + e - r) / (e / d - Math.Tan(B));
            if (t != 0.0) x = t / Math.Cos(B);
            else x = (r / Math.Sin(B)) - ((H + C) / Math.Cos(B));
            return ((1 - Math.Exp(-x * tau1)) * Math.Sin(B));
        }
        public double F0E2(double B)
        {
            if (c == 0.0) C = 0.0;
            else C = (H * Math.Tan(B) + e - r) / (e / d - Math.Tan(B));
            if (t != 0.0) x = t / Math.Cos(B);
            else x = (r / Math.Sin(B)) - ((H + C) / Math.Cos(B));
            return ((1 - Math.Exp(-x * tau2)) * Math.Sin(B));
        }
        public double F2E1(double B)
        {
            if (c == 0.0) C = 0.0;
            else C = (H * Math.Tan(B) + e - r) / (e / d - Math.Tan(B));
            if (t != 0.0) x = t / Math.Cos(B);
            else x = (r / Math.Sin(B)) - ((H + C) / Math.Cos(B));
            return ((1 - Math.Exp(-x * tau1)) * (3 * Math.Cos(B) * Math.Cos(B) - 1) * Math.Sin(B) / 2);
        }
        public double F2E2(double B)
        {
            if (c == 0.0) C = 0.0;
            else C = (H * Math.Tan(B) + e - r) / (e / d - Math.Tan(B));
            if (t != 0.0) { x = t / Math.Cos(B); }
            else { x = (r / Math.Sin(B)) - ((H + C) / Math.Cos(B)); }
            return ((1 - Math.Exp(-x * tau2)) * (3 * Math.Cos(B) * Math.Cos(B) - 1) * Math.Sin(B) / 2);
        }
        public double F4E1(double B)
        {
            if (c == 0.0) C = 0.0;
            else C = (H * Math.Tan(B) + e - r) / (e / d - Math.Tan(B));
            if (t != 0.0) x = t / Math.Cos(B);
            else x = (r / Math.Sin(B)) - ((H + C) / Math.Cos(B));
            return ((1 - Math.Exp(-x * tau1)) * (35 * Math.Pow(Math.Cos(B), 4.0) - 30 * Math.Pow(Math.Cos(B), 2.0) + 3) * Math.Sin(B) / 8);
        }
        public double F4E2(double B)
        {
            if (c == 0.0) C = 0.0;
            else C = (H * Math.Tan(B) + e - r) / (e / d - Math.Tan(B));
            if (t != 0.0) x = t / Math.Cos(B);
            else x = (r / Math.Sin(B)) - ((H + C) / Math.Cos(B));
            return ((1 - Math.Exp(-x * tau2)) * (35 * Math.Pow(Math.Cos(B), 4.0) - 30 * Math.Pow(Math.Cos(B), 2.0) + 3) * Math.Sin(B) / 8);
        }
        private void btIntegrate_Click(object sender, EventArgs g)
        {
            double a22 = double.Parse(txA22.Text); double a24 = double.Parse(txA24.Text);
            double a42 = double.Parse(txA42.Text); double a44 = double.Parse(txA44.Text);
            double[] Q2E1 = new double[500]; double[] Q2E2 = new double[500];
            double[] Q4E1 = new double[500]; double[] Q4E2 = new double[500];
            double[] A22 = new double[500]; double[] A24 = new double[500];
            double[] A42 = new double[500]; double[] A44 = new double[500];
            double b = 0; int n = 20;
            int NoExtapol = 0; double tol = .000001; // %
            double j0E1 = 0, j0E2 = 0, j2E1 = 0, j2E2 = 0, j4E1 = 0, j4E2 = 0;
            double J0E1 = 0, J0E2 = 0, J2E1 = 0, J2E2 = 0, J4E1 = 0, J4E2 = 0;
            double[] h = new double[500];
            intV = int.Parse(txDInterV.Text); line = new string[intV + 3];
            r = double.Parse(txR.Text);
            tau1 = double.Parse(txE1.Text); tau2 = double.Parse(txE2.Text);
            double t0 = double.Parse(txT.Text);
            double a = 0.0; double b1 = 0.0; double b2 = 0.0;
            double Max = double.Parse(txMaxD.Text); double Min = double.Parse(txMinD.Text);

            if (rbNaI.Checked == true)
            {
                e = 0.0; d = 0.0; c = 0.0;
                for (int i = 1; i <= intV + 1; i++)
                {
                    h[i] = (Max - Min) * (i - 1) / intV + Min; H = h[i];

                    J0E1 = 0; J0E2 = 0; J2E1 = 0; J2E2 = 0; J4E1 = 0; J4E2 = 0;
                    for (int f = 0; f <= 1; f++)
                    {
                        if (f == 0)
                        {
                            a = 0.0; t = t0; b = Bmax(h[i]); b1 = b;
                        }
                        else
                        {
                            a = b1; t = 0.0; b = Bmax(h[i]);
                        }
                        j0E1 = 0; j0E2 = 0; j2E1 = 0; j2E2 = 0; j4E1 = 0; j4E2 = 0;
                        Integration.func f0E1 = F0E1; Integration.Romberg(f0E1, a, b, n, tol, ref j0E1, ref NoExtapol);
                        Integration.func f0E2 = F0E2; Integration.Romberg(f0E2, a, b, n, tol, ref j0E2, ref NoExtapol);
                        Integration.func f2E1 = F2E1; Integration.Romberg(f2E1, a, b, n, tol, ref j2E1, ref NoExtapol);
                        Integration.func f2E2 = F2E2; Integration.Romberg(f2E2, a, b, n, tol, ref j2E2, ref NoExtapol);
                        Integration.func f4E1 = F4E1; Integration.Romberg(f4E1, a, b, n, tol, ref j4E1, ref NoExtapol);
                        Integration.func f4E2 = F4E2; Integration.Romberg(f4E2, a, b, n, tol, ref j4E2, ref NoExtapol);
                        J0E1 = J0E1 + j0E1; J0E2 = J0E2 + j0E2;
                        J2E1 = J2E1 + j2E1; J2E2 = J2E2 + j2E2;
                        J4E1 = J4E1 + j4E1; J4E2 = J4E2 + j4E2;
                    }

                    Q2E1[i] = J2E1 / J0E1; Q2E2[i] = J2E2 / J0E2;
                    Q4E1[i] = J4E1 / J0E1; Q4E2[i] = J4E2 / J0E2;
                    A22[i] = a22 * Q2E1[i] * Q2E2[i]; A24[i] = a24 * Q2E1[i] * Q4E2[i];
                    A42[i] = a42 * Q4E1[i] * Q2E2[i]; A44[i] = a44 * Q4E1[i] * Q4E2[i];
                    line[i] = h[i].ToString("N7") + "\t" + Q2E1[i].ToString("N5") + "\t" + Q2E2[i].ToString("N5") + "\t" +
                                                           Q4E1[i].ToString("N5") + "\t" + Q4E2[i].ToString("N5") + "\t" +
                                                           A22[i].ToString("N5") + "\t" + A24[i].ToString("N5") + "\t" +
                                                           A42[i].ToString("N5") + "\t" + A44[i].ToString("N5");
                }
            }
            else if (rbBaF2Anu.Checked == true)
            {
                double d1 = double.Parse(txd.Text); double e1 = double.Parse(txe.Text);
                double H1 = d1 * (r - e1) / e1; double H2 = H1 * t0 / d1;
                for (int i = 1; i <= intV + 1; i++)
                {
                    e = 0.0; d = 0.0; c = 0.0;
                    h[i] = (Max - Min) * (i - 1) / intV + Min; H = h[i];
                    J0E1 = 0; J0E2 = 0; J2E1 = 0; J2E2 = 0; J4E1 = 0; J4E2 = 0;
                    if (0.0 < H && H < H1)
                    {
                        for (int f = 0; f <= 1; f++)
                        {
                            if (f == 0)
                            {
                                a = 0.0; t = t0; b = Bmax(h[i]); b1 = b;
                            }
                            else
                            {
                                e = e1; a = b1; t = 0.0; b = Bmax(h[i]);
                            }
                            j0E1 = 0; j0E2 = 0; j2E1 = 0; j2E2 = 0; j4E1 = 0; j4E2 = 0;
                            Integration.func f0E1 = F0E1; Integration.Romberg(f0E1, a, b, n, tol, ref j0E1, ref NoExtapol);
                            Integration.func f0E2 = F0E2; Integration.Romberg(f0E2, a, b, n, tol, ref j0E2, ref NoExtapol);
                            Integration.func f2E1 = F2E1; Integration.Romberg(f2E1, a, b, n, tol, ref j2E1, ref NoExtapol);
                            Integration.func f2E2 = F2E2; Integration.Romberg(f2E2, a, b, n, tol, ref j2E2, ref NoExtapol);
                            Integration.func f4E1 = F4E1; Integration.Romberg(f4E1, a, b, n, tol, ref j4E1, ref NoExtapol);
                            Integration.func f4E2 = F4E2; Integration.Romberg(f4E2, a, b, n, tol, ref j4E2, ref NoExtapol);
                            J0E1 = J0E1 + j0E1; J0E2 = J0E2 + j0E2;
                            J2E1 = J2E1 + j2E1; J2E2 = J2E2 + j2E2;
                            J4E1 = J4E1 + j4E1; J4E2 = J4E2 + j4E2;
                        }
                    }
                    if (H1 < H && H < H2)
                    {
                        for (int f = 0; f <= 2; f++)
                        {
                            if (f == 0)
                            {
                                a = 0.0; t = t0; b = Bmax(h[i]); b1 = b;
                            }
                            else if (f == 1)
                            {
                                e = e1; a = b1; t = 0.0; b = Bmax(h[i]); b2 = b;
                            }
                            else
                            {
                                d = d1; a = b2; t = 0.0; b = Bmax(h[i]); c = 1;
                            }
                            j0E1 = 0; j0E2 = 0; j2E1 = 0; j2E2 = 0; j4E1 = 0; j4E2 = 0;
                            Integration.func f0E1 = F0E1; Integration.Romberg(f0E1, a, b, n, tol, ref j0E1, ref NoExtapol);
                            Integration.func f0E2 = F0E2; Integration.Romberg(f0E2, a, b, n, tol, ref j0E2, ref NoExtapol);
                            Integration.func f2E1 = F2E1; Integration.Romberg(f2E1, a, b, n, tol, ref j2E1, ref NoExtapol);
                            Integration.func f2E2 = F2E2; Integration.Romberg(f2E2, a, b, n, tol, ref j2E2, ref NoExtapol);
                            Integration.func f4E1 = F4E1; Integration.Romberg(f4E1, a, b, n, tol, ref j4E1, ref NoExtapol);
                            Integration.func f4E2 = F4E2; Integration.Romberg(f4E2, a, b, n, tol, ref j4E2, ref NoExtapol);
                            J0E1 = J0E1 + j0E1; J0E2 = J0E2 + j0E2;
                            J2E1 = J2E1 + j2E1; J2E2 = J2E2 + j2E2;
                            J4E1 = J4E1 + j4E1; J4E2 = J4E2 + j4E2;
                        }
                    }
                    if (H > H2)
                    {
                        for (int f = 0; f <= 1; f++)
                        {
                            if (f == 0)
                            {
                                e = e1; a = 0.0; t = t0; b = Bmax(h[i]); b1 = b;
                            }
                            else
                            {
                                d = d1; a = b1; t = 0.0; b = Bmax(h[i]); c = 1;
                            }
                            j0E1 = 0; j0E2 = 0; j2E1 = 0; j2E2 = 0; j4E1 = 0; j4E2 = 0;
                            Integration.func f0E1 = F0E1; Integration.Romberg(f0E1, a, b, n, tol, ref j0E1, ref NoExtapol);
                            Integration.func f0E2 = F0E2; Integration.Romberg(f0E2, a, b, n, tol, ref j0E2, ref NoExtapol);
                            Integration.func f2E1 = F2E1; Integration.Romberg(f2E1, a, b, n, tol, ref j2E1, ref NoExtapol);
                            Integration.func f2E2 = F2E2; Integration.Romberg(f2E2, a, b, n, tol, ref j2E2, ref NoExtapol);
                            Integration.func f4E1 = F4E1; Integration.Romberg(f4E1, a, b, n, tol, ref j4E1, ref NoExtapol);
                            Integration.func f4E2 = F4E2; Integration.Romberg(f4E2, a, b, n, tol, ref j4E2, ref NoExtapol);
                            J0E1 = J0E1 + j0E1; J0E2 = J0E2 + j0E2;
                            J2E1 = J2E1 + j2E1; J2E2 = J2E2 + j2E2;
                            J4E1 = J4E1 + j4E1; J4E2 = J4E2 + j4E2;
                        }
                    }

                    Q2E1[i] = J2E1 / J0E1; Q2E2[i] = J2E2 / J0E2;
                    Q4E1[i] = J4E1 / J0E1; Q4E2[i] = J4E2 / J0E2;
                    A22[i] = a22 * Q2E1[i] * Q2E2[i]; A24[i] = a24 * Q2E1[i] * Q4E2[i];
                    A42[i] = a42 * Q4E1[i] * Q2E2[i]; A44[i] = a44 * Q4E1[i] * Q4E2[i];
                    line[i] = h[i].ToString("N3") + "\t" + Q2E1[i].ToString("N5") + "\t" + Q2E2[i].ToString("N5") + "\t" +
                                                           Q4E1[i].ToString("N5") + "\t" + Q4E2[i].ToString("N5") + "\t" +
                                                           A22[i].ToString("N5") + "\t" + A24[i].ToString("N5") + "\t" +
                                                           A42[i].ToString("N5") + "\t" + A44[i].ToString("N5");
                }
            }
            else
            {
                e = 0.0; d = 0.0; c = 0.0;
                for (int i = 1; i <= intV + 1; i++)
                {
                    h[i] = (Max - Min) * (i - 1) / intV + Min; H = h[i];

                    J0E1 = 0; J0E2 = 0; J2E1 = 0; J2E2 = 0; J4E1 = 0; J4E2 = 0;
                    for (int f = 0; f <= 1; f++)
                    {
                        a = 0.0; t = t0; b = Bmax(h[i]); b1 = b;

                        j0E1 = 0; j0E2 = 0; j2E1 = 0; j2E2 = 0; j4E1 = 0; j4E2 = 0;
                        Integration.func f0E1 = F0E1; Integration.Romberg(f0E1, a, b, n, tol, ref j0E1, ref NoExtapol);
                        Integration.func f0E2 = F0E2; Integration.Romberg(f0E2, a, b, n, tol, ref j0E2, ref NoExtapol);
                        Integration.func f2E1 = F2E1; Integration.Romberg(f2E1, a, b, n, tol, ref j2E1, ref NoExtapol);
                        Integration.func f2E2 = F2E2; Integration.Romberg(f2E2, a, b, n, tol, ref j2E2, ref NoExtapol);
                        Integration.func f4E1 = F4E1; Integration.Romberg(f4E1, a, b, n, tol, ref j4E1, ref NoExtapol);
                        Integration.func f4E2 = F4E2; Integration.Romberg(f4E2, a, b, n, tol, ref j4E2, ref NoExtapol);
                        J0E1 = J0E1 + j0E1; J0E2 = J0E2 + j0E2;
                        J2E1 = J2E1 + j2E1; J2E2 = J2E2 + j2E2;
                        J4E1 = J4E1 + j4E1; J4E2 = J4E2 + j4E2;
                    }

                    Q2E1[i] = J2E1 / J0E1; Q2E2[i] = J2E2 / J0E2;
                    Q4E1[i] = J4E1 / J0E1; Q4E2[i] = J4E2 / J0E2;
                    A22[i] = a22 * Q2E1[i] * Q2E2[i]; A24[i] = a24 * Q2E1[i] * Q4E2[i];
                    A42[i] = a42 * Q4E1[i] * Q2E2[i]; A44[i] = a44 * Q4E1[i] * Q4E2[i];
                    line[i] = h[i].ToString("N3") + "\t" + Q2E1[i].ToString("N5") + "\t" + Q2E2[i].ToString("N5") + "\t" +
                                                           Q4E1[i].ToString("N5") + "\t" + Q4E2[i].ToString("N5") + "\t" +
                                                           A22[i].ToString("N5") + "\t" + A24[i].ToString("N5") + "\t" +
                                                           A42[i].ToString("N5") + "\t" + A44[i].ToString("N5");
                }
            }
            line[0] = "Step" + "\t" + "Q2E1" + "\t" + "Q2E2" + "\t" + "Q4E1" + "\t" + "Q4E2" + "\t" + "A22" + "\t" + "A24" + "\t" + "A42" + "\t" + "A44";
            rtdisplay.Lines = line;
        }
        private void btExport_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Title = " Export data in txt format";
            saveFileDialog1.Filter = "Akk file(*.Akk)|*.akk| Text file(*.txt)|*.txt";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileStream fsop = new FileStream(saveFileDialog1.FileName, FileMode.Create, FileAccess.Write);
                StreamWriter swop = new StreamWriter(fsop);

                for (int k = 1; k <= intV; k++)
                {
                    swop.WriteLine(line[k]);
                }
                swop.Flush();
                swop.Close();
            }
        }

        private void AKK_FormClosing(object sender, FormClosingEventArgs e)
        {
        if (e.CloseReason != CloseReason.UserClosing) return;
        e.Cancel = true;
        Hide();
        }       
    }
}
