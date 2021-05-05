using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace APIData
{
    public partial class ABIData : Form
    {
        ParseServer ps;

        public ABIData()
        {
            InitializeComponent();
            //gens = new char[] { 'T' };
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

            ShowGen();
        }

        private void ShowGen()
        {
            List<GenViewMode> genList = new List<GenViewMode>();
            for (int i = 0; i < ps.DataInfo.Bases.Length; ++i)
            {
                if (i % 74 == 0)
                {
                    int n = i / 74;
                    switch (n)
                    {
                        case 0:
                            break;
                        case 1:
                            using (Graphics g = panGen1.CreateGraphics())
                                GensDrawing.ShowGen(g, genList);
                            break;
                        case 2:
                            using (Graphics g = panGen2.CreateGraphics())
                                GensDrawing.ShowGen(g, genList);
                            break;
                        case 3:
                            using (Graphics g = panGen3.CreateGraphics())
                                GensDrawing.ShowGen(g, genList);
                            break;
                        case 4:
                            using (Graphics g = panGen4.CreateGraphics())
                                GensDrawing.ShowGen(g, genList);
                            break;
                        default:
                            return;
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
        }
    }
}
