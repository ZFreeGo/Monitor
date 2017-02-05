using OxyPlot;
using OxyPlot.Axes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using ZFreeGo.Monitor.AutoStudio.CommCenter;
using ZFreeGo.ParamMonitor.PlotCurves;

namespace ZFreeGo.Monitor.AutoStudio
{

    public partial class MainWindow 
    {
        /// <summary>
        ///A相电流显示模型
        /// </summary>
        private PlotCurve plotIAModel;
        /// <summary>
        /// B相电流显示模型
        /// </summary>
        private PlotCurve plotIBModel;
        /// <summary>
        /// C相电流显示模型
        /// </summary>
        private PlotCurve plotICModel;
        /// <summary>
        /// 零序电流
        /// </summary>
        private PlotCurve plotZCModel;
        /// <summary>
        /// A相电压显示模型
        /// </summary>
        private PlotCurve plotUAModel;
        /// <summary>
        /// B相电压显示模型
        /// </summary>
        private PlotCurve plotUBModel;
        /// <summary>
        /// C相电压显示模型
        /// </summary>
        private PlotCurve plotUCModel;
        /// <summary>
        /// 零序电压
        /// </summary>
        private PlotCurve plotZVModel;
        /// <summary>
        /// 开关位置
        /// </summary>
        private PlotCurve plotSPModel;

        /// <summary>
   

        /// <summary>
        /// 显示模型曲线
        /// </summary>
        private PlotCurve[] plotCollect = new PlotCurve[9]; 



       // private int plotTempShowTimeLen;

        private OxyColor[] colorsWave;

        void TestWave()
        {

            DateTime time = DateTime.Now;

            colorsWave = new OxyColor[9] { OxyColors.Yellow, OxyColors.Green, OxyColors.Red, OxyColors.Black,
                OxyColors.Yellow, OxyColors.Green, OxyColors.Red,  OxyColors.Black,OxyColors.Red};

            InitSingleCurve(ref plotIAModel, plotIA, "A相电流", "kA", colorsWave[0]);
            InitSingleCurve(ref plotIBModel, plotIB, "B相电流", "kA", colorsWave[1]);
            InitSingleCurve(ref plotICModel, plotIC, "C相电流", "kA", colorsWave[2]);
            InitSingleCurve(ref plotICModel, plotZC, "零序电流", "A", colorsWave[3]);
            InitSingleCurve(ref plotUAModel, plotUA, "A相电压", "kV", colorsWave[4]);
            InitSingleCurve(ref plotUBModel, plotUB, "B相电压", "kV", colorsWave[5]);
            InitSingleCurve(ref plotUCModel, plotUC, "C相电压", "kV", colorsWave[6]);
            InitSingleCurve(ref plotICModel, plotZV, "零序电压", "A", colorsWave[7]);
            InitSingleCurve(ref plotSPModel, plotSP, "开关状态", "1/0", colorsWave[8]);

            plotCollect[0] = plotIAModel;
            plotCollect[1] = plotIBModel;
            plotCollect[2] = plotICModel;
            plotCollect[3] = plotZCModel;
            plotCollect[4] = plotUAModel;
            plotCollect[5] = plotUBModel;
            plotCollect[6] = plotUCModel;
            plotCollect[7] = plotZVModel;
            plotCollect[8] = plotSPModel;


            simuLationData();
            
        }

        void simuLationData()
        {
            double da = 0;
            double db = 0;
            double dc = 0;
            double fact = 1;
            for (int i = 0; i < 128 * 12; i++)
            {
                da = fact * Math.Cos(Math.PI / 64 * i + Math.PI / 3 * 0);
                db = fact * Math.Cos(Math.PI / 64 * i + Math.PI / 3 * 2);
                dc = fact * Math.Cos(Math.PI / 64 * i + Math.PI / 3 * 4);


                if (i < 128 * 6)
                {
                    fact = Math.Exp(i / (128.0 * 4.0));
                }
                else
                {
                    fact = 0;
                }


                double t = 20.0 / 128.0 * i;
                plotIAModel.LinesCollect.ElementAt(0).Elem.Points.Add(new DataPoint(t, da));
                plotIBModel.LinesCollect.ElementAt(0).Elem.Points.Add(new DataPoint(t, db));
                plotICModel.LinesCollect.ElementAt(0).Elem.Points.Add(new DataPoint(t, dc));
                plotUAModel.LinesCollect.ElementAt(0).Elem.Points.Add(new DataPoint(t, da));
                plotUBModel.LinesCollect.ElementAt(0).Elem.Points.Add(new DataPoint(t, db));
                plotUCModel.LinesCollect.ElementAt(0).Elem.Points.Add(new DataPoint(t, dc));

            }
            plotIAModel.MplotModel.InvalidatePlot(true);
        }
        private void updatePlot(PlotCurve plotM, int index, double y, int tiemLen, DateTime time)
        {
            try
            {
                //检查显示时间窗口，超过时间的予以移除
                foreach (var line in plotM.LinesCollect)
                {
                    int len = line.Elem.Points.Count; //获取点数
                    for (int i = 0; i < len; i++)
                    {

                        var t0 = DateTimeAxis.ToDateTime(line.Elem.Points[i].X);
                        if ((time - t0).TotalSeconds > tiemLen)
                        {
                            line.Elem.Points.RemoveAt(0);
                            i--;
                            len--;
                            continue; //此处大于60s 继续
                        }
                        break; //没有大于则停止检测


                    }
                }



                plotM.LinesCollect.ElementAt(index).Elem.Points.Add(new DataPoint(DateTimeAxis.ToDouble(time), y));

                plotM.MplotModel.InvalidatePlot(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "更新显示");
            }

        }
        /// <summary>
        /// 初始化单曲线显示
        /// </summary>
        /// <param name="plot">显示模型</param>
        /// <param name="pv">显示视图</param>
        /// <param name="title">标题</param>
        /// <param name="strLabel">图例</param>
        /// <param name="colors">颜色</param>
        private void InitSingleCurve(ref PlotCurve plot, OxyPlot.Wpf.PlotView pv, string title, string strLabel, OxyColor colors)
        {
            try
            {
                plot = new PlotCurve(strLabel, colors, title);
                    pv.Model = plot.GetModel();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "初始化显示plot" + title );
            }
        }

        /// <summary>
        /// 波形数据显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void recordCollect_WaveformArrived(object sender, WaveformArrivedEventArgs e)
        {
            try
            {
                UpdatePlotData(e.RecordList);
                if (sender is RecordDatOneCollect)
                {
                    var record = (RecordDatOneCollect)sender;
                    record.WaveformArrived -= recordCollect_WaveformArrived;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "recordCollect_WaveformArrived波形数据显示");
            }
        }
        /// <summary>
        /// 更新波形显示
        /// </summary>
        /// <param name="recordList"></param>
        private void UpdatePlotData(List<RecordDataItem> recordList)
        {
            try
            {
                int len = recordList[0].WaveDataAdc.Length;
                //删除所有元素
                plotIAModel.LinesCollect.ElementAt(0).Elem.Points.Clear();
                plotIBModel.LinesCollect.ElementAt(0).Elem.Points.Clear();
                plotICModel.LinesCollect.ElementAt(0).Elem.Points.Clear();
                plotZCModel.LinesCollect.ElementAt(0).Elem.Points.Clear();
                plotUAModel.LinesCollect.ElementAt(0).Elem.Points.Clear();
                plotUBModel.LinesCollect.ElementAt(0).Elem.Points.Clear();
                plotUCModel.LinesCollect.ElementAt(0).Elem.Points.Clear();
                plotZVModel.LinesCollect.ElementAt(0).Elem.Points.Clear();

                for (int i = 0; i < len; i++)
                {
                    var t = (double)((double)i * 20.0 / 128.0);
                    plotIAModel.LinesCollect.ElementAt(0).Elem.Points.Add(new DataPoint(t, recordList[0].WaveDataAdc[i]));
                    plotIBModel.LinesCollect.ElementAt(0).Elem.Points.Add(new DataPoint(t, recordList[1].WaveDataAdc[i]));
                    plotICModel.LinesCollect.ElementAt(0).Elem.Points.Add(new DataPoint(t, recordList[2].WaveDataAdc[i]));
                    plotZCModel.LinesCollect.ElementAt(0).Elem.Points.Add(new DataPoint(t, recordList[3].WaveDataAdc[i]));
                    plotUAModel.LinesCollect.ElementAt(0).Elem.Points.Add(new DataPoint(t, recordList[4].WaveDataAdc[i]));
                    plotUBModel.LinesCollect.ElementAt(0).Elem.Points.Add(new DataPoint(t, recordList[5].WaveDataAdc[i]));
                    plotUCModel.LinesCollect.ElementAt(0).Elem.Points.Add(new DataPoint(t, recordList[6].WaveDataAdc[i]));
                    plotZVModel.LinesCollect.ElementAt(0).Elem.Points.Add(new DataPoint(t, recordList[7].WaveDataAdc[i]));
                }

                plotIAModel.MplotModel.InvalidatePlot(true);
                plotIBModel.MplotModel.InvalidatePlot(true);
                plotICModel.MplotModel.InvalidatePlot(true);
                plotZCModel.MplotModel.InvalidatePlot(true);
                plotUAModel.MplotModel.InvalidatePlot(true);
                plotUBModel.MplotModel.InvalidatePlot(true);
                plotUCModel.MplotModel.InvalidatePlot(true);
                plotZVModel.MplotModel.InvalidatePlot(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "UpdatePlotData");
            }

        }


        
    }
}
