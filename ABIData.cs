using System;
using System.Drawing;
using System.Windows.Forms;

namespace APIData
{
    public partial class ABIData : Form
    {
        ParseServer ps;

        char[] gens;

        public ABIData()
        {
            InitializeComponent();
            gens = new char[] { 'T' };
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
            gens = ps.DataInfo.Bases;

            ShowGen();
        }

        private void ShowGen()
        {
            using (Graphics g = panGen.CreateGraphics())
            {
                Rectangle r = new Rectangle(0, 0, 1125, 300);

                g.FillRectangle(Brushes.White, r);
                Brush colorG = new SolidBrush(Color.FromArgb(0, 0, 0));
                Brush colorT = new SolidBrush(Color.FromArgb(255, 0, 0));
                Brush colorC = new SolidBrush(Color.FromArgb(0, 0, 255));
                Brush colorA = new SolidBrush(Color.FromArgb(0, 255, 0));
                Font font = new Font("Arial", 8f);
                for (int i = 0; i < gens.Length; ++i)
                {
                    string gen = gens[i].ToString();
                    switch (gen)
                    {
                        case "A":
                            g.DrawString(gen, font, colorA, 0 + i * 15, 210);
                            //DrawContent(g, ps.DataInfo.Prob_A, colorA);
                            break;
                        case "C":
                            g.DrawString(gen, font, colorC, 0 + i * 15, 210);
                            //DrawContent(g, ps.DataInfo.Prob_C, colorC);
                            break;
                        case "G":
                            g.DrawString(gen, font, colorG, 0 + i * 15, 210);
                            //DrawContent(g, ps.DataInfo.Prob_G, colorG);
                            break;
                        case "T":
                            g.DrawString(gen, font, colorT, 0 + i * 15, 210);
                            //DrawContent(g, ps.DataInfo.Prob_T, colorT);
                            break;
                    }

                    if (i >= 73)
                        break;
                }

                DrawContent(g, ps.DataInfo.Prob_A, colorA, 74);
                //DrawContent(g, ps.DataInfo.Prob_C, colorC, 74);
                //DrawContent(g, ps.DataInfo.Prob_G, colorG, 74);
                //DrawContent(g, ps.DataInfo.Prob_T, colorT, 74);
            }
        }

        const float XSlice = 7.5f; // X轴刻度宽度
        const float XSpace = 0;
        const float YSpace = 25;
        const float YSliceBegin = 0;
        const float YSliceValue = 20; //Y轴刻度的数值宽度
        const float YSlice = 20; //Y轴刻度宽度

        /// <summary>
        /// 画曲线
        /// </summary>
        /// <param name="objGraphics"></param>
        private void DrawContent(Graphics objGraphics, byte[] fltCurrentValues, Brush clrCurrentColor, int length)
        {
            Pen CurvePen = new Pen(clrCurrentColor, 1);
            PointF[] CurvePointF = new PointF[length * 2 + 1];
            float keys = 0;
            float values = 0;
            CurvePointF[0] = new PointF(XSpace, 200);
            for (int i = 0; i < length; i++)
            {
                keys = XSlice * (i * 2 + 1) + XSpace;
                values = (panGen.Height - YSpace) + YSliceBegin - YSlice * (fltCurrentValues[i] / YSliceValue);

                CurvePointF[i * 2 + 1] = new PointF(keys, values);
                CurvePointF[i * 2 + 2] = new PointF(XSlice * (i * 2 + 2) + XSpace, (panGen.Height - YSpace) + YSliceBegin);
            }

            for (int i = 2; i < CurvePointF.Length - 1; i += 2)
            {
                //CurvePointF[i].Y = 200 - Math.Abs(CurvePointF[i + 1].Y - CurvePointF[i - 1].Y) * 0.1148f;
                //if (CurvePointF[i + 1].Y == 200 && CurvePointF[i - 1].Y == 200)
                //    continue;
                //CurvePointF[i].Y = 0.885f * (CurvePointF[i + 1].Y + CurvePointF[i - 1].Y) - 107.03f;
                CurvePointF[i].Y = 0.25f * (CurvePointF[i + 1].Y + CurvePointF[i - 1].Y) + 1.7746f * Math.Abs(CurvePointF[i + 1].Y - CurvePointF[i - 1].Y);
            }

            objGraphics.DrawCurve(CurvePen, CurvePointF, 0.5f);
        }
    }
}
