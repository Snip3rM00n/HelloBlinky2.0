/*
	Hello Blinky 2.0
	By: Anthony McKeever (GitHub: Snip3rM00n)
	
	A varient of the Microsoft IoT Introductory project "HelloBlinky".
	This version allows the user to change the blink interval on the fly using the app's UI.
	Requires Raspberry Pi 2 - Connect video to see UI.
	
	File: MainPage.xaml.cs - The code that powers the LED & UI.
	
	Coded In: C# (.NET Framework - Windows Uinversal App)
*/

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.Devices.Gpio;

namespace blinky
{
	public sealed partial class MainPage : Page
	{
		private const int LED_PIN = 5;
		private GpioPin pin;
		private GpioPinValue pinValue;
		private DispatcherTimer timer;
		private SolidColorBrush redBrush = new SolidColorBrush(Windows.UI.Colors.Red);
		private SolidColorBrush greyBrush = new SolidColorBrush(Windows.UI.Colors.LightGray);

		public MainPage()
		{
			InitializeComponent();
			timer = new DispatcherTimer();
			timer.Interval = TimeSpan.FromMilliseconds(500);
			timer.Tick += Timer_Tick;
			InitGPIO();

			if (pin != null)
			{
				timer.Start();
			}
		}

		private void InitGPIO()
		{
			var gpio = GpioController.GetDefault();

			if (gpio == null)
			{
				pin = null;
				GpioStatus.Text = "There is no GPIO controller on this device";
				return;
			}

			pin = gpio.OpenPin(LED_PIN);
			pinValue = GpioPinValue.High;
			pin.Write(pinValue);
			pin.SetDriveMode(GpioPinDriveMode.Output);
			GpioStatus.Text = "GPIO pin initialized correctly";
		}

		private void Timer_Tick(object sender, object e)
		{
			switch (pinValue)
			{
				case GpioPinValue.High:
					pinValue = GpioPinValue.Low;
					pin.Write(pinValue);
					LED.Fill = redBrush;
					break;

				default:
					pinValue = GpioPinValue.High;
					pin.Write(pinValue);
					LED.Fill = greyBrush;
					break;
			}
		}

		private void setTime_Click(object sender, RoutedEventArgs e)
		{
			int nMs = 0;

			try
			{
				nMs = Convert.ToInt32(newMS.Text);
				GpioStatus.Text = "New timer interval set!";
			}
			catch
			{
				nMs = 500;
				GpioStatus.Text = "Invalid input... please input an integer";
			}

			DisplayText.Text = string.Format("{0}ms", nMs);
			timer.Interval = TimeSpan.FromMilliseconds(nMs);
		}
	}
}
