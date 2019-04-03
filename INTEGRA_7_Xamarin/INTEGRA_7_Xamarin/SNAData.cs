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
     * This class holds all settings for a Super Natural acoustic tone.
     */

    [DataContract]
    public class SNAData
    {
        [DataMember]
        public CommonState.SimpleToneTypes ToneType { get; set; }
        [DataMember]
        public SuperNATURALAcousticTone ToneData { get; set; }
        [DataMember]
        public CommonMFX MFX { get; set; }

        public SNAData()
        {
            ToneType = CommonState.SimpleToneTypes.SUPERNATURAL_ACOUSTIC_TONE;
        }

        public static async Task<SNAData> Deserialize<SNAData>(SNAData snaData)
        {
            try
            {
                FileData openStudioSetFile = await CrossFilePicker.Current.PickFile();
                if (openStudioSetFile != null && openStudioSetFile.FilePath.ToLower().EndsWith(".sna_xml"))
                {
                    byte[] data = openStudioSetFile.DataArray;
                    await Task.Run(() =>
                    {
                        MemoryStream memoryStream = new MemoryStream(data);
                        memoryStream.Position = 0;
                        DataContractSerializer dataContractSerializer = new DataContractSerializer(typeof(SNAData));
                        XmlReader xmlReader = XmlReader.Create(memoryStream);
                        XmlDictionaryReader xmlDictionaryReader = XmlDictionaryReader.CreateDictionaryReader(xmlReader);
                        snaData = (SNAData)dataContractSerializer.ReadObject(xmlDictionaryReader);
                        xmlReader.Dispose();
                    });
                }
                else if (openStudioSetFile != null)
                {
                    await MainPage.GetMainPage().DisplayAlert("Integra-7 Librarian", "You can only restore sna data from a file with " +
                        "a file extension '.sna_xml'!", "Cancel");
                }
            }
            catch (Exception e) { }
            return snaData;
        }

        public static async Task Serialize<SNAData>(SNAData snaData, String name)
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
                            var dataContractSerializer = new DataContractSerializer(typeof(SNAData));
                            dataContractSerializer.WriteObject(memoryStream, snaData);
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
        public void UpdateIntegra7FromSNA()
        {
            byte[] address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x00 });
            SendParameter(address, (superNATURALAcousticTone.superNATURALAcousticToneCommon.Name));

            address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x20 });
            SendParameter(address, new byte[] { 0x00, superNATURALAcousticTone.superNATURALAcousticToneCommon.InstNumber });
            
            address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x1b });
            SendParameter(address, (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.Category));

            address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x1c });
            SendParameter(address, (new byte[] { (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.PhraseNumber / 16),
                (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.PhraseNumber % 16) }));

            address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x1e });
            SendParameter(address, (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.PhraseOctaveShift));

            address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x10 });
            SendParameter(address, (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.ToneLevel));

            address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x11 });
            SendParameter(address, (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.MonoPoly));

            address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x1a });
            SendParameter(address, (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.OctaveShift));

            address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x13 });
            SendParameter(address, (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.CutoffOffset));

            address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x14 });
            SendParameter(address, (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.ResonanceOffset));

            address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x15 });
            SendParameter(address, (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.AttackTimeOffset));

            address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x16 });
            SendParameter(address, (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.ReleaseTimeOffset));

            address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x12 });
            SendParameter(address, (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.PortamentoTimeOffset));

            address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x17 });
            SendParameter(address, (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.VibratoRate));

            address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x18 });
            SendParameter(address, (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.VibratoDepth));

            address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x19 });
            SendParameter(address, (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.VibratoDelay));

            address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x22 });
            SendParameter(address, (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter1));

            address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x23 });
            SendParameter(address, (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter2));

            address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x24 });
            SendParameter(address, (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter3));

            address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x25 });
            SendParameter(address, (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter4));

            address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x26 });
            SendParameter(address, (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter5));

            address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x27 });
            SendParameter(address, (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter6));

            address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x22 });
            SendParameter(address, (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter1));

            address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x22 });
            SendParameter(address, (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter1));

            address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x23 });
            SendParameter(address, (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter2));

            address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x24 });
            SendParameter(address, (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter3));

            address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x25 });
            SendParameter(address, (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter4));

            address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x26 });
            SendParameter(address, (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter5));

            address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x27 });
            SendParameter(address, (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter6));

            address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x28 });
            SendParameter(address, (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter7));

            address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x29 });
            SendParameter(address, (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter8));

            address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x2a });
            SendParameter(address, (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter9));

            address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x37 });
            SendParameter(address, (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter22));

            address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x2b });
            SendParameter(address, (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter10));

            address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x36 });
            SendParameter(address, (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter21));

            address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x30 });
            SendParameter(address, (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter15));

            address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x31 });
            SendParameter(address, (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter16));

            address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x2d });
            SendParameter(address, (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter12));

            address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x32 });
            SendParameter(address, (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter17));

            address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x33 });
            SendParameter(address, (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter18));

            address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x2c });
            SendParameter(address, (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter11));

            address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x34 });
            SendParameter(address, (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter19));

            address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x35 });
            SendParameter(address, (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter20));

            address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x2e });
            SendParameter(address, (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter13));

            address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x2f });
            SendParameter(address, (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter14));

            address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x24 });
            SendParameter(address, (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter3));

            address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x22 });
            SendParameter(address, (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter1));

            address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x23 });
            SendParameter(address, (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter2));

            address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x2b });
            SendParameter(address, (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter10));

            address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x23 });
            SendParameter(address, (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter2));

            address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x25 });
            SendParameter(address, (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter4));

            address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x2e });
            SendParameter(address, (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter13));

            address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x23 });
            SendParameter(address, (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter2));

            address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x23 });
            SendParameter(address, (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter2));

            address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x2c });
            SendParameter(address, (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter11));

            address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x22 });
            SendParameter(address, (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter1));

            address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x25 });
            SendParameter(address, (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter4));

            address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x28 });
            SendParameter(address, (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter7));

            address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x29 });
            SendParameter(address, (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter8));

            address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x23 });
            SendParameter(address, (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter2));

            address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x2D });
            SendParameter(address, (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter12));

            address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x26 });
            SendParameter(address, (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter5));

            address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x27 });
            SendParameter(address, (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter6));

            address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x25 });
            SendParameter(address, (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter4));

            address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x26 });
            SendParameter(address, (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter5));

            address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x27 });
            SendParameter(address, (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter6));

            address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMON, new byte[] { 0x2a });
            SendParameter(address, (byte)(superNATURALAcousticTone.superNATURALAcousticToneCommon.ModifyParameter9));

            address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMONMFX, new byte[] { 0x05 });
            SendParameter(address, (byte)(commonMFX.MFXControlSource[0]));

            address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMONMFX, new byte[] { 0x07 });
            SendParameter(address, (byte)(commonMFX.MFXControlSource[1]));

            address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMONMFX, new byte[] { 0x09 });
            SendParameter(address, (byte)(commonMFX.MFXControlSource[2]));

            address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMONMFX, new byte[] { 0x0b });
            SendParameter(address, (byte)(commonMFX.MFXControlSource[3]));

            address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMONMFX, new byte[] { 0x0d });
            SendParameter(address, (byte)(commonMFX.MFXControlAssign[0]));

            address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMONMFX, new byte[] { 0x0e });
            SendParameter(address, (byte)(commonMFX.MFXControlAssign[1]));

            address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMONMFX, new byte[] { 0x0f });
            SendParameter(address, (byte)(commonMFX.MFXControlAssign[2]));

            address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMONMFX, new byte[] { 0x10 });
            SendParameter(address, (byte)(commonMFX.MFXControlAssign[3]));

            address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMONMFX, new byte[] { 0x06 });
            SendParameter(address, (byte)(commonMFX.MFXControlSens[0]));

            address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMONMFX, new byte[] { 0x08 });
            SendParameter(address, (byte)(commonMFX.MFXControlSens[1]));

            address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMONMFX, new byte[] { 0x0a });
            SendParameter(address, (byte)(commonMFX.MFXControlSens[2]));

            address = MakeAddress(ProgramType.SUPERNATURAL_ACOUSTIC_TONE, ParameterPage.COMMONMFX, new byte[] { 0x0c });
            SendParameter(address, (byte)(commonMFX.MFXControlSens[3]));
        }
    }
}
