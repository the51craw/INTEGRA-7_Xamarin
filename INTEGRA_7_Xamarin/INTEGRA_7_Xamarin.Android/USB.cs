using Android.App;
using Android.Content.PM;
using Android.OS;
using Xamarin.Forms;
using Android.Content;
using Android.Hardware.Usb;
using System.Collections.Generic;
using System;
using Java.Nio;

namespace INTEGRA_7_Xamarin.Droid
{
    [Android.Runtime.Register("eu.mrmartin.MIDI", "(Ljava/nio/ByteBuffer;I)Z", "GetQueue_Ljava_nio_ByteBuffer_IHandler")]

    public class USB : BroadcastReceiver
    {
        private static String ACTION_USB_PERMISSION = "eu.mrmartin.MIDI.USB_PERMISSION";
        private static String USB_DEVICE_ATTACHED = "android.hardware.usb.action.USB_DEVICE_ATTACHED";
        private static String USB_DEVICE_DETACHED = "android.hardware.usb.action.USB_DEVICE_DETACHED";
        public MainActivity MainActivity { get; set; }
        public UsbManager Manager { get; set; }
        public UsbInterface Interface { get; set; }
        public UsbDevice Device { get; set; }
        public UsbEndpoint OutputEndpoint { get; set; }
        public UsbEndpoint InputEndpoint { get; set; }
        public Boolean HasPermission { get; set; }
        public UsbDeviceConnection DeviceConnection { get; set; }
        public UsbRequest OutputRequest { get; set; }
        public UsbRequest InputRequest { get; set; }
        public Boolean IsReady { get; set;}

        public USB(UsbManager Manager, MainActivity MainActivity)
        {
            if (DeviceConnection != null)
            {
                DeviceConnection.ReleaseInterface(Interface);
                DeviceConnection.Close();
                DeviceConnection.Dispose();
            }

            IsReady = false;

            // The UsbManager can only be obtained from MainActivity!
            this.Manager = Manager;

            // We might need to access the main activity:
            this.MainActivity = MainActivity;

            // Parse for usb devices:
            ParseForUsbDevices();
        }

        ~USB()
        {
            if (IsReady)
            {
                DeviceConnection.ReleaseInterface(Interface);
                DeviceConnection.Close();
                DeviceConnection.Dispose();
            }
        }

        public override void OnReceive(Context context, Intent intent)
        {
            String action = intent.Action;
            if (!IsReady && ACTION_USB_PERMISSION.Equals(action))
            {
                lock (this)
                {
                    Device = (UsbDevice)intent.GetParcelableExtra(UsbManager.ExtraDevice);

                    if (intent.GetBooleanExtra(UsbManager.ExtraPermissionGranted, false))
                    {
                        if (Device != null)
                        {
                            DeviceConnection = Manager.OpenDevice(Device);
                            if (DeviceConnection.ClaimInterface(Interface, true))
                            {
                                OutputRequest = new UsbRequest();
                                OutputRequest.Initialize(DeviceConnection, OutputEndpoint);
                                InputRequest = new UsbRequest();
                                InputRequest.Initialize(DeviceConnection, InputEndpoint);
                                HasPermission = true;
                                IsReady = true;
                            }
                        }
                    }
                }
            }
            else if (!IsReady && !HasPermission && USB_DEVICE_ATTACHED.Equals(action))
            {
                // Parse for usb devices:
                ParseForUsbDevices();

                // Hook up USB :
                PendingIntent pendingIntent = PendingIntent.GetBroadcast(MainActivity, 0, new Intent(ACTION_USB_PERMISSION), 0);
                IntentFilter filter = new IntentFilter(ACTION_USB_PERMISSION);
                filter.AddAction(USB_DEVICE_ATTACHED);
                MainActivity.RegisterReceiver(this, filter);

                if (Device != null && Interface != null && OutputEndpoint != null && InputEndpoint != null)
                {
                    Manager.RequestPermission(Device, pendingIntent);
                    HasPermission = Manager.HasPermission(Device);
                }
            }
            else if (USB_DEVICE_DETACHED.Equals(action))
            {
                IsReady = false;
            }
        }

        public void ParseForUsbDevices()
        {
            Int32 deviceIndex = 0;
            Int32 deviceCount = 0;
            Int32 interfaceIndex = 0;
            Int32 endpointIndex = 0;
            UsbDevice[] usbDevices = null;
            UsbInterface[][] usbInterfaces = null;
            UsbEndpoint[][][] usbEndpoints = null;

            // Get the USB devices (normally one in an Android device):
            usbDevices = new UsbDevice[Manager.DeviceList.Count];
            foreach (KeyValuePair<string, UsbDevice> keyValuePair in Manager.DeviceList)
            {
                usbDevices[deviceIndex] = keyValuePair.Value;
                deviceIndex++;
            }
            deviceCount = deviceIndex;

            // Create usbInterface and usbEnpont lists:
            usbInterfaces = new UsbInterface[deviceCount][];
            usbEndpoints = new UsbEndpoint[deviceCount][][];

            // Loop all (one) devices and look for interfaces and endpoints:
            for (deviceIndex = 0; deviceIndex < deviceCount; deviceIndex++)
            {
                if (usbDevices[deviceIndex].InterfaceCount > 0)
                {
                    if (usbDevices[deviceIndex].ProductName == "INTEGRA-7")
                    {
                        Device = usbDevices[deviceIndex];
                        usbInterfaces[deviceIndex] = new UsbInterface[usbDevices[deviceIndex].InterfaceCount];
                        for (interfaceIndex = 0; interfaceIndex < usbDevices[deviceIndex].InterfaceCount; interfaceIndex++)
                        {
                            usbInterfaces[deviceIndex][interfaceIndex] = usbDevices[deviceIndex].GetInterface(interfaceIndex);
                        }
                        usbEndpoints[deviceIndex] = new UsbEndpoint[usbDevices[deviceIndex].InterfaceCount][];
                        for (interfaceIndex = 0; interfaceIndex < usbDevices[deviceIndex].InterfaceCount; interfaceIndex++)
                        {
                            usbEndpoints[deviceIndex][interfaceIndex] = new UsbEndpoint[usbInterfaces[deviceIndex][interfaceIndex].EndpointCount];
                            Interface = usbInterfaces[deviceIndex][interfaceIndex];
                            for (endpointIndex = 0; endpointIndex < usbInterfaces[deviceIndex][interfaceIndex].EndpointCount; endpointIndex++)
                            {
                                usbEndpoints[deviceIndex][interfaceIndex][endpointIndex] = usbInterfaces[deviceIndex][interfaceIndex].GetEndpoint(endpointIndex);
                                if (usbEndpoints[deviceIndex][interfaceIndex][endpointIndex].Direction == Android.Hardware.Usb.UsbAddressing.Out)
                                {
                                    OutputEndpoint = usbEndpoints[deviceIndex][interfaceIndex][endpointIndex];
                                }
                                else
                                {
                                    InputEndpoint = usbEndpoints[deviceIndex][interfaceIndex][endpointIndex];
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}