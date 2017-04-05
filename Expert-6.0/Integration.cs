using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Expert
{

    /* ---------------------------------------------------------
     * Integration class:
     * 
     * 1. Basic Mid-Point Rule MPR(f,h);
     * 2. Improved Mid-Point Rule optMPR(f,h);
     * 3. Composite Trapezoidal Rule CTR(f,h);
     * 4. Composite Simpson's Rule 1/3rd CSR13(f,h);
     * 5. Runge-Kutta Rule of order 4 RK4R(f,h);
     * 6. Gauss-Legendre quadrature rules;
     * 7. Romberg integration rule.
     * 
     * Fabien Cornaton, Tecnológico de Monterrey, February 2010.
     * --------------------------------------------------------- */
    public static class Integration
    {
        // The delegated function:
        public delegate double func(double x);

        #region Tools
        private static void GL_AbsWeights(int pts, ref double[] gp, ref double[] w, ref int ierr)
        {
            /* --------------------------------------------------------
             * Function used by GLR to get points abcissae and weights:
             * --------------------------------------------------------
             * Input arguments
             * ---------------
             * pts = number of Gauss points
             * -------------------------------------------------------- */
            double gp1 = 0;
            double gp2 = 0;
            double gp3 = 0;
            double gp4 = 0;
            double w1 = 0;
            double w2 = 0;
            double w3 = 0;
            double w4 = 0;
            ierr = 0;
            switch (pts)
            {
                case 1:
                    gp[0] = 0;
                    w[0] = 2;
                    break;
                case 2:
                    gp1 = 1 / Math.Sqrt(3);
                    gp[0] = -gp1;
                    gp[1] = gp1;
                    w[0] = 1;
                    w[1] = 1;
                    break;
                case 3:
                    gp1 = Math.Sqrt(0.6);
                    gp2 = 0;
                    gp[0] = -gp1;
                    gp[1] = gp2;
                    gp[2] = gp1;
                    w1 = 0.55555555555555556;
                    w2 = 0.88888888888888889;
                    w[0] = w1;
                    w[1] = w2;
                    w[2] = w1;
                    break;
                case 4:
                    gp1 = -0.8611363115940526;
                    gp2 = -0.3399810435848563;
                    gp[0] = gp1;
                    gp[1] = gp2;
                    gp[2] = -gp2;
                    gp[3] = -gp1;
                    w1 = 0.3478548451374539;
                    w2 = 0.6521451548625461;
                    w[0] = w1;
                    w[1] = w2;
                    w[2] = w2;
                    w[3] = w1;
                    break;
                case 5:
                    gp1 = -0.906179845929659;
                    gp2 = -0.538469309914490;
                    gp3 = 0;
                    gp[0] = gp1;
                    gp[1] = gp2;
                    gp[2] = gp3;
                    gp[3] = -gp2;
                    gp[4] = -gp1;
                    w1 = 0.236926885015596;
                    w2 = 0.478628670384404;
                    w3 = 0.568888888888889;
                    w[0] = w1;
                    w[1] = w2;
                    w[2] = w3;
                    w[3] = w2;
                    w[4] = w1;
                    break;
                case 6:
                    gp1 = -0.9324695142031520;
                    gp2 = -0.6612093864662645;
                    gp3 = -0.2386191860831969;
                    gp[0] = gp1;
                    gp[1] = gp2;
                    gp[2] = gp3;
                    gp[3] = -gp3;
                    gp[4] = -gp2;
                    gp[5] = -gp1;
                    w1 = 0.1713244923791703;
                    w2 = 0.3607615730481386;
                    w3 = 0.4679139345726910;
                    w[0] = w1;
                    w[1] = w2;
                    w[2] = w3;
                    w[3] = w3;
                    w[4] = w2;
                    w[5] = w1;
                    break;
                case 7:
                    gp1 = -0.949107912342758524526189684048;
                    gp2 = -0.741531185599394439863864773281;
                    gp3 = -0.405845151377397166906606412077;
                    gp4 = 0;
                    gp[0] = gp1;
                    gp[1] = gp2;
                    gp[2] = gp3;
                    gp[3] = gp4;
                    gp[4] = -gp3;
                    gp[5] = -gp2;
                    gp[6] = -gp1;
                    w1 = 0.129484966168869693270611432679;
                    w2 = 0.279705391489276667901467771424;
                    w3 = 0.381830050505118944950369775489;
                    w4 = 0.417959183673469387755102040816;
                    w[0] = w1;
                    w[1] = w2;
                    w[2] = w3;
                    w[3] = w4;
                    w[4] = w3;
                    w[5] = w2;
                    w[6] = w1;
                    break;
                case 8:
                    gp1 = -0.9602898564975363;
                    gp2 = -0.7966664774136267;
                    gp3 = -0.5255324099163290;
                    gp4 = -0.1834346424956498;
                    gp[0] = gp1;
                    gp[1] = gp2;
                    gp[2] = gp3;
                    gp[3] = gp4;
                    gp[4] = -gp4;
                    gp[5] = -gp3;
                    gp[6] = -gp2;
                    gp[7] = -gp1;
                    w1 = 0.1012285362903761;
                    w2 = 0.2223810344533745;
                    w3 = 0.3137066458778873;
                    w4 = 0.3626837833783620;
                    w[0] = w1;
                    w[1] = w2;
                    w[2] = w3;
                    w[3] = w4;
                    w[4] = w4;
                    w[5] = w3;
                    w[6] = w2;
                    w[7] = w1;
                    break;
                default:
                    MessageBox.Show("Error in : Unhandled number of integration points: " + pts.ToString());
                    ierr = -1;
                    break;
            }
        }

        private static double GL_VarChange(double a, double b, double xgp)
        {
            /* ----------------------------------------------------
             * Function used by GLR to make the change of variable:
             * ----------------------------------------------------
             * Input arguments
             * ---------------
             * xgp = Gauss point coordinates
             * ---------------------------------------------------- */
            return ((b + a) + (b - a) * xgp) / 2;
        }

        public static double TR(func f, double a, double b)
        {
            /* -------------------------------------
             * This is the Trapezoidal Rule TR(f,h):
             * -------------------------------------
             * Input arguments
             * ---------------
             * f = delegated function
             * a = lower integration bound
             * b = upper integration bound
             * ------------------------------------- */
            return (b - a) / 2.0 * (f(a) + f(b));
        }

        public static double TR(double a, double b, double ya, double yb)
        {
            /* ----------------------------------------
             * This is the Trapezoidal Rule TR(x,f(x)):
             * ----------------------------------------
             * Input arguments
             * ---------------
             * a  = lower integration bound
             * b  = upper integration bound
             * ya = function value at a
             * yb = function value at b
             * ---------------------------------------- */
            return (b - a) / 2.0 * (ya + yb);
        }
        #endregion

        #region Integration.Methods
        public static double MPR(func f, double a, double b, int m)
        {
            /* ------------------------------------------
             * This is the basic Mid-Point Rule MPR(f,h):
             * ------------------------------------------
             * Input arguments
             * ---------------
             * f = delegated function
             * a = lower integration bound
             * b = upper integration bound
             * m = number of intervals
             */

            // The stepsize:
            double h = (b - a) / m;

            // Calculate the summation term:
            double result = 0;
            for (int i = 1; i <= m; i++)
                result += f(a + h * (i - 0.5));

            // Return the result:
            return h * result;
        }

        public static double OptMPR(func f, double a, double b, int m)
        {
            /* -------------------------------------------
             * This is a modified Mid-Point Rule MPR(f,h):
             * -------------------------------------------
             * Input arguments
             * ---------------
             * f = delegated function
             * a = lower integration bound
             * b = upper integration bound
             * m = number of intervals
             * ------------------------------------------- */

            double sum = 0.0;
            for (int j = 2; j <= m; j++)
            {
                // Compute stepsize for n-1 internal rectangles: 
                int n = (int)Math.Pow(2, j);
                double h = (b - a) / n;
                // Approximate 1/2-area in first rectangle = (h/2) * f(a):
                sum = f(a) * (h / 2.0);
                // Apply midpoint rule:
                for (int i = 1; i <= n - 1; i++)
                    // Given length = f(x), compute the area of the rectangle of width 'h':
                    sum += f(a + i * h) * h;
                // Approximate 1/2 area in last rectangle: (h/2) * f(b):
                sum += f(b) * (h / 2);
            }

            // Return result:
            return sum;
        }

        public static double CTR(func f, double a, double b, int m)
        {
            /* ------------------------------------------------
             * This is the Composite Trapezoidal Rule CTR(f,h):
             * ------------------------------------------------
             * Input arguments
             * ---------------
             * f = delegated function
             * a = lower integration bound
             * b = upper integration bound
             * m = number of intervals
             * ------------------------------------------------ */

            // The stepsize:
            double h = (b - a) / m;

            // Calculate the summation term:
            double sum = 0.5 * (f(a) + f(b));
            for (int i = 1; i < m; i++)
                sum += f(a + i * h);

            // Return the result:
            return h * sum;
        }

        public static double CSR13(func f, double a, double b, int m)
        {
            /* ----------------------------------------------------
             * This is the Composite Simpson Rule 1/3rd CSR13(f,h):
             * ----------------------------------------------------
             * Input arguments
             * ---------------
             * f = delegated function to be parssed
             * a = lower integration bound
             * b = upper integration bound
             * m = number of intervals
             * ---------------------------------------------------- */

            // The stepsize:
            if (m % 2 != 0) m++;
            int n = m / 2;
            double h = (b - a) / m;

            // Calculate the sum of f(x_[2k]) (even x_k, k = 1,..,n-1):
            double seven = 0.0;
            for (int i = 1; i < n; i++)
                seven += f(a + (2 * i) * h);
            seven *= (2.0 / 3.0);

            // Calculate the sum of f(x_[2k-1]) (odd x_k, k = 1,..,n):
            double sodd = 0.0;
            for (int i = 1; i <= n; i++)
                sodd += f(a + (2 * i - 1) * h);
            sodd *= (4.0 / 3.0);

            // Assemble all terms and return result:
            double sint = (1.0 / 3.0) * (f(a) + f(b)) + seven + sodd;
            return h * sint;
        }

        public static double GLR(byte pts, func f, double a, double b)
        {
            /* ----------------------------------------------------------
             * This is the Gauss-Legendre quadrature rules up to order 8:
             * ----------------------------------------------------------
             * Input arguments
             * ---------------
             * pts = number of quadrature points
             * f   = delegated function
             * a   = lower integration bound
             * b   = upper integration bound
             * ---------------------------------------------------------- */

            double gli = 0;
            // Get Gauss points and weights:
            int err = 0;
            double[] gp = new double[pts];
            double[] w = new double[pts];
            GL_AbsWeights(pts, ref gp, ref w, ref err);

            // Apply the quadrature rule:
            if (err == 0)
            {
                for (int i = 0; i < gp.Length; i++)
                    gli += f(GL_VarChange(a, b, gp[i])) * w[i];
            }

            // Scale by determinant and return result:
            return gli * (b - a) / 2;
        }

        public static void Romberg(func f, double a, double b, int mxex, double tol, ref double rint, ref int nextr)
        {
            /* ------------------------------------------
             * This is the Romberg integration algorithm:
             * ------------------------------------------
             * Input arguments
             * ---------------
             * f     = delegated function
             * a     = lower integration bound
             * b     = upper integration bound
             * mxex  = maximum number of extrapolations
             * tol   = tolerance on relative error (in %)
             * 
             * ----------------
             * Output arguments
             * ----------------
             * rint  = the integral
             * nextr = number of extrapolations performed
             * ------------------------------------------ */

            // Romberg triangular matrix:
            double[,] R = new double[mxex, mxex];

            // Initial guess –> call the Trapezoidal Rule:
            R[0, 0] = TR(f, a, b);

            // Loop on allowed number of extrapolations:
            int n = 0;
            double err = 1.0e+30;
            do
            {
                n++;
                if (n == mxex)
                {
                    n--;
                    break;
                }
                // Improved estimate –> call the Composite Trapezoidal Rule with 2^n segments:
                R[0, n] = CTR(f, a, b, (int)Math.Pow(2, n));
                // Apply recurrence formula:
                for (int i = 1; i <= n; i++)
                {
                    int j = n - i;
                    R[i, j] = (Math.Pow(4, i) * R[i - 1, j + 1] - R[i - 1, j]) / (Math.Pow(4, i) - 1);
                }
                // Evaluate absolute relative error:
                err = Math.Abs((R[1, n - 1] - R[0, n]) / R[0, n]);
                err *= 100;
            } while (n < mxex && err > tol);

            // Return solution:
            nextr = n + 1;
            rint = R[n, 0];
        }

        public static double RK4R(func f, double a, double b, double p0, int m, int nsi)
        {
            /* --------------------------------------------------
             * This is the Runge-Kutta Rule of order 4 RK4R(f,h):
             * --------------------------------------------------
             * Input arguments
             * ---------------
             * f   = delegated function
             * a   = lower integration bound
             * b   = upper integration bound
             * p0  = value of the primitive at x = a        
             * m   = number of intervals
             * nsi = number of sub-intervals
             * -------------------------------------------------- */
            int ni;
            double y, u;
            double h = (b - a) / (nsi * m);
            double h2 = h / 2.0;
            double h6 = h2 / 3.0;
            double lr = p0;
            double ly = f(a);
            double x = 0.0;

            for (int i = 1; i <= m; i++)
            {
                ni = (i - 1) * nsi - 1;
                for (int j = 1; j <= nsi; j++)
                {
                    x = a + h * (ni + j);
                    u = 4.0 * f(x + h2);
                    x += h;
                    y = f(x);
                    lr += h6 * (ly + u + y);
                    ly = y;
                }
            }

            // Return result:
            return lr;
        }
        #endregion

        #region Continuous.Integrals
        public static double[] Integral(func f, double a, double b, int n)
        {
            /* ------------------------------------
             * Calculates the integral of the input  
             * function f between a and b.
             * Trapezoidal rule.
             * ------------------------------------ */

            double h = (b - a) / n;
            double[] dy = new double[n + 1];
            dy[0] = 0;
            for (int i = 1; i <= n; i++)
            {
                double x0 = a + (i - 1) * h;
                double x1 = x0 + h;
                dy[i] = dy[i - 1] + TR(f, x0, x1);
            }
            return dy;
        }

        public static double[] Integral(double[] x, double[] y)
        {
            /* ----------------------------------
             * Calculates the integral of 
             * discrete input data y[], assuming
             * that the point are equally spaced.
             * Trapezoidal rule.
             * ---------------------------------- */

            int n = x.Length;
            double[] dy = new double[n];
            dy[0] = 0;
            for (int i = 1; i < n; i++)
                dy[i] = dy[i - 1] + TR(x[i - 1], x[i], y[i - 1], y[i]);
            return dy;
        }
        #endregion
    }
}

