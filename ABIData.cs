using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace APIData
{
    public partial class ABIData : Form
    {
        ParseServer ps;
        Bitmap bm;

        public ABIData()
        {
            InitializeComponent();

            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor, true);

            //SetStyle(ControlStyles.UserPaint, true);
            //SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.  
            //SetStyle(ControlStyles.DoubleBuffer, true); // 双缓冲
            //SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            //Type t = panGen.GetType();
            //PropertyInfo pi = t.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            //pi.SetValue(panGen, true, null);
        }

        private void btnFilePath_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "*|*.ab1";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txtFilePath.Text = ofd.FileName;
                btnParse.Enabled = true;
            }
        }

        private void btnParse_Click(object sender, EventArgs e)
        {
            ps = new ParseServer(txtFilePath.Text);
            ps.Parse();
            btnParse.Enabled = false;

            DrawGenBitmap();
            //ShowGen();
        }

        private void DrawGenBitmap()
        {
            if (ps == null || ps.DataInfo == null || ps.DataInfo.Bases == null || ps.DataInfo.Bases.Length <= 0)
                return;

            panGen.Height = (ps.DataInfo.Bases.Length / 100 + 1) * 110 + 100;

            bm = new Bitmap(panGen.Width, panGen.Height);
            using (Graphics g = Graphics.FromImage(bm))
            {
                List<GenViewMode> genList = new List<GenViewMode>();
                int i = 0;
                for (; i < ps.DataInfo.Bases.Length; ++i)
                {
                    if (i % 100 == 0)
                    {
                        if (i > 0)
                        {
                            int n = i / 100;

                            GensDrawing.ShowGen(g, genList, n);
                        }

                        genList = new List<GenViewMode>();
                    }

                    genList.Add(new GenViewMode()
                    {
                        Gen = ps.DataInfo.Bases[i].ToString(),
                        ProbA = ps.DataInfo.Prob_A[i],
                        ProbC = ps.DataInfo.Prob_G[i],
                        ProbG = ps.DataInfo.Prob_C[i],
                        ProbT = ps.DataInfo.Prob_T[i]
                    });
                }

                if (genList.Count < 100)
                {
                    int n = i / 100 + 1;
                    GensDrawing.ShowGen(g, genList, n);
                }
            }
        }

        private void ABIData_Paint(object sender, PaintEventArgs e)
        {
            ShowGen();
        }

        private void ShowGen()
        {
            if (bm != null)
            {
                using (Graphics panG = panGen.CreateGraphics())
                {
                    panG.DrawImage(bm, 0, 0);
                }
            }
        }
    }
}
