using OxyPlot;
using OxyPlot.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using ZFreeGo.FileOperation.Comtrade;
using ZFreeGo.FileOperation.Comtrade.ConfigContent;
using ZFreeGo.FileOperation.Comtrade.DataContent;

namespace ZFreeGo.ParamMonitor.PlotCurves
{
    /// <summary>
    /// 绘制模型管理器
    /// </summary>
    public class PlotModelManager
    {
        /// <summary>
        /// 显示曲线模型数组,用于显示曲线控制
        /// </summary>
        private List<PlotCurve> plotCollect;

        /// <summary>
        /// Contrade文件管理器，用于获取基本信息
        /// </summary>
        private ComtradeConfigFile configFile;
        private ComtradeDataFile dataFile;

        private List<OxyColor> colorsWave;

     //   private StackPanel stackPlanel;

        private List<PlotView> plotViewCollect;

        /// <summary>
        /// 显示曲线元素
        /// </summary>
        public List<PlotView> PlotViewCollect
        {
            get
            {
                return plotViewCollect;
            }
        }
        /// <summary>
        /// 模型初始化
        /// </summary>
        /// <param name="inConfigFile">配置文件</param>
        public PlotModelManager(ComtradeConfigFile inConfigFile, ComtradeDataFile inDataFile)
        {
            try
            {
                configFile = inConfigFile;
                dataFile = inDataFile;

                plotCollect = new List<PlotCurve>();
                colorsWave = new List<OxyColor>();
                plotViewCollect = new List<PlotView>();

                //测试颜色
                var colors = new OxyColor[9] { OxyColors.Yellow, OxyColors.Green, OxyColors.Red, OxyColors.Black,
                OxyColors.Yellow, OxyColors.Green, OxyColors.Red,  OxyColors.Black,OxyColors.Red};
                colorsWave.AddRange(colors);
                initWaveProperty();

                
                //LoadShowData(inDataFile);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "PlotModelManager");
            }

        }

   
        /// <summary>
        /// 初始化显示曲线
        /// </summary>
        private void initWaveProperty()
        {


            for(int i = 0; i < configFile.RowChannelNumType.AnalogChannelCount; i++)
            {
                //plotView
                var plotView = new PlotView();
                plotView.MinHeight = 300;
                plotView.Name ="Analog" + configFile.RowAnalogChannel[i].ChannelIndex;
                plotViewCollect.Add(plotView);


                //plotModel
                string strLabel = configFile.RowAnalogChannel[i].ChannelUnit;
                string title = configFile.RowAnalogChannel[i].ChannelID;

                var plot = new PlotCurve(strLabel, colorsWave[i], title);
                plotView.Model = plot.GetModel();
                plotCollect.Add(plot);
               
            }
            for (int i = 0; i < configFile.RowChannelNumType.DigitalChannelCount; i++)
            {
                //plotView
                var plotView = new PlotView();
                plotView.MinHeight = 300;
                plotView.Name = "Digital" + configFile.RowDigitalChannel[i].ChannelIndex;
                plotViewCollect.Add(plotView);


                //plotModel
                string strLabel = "1/0";
                string title = configFile.RowDigitalChannel[i].ChannelID;

                var plot = new PlotCurve(strLabel, colorsWave[i], title);
                plotView.Model = plot.GetModel();
                plotCollect.Add(plot);

            }


        }
        /// <summary>
        /// 初始化曲线属性
        /// </summary>
        //void InitWaveProperty()
        //{

        //    DateTime time = DateTime.Now;

        //    colorsWave = new OxyColor[9] { OxyColors.Yellow, OxyColors.Green, OxyColors.Red, OxyColors.Black,
        //        OxyColors.Yellow, OxyColors.Green, OxyColors.Red,  OxyColors.Black,OxyColors.Red};

        //    InitSingleCurve(ref plotIAModel, plotIA, "A相电流", "kA", colorsWave[0]);
        //    InitSingleCurve(ref plotIBModel, plotIB, "B相电流", "kA", colorsWave[1]);
        //    InitSingleCurve(ref plotICModel, plotIC, "C相电流", "kA", colorsWave[2]);
        //    InitSingleCurve(ref plotICModel, plotZC, "零序电流", "A", colorsWave[3]);
        //    InitSingleCurve(ref plotUAModel, plotUA, "A相电压", "kV", colorsWave[4]);
        //    InitSingleCurve(ref plotUBModel, plotUB, "B相电压", "kV", colorsWave[5]);
        //    InitSingleCurve(ref plotUCModel, plotUC, "C相电压", "kV", colorsWave[6]);
        //    InitSingleCurve(ref plotICModel, plotZV, "零序电压", "A", colorsWave[7]);
        //    InitSingleCurve(ref plotSPModel, plotSP, "开关状态", "1/0", colorsWave[8]);

        //    plotCollect[0] = plotIAModel;
        //    plotCollect[1] = plotIBModel;
        //    plotCollect[2] = plotICModel;
        //    plotCollect[3] = plotZCModel;
        //    plotCollect[4] = plotUAModel;
        //    plotCollect[5] = plotUBModel;
        //    plotCollect[6] = plotUCModel;
        //    plotCollect[7] = plotZVModel;
        //    plotCollect[8] = plotSPModel;


        //    simuLationData();

        //}


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
                MessageBox.Show(ex.Message, "初始化显示plot" + title);
            }
        }

        /// <summary>
        /// 载入每条曲线上一个点
        /// </summary>
        /// <param name="ascii">ASCII数据</param>
        private void LoadCurvePoint(ASCIIContent ascii)
        {
            for (int analogNum = 0;
                           analogNum < configFile.RowChannelNumType.AnalogChannelCount; analogNum++)
            {

                int data = ascii.AnalogChannelData[analogNum];
                var point = new DataPoint(ascii.SampleTimeStamp, data);
                plotCollect[analogNum].LinesCollect.ElementAt(0).Elem.Points.Add(point);

            }
            for (int digitalNum = 0;
                digitalNum < configFile.RowChannelNumType.DigitalChannelCount; digitalNum++)
            {

                int data = ascii.DigitalChannelData[digitalNum];
                var point = new DataPoint(ascii.SampleTimeStamp, data);
                plotCollect[configFile.RowChannelNumType.AnalogChannelCount + digitalNum].
                    LinesCollect.ElementAt(0).Elem.Points.Add(point);

            }
        }
        /// <summary>
        /// 载入ASCII显示数据
        /// </summary>
        public void loadShowASCIIData()
        {
            try
            {

                int showLen = dataFile.AsciiData.Count;//configFile.RowSampleRateInformation.EndSampleCount;

                for (int index = 0; index < showLen; index++)
                {
                    var ascii = new ASCIIContent(dataFile.AsciiData[index],
                         configFile.RowChannelNumType.AnalogChannelCount,
                         configFile.RowChannelNumType.DigitalChannelCount);


                    LoadCurvePoint(ascii);                   

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "载入显示数据");
            }

        }
        /// <summary>
        /// 载入binary显示数据
        /// </summary>
        private void  loadShowBinaryData()
        {
            try
            {
               
               
                int showLen = dataFile.BinaryData.Count;//configFile.RowSampleRateInformation.EndSampleCount;

                for (int index = 0; index < showLen; index++)
                {
                    var binaryOrigin = new BinaryContent(dataFile.BinaryData[index],
                         configFile.RowChannelNumType.AnalogChannelCount,
                         configFile.RowChannelNumType.DigitalChannelCount);
                    var ascii = new ASCIIContent(binaryOrigin);


                    LoadCurvePoint(ascii);
                   
                }
               foreach(var m in plotCollect)
               {
                   m.MplotModel.InvalidatePlot(true);
                   
               }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "载入显示数据");
            }

        }

        /// <summary>
        /// 载入显示数据
        /// </summary>
        public void LoadShowData()
        {
            if (configFile.RowDataFileType.DataType == DataFileType.BINARY)
            {
                loadShowBinaryData();
            }
            else if (configFile.RowDataFileType.DataType == DataFileType.ASCII)
            {
                loadShowASCIIData();
            }
        }
        //void simuLationData()
        //{
        //    double da = 0;
        //    double db = 0;
        //    double dc = 0;
        //    double fact = 1;
        //    for (int i = 0; i < 128 * 12; i++)
        //    {
        //        da = fact * Math.Cos(Math.PI / 64 * i + Math.PI / 3 * 0);
        //        db = fact * Math.Cos(Math.PI / 64 * i + Math.PI / 3 * 2);
        //        dc = fact * Math.Cos(Math.PI / 64 * i + Math.PI / 3 * 4);


        //        if (i < 128 * 6)
        //        {
        //            fact = Math.Exp(i / (128.0 * 4.0));
        //        }
        //        else
        //        {
        //            fact = 0;
        //        }


        //        double t = 20.0 / 128.0 * i;
        //        plotIAModel.LinesCollect.ElementAt(0).Elem.Points.Add(new DataPoint(t, da));
        //        plotIBModel.LinesCollect.ElementAt(0).Elem.Points.Add(new DataPoint(t, db));
        //        plotICModel.LinesCollect.ElementAt(0).Elem.Points.Add(new DataPoint(t, dc));
        //        plotUAModel.LinesCollect.ElementAt(0).Elem.Points.Add(new DataPoint(t, da));
        //        plotUBModel.LinesCollect.ElementAt(0).Elem.Points.Add(new DataPoint(t, db));
        //        plotUCModel.LinesCollect.ElementAt(0).Elem.Points.Add(new DataPoint(t, dc));

        //    }
        //    plotIAModel.MplotModel.InvalidatePlot(true);
        //}
    }
}
