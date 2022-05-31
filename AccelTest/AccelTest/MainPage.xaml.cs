using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace AccelTest
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        private double xPosition = 0.0;
        private double yPosition = 0.0;
        private double zValue = 0.0;
        private double zValueAccumulator = 0.0;
        public MainPage()
        {
            InitializeComponent();
        }

        private void ButtonStart_Clicked(object sender, EventArgs e)
        {
            if (Accelerometer.IsMonitoring)
                return;
            Accelerometer.ReadingChanged += Accelerometer_ReadingChanged;
            Accelerometer.Start(SensorSpeed.UI);
        }

        private void Accelerometer_ReadingChanged(object sender, AccelerometerChangedEventArgs e)
        {
            //LabelX.Text = $"X: {e.Reading.Acceleration.X}";
            //LabelY.Text = $"Y: {e.Reading.Acceleration.Y}";
            //LabelZ.Text = $"Z: {e.Reading.Acceleration.Z}";
            xPosition -= e.Reading.Acceleration.X * 50;
            yPosition += e.Reading.Acceleration.Y * 50;
            zValue = e.Reading.Acceleration.Z;
            zValueAccumulator += (e.Reading.Acceleration.Z - 1);

            if (xPosition < 0) xPosition = 0;
            if (yPosition > 0) yPosition = 0;
            if (xPosition > 179) xPosition = 179;
            if (yPosition < -503) yPosition = -503;
            if (zValue > 2) { zValue = 2; }
            if (zValue < 0.5) { zValue = 0.5; }

            ButtonStart.TranslateTo(xPosition, yPosition, 1, null);
            ButtonStart.ScaleTo(1 / zValue);
            ButtonStart.RotateTo(zValueAccumulator);
            ButtonStart.Text = $"X: {xPosition}\nY: {yPosition}\nZ: {zValue}";
            //ButtonStart.BackgroundColor = Color.FromRgb(e.Reading.Acceleration.X * 100, e.Reading.Acceleration.Y * 100, zValue * 100);
        }

        private void ButtonStop_Clicked(object sender, EventArgs e)
        {
            if (!Accelerometer.IsMonitoring)
                return;
            Accelerometer.ReadingChanged -= Accelerometer_ReadingChanged;
            Accelerometer.Stop();
        }

        //private void BatteryInfo_Clicked(object sender, EventArgs e)
        //{
        //    var level = Battery.ChargeLevel;
        //    var state = Battery.State;
        //    var source = Battery.PowerSource;

        //    LabelBatteryChargeLevel.Text = $"Charge level: \n{ level * 100}%";
        //    LabelBatteryState.Text = $"Battery state: \n{ state }";
        //    LabelBatteryPowerSource.Text = $"Battery power source: \n{ source }";
        //}
    }
}
