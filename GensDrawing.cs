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
        const float YSpace = 25;
        const float YSliceBegin = 0;
        const float YSliceValue = 20; //Y轴刻度的数值宽度
        const float YSlice = 20; //Y轴刻度宽度

        static readonly Brush colorG = new SolidBrush(Color.FromArgb(0, 0, 0));
        static readonly Brush colorT = new SolidBrush(Color.FromArgb(255, 0, 0));
        static readonly Brush colorC = new SolidBrush(Color.FromArgb(0, 0, 255));
        static readonly Brush colorA = new SolidBrush(Color.FromArgb(0, 255, 0));
        static readonly Font font = new Font("Arial", 8f);

        public static void ShowGen(Graphics g, List<GenViewMode> ps)
        {
            Rectangle r = new Rectangle(0, 0, 1125, 300);

            g.FillRectangle(Brushes.White, r);
            for (int i = 0; i < ps.Count; ++i)
            {
                string gen = ps[i].Gen;
                switch (gen)
                {
                    case "A":
                        g.DrawString(gen, font, colorA, 0 + i * 15, g.VisibleClipBounds.Height - 22);
                        break;
                    case "C":
                        g.DrawString(gen, font, colorC, 0 + i * 15, g.VisibleClipBounds.Height - 22);
                        break;
                    case "G":
                        g.DrawString(gen, font, colorG, 0 + i * 15, g.VisibleClipBounds.Height - 22);
                        break;
                    case "T":
                        g.DrawString(gen, font, colorT, 0 + i * 15, g.VisibleClipBounds.Height - 22);
                        break;
                }

                if (i >= 73)
                    break;
            }

            DrawContent(g, ps.Select((p) => p.ProbA).ToArray(), colorA);
            DrawContent(g, ps.Select((p) => p.ProbC).ToArray(), colorC);
            DrawContent(g, ps.Select((p) => p.ProbG).ToArray(), colorG);
            DrawContent(g, ps.Select((p) => p.ProbT).ToArray(), colorT);
        }

        /// <summary>
        /// 画曲线
        /// </summary>
        /// <param name="objGraphics"></param>
        private static void DrawContent(Graphics objGraphics, byte[] fltCurrentValues, Brush clrCurrentColor)
        {
            Pen CurvePen = new Pen(clrCurrentColor, 1);
            PointF[] CurvePointF = new PointF[fltCurrentValues.Length * 2 + 1];
            float keys = 0;
            float values = 0;
            CurvePointF[0] = new PointF(XSpace, objGraphics.VisibleClipBounds.Height - 25);
            for (int i = 0; i < fltCurrentValues.Length; i++)
            {
                keys = XSlice * (i * 2 + 1) + XSpace;
                values = (objGraphics.VisibleClipBounds.Height - YSpace) + YSliceBegin - YSlice * (fltCurrentValues[i] / YSliceValue);

                CurvePointF[i * 2 + 1] = new PointF(keys, values);
                CurvePointF[i * 2 + 2] = new PointF(XSlice * (i * 2 + 2) + XSpace, (objGraphics.VisibleClipBounds.Height - YSpace) + YSliceBegin);
            }

            for (int i = 2; i < CurvePointF.Length - 1; i += 2)
            {
                float sum = objGraphics.VisibleClipBounds.Height * 2 - CurvePointF[i + 1].Y - CurvePointF[i - 1].Y;

                CurvePointF[i].Y = objGraphics.VisibleClipBounds.Height - ((25.0f / sum - 0.5f) * 0.3f + 0.5f) * sum + 0.2352f * Math.Abs(CurvePointF[i + 1].Y - CurvePointF[i - 1].Y);
            }

            objGraphics.DrawCurve(CurvePen, CurvePointF, 0.5f);
        }
    }
}
