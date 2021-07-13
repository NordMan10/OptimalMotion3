using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using OptimalMotion2.Enums;
using OptimalMotion2.Domain.Static;

namespace OptimalMotion2.Domain
{
    public class Chart : IChart
    {
        public Chart(PictureBox graphicBase)
        {
            
            this.graphicBase = graphicBase;
            graphicBase.Paint += GraphicBase_Paint;
            
        }

        private readonly PictureBox graphicBase;
        /// <summary>
        /// Длина графика в пикселях
        /// </summary>
        private int lineLength;
        private int pixelsCountForMinute;
        private int YLineCoordinate = 400;
        private int XLineStartCoordinate = 50;
        private List<IChartMomentData> data = new List<IChartMomentData>();
        private Pen defaultPen = new Pen(Color.Black, 3f);
        private Pen landingPen = new Pen(Color.Black, 3f);
        private Pen takingOffPen = new Pen(Color.FromArgb(255, 36, 215, 42), 3f);

        private void GraphicBase_Paint(object sender, PaintEventArgs e)
        {
            lineLength = graphicBase.Width;
            pixelsCountForMinute = GetPixelsCountForRange(60);
            
            DrawMoments(e);
        }

        public void DrawTimeLine(Pen pen, PaintEventArgs e, int xLineStartCoordinate,
            int yLineCoordinate)
        {
            var startPoint = new Point(xLineStartCoordinate, yLineCoordinate);
            var endPoint = new Point(lineLength - xLineStartCoordinate, yLineCoordinate);

            e.Graphics.DrawLine(pen, startPoint, endPoint);
            DrawTriangles(e, pen, endPoint);
            DrawTimeLineCaptions(e, pen, startPoint, endPoint);
        }

        private void DrawTriangles(PaintEventArgs e, Pen pen, Point endPoint)
        {
            var trianglePoints = new List<PointF>
            {
                new PointF(endPoint.X, endPoint.Y - 5),
                new PointF(endPoint.X, endPoint.Y + 5),
                new PointF(endPoint.X + 9, endPoint.Y)
            };

            e.Graphics.FillPolygon(pen.Brush, trianglePoints.ToArray());
        }

        private void DrawCaption(PaintEventArgs e, Pen pen, Point point, string text)
        {
            var font = new Font("Roboto", 10f);

            e.Graphics.DrawString(text, font, pen.Brush, new PointF(point.X - 5, point.Y + 5));
        }

        private void DrawTimeLineCaptions(PaintEventArgs e, Pen pen, Point startPoint, Point endPoint)
        {
            DrawCaption(e, pen, new Point(startPoint.X, startPoint.Y + 20), "0");
            DrawCaption(e, pen, new Point(endPoint.X - 8, endPoint.Y + 20), 
                (ModellingParameters.ModellingTime / 60).ToString());
        }

        private void DrawMoments(PaintEventArgs e)
        {
            var lineHeight = 50;
            for (var i = 0; i < data.Count; i++)
            {
                if (data[i].SubType == ChartMomentDataType.Outdated)
                {
                    YLineCoordinate = 100;
                    var pen = new Pen(Color.Gray, 3);
                    DrawChart(pen, e, XLineStartCoordinate, YLineCoordinate, lineHeight, data[i].Moment,
                        data[i].AircraftId.Id.ToString());
                }
                else if (data[i].Type == AircraftBehavior.TakingOff)
                {
                    YLineCoordinate = 300;
                    var pen = takingOffPen; // ??
                    if (data[i].SubType == ChartMomentDataType.Conflict) 
                        pen = new Pen(Color.Red, 3); // ??
                    DrawChart(pen, e, XLineStartCoordinate, YLineCoordinate, lineHeight, data[i].Moment, 
                        data[i].AircraftId.Id.ToString());
                }
                else
                {
                    YLineCoordinate = 500;
                    var pen = landingPen;
                    DrawChart(pen, e, XLineStartCoordinate, YLineCoordinate, lineHeight, data[i].Moment,
                        Convertation.GetMinutesFromSeconds(data[i].Moment.Value).ToString());
                }
            }
        }

        private void DrawChart(Pen pen, PaintEventArgs e, int xLineStartCoordinate, 
            int yLineCoordinate, int lineHeight, IMoment moment, string captionText)
        {
            DrawTimeLine(defaultPen, e, xLineStartCoordinate, yLineCoordinate);

            var startPoint = new Point(xLineStartCoordinate + Convertation.GetMinutesFromSeconds(moment.Value) * pixelsCountForMinute,
                    yLineCoordinate);
            var endPoint = new Point(xLineStartCoordinate + Convertation.GetMinutesFromSeconds(moment.Value) * pixelsCountForMinute,
                yLineCoordinate - lineHeight);
            e.Graphics.DrawLine(pen, startPoint, endPoint);

            DrawCaption(e, pen, new Point(startPoint.X - 5, startPoint.Y), captionText);
        }

        /// <summary>
        /// Добавление момента
        /// </summary>
        /// <param name="chartMomentData"></param>
        public void AddMoment(IChartMomentData chartMomentData)
        {
            data.Add(chartMomentData);
            graphicBase.Invalidate();
        }

        /// <summary>
        /// Удаление момента: (ИНТЕРФЕЙС) (И.2)
        /// </summary>
        /// <param name="id"></param>
        public void RemoveMoment(IAircraftId id)
        {
            var index =  data.FindIndex(item => item.AircraftId.Id == id.Id);
            data.RemoveAt(index);
        }

        /// <summary>
        /// Перемещение момента
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newRow"></param>
        public void MoveMoment(IAircraftId id, IChartMomentData chartMomentData)
        {
            RemoveMoment(id);

            data.Add(chartMomentData);
        }

        /// <summary>
        /// Очищает график, удаляя все сохраненные значения
        /// </summary>
        public void Reset()
        {
            data.Clear();
            graphicBase.Invalidate();
        }

        /// <summary>
        /// Возвращает количество пикселей на временной оси для заданного в секундах промежутка времени 
        /// </summary>
        /// <param name="range">Промежуток времени в секундах</param>
        /// <returns></returns>
        private int GetPixelsCountForRange(int range)
        {
            //? Множитель для сохранения пропорции в зависимости от интервала временной оси
            //var factor = (double)range / timeLineRange;

            return (int)(lineLength / range);
        }
    }
}
