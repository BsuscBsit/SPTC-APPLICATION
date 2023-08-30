using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace SPTC_APPLICATION.View
{
    internal class Scaler
    {
        public static double ConvertCentimetersToDIP(double centimeters, Visual visual)
        {
            // Get the current presentation source for the specified visual
            PresentationSource source = PresentationSource.FromVisual(visual);

            if (source != null)
            {
                // Retrieve the current DPI settings
                double dpiX = 96.0 * source.CompositionTarget.TransformToDevice.M11;

                // Calculate the equivalent DIP value
                double dipValue = centimeters * (dpiX / 2.54);

                return dipValue;
            }

            return 0.0; // Return 0 if unable to retrieve DPI settings
        }
    }
}
