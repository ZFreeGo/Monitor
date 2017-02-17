using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFreeGo.ParamMonitor.PlotCurves
{
    /// <summary>
    /// 绘制显示曲线
    /// </summary>
    public class PlotCurve
    {
        public List<CurveElem<LineSeries>> LinesCollect;
        private int count;
        public int Count
        {
            get { return LinesCollect.Count; }
        }
        private PlotModel plotModel;
        public PlotModel MplotModel
        {
            get { return plotModel; }
        }

    

        private object xAxis;
        private object yAxis;

        public Action SetXYAxis;

        public PlotCurve(int cn, string[] strLabel, string plotTitle)
        {
            LinesCollect = new List<CurveElem<LineSeries>>();
            //生成多条曲线
            for (int i = 0; i < cn; i++)
            {
                var lineSeries1 = new LineSeries();
               // lineSeries1.StrokeThickness = 1.5;
                    //lineSeries1.Color = OxyColors.SkyBlue;
                //lineSeries1.MarkerFill = lineSeries1.Color;
                //lineSeries1.MarkerSize = 3;
                    //lineSeries1.MarkerStroke = OxyColors.White;
              //  lineSeries1.MarkerStrokeThickness = 3;
               // lineSeries1.MarkerType = MarkerType.Circle;             
                LinesCollect.Add(new CurveElem<LineSeries>(lineSeries1, strLabel[i]));

               
            }
            count = cn;

            //生成模型
            plotModel = new PlotModel();

            plotModel.Title = plotTitle;

            //默认坐标轴H:m:s 
            SetXYAxis = () => 
            {  var dateTimeAxis1 = new DateTimeAxis();
                dateTimeAxis1.StringFormat = "HH:mm:ss.fff";
                //dateTimeAxis1.IsPanEnabled = false; //禁止平移
                //dateTimeAxis1.IsZoomEnabled = false; //禁止缩放
                dateTimeAxis1.MajorGridlineStyle = LineStyle.Solid;
                xAxis = dateTimeAxis1;

                var ya = new LinearAxis();
                ya.MajorGridlineStyle = LineStyle.Solid;
                ya.MinorGridlineStyle = LineStyle.Dot;
                yAxis = ya ;
            };
                    

        }

        public PlotCurve(string strLabel, OxyColor colors,  string plotTitle)
        {
            LinesCollect = new List<CurveElem<LineSeries>>();
           
                var lineSeries1 = new LineSeries();               
                lineSeries1.Color = colors;
                lineSeries1.StrokeThickness = 1.2;
                LinesCollect.Add(new CurveElem<LineSeries>(lineSeries1, strLabel));           
            count = 1;
            //生成模型
            plotModel = new PlotModel();
            plotModel.Title = plotTitle;
            //默认坐标轴H:m:s 
            SetXYAxis = () =>
            {
                var xa = new LinearAxis();
                xa.MajorGridlineStyle = LineStyle.Solid;
                xa.MinorGridlineStyle = LineStyle.Dot;

                xa.IsPanEnabled = false; //禁止平移
                xa.IsZoomEnabled = false; //禁止缩放

                xa.Position = AxisPosition.Bottom;




                xAxis = xa;
                

                var ya = new LinearAxis();
                ya.MajorGridlineStyle = LineStyle.Solid;
                ya.MinorGridlineStyle = LineStyle.Dot;
                yAxis = ya;
            };
        }

        /// <summary>
        /// 获取显示模型
        /// </summary>
        /// <returns>显示模型</returns>
        public PlotModel GetModel()
        {
            SetXYAxis();           
            Action <object, string>  judgeXYAxis= (ax, str) =>
                {
                     if (ax is DateTimeAxis)
                    {

                        plotModel.Axes.Add(ax as DateTimeAxis);
                    }
                    else if (ax is LinearAxis)
                    {
                        plotModel.Axes.Add(ax as LinearAxis);
                    }
                    else if (ax is LogarithmicAxis)
                    {
                        plotModel.Axes.Add(ax as LogarithmicAxis);
                    }
                    else
                    {
                        throw new ArgumentException(str + "类型不在范围之内");
                    }
                };
            judgeXYAxis(xAxis, "横轴");
            judgeXYAxis(yAxis, "纵轴");

            
            foreach (var ele in LinesCollect)
            {
                ele.Elem.Title = ele.Label;
                plotModel.Series.Add(ele.Elem);

            }
           // plotModel.Background = OxyColors.SkyBlue;
            return plotModel;
        }

      
    }
}
