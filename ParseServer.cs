using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace APIData
{
    public class ParseServer
    {
        public const int READ_BASES = 1 << 0;
        public const int READ_SAMPLES = 1 << 1;
        public const int READ_COMMENTS = 1 << 2;
        public const int READ_ALL = READ_BASES | READ_SAMPLES | READ_COMMENTS;

        const string ABI_MAGIC = "ABIF";
        const string DataLabel = "DATA";
        const string BaseEntryLabel = "PBAS";
        const string FWO_Label = "FWO_";
        const string BaseConfLabel = "PCON";
        const string BasePosEntryLabel = "PLOC";
        const string CMNTLabel = "CTNM"; //CTNM  CMNT
        const string SMPLLabel = "SMPL";
        const string LANELabel = "LANE";
        const string SignalEntryLabel = "S/N%";
        const string SpacingEntryLabel = "SPAC";
        const string PPOSLabel = "PPOS";
        const string RUNDLabel = "RUND";
        const string RUNTLabel = "RUNT";
        const string MTXFLabel = "MTXF";
        const string SPACLabel = "SPAC";
        const string SVERLabel = "SVER";
        const string MCHNLabel = "MCHN";
        const string PDMFLabel = "PDMF";
        const string MODLLabel = "MODL";
        const string GelNameLabel = "GELN";

        const long indexPO = 26;
        const long IndexEntryLength = 28;

        static uint[] DataCount = { 9, 10, 11, 12 };

        private FileStream fs;

        public ABIDataInfo DataInfo { get; set; }

        long header_fudge = 0;

        static int BaseIdnex(char B)
        {
            return ((B) == 'C' ? 0 : (B) == 'A' ? 1 : (B) == 'G' ? 2 : 3);
        }

        public ParseServer(string text)
        {
            fs = new FileStream(text, FileMode.Open);
        }

        internal void Parse()
        {
            int sections = ReadSections(0);
            int numPoints = 0, numBases = 0;
            float fspacing = 0;		/* average base spacing */
            int no_bases = 0;
            int indexO = 0;   /* File offset where the index is */
            int baseO = 0;    /* File offset where the bases are stored */
            int fwo_ = 0;     /* base . lane mapping */
            int dataCO = 0;   /* File offset where the C trace is stored */
            int dataAO = 0;   /* File offset where the A trace is stored */
            int dataGO = 0;   /* File offset where the G trace is stored */
            int dataTO = 0;   /* File offset where the T trace is stored */

            if ((indexO = GetABIIndexOffset()) < 0)
                return;

            GetABIIndexEntryLW(indexO, DataLabel, DataCount[0], 3, ref numPoints);
            if (GetABIIndexEntryLW(indexO, BaseEntryLabel, 1, 3, ref numBases) == 0)
            {
                no_bases = 1;
                numBases = 0;
            }

            DataInfo = new ABIDataInfo(numPoints, numBases);
            if (GetABIIndexEntryLW(indexO, FWO_Label, 1, 5, ref fwo_) == 0)
                fwo_ = 0x43414754;

            if ((sections & READ_SAMPLES) > 0)
            {
                int[] dataxO = new int[4];

                /*Get the positions of the four traces */
                GetABIIndexEntryLW(indexO, DataLabel, DataCount[0], 5, ref dataxO[BaseIdnex((char)(fwo_ >> 24 & 255))]);
                GetABIIndexEntryLW(indexO, DataLabel, DataCount[1], 5, ref dataxO[BaseIdnex((char)(fwo_ >> 16 & 255))]);
                GetABIIndexEntryLW(indexO, DataLabel, DataCount[2], 5, ref dataxO[BaseIdnex((char)(fwo_ >> 8 & 255))]);
                GetABIIndexEntryLW(indexO, DataLabel, DataCount[3], 5, ref dataxO[BaseIdnex((char)(fwo_ & 255))]);

                dataCO = dataxO[0];
                dataAO = dataxO[1];
                dataGO = dataxO[2];
                dataTO = dataxO[3];

                // Read the traces and bases information
                if ((sections & READ_SAMPLES) > 0)
                {
                    /* Read in the C trace */
                    fs.Seek(header_fudge + dataCO, SeekOrigin.Begin);
                    GetABIint2(0, string.Empty, 0, DataInfo.TraceC, DataInfo.NPoints);

                    /* Read in the A trace */
                    fs.Seek(header_fudge + dataAO, SeekOrigin.Begin);
                    GetABIint2(0, string.Empty, 0, DataInfo.TraceA, DataInfo.NPoints);

                    /* Read in the G trace */
                    fs.Seek(header_fudge + dataGO, SeekOrigin.Begin);
                    GetABIint2(0, string.Empty, 0, DataInfo.TraceG, DataInfo.NPoints);

                    /* Read in the T trace */
                    fs.Seek(header_fudge + dataTO, SeekOrigin.Begin);
                    GetABIint2(0, string.Empty, 0, DataInfo.TraceT, DataInfo.NPoints);
                    /* Compute highest trace peak */
                    for (int i = 0; i < DataInfo.NPoints; i++)
                    {
                        if (DataInfo.MaxTraceVal < DataInfo.TraceA[i])
                            DataInfo.MaxTraceVal = DataInfo.TraceA[i];

                        if (DataInfo.MaxTraceVal < DataInfo.TraceC[i])
                            DataInfo.MaxTraceVal = DataInfo.TraceC[i];

                        if (DataInfo.MaxTraceVal < DataInfo.TraceG[i])
                            DataInfo.MaxTraceVal = DataInfo.TraceG[i];

                        if (DataInfo.MaxTraceVal < DataInfo.TraceT[i])
                            DataInfo.MaxTraceVal = DataInfo.TraceT[i];
                    }
                }

                byte[] conf = new byte[DataInfo.NBases];
                GetABIint1(indexO, BaseConfLabel, 1, conf, DataInfo.NBases);

                GetABIIndexEntryLW(indexO, BaseEntryLabel, 1, 5, ref baseO);
                fs.Seek(header_fudge + baseO, SeekOrigin.Begin);

                for (int i = 0; i < DataInfo.NBases; ++i)
                {
                    int ch = MFGetC();
                    DataInfo.Bases[i] = (((char)ch) == 'N') ? '-' : (char)ch;
                    switch (DataInfo.Bases[i])
                    {
                        case 'A':
                        case 'a':
                            DataInfo.Prob_A[i] = conf[i];
                            DataInfo.Prob_C[i] = 0;
                            DataInfo.Prob_G[i] = 0;
                            DataInfo.Prob_T[i] = 0;
                            break;

                        case 'C':
                        case 'c':
                            DataInfo.Prob_A[i] = 0;
                            DataInfo.Prob_C[i] = conf[i];
                            DataInfo.Prob_G[i] = 0;
                            DataInfo.Prob_T[i] = 0;
                            break;

                        case 'G':
                        case 'g':
                            DataInfo.Prob_A[i] = 0;
                            DataInfo.Prob_C[i] = 0;
                            DataInfo.Prob_G[i] = conf[i];
                            DataInfo.Prob_T[i] = 0;
                            break;

                        case 'T':
                        case 't':
                            DataInfo.Prob_A[i] = 0;
                            DataInfo.Prob_C[i] = 0;
                            DataInfo.Prob_G[i] = 0;
                            DataInfo.Prob_T[i] = conf[i];
                            break;

                        default:
                            DataInfo.Prob_A[i] = 0;
                            DataInfo.Prob_C[i] = 0;
                            DataInfo.Prob_G[i] = 0;
                            DataInfo.Prob_T[i] = 0;
                            break;
                    }
                }
                DataInfo.Bases[DataInfo.Bases.Length - 1] = ' ';

            }

            fs.Close();
        }

        int MFGetC()
        {
            if (fs.Position < fs.Length)
                return fs.ReadByte();

            return -1;
        }

        int GetABIint2(long indexO, string label, uint count, ushort[] data, int max_data_len)
        {
            int len, l2;
            int i;

            byte[] buffer = new byte[max_data_len * 2];
            len = GetABIint1(indexO, label, count, buffer, max_data_len * 2);
            if (-1 == len)
                return -1;

            len /= 2;
            l2 = Math.Min(len, (int)max_data_len);
            byte[] temp = new byte[2];
            for (i = 0; i < l2; i++)
            {
                temp[0] = buffer[i * 2];
                temp[1] = buffer[i * 2 + 1];
                Array.Reverse(temp);

                //data[i] = be_int2(buffer[i * 2]);
                data[i] = BitConverter.ToUInt16(temp);
            }

            return len;
        }

        int GetABIint1(long indexO, string label, uint count, byte[] data, int max_data_len)
        {
            int off = 0;
            int len2 = 0;
            int len = 0;

            if (indexO > 0)
            {
                if ((off = GetABIIndexEntryLW(indexO, label, count, 4, ref len)) == 0)
                    return -1;

                if (len == 0)
                    return 0;

                /* Determine offset */
                if (len <= 4)
                    off += 20;
                else
                    GetABIIndexEntryLW(indexO, label, count, 5, ref off);

                len2 = Math.Min(max_data_len, len);

                fs.Seek(header_fudge + off, SeekOrigin.Begin);
            }
            else
            {
                len = max_data_len;
                len2 = max_data_len;
            }

            fs.Read(data, 0, (int)len2);

            return (int)len;
        }

        private int GetABIIndexEntryLW(long index, string label, uint count, int lw, ref int val)
        {
            long entryNum = -1;
            int i;
            string entryLabel;
            int entryLw1 = 0;
            do
            {
                entryNum++;
                if (fs.Seek(header_fudge + index + (entryNum * IndexEntryLength), SeekOrigin.Begin) < 0)
                    return 0;
                entryLabel = ReadStr();
                if (entryLabel == string.Empty)
                    return 0;

                if ((entryLw1 = ReadInt()) < 0)
                    return 0;
            }
            while (!(entryLabel == label && entryLw1 == count));

            for (i = 2; i <= lw; i++)
                if ((val = ReadInt()) <  0)
                    return 0;

            return (int)(index + (entryNum * IndexEntryLength));
        }

        private int GetABIIndexOffset()
        {
            string magic = ReadStr();
            header_fudge = magic == ABI_MAGIC ? 0 : 128;

            int index;
            if (fs.Seek(header_fudge + indexPO, SeekOrigin.Begin) <= 0 || (index = ReadInt()) < 0)
                return -1;
            else
                return index;
        }

        private string ReadStr()
        {
            byte[] temp4 = new byte[4];
            if (fs.Read(temp4, 0, 4) == 0)
                return string.Empty;

            return Encoding.ASCII.GetString(temp4);
        }

        private int ReadInt()
        {
            byte[] temp4 = new byte[4];
            if (fs.Read(temp4, 0, 4) == 0)
                return -1;

            Array.Reverse(temp4);

            return BitConverter.ToInt32(temp4);
        }

        public int ReadSections(int new_sec)
        {
            int sections = READ_ALL;

            if (new_sec > 0)
                sections = new_sec;

            return sections;
        }
    }
}
