using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Expert
{
    [DefaultPropertyAttribute("Spectra No")]
    public class Par
    {
        private int nospectra = 12;
        private int chn_spectrum = 2048;
        private double halflife = 84.0;
        private double calibConst = 0.76500;
        private int noChn4Ave = 1;
        private int noChn4Add = 1;
        [CategoryAttribute(".Global Parameters"), DefaultValueAttribute(12)]
        public int N0_Spectra
        {
            get { return nospectra; }
            set { nospectra = value; }
        }

        [CategoryAttribute(".Global Parameters"), DefaultValueAttribute(2048)]
        public int Chn_per_spectrum
        {
            get { return chn_spectrum; }
            set { chn_spectrum = value; }
        }
        [CategoryAttribute(".Global Parameters"), DefaultValueAttribute(84.0)]
        public double Half_life
        {
            get { return halflife; }
            set { halflife = value; }
        }
        [CategoryAttribute(".Global Parameters"), DefaultValueAttribute(0.765)]
        public double CalibConst
        {
            get { return calibConst; }
            set { calibConst = value; }
        }
        [CategoryAttribute(".Global Parameters"), DefaultValueAttribute(1)]
        public int NoChn4Ave
        {
            get { return noChn4Ave; }
            set { noChn4Ave = value; }
        }
        [CategoryAttribute(".Global Parameters"), DefaultValueAttribute(1)]
        public int NoChn4Add
        {
            get { return noChn4Add; }
            set { noChn4Add = value; }
        }

        //??????????????????? Invert ?????????????????????????????????????????????????????????????????
        private string invert1 = "No";
        [TypeConverter(typeof(InvertConverter)), CategoryAttribute("Spectra1"), DefaultValueAttribute("No")]
        public string Invert1
        {
            get { return invert1; }
            set { invert1 = value; }
        }
        private string invert2 = "No";
        [TypeConverter(typeof(InvertConverter)), CategoryAttribute("Spectra2"), DefaultValueAttribute("No")]
        public string Invert2
        {
            get { return invert2; }
            set { invert2 = value; }
        }
        private string invert3 = "No";
        [TypeConverter(typeof(InvertConverter)), CategoryAttribute("Spectra3"), DefaultValueAttribute("No")]
        public string Invert3
        {
            get { return invert3; }
            set { invert3 = value; }
        }
        private string invert4 = "No";
        [TypeConverter(typeof(InvertConverter)), CategoryAttribute("Spectra4"), DefaultValueAttribute("No")]
        public string Invert4
        {
            get { return invert4; }
            set { invert4 = value; }
        }
        private string invert5 = "No";
        [TypeConverter(typeof(InvertConverter)), CategoryAttribute("Spectra5"), DefaultValueAttribute("No")]
        public string Invert5
        {
            get { return invert5; }
            set { invert5 = value; }
        }
        private string invert6 = "No";
        [TypeConverter(typeof(InvertConverter)), CategoryAttribute("Spectra6"), DefaultValueAttribute("No")]
        public string Invert6
        {
            get { return invert6; }
            set { invert6 = value; }
        }
        private string invert7 = "No";
        [TypeConverter(typeof(InvertConverter)), CategoryAttribute("Spectra7"), DefaultValueAttribute("No")]
        public string Invert7
        {
            get { return invert7; }
            set { invert7 = value; }
        }
        private string invert8 = "No";
        [TypeConverter(typeof(InvertConverter)), CategoryAttribute("Spectra8"), DefaultValueAttribute("No")]
        public string Invert8
        {
            get { return invert8; }
            set { invert8 = value; }
        }
        private string invert9 = "No";
        [TypeConverter(typeof(InvertConverter)), CategoryAttribute("Spectra9"), DefaultValueAttribute("No")]
        public string Invert9
        {
            get { return invert9; }
            set { invert9 = value; }
        }
        private string invert10 = "No";
        [TypeConverter(typeof(InvertConverter)), CategoryAttribute("Spektra10"), DefaultValueAttribute("No")]
        public string Inverts10
        {
            get { return invert10; }
            set { invert10 = value; }
        }
        private string invert11 = "No";
        [TypeConverter(typeof(InvertConverter)), CategoryAttribute("Spektra11"), DefaultValueAttribute("No")]
        public string Inverts11
        {
            get { return invert11; }
            set { invert11 = value; }
        }

        private string invert12 = "No";
        [TypeConverter(typeof(InvertConverter)), CategoryAttribute("Spektra12"), DefaultValueAttribute("No")]
        public string Inverts12
        {
            get { return invert12; }
            set { invert12 = value; }
        }
        //^^^^^^^^^^^^Angle^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
        private string angle1 = "NoFit";
        [TypeConverter(typeof(AngleConverter)), CategoryAttribute("Spectra1"), DefaultValueAttribute("NoFit")]
        public string Angle1
        {
            get { return angle1; }
            set { angle1 = value; }
        }
        private string angle2 = "NoFit";
        [TypeConverter(typeof(AngleConverter)), CategoryAttribute("Spectra2"), DefaultValueAttribute("NoFit")]
        public string Angle2
        {
            get { return angle2; }
            set { angle2 = value; }
        }
        private string angle3 = "NoFit";
        [TypeConverter(typeof(AngleConverter)), CategoryAttribute("Spectra3"), DefaultValueAttribute("NoFit")]
        public string Angle3
        {
            get { return angle3; }
            set { angle3 = value; }
        }
        private string angle4 = "NoFit";
        [TypeConverter(typeof(AngleConverter)), CategoryAttribute("Spectra4"), DefaultValueAttribute("NoFit")]
        public string Angle4
        {
            get { return angle4; }
            set { angle4 = value; }
        }
        private string angle5 = "NoFit";
        [TypeConverter(typeof(AngleConverter)), CategoryAttribute("Spectra5"), DefaultValueAttribute("NoFit")]
        public string Angle5
        {
            get { return angle5; }
            set { angle5 = value; }
        }
        private string angle6 = "NoFit";
        [TypeConverter(typeof(AngleConverter)), CategoryAttribute("Spectra6"), DefaultValueAttribute("NoFit")]
        public string Angle6
        {
            get { return angle6; }
            set { angle6 = value; }
        }
        private string angle7 = "NoFit";
        [TypeConverter(typeof(AngleConverter)), CategoryAttribute("Spectra7"), DefaultValueAttribute("NoFit")]
        public string Angle7
        {
            get { return angle7; }
            set { angle7 = value; }
        }
        private string angle8 = "NoFit";
        [TypeConverter(typeof(AngleConverter)), CategoryAttribute("Spectra8"), DefaultValueAttribute("NoFit")]
        public string Angle8
        {
            get { return angle8; }
            set { angle8 = value; }
        }
        private string angle9 = "NoFit";
        [TypeConverter(typeof(AngleConverter)), CategoryAttribute("Spectra9"), DefaultValueAttribute("NoFit")]
        public string Angle9
        {
            get { return angle9; }
            set { angle9 = value; }
        }
        private string angle10 = "NoFit";
        [TypeConverter(typeof(AngleConverter)), CategoryAttribute("Spektra10"), DefaultValueAttribute("NoFit")]
        public string Angles10
        {
            get { return angle10; }
            set { angle10 = value; }
        }
        private string angle11 = "NoFit";
        [TypeConverter(typeof(AngleConverter)), CategoryAttribute("Spektra11"), DefaultValueAttribute("NoFit")]
        public string Angles11
        {
            get { return angle11; }
            set { angle11 = value; }
        }
        private string angle12 = "NoFit";
        [TypeConverter(typeof(AngleConverter)), CategoryAttribute("Spektra12"), DefaultValueAttribute("NoFit")]
        public string Angles12
        {
            get { return angle12; }
            set { angle12 = value; }
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~timeZero~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        private int timeZero1 = 2048;
        [CategoryAttribute("Spectra1"), DefaultValueAttribute(2048)]
        public int TimeZero1
        {
            get { return timeZero1; }
            set { timeZero1 = value; }
        }
        private int timeZero2 = 2048;
        [CategoryAttribute("Spectra2"), DefaultValueAttribute(2048)]
        public int TimeZero2
        {
            get { return timeZero2; }
            set { timeZero2 = value; }
        }
        private int timeZero3 = 2048;
        [CategoryAttribute("Spectra3"), DefaultValueAttribute(2048)]
        public int TimeZero3
        {
            get { return timeZero3; }
            set { timeZero3 = value; }
        }
        private int timeZero4 = 2048;
        [CategoryAttribute("Spectra4"), DefaultValueAttribute(2048)]
        public int TimeZero4
        {
            get { return timeZero4; }
            set { timeZero4 = value; }
        }
        private int timeZero5 = 2048;
        [CategoryAttribute("Spectra5"), DefaultValueAttribute(2048)]
        public int TimeZero5
        {
            get { return timeZero5; }
            set { timeZero5 = value; }
        }
        private int timeZero6 = 2048;
        [CategoryAttribute("Spectra6"), DefaultValueAttribute(2048)]
        public int TimeZero6
        {
            get { return timeZero6; }
            set { timeZero6 = value; }
        }
        private int timeZero7 = 2048;
        [CategoryAttribute("Spectra7"), DefaultValueAttribute(2048)]
        public int TimeZero7
        {
            get { return timeZero7; }
            set { timeZero7 = value; }
        }
        private int timeZero8 = 2048;
        [CategoryAttribute("Spectra8"), DefaultValueAttribute(2048)]
        public int TimeZero8
        {
            get { return timeZero8; }
            set { timeZero8 = value; }
        }
        private int timeZero9 = 2048;
        [CategoryAttribute("Spectra9"), DefaultValueAttribute(2048)]
        public int TimeZero9
        {
            get { return timeZero9; }
            set { timeZero9 = value; }
        }
        private int timeZero10 = 2048;
        [CategoryAttribute("Spektra10"), DefaultValueAttribute(2048)]
        public int TimeZero10
        {
            get { return timeZero10; }
            set { timeZero10 = value; }
        }
        private int timeZero11 = 2048;
        [CategoryAttribute("Spektra11"), DefaultValueAttribute(2048)]
        public int TimeZero11
        {
            get { return timeZero11; }
            set { timeZero11 = value; }
        }
        private int timeZero12 = 2048;
        [CategoryAttribute("Spektra12"), DefaultValueAttribute(2048)]
        public int TimeZero12
        {
            get { return timeZero12; }
            set { timeZero12 = value; }
        }
    }//-------------------------End of Par class----------------------------------------
}//-------------------------End of Namespace----------------------------------------
//??????????????????? Invert ?????????????????????????????????????????????????????????????????
public class InvertConverter : StringConverter
{
    public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
    {
        return true;
    }
    public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    {
        return new StandardValuesCollection(new string[] { "Yes", "No" });
    }
}

//^^^^^^^^^^^^Angle^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
public class AngleConverter : StringConverter
{
    public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
    {
        return true;
    }
    public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    {
        return new StandardValuesCollection(new string[] { "Θ1", "Θ2=180", "NoFit" });
    }
}