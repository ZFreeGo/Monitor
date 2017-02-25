/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocatorTemplate xmlns:vm="clr-namespace:ZFreeGo.Monitor.DASDock.ViewModel"
                                   x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"
*/

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using System.Windows.Navigation;
using ZFreeGo.Monitor.DASDock.View;
using ZFreeGo.Monitor.DASModel.GetViewData;


namespace ZFreeGo.Monitor.DASDock.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

           
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<CalibrationViewModel>();
            SimpleIoc.Default.Register<CommunicationViewModel>();
            SimpleIoc.Default.Register<ParameterViewModel>();
            SimpleIoc.Default.Register<ProtectSetpointViewModel>();
            SimpleIoc.Default.Register<SOEViewModel>();
            SimpleIoc.Default.Register<TelecontrolViewModel>();
            SimpleIoc.Default.Register<TelemeteringViewModel>();
            SimpleIoc.Default.Register<TelesignalisationViewModel>();
            SimpleIoc.Default.Register<ElectricPulseViewModel>();
            
        }

        /// <summary>
        /// Gets the Main property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]


        

        public static MainViewModel main;
        public static MainViewModel Main
        {
            get
            {
                if (main == null)
                {
                    main =  ServiceLocator.Current.GetInstance<MainViewModel>();
                }
                return main;
               
            }
        }
        public static CalibrationViewModel calibration;
        public static CalibrationViewModel Calibration
        {
            get
            {
                if (calibration == null)
                {
                    calibration = ServiceLocator.Current.GetInstance<CalibrationViewModel>();
                }
                return calibration;                
            }
        }
        public static CommunicationViewModel communication;
        public static CommunicationViewModel Communication
        {
            get
            {
                if (communication == null)
                {
                    communication = ServiceLocator.Current.GetInstance<CommunicationViewModel>();
                }
                return communication;
            }
        }
        public static ParameterViewModel parameter;
        public static ParameterViewModel Parameter
        {
            get
            {
                if (parameter == null)
                {
                    parameter = ServiceLocator.Current.GetInstance<ParameterViewModel>();
                }
                return parameter;
            }
        }
        public static ProtectSetpointViewModel protectSetpoint;
        public static ProtectSetpointViewModel ProtectSetpoint
        {
            get
            {
                if (protectSetpoint == null)
                {
                    protectSetpoint = ServiceLocator.Current.GetInstance<ProtectSetpointViewModel>();
                }
                return protectSetpoint;
            }
        }
        public static SOEViewModel soe;
        public static SOEViewModel SOE
        {
            get
            {
                if (soe == null)
                {
                    soe = ServiceLocator.Current.GetInstance<SOEViewModel>();
                }
                return soe;
            }
        }
        public static TelecontrolViewModel telecontrol;
        public static TelecontrolViewModel Telecontrol
        {
            get
            {
                if (telecontrol == null)
                {
                    telecontrol = ServiceLocator.Current.GetInstance<TelecontrolViewModel>();
                }
                return telecontrol;
            }
        }
        public static TelemeteringViewModel telemetering;
        public static TelemeteringViewModel Telemetering
        {
            get
            {
                if (telemetering == null)
                {
                    telemetering = ServiceLocator.Current.GetInstance<TelemeteringViewModel>();
                }
                return telemetering;
            }
        }
        public static TelesignalisationViewModel telesignalisation;
        public static TelesignalisationViewModel Telesignalisation
        {
            get
            {
                if (telesignalisation == null)
                {
                    telesignalisation = ServiceLocator.Current.GetInstance<TelesignalisationViewModel>();
                }
                return telesignalisation;
            }
        }
        public static ElectricPulseViewModel electricPulsen;
        public static ElectricPulseViewModel ElectricPulse
        {
            get
            {
                if (electricPulsen == null)
                {
                    electricPulsen = ServiceLocator.Current.GetInstance<ElectricPulseViewModel>();
                }
                return electricPulsen;
            }
        }
        /// <summary>
        /// Cleans up all the resources.
        /// </summary>
        public static void Cleanup()
        {
        }
    }
}