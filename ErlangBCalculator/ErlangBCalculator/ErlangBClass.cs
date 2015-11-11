using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErlangBCalculator
{
    class ErlangBMath
    {
        /*
         * Returns the "Probability of Congestion" or "Grade of Service"
         * for given traffic and number of channels or lines.
         * t        traffic in Erlang
         * c        Number of channels or lines
         * return   Probability of congestion
         */
        public static double GradeOfService(double t, int c)
        {
            double g, s, i;
            if (t > 0)
            {
                if ((c >= 1) /*&& (c <= 20000)*/)
                {
                    s = 0; i = 0;
                    while (i <= c)
                    {
                        s = (1 + s) * (i / t);
                        i++;
                    }
                    g = 1 / (1 + s);
                }
                else g = -1; //Number of channels out of range - to fix: throw an exception
            }
            else g = -2; //Traffic > 0 - to fix: throw an exception
            return g;
        }

        /*
         * Returns the "Traffic" for given number of channels or lines
         * and probability of congestion.
         * c        Number of channels or lines
         * g        Probability of congestion
         * return   Traffic in Erlang
         */
        public static double Traffic(int c, double g)
        {
            double l, r, fR, m, fm;
            r = -1;
            if ((c >= 1) && (c <= 5000))
            {
                if ((g > 0) && (g <= 0.5))
                {
                    l = 0;
                    r = c * (1 + g * 2);
                    fR = GradeOfService(r, c);
                    while (fR > g)
                    {
                        l = r;
                        r = r - r / 10;
                        fR = GradeOfService(r, c);
                    }
                    while (System.Math.Abs(l - r) > 0.0001)
                    {
                        m = (l + r) / 2;
                        fm = GradeOfService(m, c);
                        if (fm > g) l = m; else r = m;
                    }
                }
                else r = -1; //Grade of service out of range
            }
            else r = -2; //Number of channels out of range
            return r;
        }

        /*
         * Returns the number of required circuits or channels for a given traffic
         * and probability of congestion.
         * t        Traffic in Erlang
         * g        Probability of congestion
         * return   Number of channels or lines
         */
        public static int NumberOfChannels(double t, double g)
        {
            double l, r, fR, m, fm;
            r = -1;
            if (t >= 1)
            {
                if ((g > 0) && (g < 0.5))
                {
                    l = 0;
                    r = t;
                    fR = GradeOfService(t, (int)r);
                    while (fR > g)
                    {
                        l = r;
                        r = r + 32;
                        fR = GradeOfService(t, (int)r);
                    }
                    while ((r - l) > 1)
                    {
                        m = (l + r) / 2;
                        fm = GradeOfService(t, (int)m);
                        if (fm > g) l = m; else r = m;
                    }
                }
                else r = -1; //Grade of service out of range
            }
            else r = -2; //Traffic must be greater than 0
            return (int)r;
        }
    }
}
