namespace APIData
{
    public class APIDataInfo
    {
        public APIDataInfo(int num_points, int num_bases)
        {
            NPoints = num_points;
            NBases = num_bases;

            /*   
             * Initialise the body, all pointers are set to null so we can
             * happily call `read_deallocate()`.
             */
            LeftCutoff = 0;
            RightCutoff = 0;
            MaxTraceVal = 0;
            Baseline = 0;

            TraceC = new ushort[num_points + 1];
            TraceA = new ushort[num_points + 1];
            TraceG = new ushort[num_points + 1];
            TraceT = new ushort[num_points + 1];

            Bases = new char[num_bases + 1];
            BasePos = new ushort[num_bases + 1];

            Info = null;
            Format = 0;
            Trace_name = null;

            Prob_A = new byte[num_bases + 1];
            Prob_C = new byte[num_bases + 1];
            Prob_G = new byte[num_bases + 1];
            Prob_T = new byte[num_bases + 1];

            Orig_Trace_Format = 0;
            //orig_trace = null;
            //orig_trace_free = null;

            ident = null;

            Nflows = 0;
            Flow_order = null;
            Flow = null;
            Flow_raw = null;

            //private_data = null;
            Private_Size = 0;
        }

        public const int READ_BASES = 1 << 0;
        public const int READ_SAMPLES = 1 << 1;
        public const int READ_COMMENTS = 1 << 2;
        public const int READ_ALL = READ_BASES | READ_SAMPLES | READ_COMMENTS;

        public int Format { get; set; }      /* Trace file format */
        public string Trace_name { get; set; }  /* Trace file name */

        public int NPoints { get; private set; }    /* No. of points of data */
        public int NBases { get; private set; }      /* No. of bases */

        /* Traces */
        public ushort[] TraceA { get; set; }      /* Array of length `NPoints' */
        public ushort[] TraceC { get; set; }      /* Array of length `NPoints' */
        public ushort[] TraceG { get; set; }      /* Array of length `NPoints' */
        public ushort[] TraceT { get; set; }      /* Array of length `NPoints' */
        public ushort MaxTraceVal { get; set; } /* The maximal value in any trace */
        public int Baseline { get; set; }    /* The zero offset for TRACE values */

        /* Bases */
        public char[] Bases { get; set; }        /* Array of length `NBases' */
        public ushort[] BasePos { get; set; }     /* Array of length `NBases' */

        /* Cutoffs */
        public int LeftCutoff { get; set; }  /* Number of unwanted bases */
        public int RightCutoff { get; set; } /* First unwanted base at right end */

        /* Miscellaneous Sequence Information */
        public string Info { get; set; }        /* misc seq info, eg comments */

        /* Probability information */
        public byte[] Prob_A { get; set; }      /* Array of length 'NBases' */
        public byte[] Prob_C { get; set; }      /* Array of length 'NBases' */
        public byte[] Prob_G { get; set; }      /* Array of length 'NBases' */
        public byte[] Prob_T { get; set; }      /* Array of length 'NBases' */

        /* The original input format data, or NULL if inapplicable */
        public int Orig_Trace_Format { get; set; }
        //void (* orig_trace_free) (void* ptr);
        //void* orig_trace;

        public string ident;         /* Seq id, NULL for unknown. Malloced data.
				Owned and freed by io_lib. */

        /* Pyrosequencing "peaks" (more like spikes). NULL if not used */
        public int Nflows { get; set; }     /* Number of "flows" */
        public string Flow_order { get; set; } /* Bases flowed across */
        public float[] Flow { get; set; }       /* Processed to be 1 base unit oriented */
        public uint[] Flow_raw { get; set; }   /* Unprocessed data */

        //void private_data;      /* The 'private data' block and size from SCF, */
        public int Private_Size { get; set; }        /*         NULL & 0 if not present.            */
    }
}
