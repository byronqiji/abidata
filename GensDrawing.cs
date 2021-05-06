using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace APIData
{
    public class GensDrawing
    {
        const float XSlice = 7.5f; // X轴刻度宽度
        const float XSpace = 0;
        const float YSpace = 40;
        const float YSliceBegin = 0;
        const float YSliceValue = 20; //Y轴刻度的数值宽度
        const float YSlice = 20; //Y轴刻度宽度

        const int Height = 100;

        static readonly Brush colorG = new SolidBrush(Color.FromArgb(0, 0, 0));
        static readonly Brush colorT = new SolidBrush(Color.FromArgb(255, 0, 0));
        static readonly Brush colorC = new SolidBrush(Color.FromArgb(0, 0, 255));
        static readonly Brush colorA = new SolidBrush(Color.FromArgb(0, 255, 0));
        static readonly Font font = new Font("Arial", 8f);
        static readonly Font fontIndex = new Font("Arial", 6f);

        public static void ShowGen(Graphics g, List<GenViewMode> ps, int index)
        {
            int y = index * (Height + 10);

            Rectangle r = new Rectangle(0, (index - 1) * (Height + 10), 1510, Height);

            g.FillRectangle(Brushes.White, r);
            for (int i = 0; i < ps.Count; ++i)
            {
                string gen = ps[i].Gen;
                switch (gen)
                {
                    case "A":
                        g.DrawString(gen, font, colorA, i * 15, y - 37);
                        break;
                    case "C":
                        g.DrawString(gen, font, colorC, i * 15, y - 37);
                        break;
                    case "G":
                        g.DrawString(gen, font, colorG, i * 15, y - 37);
                        break;
                    case "T":
                        g.DrawString(gen, font, colorT, i * 15, y - 37);
                        break;
                }

                if (i > 0 && (i + 1) % 10 == 0)
                {
                    g.DrawString(((index - 1) * 100 + i + 1).ToString(), fontIndex, colorG, i * 15, y - 22);
                }
            }

            DrawContent(g, ps.Select((p) => p.ProbA).ToArray(), colorA, y);
            DrawContent(g, ps.Select((p) => p.ProbC).ToArray(), colorC, y);
            DrawContent(g, ps.Select((p) => p.ProbG).ToArray(), colorG, y);
            DrawContent(g, ps.Select((p) => p.ProbT).ToArray(), colorT, y);
        }

        /// <summary>
        /// 画曲线
        /// </summary>
        /// <param name="objGraphics"></param>
        private static void DrawContent(Graphics objGraphics, byte[] fltCurrentValues, Brush clrCurrentColor, float y)
        {
            Pen CurvePen = new Pen(clrCurrentColor, 1);
            PointF[] CurvePointF = new PointF[fltCurrentValues.Length * 2 + 1];
            CurvePointF[0] = new PointF(XSpace, y - YSpace);
            for (int i = 0; i < fltCurrentValues.Length; i++)
            {
                float keys = XSlice * (i * 2 + 1) + XSpace;
                float values = (y - YSpace) + YSliceBegin - YSlice * (fltCurrentValues[i] / YSliceValue);

                CurvePointF[i * 2 + 1] = new PointF(keys, values);
                CurvePointF[i * 2 + 2] = new PointF(XSlice * (i * 2 + 2) + XSpace, (y - YSpace) + YSliceBegin);
            }

            for (int i = 2; i < CurvePointF.Length - 1; i += 2)
            {
                float sum = y * 2 - CurvePointF[i + 1].Y - CurvePointF[i - 1].Y;

                CurvePointF[i].Y = y - ((YSpace / sum - 0.5f) * 0.3f + 0.5f) * sum + 0.2352f * Math.Abs(CurvePointF[i + 1].Y - CurvePointF[i - 1].Y);
            }

            objGraphics.DrawCurve(CurvePen, CurvePointF, 0.5f);
        }
    }
}
