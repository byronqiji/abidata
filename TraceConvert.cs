using System;

namespace ABIData
{
    public class TraceConvert
    {
        private ABIDataInfo dataInfo;

        public TraceConvert(ABIDataInfo dataInfo)
        {
            this.dataInfo = dataInfo;
        }

        internal void SubtractBackgroud()
        {
            SubtractBackgroud(dataInfo.TraceA);
            SubtractBackgroud(dataInfo.TraceC);
            SubtractBackgroud(dataInfo.TraceG);
            SubtractBackgroud(dataInfo.TraceT);
        }

        private void SubtractBackgroud(ushort[] trace)
        {
            ushort[] temp = new ushort[trace.Length];
            ushort minValue = ushort.MaxValue;
            int win_len = 501;
            int win_len2 = win_len / 2;

            for (int i = 0; i < trace.Length; ++i)
            {
                minValue = ushort.MaxValue;
                for (int j = -win_len2; j < win_len2; ++j)
                {
                    if (i + j < 0)
                        continue;

                    if (i + j >= trace.Length)
                        break;

                    if (trace[i + j] < minValue)
                        minValue = trace[i + j];
                }

                temp[i] = (ushort)(trace[i] - minValue);
            }

            temp.CopyTo(trace, 0);
        }

        internal void Skip()
        {
            int i, j;
            for (i = j = 0; j < dataInfo.NPoints / 2; i += 2, j++)
            {
                dataInfo.TraceA[j] = (ushort)((dataInfo.TraceA[i] + dataInfo.TraceA[i + 1]) / 2);
                dataInfo.TraceC[j] = (ushort)((dataInfo.TraceC[i] + dataInfo.TraceC[i + 1]) / 2);
                dataInfo.TraceG[j] = (ushort)((dataInfo.TraceG[i] + dataInfo.TraceG[i + 1]) / 2);
                dataInfo.TraceT[j] = (ushort)((dataInfo.TraceT[i] + dataInfo.TraceT[i + 1]) / 2);
            }

            dataInfo.NPoints = j;

            for (i = 0; i < dataInfo.NBases; i++)
            {
                dataInfo.BasePos[i] /= 2;
            }

            if (dataInfo.NPoints > dataInfo.NBases * 2)
                Skip();
        }

        internal void ScaleTrace(int scale)
        {
            double s;
            int i;

            if (dataInfo.MaxTraceVal <= scale)
                return;

            s = ((double)scale) / dataInfo.MaxTraceVal;

            for (i = 0; i < dataInfo.NBases; i++)
            {
                dataInfo.ShowTraceA[i] = (ushort)(dataInfo.ShowTraceA[i] * s + 0.5);
                dataInfo.ShowTraceC[i] = (ushort)(dataInfo.ShowTraceC[i] * s + 0.5);
                dataInfo.ShowTraceG[i] = (ushort)(dataInfo.ShowTraceG[i] * s + 0.5);
                dataInfo.ShowTraceT[i] = (ushort)(dataInfo.ShowTraceT[i] * s + 0.5);
            }

            dataInfo.MaxTraceVal = (ushort)scale;
        }

        internal void ScaleHeight()
        {
            double marker = 0;
            ushort minMarker = 100, mtv = 0;

            ushort[][] temp = new ushort[4][];
            temp[0] = dataInfo.ShowTraceA;
            temp[1] = dataInfo.ShowTraceC;
            temp[2] = dataInfo.ShowTraceG;
            temp[3] = dataInfo.ShowTraceT;

            for (int i = 0; i < dataInfo.NBases; ++i)
            {
                ushort max = 0;
                for (int j = 0; j < 4; ++j)
                {
                    if (max < temp[j][i])
                        max = temp[j][i];
                }

                if (marker == 0)
                    marker = max;
                else
                {
                    if (max >= marker)
                    {
                        /* attack */
                        marker += (max - marker) / 20.0;
                    }
                    else
                    {
                        /* decay */
                        marker -= (marker - max) / 10.0;
                    }
                }

                if (marker < minMarker)
                    marker = minMarker;

                for (int j = 0; j < 4; j++)
                {
                    double n = temp[j][i] * 2000.0 / marker;
                    temp[j][i] = (ushort)(n > 32767 ? 32767 : n);
                    if (mtv < temp[j][i])
                        mtv = temp[j][i];
                }
            }

            dataInfo.MaxTraceVal = mtv;
        }

        internal void SetBaseLine()
        {
        }

        internal void Noneg()
        {
            int k;
            for (k = dataInfo.NPoints - 1; k >= 0; --k)
            {
                if (dataInfo.TraceA[k] > 0 || dataInfo.TraceC[k] > 0 || dataInfo.TraceG[k] > 0 || dataInfo.TraceT[k] > 0)
                        break;
            }

            ushort[] t = null;
            for (int j = 0; j < 4; ++j)
            {
                switch (j)
                {
                    case 0:
                        t = dataInfo.TraceA;
                        break;
                    case 1:
                        t = dataInfo.TraceC;
                        break;
                    case 2:
                        t = dataInfo.TraceG;
                        break;
                    case 3:
                        t = dataInfo.TraceT;
                        break;
                }

                short min = 0;
                for (int i = 0; i <= k; ++i)
                {
                    if (t != null && min > (short)t[i])
                        min = (short)t[i];
                }

                for (int i = 0; i <= k; ++i)
                    t[i] = (ushort)(t[i] - min);
            }
        }

        internal void SetMax()
        {
            ushort max = 0;
            dataInfo.ShowTraceA = new ushort[dataInfo.NBases];
            dataInfo.ShowTraceC = new ushort[dataInfo.NBases];
            dataInfo.ShowTraceG = new ushort[dataInfo.NBases];
            dataInfo.ShowTraceT = new ushort[dataInfo.NBases];

            for (int i = 0; i < dataInfo.NBases; ++i)
            {
                dataInfo.ShowTraceA[i] = dataInfo.TraceA[dataInfo.BasePos[i]];
                dataInfo.ShowTraceC[i] = dataInfo.TraceC[dataInfo.BasePos[i]];
                dataInfo.ShowTraceG[i] = dataInfo.TraceG[dataInfo.BasePos[i]];
                dataInfo.ShowTraceT[i] = dataInfo.TraceT[dataInfo.BasePos[i]];

                switch (dataInfo.Bases[i])
                {
                    case 'a':
                    case 'A':
                        //dataInfo.ShowTraceA[i] = dataInfo.TraceA[dataInfo.BasePos[i]];
                        //dataInfo.ShowTraceC[i] = 0;
                        //dataInfo.ShowTraceG[i] = 0;
                        //dataInfo.ShowTraceT[i] = 0;
                        if (dataInfo.TraceA[dataInfo.BasePos[i]] > max)
                            max = dataInfo.TraceA[dataInfo.BasePos[i]];
                        break;
                    case 'c':
                    case 'C':
                        //dataInfo.ShowTraceC[i] = dataInfo.TraceC[dataInfo.BasePos[i]];
                        //dataInfo.ShowTraceA[i] = 0;
                        //dataInfo.ShowTraceG[i] = 0;
                        //dataInfo.ShowTraceT[i] = 0;
                        if (dataInfo.TraceC[dataInfo.BasePos[i]] > max)
                            max = dataInfo.TraceC[dataInfo.BasePos[i]];
                        break;
                    case 'g':
                    case 'G':
                        //dataInfo.ShowTraceG[i] = dataInfo.TraceG[dataInfo.BasePos[i]];
                        //dataInfo.ShowTraceC[i] = 0;
                        //dataInfo.ShowTraceA[i] = 0;
                        //dataInfo.ShowTraceT[i] = 0;
                        if (dataInfo.TraceG[dataInfo.BasePos[i]] > max)
                            max = dataInfo.TraceG[dataInfo.BasePos[i]];
                        break;
                    case 't':
                    case 'T':
                        //dataInfo.ShowTraceT[i] = dataInfo.TraceT[dataInfo.BasePos[i]];
                        //dataInfo.ShowTraceC[i] = 0;
                        //dataInfo.ShowTraceG[i] = 0;
                        //dataInfo.ShowTraceA[i] = 0;
                        if (dataInfo.TraceT[dataInfo.BasePos[i]] > max)
                            max = dataInfo.TraceT[dataInfo.BasePos[i]];
                        break;
                }
            }

            for (int i = 0; i < dataInfo.NPoints; ++i)
            {
                if (dataInfo.TraceA[i] > max)
                    dataInfo.TraceA[i] = max;

                if (dataInfo.TraceC[i] > max)
                    dataInfo.TraceC[i] = max;

                if (dataInfo.TraceG[i] > max)
                    dataInfo.TraceG[i] = max;

                if (dataInfo.TraceT[i] > max)
                    dataInfo.TraceT[i] = max;
            }

            if (dataInfo.MaxTraceVal > max)
                dataInfo.MaxTraceVal = max;
        }
    }
}
