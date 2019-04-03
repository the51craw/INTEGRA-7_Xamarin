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

namespace INTEGRA_7_Xamarin
{
    /**
     * This class holds all settings for a Super Natural synth tone.
     */

    [DataContract]
    public class SNSData
    {
        [DataMember]
        public CommonState.SimpleToneTypes ToneType { get; set; }
        [DataMember]
        public SuperNATURALSynthTone ToneData { get; set; }
        [DataMember]
        public CommonMFX MFX { get; set; }

        public SNSData()
        {
            ToneType = CommonState.SimpleToneTypes.SUPERNATURAL_SYNTH_TONE;
        }

        public static async Task<SNSData> Deserialize<SNSData>(SNSData snsData)
        {
            try
            {
                FileData openStudioSetFile = await CrossFilePicker.Current.PickFile();
                if (openStudioSetFile != null && openStudioSetFile.FilePath.ToLower().EndsWith(".sns_xml"))
                {
                    byte[] data = openStudioSetFile.DataArray;
                    await Task.Run(() =>
                    {
                        MemoryStream memoryStream = new MemoryStream(data);
                        memoryStream.Position = 0;
                        DataContractSerializer dataContractSerializer = new DataContractSerializer(typeof(SNSData));
                        XmlReader xmlReader = XmlReader.Create(memoryStream);
                        XmlDictionaryReader xmlDictionaryReader = XmlDictionaryReader.CreateDictionaryReader(xmlReader);
                        snsData = (SNSData)dataContractSerializer.ReadObject(xmlDictionaryReader);
                        xmlReader.Dispose();
                    });
                }
                else if (openStudioSetFile != null)
                {
                    await MainPage.GetMainPage().DisplayAlert("Integra-7 Librarian", "You can only restore sns data from a file with " +
                        "a file extension '.sns_xml'!", "Cancel");
                }
            }
            catch (Exception e) { }
            return snsData;
        }

        public static async Task Serialize<SNSData>(SNSData snsData, String name)
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
                            var dataContractSerializer = new DataContractSerializer(typeof(SNSData));
                            dataContractSerializer.WriteObject(memoryStream, snsData);
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
                MainPage.GetMainPage().uIHandler.myFileIO.SaveFileAsync(xmlText, ".pcm_xml", name);
            }
            catch (Exception e) { }
        }
    }

    public partial class UIHandler
    {
        public void UpdateIntegra7FromSNS()
        {
            byte[] address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.COMMON, new byte[] { 0x00 });
            SendParameter(address, (superNATURALSynthTone.superNATURALSynthToneCommon.Name));

            //address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.COMMON, new byte[] { 0x20 });
            //SendParameter(address, new byte[] { 0x00, superNATURALSynthTone.superNATURALSynthToneCommon.InstNumber });

            address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.COMMONMFX, new byte[] { 0x05 });
            SendParameter(address, (byte)(commonMFX.MFXControlSource[0]));

            address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.COMMONMFX, new byte[] { 0x07 });
            SendParameter(address, (byte)(commonMFX.MFXControlSource[1]));

            address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.COMMONMFX, new byte[] { 0x09 });
            SendParameter(address, (byte)(commonMFX.MFXControlSource[2]));

            address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.COMMONMFX, new byte[] { 0x0b });
            SendParameter(address, (byte)(commonMFX.MFXControlSource[3]));

            address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.COMMONMFX, new byte[] { 0x0d });
            SendParameter(address, (byte)(commonMFX.MFXControlAssign[0]));

            address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.COMMONMFX, new byte[] { 0x0e });
            SendParameter(address, (byte)(commonMFX.MFXControlAssign[1]));

            address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.COMMONMFX, new byte[] { 0x0f });
            SendParameter(address, (byte)(commonMFX.MFXControlAssign[2]));

            address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.COMMONMFX, new byte[] { 0x10 });
            SendParameter(address, (byte)(commonMFX.MFXControlAssign[3]));

            address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.COMMONMFX, new byte[] { 0x06 });
            SendParameter(address, (byte)(commonMFX.MFXControlSens[0]));

            address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.COMMONMFX, new byte[] { 0x08 });
            SendParameter(address, (byte)(commonMFX.MFXControlSens[1]));

            address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.COMMONMFX, new byte[] { 0x0a });
            SendParameter(address, (byte)(commonMFX.MFXControlSens[2]));

            address = MakeAddress(ProgramType.SUPERNATURAL_SYNTH_TONE, ParameterPage.COMMONMFX, new byte[] { 0x0c });
            SendParameter(address, (byte)(commonMFX.MFXControlSens[3]));
        }
    }
}
