using System;
using Raspberry.IO.GeneralPurpose;
using Raspberry.IO.GeneralPurpose.Behaviors;
using System.Threading;

namespace BlinkyLight
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			var led = ConnectorPin.P1Pin7.Output();

			var connection = new GpioConnection (led);

			for (var i = 0; i < 100; i++) {
				connection.Toggle (led);
				Thread.Sleep (250);
			}
		}
	}
}
