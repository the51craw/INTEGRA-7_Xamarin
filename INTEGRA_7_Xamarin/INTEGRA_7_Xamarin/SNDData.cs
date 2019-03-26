using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using Plugin.FilePicker.Abstractions;
using Plugin.FilePicker;
using Xamarin.Forms;

namespace Integra_7_Xamarin
{
    /**
     * This class holds all settings for a Super Natural drumkit tone.
     */

    [DataContract]
    public class SNDData
    {
        [DataMember]
        public CommonState.SimpleToneTypes ToneType { get; set; }
        [DataMember]
        public SuperNATURALDrumKit ToneData { get; set; }
        [DataMember]
        public CommonMFX MFX { get; set; }

        public SNDData()
        {
            ToneType = CommonState.SimpleToneTypes.SUPERNATURAL_DRUM_KIT;
        }

        public static async Task<SNDData> Deserialize<SNDData>(SNDData sndData)
        {
            try
            {
                FileData openStudioSetFile = await CrossFilePicker.Current.PickFile();
                if (openStudioSetFile != null && openStudioSetFile.FilePath.ToLower().EndsWith(".pcm_xml"))
                {
                    byte[] data = openStudioSetFile.DataArray;
                    await Task.Run(() =>
                    {
                        MemoryStream memoryStream = new MemoryStream(data);
                        memoryStream.Position = 0;
                        DataContractSerializer dataContractSerializer = new DataContractSerializer(typeof(SNDData));
                        XmlReader xmlReader = XmlReader.Create(memoryStream);
                        XmlDictionaryReader xmlDictionaryReader = XmlDictionaryReader.CreateDictionaryReader(xmlReader);
                        sndData = (SNDData)dataContractSerializer.ReadObject(xmlDictionaryReader);
                        xmlReader.Dispose();
                    });
                }
                else if (openStudioSetFile != null)
                {
                    await MainPage.GetMainPage().DisplayAlert("Integra-7 Librarian", "You can only restore snd drumkit data from a file with " +
                        "a file extension '.sndd_xml'!", "Cancel");
                }
            }
            catch (Exception e) { }
            return sndData;
        }

        public static async Task Serialize<SNDData>(SNDData sndData, String name)
        {
            try
            {
                String xmlText = "";
                await Task.Run(() =>
                    {
                        MemoryStream memoryStream = new MemoryStream();
                        XmlWriter xmlWriter = XmlWriter.Create(memoryStream);
                        using (XmlDictionaryWriter xmlDictionaryWriter = XmlDictionaryWriter.CreateDictionaryWriter(xmlWriter))
                        {
                            var dataContractSerializer = new DataContractSerializer(typeof(SNDData));
                            dataContractSerializer.WriteObject(memoryStream, sndData);
                            memoryStream.Seek(0, SeekOrigin.Begin);
                            for (Int32 i = 0; i < memoryStream.Length; i++)
                            {
                                xmlText += (char)memoryStream.ReadByte();
                            }
                            xmlWriter.Flush();
                            xmlDictionaryWriter.Dispose();
                        }
                        xmlWriter.Dispose();
                        memoryStream.Dispose();
                    });
                xmlText = xmlText.Replace("><", ">\n<");
                String[] lines = xmlText.Split('\n');
                String indent = "";
                xmlText = "";
                Boolean firstLine = true;
                foreach (String line in lines)
                {
                    if (firstLine)
                    {
                        xmlText = line + "\r\n";
                        firstLine = false;
                    }
                    else if (line.StartsWith("</"))
                    {
                        if (indent.Length > 0)
                        {
                            indent = indent.Remove(0, 1);
                        }
                        xmlText += indent + line + "\r\n";
                    }
                    else if (line.Contains("</"))
                    {
                        xmlText += indent + line + "\r\n";
                    }
                    else if (line.EndsWith("/>"))
                    {
                        xmlText += indent + line + "\r\n";
                    }
                    else
                    {
                        xmlText += indent + line + "\r\n";
                        indent += "\t";
                    }
                }
                MainPage.GetMainPage().uIHandler.myFileIO.SaveFileAsync(xmlText, ".sndd_xml", name);
            }
            catch (Exception e) { }
        }
    }

    public partial class UIHandler
    {
        public void UpdateIntegra7FromSND()
        {
            byte[] address;
            byte[] value;
            String switchText = "";

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMON, new byte[] { 0x00 });
            SendParameter(address, superNATURALDrumKit.superNATURALDrumKitCommon.Name);

            // Instrument bank is a bit special. Bank and sound number are put together at address 0 (4 nibbles!)
            // There is only one expansion bank that contains SuperNATURAL Drum Kits: ExSN6
            // Internal sounds start at 0x00 0x00 0x00 0x00
            // ExSN6 sounds start at    0x00 0x00 0x0a 0x09
            // Remember that the addressing is nibble only, i.e. ExSN6 starts at 0xa9 (169)
            // So, we simply make a combobox with the texts Internal and ExSN6
            // The selected index multiplied with 169 will be an offset to add (nibblewise) to the sound number.
            // When using the offset (and instrument number) remember to use a key offset address!
            for (Int32 i = 0; i < 62; i++)
            {
                address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x00 });

                value = new byte[] { 0x00, 0x00, (byte)(superNATURALDrumKit.superNATURALDrumKitKey[i].BankNumber * 0x0a),
                                (byte)(superNATURALDrumKit.superNATURALDrumKitKey[i].BankNumber * 0x09)};
                value = AddBytes16(value, SplitToBytes16((UInt16)(superNATURALDrumKit
                    .superNATURALDrumKitKey[i].InstNumber)));
                SendParameter(address, value);
            }

            //switch (switchText)
            //{
            //    case "cbEditTone_superNATURALDrumKit_Druminstrument_BankNumber":
            //        handleControlEvents = false;
            //        superNATURALDrumKit.superNATURALDrumKitKey[currentKey].BankNumber = (byte)(comboBox.SelectedIndex);
            //        // Bank number is 0 for Internal and 1 for ExSN6.Read more in MakeDynamicControls.cs AddSupernaturalDrumKitDruminstrumentControls()
            //        switch (superNATURALDrumKit.superNATURALDrumKitKey[currentKey].BankNumber)
            //        {
            //            case 0:
            //                cbEditTone_superNATURALDrumKit_Druminstrument_InstNumber_INT.Visibility = Visibility.Visible;
            //                cbEditTone_superNATURALDrumKit_Druminstrument_InstNumber_ExSN6.Visibility = Visibility.Collapsed;
            //                cbEditTone_superNATURALDrumKit_Druminstrument_InstNumber_INT.IsEnabled = true;
            //                cbEditTone_superNATURALDrumKit_Druminstrument_InstNumber_ExSN6.IsEnabled = false;
            //                try
            //                {
            //                    cbEditTone_superNATURALDrumKit_Druminstrument_InstNumber_INT.SelectedIndex =
            //                    superNATURALDrumKit.superNATURALDrumKitKey[currentKey].InstNumber[0];
            //                }
            //                catch
            //                {
            //                    cbEditTone_superNATURALDrumKit_Druminstrument_InstNumber_INT.SelectedIndex = 0;
            //                }
            //                break;
            //            case 1:
            //                cbEditTone_superNATURALDrumKit_Druminstrument_InstNumber_INT.Visibility = Visibility.Collapsed;
            //                cbEditTone_superNATURALDrumKit_Druminstrument_InstNumber_ExSN6.Visibility = Visibility.Visible;
            //                cbEditTone_superNATURALDrumKit_Druminstrument_InstNumber_INT.IsEnabled = false;
            //                cbEditTone_superNATURALDrumKit_Druminstrument_InstNumber_ExSN6.IsEnabled = true;
            //                try
            //                {
            //                    cbEditTone_superNATURALDrumKit_Druminstrument_InstNumber_ExSN6.SelectedIndex =
            //                        superNATURALDrumKit.superNATURALDrumKitKey[currentKey].InstNumber[1];
            //                }
            //                catch
            //                {
            //                    cbEditTone_superNATURALDrumKit_Druminstrument_InstNumber_ExSN6.SelectedIndex = 0;
            //                }
            //                break;
            //        }
            //        SuperNaturalDrumKitSetVariation();
            //        address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x00 });
            //        value = new byte[] { 0x00, 0x00, (byte)(superNATURALDrumKit.superNATURALDrumKitKey[currentKey].BankNumber * 0x0a),
            //                    (byte)(superNATURALDrumKit.superNATURALDrumKitKey[currentKey].BankNumber * 0x09)};
            //        value = AddBytes16(value, SplitToBytes16((UInt16)(superNATURALDrumKit
            //            .superNATURALDrumKitKey[currentKey].InstNumber[superNATURALDrumKit.superNATURALDrumKitKey[currentKey].BankNumber])));
            //        SendParameter(address, value);
            //    break;
            //    case "cbEditTone_superNATURALDrumKit_Druminstrument_InstNumber":
            //        switch (cbEditTone_superNATURALDrumKit_Druminstrument_BankNumber.SelectedIndex)
            //        {
            //            case 0:
            //                {
            //                    try
            //                    {
            //                        // When changing wave for a SN-D key, also update in commonState.keyNames so it can later be saved in 
            //                        commonState.keyNames[cbEditTone_superNATURALDrumKit_Druminstrument_InstNumber_INT.SelectedIndex] =
            //                            ((String)cbEditTone_superNATURALDrumKit_Druminstrument_InstNumber_INT.SelectedItem)
            //                            .Remove(0, ((String)cbEditTone_superNATURALDrumKit_Druminstrument_InstNumber_INT.SelectedItem)
            //                            .IndexOf(':') + 1).Trim();
            //                    }
            //                    catch { }
            //                    superNATURALDrumKit.superNATURALDrumKitKey[currentKey].InstNumber[superNATURALDrumKit.superNATURALDrumKitKey[currentKey].BankNumber] =
            //                        (byte)(comboBox.SelectedIndex);
            //                    SuperNaturalDrumKitSetVariation();
            //                    address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x00 });
            //                    value = new byte[] { 0x00, 0x00, (byte)(superNATURALDrumKit.superNATURALDrumKitKey[currentKey].BankNumber * 0x0a),
            //                                (byte)(superNATURALDrumKit.superNATURALDrumKitKey[currentKey].BankNumber * 0x09)};
            //                    value = AddBytes16(value, SplitToBytes16((UInt16)(superNATURALDrumKit
            //                        .superNATURALDrumKitKey[currentKey].InstNumber[superNATURALDrumKit.superNATURALDrumKitKey[currentKey].BankNumber])));
            //                    SendParameter(address, value);
            //                }
            //                break;
            //            case 1:
            //                {
            //                    try
            //                    {
            //                        // When changing wave for a SN-D key, also update in commonState.keyNames so it can later be saved in 
            //                        commonState.keyNames[cbEditTone_superNATURALDrumKit_Druminstrument_InstNumber_ExSN6.SelectedIndex] =
            //                            ((String)cbEditTone_superNATURALDrumKit_Druminstrument_InstNumber_ExSN6.SelectedItem)
            //                            .Remove(0, ((String)cbEditTone_superNATURALDrumKit_Druminstrument_InstNumber_ExSN6.SelectedItem)
            //                            .IndexOf(':') + 1).Trim();
            //                    }
            //                    catch { }
            //                    superNATURALDrumKit.superNATURALDrumKitKey[currentKey].InstNumber[superNATURALDrumKit.superNATURALDrumKitKey[currentKey].BankNumber] =
            //                        (byte)(comboBox.SelectedIndex);
            //                    SuperNaturalDrumKitSetVariation();
            //                    address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x00 });
            //                    value = new byte[] { 0x00, 0x00, (byte)(superNATURALDrumKit.superNATURALDrumKitKey[currentKey].BankNumber * 0x0a),
            //                                (byte)(superNATURALDrumKit.superNATURALDrumKitKey[currentKey].BankNumber * 0x09)};
            //                    value = AddBytes16(value, SplitToBytes16((UInt16)(superNATURALDrumKit
            //                        .superNATURALDrumKitKey[currentKey].InstNumber[superNATURALDrumKit.superNATURALDrumKitKey[currentKey].BankNumber] - 1)));
            //                    SendParameter(address, value);
            //                }
            //                break;
            //        }
            //        break;
            //}


            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMON, new byte[] { 0x00 });
            SendParameter(address, superNATURALDrumKit.superNATURALDrumKitCommon.Name);

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMON, new byte[] { 0x12 });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitCommon.PhraseNumber));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMON, new byte[] { 0x10 });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitCommon.KitLevel));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMON, new byte[] { 0x11 });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitCommon.AmbienceLevel));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x04 });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitKey[currentKey].Level));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x05 });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitKey[currentKey].Pan));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x06 });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitKey[currentKey].ChorusSendLevel));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x07 });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitKey[currentKey].ReverbSendLevel));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x08 });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitKey[currentKey].Tune));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x0c });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitKey[currentKey].Attack));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x0d });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitKey[currentKey].Decay));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x0e });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitKey[currentKey].Brilliance));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x0f });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitKey[currentKey].Variation));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x12 });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitKey[currentKey].OutputAssign));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x10 });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitKey[currentKey].DynamicRange));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x11 });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitKey[currentKey].StereoWidth));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x00 });
            SendParameter(address, (byte)((Boolean)superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[0].CompSwitch ? 1 : 0));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x0e });
            SendParameter(address, (byte)((Boolean)superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[1].CompSwitch ? 1 : 0));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x1c });
            SendParameter(address, (byte)((Boolean)superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[2].CompSwitch ? 1 : 0));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x2a });
            SendParameter(address, (byte)((Boolean)superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[3].CompSwitch ? 1 : 0));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x38 });
            SendParameter(address, (byte)((Boolean)superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[4].CompSwitch ? 1 : 0));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x46 });
            SendParameter(address, (byte)((Boolean)superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[5].CompSwitch ? 1 : 0));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x01 });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[0].CompAttackTime));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x0f });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[1].CompAttackTime));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x1d });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[2].CompAttackTime));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x2b });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[3].CompAttackTime));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x39 });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[4].CompAttackTime));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x47 });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[5].CompAttackTime));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x02 });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[0].CompReleaseTime));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x10 });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[1].CompReleaseTime));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x1e });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[2].CompReleaseTime));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x2c });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[3].CompReleaseTime));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x3a });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[4].CompReleaseTime));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x48 });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[5].CompReleaseTime));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x03 });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[0].CompThreshold));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x11 });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[1].CompThreshold));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x1f });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[2].CompThreshold));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x2d });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[3].CompThreshold));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x3b });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[4].CompThreshold));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x49 });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[5].CompThreshold));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x04 });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[0].CompRatio));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x12 });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[1].CompRatio));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x20 });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[2].CompRatio));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x2e });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[3].CompRatio));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x3c });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[4].CompRatio));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x4a });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[5].CompRatio));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x05 });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[0].CompOutputGain));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x13 });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[1].CompOutputGain));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x21 });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[2].CompOutputGain));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x2f });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[3].CompOutputGain));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x3d });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[4].CompOutputGain));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x4b });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[5].CompOutputGain));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x06 });
            SendParameter(address, (byte)((Boolean)superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[0].EQSwitch ? 1 : 0));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x14 });
            SendParameter(address, (byte)((Boolean)superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[1].EQSwitch ? 1 : 0));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x22 });
            SendParameter(address, (byte)((Boolean)superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[2].EQSwitch ? 1 : 0));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x30 });
            SendParameter(address, (byte)((Boolean)superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[3].EQSwitch ? 1 : 0));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x4e });
            SendParameter(address, (byte)((Boolean)superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[4].EQSwitch ? 1 : 0));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x5c });
            SendParameter(address, (byte)((Boolean)superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[5].EQSwitch ? 1 : 0));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x07 });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[0].EQLowFreq));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x15 });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[1].EQLowFreq));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x23 });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[2].EQLowFreq));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x31 });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[3].EQLowFreq));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x3f });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[4].EQLowFreq));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x4d });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[5].EQLowFreq));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x08 });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[0].EQLowGain));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x16 });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[1].EQLowGain));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x24 });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[2].EQLowGain));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x32 });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[3].EQLowGain));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x40 });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[4].EQLowGain));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x4e });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[5].EQLowGain));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x09 });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[0].EQMidFreq));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x17 });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[1].EQMidFreq));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x25 });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[2].EQMidFreq));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x33 });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[3].EQMidFreq));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x41 });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[4].EQMidFreq));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x4f });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[5].EQMidFreq));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x0a });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[0].EQMidGain));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x18 });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[1].EQMidGain));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x26 });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[2].EQMidGain));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x34 });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[3].EQMidGain));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x42 });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[4].EQMidGain));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x50 });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[5].EQMidGain));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x0b });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[0].EQMidQ));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x19 });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[1].EQMidQ));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x27 });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[2].EQMidQ));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x35 });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[3].EQMidQ));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x43 });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[4].EQMidQ));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x51 });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[5].EQMidQ));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x0c });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[0].EQHighFreq));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x1a });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[1].EQHighFreq));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x28 });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[2].EQHighFreq));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x36 });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[3].EQHighFreq));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x44 });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[4].EQHighFreq));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x52 });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[5].EQHighFreq));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x0d });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[0].EQHighGain));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x1b });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[1].EQHighGain));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x29 });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[2].EQHighGain));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x37 });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[3].EQHighGain));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x45 });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[4].EQHighGain));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x53 });
            SendParameter(address, (byte)(superNATURALDrumKit.superNATURALDrumKitCommonCompEQ.CompEQ[5].EQHighGain));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONMFX, new byte[] { 0x05 });
            SendParameter(address, (byte)(commonMFX.MFXControlSource[0]));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONMFX, new byte[] { 0x07 });
            SendParameter(address, (byte)(commonMFX.MFXControlSource[1]));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONMFX, new byte[] { 0x09 });
            SendParameter(address, (byte)(commonMFX.MFXControlSource[2]));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONMFX, new byte[] { 0x0b });
            SendParameter(address, (byte)(commonMFX.MFXControlSource[3]));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONMFX, new byte[] { 0x0d });
            SendParameter(address, (byte)(commonMFX.MFXControlAssign[0]));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONMFX, new byte[] { 0x0e });
            SendParameter(address, (byte)(commonMFX.MFXControlAssign[1]));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONMFX, new byte[] { 0x0f });
            SendParameter(address, (byte)(commonMFX.MFXControlAssign[2]));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONMFX, new byte[] { 0x10 });
            SendParameter(address, (byte)(commonMFX.MFXControlAssign[3]));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONMFX, new byte[] { 0x06 });
            SendParameter(address, (byte)(commonMFX.MFXControlSens[0]));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONMFX, new byte[] { 0x08 });
            SendParameter(address, (byte)(commonMFX.MFXControlSens[1]));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONMFX, new byte[] { 0x0a });
            SendParameter(address, (byte)(commonMFX.MFXControlSens[2]));

            address = MakeAddress(ProgramType.SUPERNATURAL_DRUM_KIT, ParameterPage.COMMONMFX, new byte[] { 0x0c });
            SendParameter(address, (byte)(commonMFX.MFXControlSens[3]));

        }
    }
}
