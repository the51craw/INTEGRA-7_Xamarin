using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Hardware.Usb;

namespace INTEGRA_7_Xamarin.Droid
{
    public class UsbReceiver : BroadcastReceiver
    {
        private static String ACTION_USB_PERMISSION = "com.android.example.USB_PERMISSION";

        public override void OnReceive(Context context, Intent intent)
        {
            String action = intent.Action;
            if (ACTION_USB_PERMISSION.Equals(action))
            {
                lock (this)
                {
                    UsbDevice device = (UsbDevice)intent
                            .GetParcelableExtra(UsbManager.ExtraDevice);

                    if (intent.GetBooleanExtra(
                            UsbManager.ExtraPermissionGranted, false))
                    {
                        if (device != null)
                        {
                            // call method to set up device communication
                        }
                    }
                    else
                    {

                    }
                }
            }
            else
            {

            }
        }
    }

}