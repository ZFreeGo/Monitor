using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace ZFreeGo.ParamMonitor.PlotCurves
{
    /// <summary>
    /// 曲线元素
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CurveElem<T>
    {
        /// <summary>
        /// 图例
        /// </summary>
        private string label;
        public string Label
        {
            set {label = value; }
            get {return label;}
        }
        private T elem;
        public T Elem
        {
            set { elem = value; }
            get { return elem; }
        }
        private OxyColor lineColor;
        /// <summary>
        /// 线条颜色
        /// </summary>
        public OxyColor LineColor
        {
            set { lineColor = value; }
            get { return lineColor; }
     
        }
        private double thickness;
        /// <summary>
        /// 线条宽度
        /// </summary>
        public double Thickness
        {
            set { thickness = value; }
            get { return thickness; }

        }


        public CurveElem(T ob, string lab)
        {
            this.elem = ob;
            this.label = lab;
            
        }
        public CurveElem(T ob, string lab, OxyColor lineColor, double thickness) : this(ob, lab)
        {
            LineColor = lineColor;
            Thickness = thickness;
        }
    }
}
