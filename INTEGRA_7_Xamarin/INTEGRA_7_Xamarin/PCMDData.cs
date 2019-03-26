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
     * This class holds all settings for a PCM drumkit tone.
     */

    [DataContract]
    public class PCMDData
    {
        [DataMember]
        public CommonState.SimpleToneTypes ToneType { get; set; }
        [DataMember]
        public PCMDrumKit ToneData { get; set; }
        [DataMember]
        public CommonMFX MFX { get; set; }

        public PCMDData()
        {
            ToneType = CommonState.SimpleToneTypes.PCM_DRUM_KIT;
        }

        public static async Task<PCMDData> Deserialize<PCMDData>(PCMDData pcmdData)
        {
            try
            {
                FileData openStudioSetFile = await CrossFilePicker.Current.PickFile();
                if (openStudioSetFile != null && openStudioSetFile.FilePath.ToLower().EndsWith(".pcmd_xml"))
                {
                    byte[] data = openStudioSetFile.DataArray;
                    await Task.Run(() =>
                    {
                        MemoryStream memoryStream = new MemoryStream(data);
                        memoryStream.Position = 0;
                        DataContractSerializer dataContractSerializer = new DataContractSerializer(typeof(PCMDData));
                        XmlReader xmlReader = XmlReader.Create(memoryStream);
                        XmlDictionaryReader xmlDictionaryReader = XmlDictionaryReader.CreateDictionaryReader(xmlReader);
                        pcmdData = (PCMDData)dataContractSerializer.ReadObject(xmlDictionaryReader);
                        xmlReader.Dispose();
                    });
                }
                else if (openStudioSetFile != null)
                {
                    await MainPage.GetMainPage().DisplayAlert("Integra-7 Librarian", "You can only restore pcm drumset data from a file with " +
                        "a file extension '.pcmd_xml'!", "Cancel");
                }
            }
            catch (Exception e) { }
            return pcmdData;
        }

        public static async Task Serialize<PCMDData>(PCMDData pcmdData, String name)
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
                            var dataContractSerializer = new DataContractSerializer(typeof(PCMDData));
                            dataContractSerializer.WriteObject(memoryStream, pcmdData);
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
                MainPage.GetMainPage().uIHandler.myFileIO.SaveFileAsync(xmlText, ".pcmd_xml", name);
            }
            catch (Exception e) { }
        }
    }

    public partial class UIHandler
    {
        public void UpdateIntegra7FromPCMD()
        {
            byte[] address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMON, new byte[] { 0x00 });
            SendParameter(address, pCMDrumKit.pCMDrumKitCommon.Name);

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMON2, new byte[] { 0x10 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitCommon2.PhraseNumber));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMON, new byte[] { 0x0c });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitCommon.DrumKitLevel));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x0c });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitPartial[currentKey].AssignType));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x0d });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitPartial[currentKey].MuteGroup));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x15 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitPartial[currentKey].PartialEnvMode));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x1c });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitPartial[currentKey].PartialPitchBendRange));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x1d });
            SendParameter(address, (byte)((Boolean)pCMDrumKit.pCMDrumKitPartial[currentKey].PartialReceiveExpression ? 1 : 0));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x1e });
            SendParameter(address, (byte)((Boolean)pCMDrumKit.pCMDrumKitPartial[currentKey].PartialReceiveHold_1 ? 1 : 0));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x41 });
            SendParameter(address, (byte)((Boolean)pCMDrumKit.pCMDrumKitPartial[currentKey].OneShotMode ? 1 : 0));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x21 });
            SendParameter(address, (byte)((Boolean)pCMDrumKit.pCMDrumKitPartial[currentKey].WMT[0].WMTWaveSwitch ? 1 : 0));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x3e });
            SendParameter(address, (byte)((Boolean)pCMDrumKit.pCMDrumKitPartial[currentKey].WMT[1].WMTWaveSwitch ? 1 : 0));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x5b });
            SendParameter(address, (byte)((Boolean)pCMDrumKit.pCMDrumKitPartial[currentKey].WMT[2].WMTWaveSwitch ? 1 : 0));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x78 });
            SendParameter(address, (byte)((Boolean)pCMDrumKit.pCMDrumKitPartial[currentKey].WMT[3].WMTWaveSwitch ? 1 : 0));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x22 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitPartial[currentKey].WMT[currentPartial].WMTWaveGroupType));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { (byte)0x27 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitPartial[currentKey].WMT[currentPartial].WMTWaveNumberL));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { (byte)0x2b });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitPartial[currentKey].WMT[currentPartial].WMTWaveNumberR));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { (byte)0x2f });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitPartial[currentKey].WMT[currentPartial].WMTWaveGain));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { (byte)0x33 });
            SendParameter(address, (byte)((Boolean)pCMDrumKit.pCMDrumKitPartial[currentKey].WMT[currentPartial].WMTWaveTempoSync ? 1 : 0));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { (byte)0x30 });
            SendParameter(address, (byte)((Boolean)pCMDrumKit.pCMDrumKitPartial[currentKey].WMT[currentPartial].WMTWaveFXMSwitch ? 1 : 0));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { (byte)0x31 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitPartial[currentKey].WMT[currentPartial].WMTWaveFXMColor));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { (byte)0x32 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitPartial[currentKey].WMT[currentPartial].WMTWaveFXMDepth));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { (byte)0x34 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitPartial[currentKey].WMT[currentPartial].WMTWaveCoarseTune));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { (byte)0x35 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitPartial[currentKey].WMT[currentPartial].WMTWaveFineTune));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { (byte)0x39 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitPartial[currentKey].WMT[currentPartial].WMTWaveLevel));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { (byte)0x37 });
            SendParameter(address, (byte)((Boolean)pCMDrumKit.pCMDrumKitPartial[currentKey].WMT[currentPartial].WMTWaveRandomPanSwitch ? 1 : 0));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { (byte)0x38 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitPartial[currentKey].WMT[currentPartial].WMTWaveAlternatePanSwitch));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { (byte)0x20 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitPartial[currentKey].WMTVelocityControl));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { (byte)0x3d });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitPartial[currentKey].WMT[currentPartial].WMTVelocityFadeWidthUpper));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { (byte)0x3b });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitPartial[currentKey].WMT[currentPartial].WMTVelocityRangeUpper));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { (byte)0x3a });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitPartial[currentKey].WMT[currentPartial].WMTVelocityRangeLower));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { (byte)0x3c });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitPartial[currentKey].WMT[currentPartial].WMTVelocityFadeWidthLower));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { (byte)0x0f });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitPartial[currentKey].PartialCoarseTune));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { (byte)0x10 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitPartial[currentKey].PartialFineTune));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { (byte)0x11 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitPartial[currentKey].PartialRandomPitchDepth));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x15 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitPartial[currentKey].PitchEnv.PitchEnvDepth));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x16 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitPartial[currentKey].PitchEnv.PitchEnvVelocitySens));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x17 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitPartial[currentKey].PitchEnv.PitchEnvTime1VelocitySens));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x18 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitPartial[currentKey].PitchEnv.PitchEnvTime4VelocitySens));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x19 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitPartial[currentKey].PitchEnv.PitchEnvTime[0]));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x1a });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitPartial[currentKey].PitchEnv.PitchEnvTime[1]));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x1b });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitPartial[currentKey].PitchEnv.PitchEnvTime[2]));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x1c });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitPartial[currentKey].PitchEnv.PitchEnvTime[3]));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x1d });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitPartial[currentKey].PitchEnv.PitchEnvLevel[0]));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x1e });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitPartial[currentKey].PitchEnv.PitchEnvLevel[1]));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x1f });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitPartial[currentKey].PitchEnv.PitchEnvLevel[2]));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x20 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitPartial[currentKey].PitchEnv.PitchEnvLevel[3]));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x21 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitPartial[currentKey].PitchEnv.PitchEnvLevel[4]));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x22 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitPartial[currentKey].TVF.TVFFilterType));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x23 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitPartial[currentKey].TVF.TVFCutoffFrequency));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x26 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitPartial[currentKey].TVF.TVFResonance));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x24 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitPartial[currentKey].TVF.TVFCutoffVelocityCurve));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x25 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitPartial[currentKey].TVF.TVFCutoffVelocitySens));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x27 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitPartial[currentKey].TVF.TVFResonanceVelocitySens));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x29 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitPartial[currentKey].TVF.TVFEnvVelocityCurve));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x2a });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitPartial[currentKey].TVF.TVFEnvVelocitySens));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x2b });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitPartial[currentKey].TVF.TVFEnvTime1VelocitySens));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x2c });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitPartial[currentKey].TVF.TVFEnvTime4VelocitySens));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x28 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitPartial[currentKey].TVF.TVFEnvDepth));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x2d });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitPartial[currentKey].TVF.TVFEnvTime[0]));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x2e });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitPartial[currentKey].TVF.TVFEnvTime[1]));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x2f });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitPartial[currentKey].TVF.TVFEnvTime[2]));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x30 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitPartial[currentKey].TVF.TVFEnvTime[3]));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x31 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitPartial[currentKey].TVF.TVFEnvLevel[0]));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x32 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitPartial[currentKey].TVF.TVFEnvLevel[1]));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x33 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitPartial[currentKey].TVF.TVFEnvLevel[2]));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x34 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitPartial[currentKey].TVF.TVFEnvLevel[3]));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x35 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitPartial[currentKey].TVF.TVFEnvLevel[4]));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x0e });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitPartial[currentKey].PartialLevel));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x36 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitPartial[currentKey].TVA.TVALevelVelocityCurve));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x37 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitPartial[currentKey].TVA.TVALevelVelocitySens));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x12 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitPartial[currentKey].PartialPan));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x13 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitPartial[currentKey].PartialRandomPanDepth));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x14 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitPartial[currentKey].PartialAlternatePanDepth));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x42 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitPartial[currentKey].RelativeLevel));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x38 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitPartial[currentKey].TVA.TVAEnvTime1VelocitySens));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x39 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitPartial[currentKey].TVA.TVAEnvTime4VelocitySens));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x3a });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitPartial[currentKey].TVA.TVAEnvTime[0]));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x3b });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitPartial[currentKey].TVA.TVAEnvTime[1]));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x3c });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitPartial[currentKey].TVA.TVAEnvTime[2]));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x3d });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitPartial[currentKey].TVA.TVAEnvTime[3]));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x3e });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitPartial[currentKey].TVA.TVAEnvLevel[0]));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x3f });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitPartial[currentKey].TVA.TVAEnvLevel[1]));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x01, 0x40 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitPartial[currentKey].TVA.TVAEnvLevel[2]));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x1b });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitPartial[currentKey].PartialOutputAssign));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x16 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitPartial[currentKey].PartialOutputLevel));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x19 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitPartial[currentKey].PartialChorusSendLevel));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.PARTIAL, new byte[] { 0x1a });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitPartial[currentKey].PartialReverbSendLevel));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x00 });
            SendParameter(address, (byte)((Boolean)pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[0].CompSwitch ? 1 : 0));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x0e });
            SendParameter(address, (byte)((Boolean)pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[1].CompSwitch ? 1 : 0));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x1c });
            SendParameter(address, (byte)((Boolean)pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[2].CompSwitch ? 1 : 0));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x2a });
            SendParameter(address, (byte)((Boolean)pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[3].CompSwitch ? 1 : 0));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x38 });
            SendParameter(address, (byte)((Boolean)pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[4].CompSwitch ? 1 : 0));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x46 });
            SendParameter(address, (byte)((Boolean)pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[5].CompSwitch ? 1 : 0));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x01 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[0].CompAttackTime));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x0f });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[1].CompAttackTime));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x0d });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[2].CompAttackTime));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x2b });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[3].CompAttackTime));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x39 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[4].CompAttackTime));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x47 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[5].CompAttackTime));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x02 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[0].CompReleaseTime));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x10 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[1].CompReleaseTime));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x1e });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[2].CompReleaseTime));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x2c });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[3].CompReleaseTime));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x3a });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[4].CompReleaseTime));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x48 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[5].CompReleaseTime));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x03 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[0].CompThreshold));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x11 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[1].CompThreshold));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x1f });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[2].CompThreshold));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x2d });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[3].CompThreshold));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x3b });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[4].CompThreshold));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x4a });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[5].CompThreshold));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x04 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[0].CompRatio));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x12 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[1].CompRatio));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x20 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[2].CompRatio));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x2e });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[3].CompRatio));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x3c });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[4].CompRatio));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x4a });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[5].CompRatio));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x05 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[0].CompOutputGain));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x13 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[1].CompOutputGain));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x21 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[2].CompOutputGain));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x2f });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[3].CompOutputGain));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x3d });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[4].CompOutputGain));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x4b });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[5].CompOutputGain));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x06 });
            SendParameter(address, (byte)((Boolean)pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[0].EQSwitch ? 1 : 0));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x14 });
            SendParameter(address, (byte)((Boolean)pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[1].EQSwitch ? 1 : 0));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x22 });
            SendParameter(address, (byte)((Boolean)pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[2].EQSwitch ? 1 : 0));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x30 });
            SendParameter(address, (byte)((Boolean)pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[3].EQSwitch ? 1 : 0));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x3e });
            SendParameter(address, (byte)((Boolean)pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[4].EQSwitch ? 1 : 0));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x4c });
            SendParameter(address, (byte)((Boolean)pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[5].EQSwitch ? 1 : 0));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x07 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[0].EQLowFreq));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x15 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[1].EQLowFreq));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x23 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[2].EQLowFreq));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x31 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[3].EQLowFreq));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x3f });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[4].EQLowFreq));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x4d });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[5].EQLowFreq));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x09 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[0].EQMidFreq));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x17 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[1].EQMidFreq));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x25 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[2].EQMidFreq));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x33 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[3].EQMidFreq));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x41 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[4].EQMidFreq));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x4f });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[5].EQMidFreq));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x0b });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[0].EQMidQ));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x19 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[1].EQMidQ));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x27 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[2].EQMidQ));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x35 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[3].EQMidQ));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x43 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[4].EQMidQ));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x51 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[5].EQMidQ));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x0c });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[0].EQHighFreq));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x1a });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[1].EQHighFreq));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x28 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[2].EQHighFreq));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x36 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[3].EQHighFreq));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x44 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[4].EQHighFreq));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x52 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[5].EQHighFreq));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x08 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[0].EQLowGain));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x16 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[1].EQLowGain));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x24 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[2].EQLowGain));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x32 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[3].EQLowGain));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x40 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[4].EQLowGain));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x1e });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[5].EQLowGain));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x0a });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[0].EQMidGain));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x18 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[1].EQMidGain));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x26 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[2].EQMidGain));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x34 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[3].EQMidGain));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x42 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[4].EQMidGain));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x50 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[5].EQMidGain));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x0d });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[0].EQHighGain));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x1b });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[1].EQHighGain));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x29 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[2].EQHighGain));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x37 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[3].EQHighGain));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x45 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[4].EQHighGain));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONCOMPEQ, new byte[] { 0x53 });
            SendParameter(address, (byte)(pCMDrumKit.pCMDrumKitCommonCompEQ.CompEq[5].EQHighGain));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONMFX, new byte[] { 0x05 });
            SendParameter(address, (byte)(commonMFX.MFXControlSource[0]));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONMFX, new byte[] { 0x07 });
            SendParameter(address, (byte)(commonMFX.MFXControlSource[1]));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONMFX, new byte[] { 0x09 });
            SendParameter(address, (byte)(commonMFX.MFXControlSource[2]));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONMFX, new byte[] { 0x0b });
            SendParameter(address, (byte)(commonMFX.MFXControlSource[3]));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONMFX, new byte[] { 0x0d });
            SendParameter(address, (byte)(commonMFX.MFXControlAssign[0]));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONMFX, new byte[] { 0x0e });
            SendParameter(address, (byte)(commonMFX.MFXControlAssign[1]));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONMFX, new byte[] { 0x0f });
            SendParameter(address, (byte)(commonMFX.MFXControlAssign[2]));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONMFX, new byte[] { 0x10 });
            SendParameter(address, (byte)(commonMFX.MFXControlAssign[3]));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONMFX, new byte[] { 0x06 });
            SendParameter(address, (byte)(commonMFX.MFXControlSens[0]));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONMFX, new byte[] { 0x08 });
            SendParameter(address, (byte)(commonMFX.MFXControlSens[1]));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONMFX, new byte[] { 0x0a });
            SendParameter(address, (byte)(commonMFX.MFXControlSens[2]));

            address = MakeAddress(ProgramType.PCM_DRUM_KIT, ParameterPage.COMMONMFX, new byte[] { 0x0c });
            SendParameter(address, (byte)(commonMFX.MFXControlSens[3]));
        }
    }
}
