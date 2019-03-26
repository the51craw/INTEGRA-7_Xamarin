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
     * This class holds all settings for a PCM tone.
     */

    [DataContract]
    public class PCMData
    {
        [DataMember]
        public CommonState.SimpleToneTypes ToneType { get; set; }
        [DataMember]
        public PCMSynthTone ToneData { get; set; }
        [DataMember]
        public CommonMFX MFX { get; set; }

        public PCMData()
        {
            ToneType = CommonState.SimpleToneTypes.PCM_SYNTH_TONE;
        }

        public static async Task<PCMData> Deserialize<PCMData>(PCMData pcmData)
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
                        DataContractSerializer dataContractSerializer = new DataContractSerializer(typeof(PCMData));
                        XmlReader xmlReader = XmlReader.Create(memoryStream);
                        XmlDictionaryReader xmlDictionaryReader = XmlDictionaryReader.CreateDictionaryReader(xmlReader);
                        pcmData = (PCMData)dataContractSerializer.ReadObject(xmlDictionaryReader);
                        xmlReader.Dispose();
                    });
                }
                else if (openStudioSetFile != null)
                {
                    await MainPage.GetMainPage().DisplayAlert("Integra-7 Librarian", "You can only restore pcm data from a file with " +
                        "a file extension '.pcm_xml'!", "Cancel");
                }
            }
            catch (Exception e) { }
            return pcmData;
        }

        public static async Task Serialize<PCMData>(PCMData pcmData, String name)
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
                            var dataContractSerializer = new DataContractSerializer(typeof(PCMData));
                            dataContractSerializer.WriteObject(memoryStream, pcmData);
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
        public void UpdateIntegra7FromPCM()
        {
            byte[] address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMON, new byte[] { 0x00 });
            SendParameter(address, (pCMSynthTone.pCMSynthToneCommon.Name));

            //address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMON, new byte[] { 0x20 });
            //SendParameter(address, new byte[] { 0x00, pCMSynthTone.pCMSynthToneCommon.InstNumber });

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PMT, new byte[] { 0x05 });
            SendParameter(address, (byte)((Boolean)pCMSynthTone.pCMSynthTonePMT.PMTPartialSwitch[0] ? 1 : 0));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PMT, new byte[] { 0x0e });
            SendParameter(address, (byte)((Boolean)pCMSynthTone.pCMSynthTonePMT.PMTPartialSwitch[1] ? 1 : 0));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PMT, new byte[] { 0x17 });
            SendParameter(address, (byte)((Boolean)pCMSynthTone.pCMSynthTonePMT.PMTPartialSwitch[2] ? 1 : 0));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PMT, new byte[] { 0x20 });
            SendParameter(address, (byte)((Boolean)pCMSynthTone.pCMSynthTonePMT.PMTPartialSwitch[3] ? 1 : 0));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMON, 0x17);
            SendParameter(address, (byte)((Boolean)pCMSynthTone.pCMSynthToneCommon.LegatoSwitch ? 1 : 0));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMON, 0x18);
            SendParameter(address, (byte)((Boolean)pCMSynthTone.pCMSynthToneCommon.LegatoRetrigger ? 1 : 0));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMON, 0x19);
            SendParameter(address, (byte)((Boolean)pCMSynthTone.pCMSynthToneCommon.PortamentoSwitch ? 1 : 0));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMON2, new byte[] { 0x10 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthToneCommon2.ToneCategory));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMON2, new byte[] { 0x38 });
            SendParameter(address, new byte[] { 0x00, 0x00,
                (byte)(pCMSynthTone.pCMSynthToneCommon2.PhraseNumber / 16),
                (byte)(pCMSynthTone.pCMSynthToneCommon2.PhraseNumber % 16)});

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMON2, 0x13);
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthToneCommon2.PhraseOctaveShift));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMON, 0x10);
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthToneCommon.Priority));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMON, 0x13);
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthToneCommon.OctaveShift));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMON, 0x14);
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthToneCommon.TuneDepth));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMON, 0x16);
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthToneCommon.MonoPoly));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMON, 0x1a);
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthToneCommon.PortamentoMode));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMON, 0x1b);
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthToneCommon.PortamentoType));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMON, 0x1c);
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthToneCommon.PortamentoStart));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMON, 0x0e);
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthToneCommon.Level));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMON, 0x0f);
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthToneCommon.Pan));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMON, 0x11);
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthToneCommon.CoarseTune));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMON, 0x12);
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthToneCommon.FineTune));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMON, 0x15);
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthToneCommon.AnalogFeel));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMON, 0x22);
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthToneCommon.CutoffOffset));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMON, 0x23);
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthToneCommon.ResonanceOffset));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMON, 0x24);
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthToneCommon.AttackTimeOffset));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMON, 0x25);
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthToneCommon.ReleaseTimeOffset));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMON, 0x26);
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthToneCommon.VelocitySenseOffset));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMON, 0x1d);
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthToneCommon.PortamentoTime));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x27 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].WaveGroupType));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x34 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].WaveGain));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x36 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].WaveFXMColor));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x09 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialDelayMode));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x2c });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].WaveNumberL));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x30 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].WaveNumberR));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x0a });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialDelayTime));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x38 });
            SendParameter(address, (byte)((Boolean)pCMSynthTone.pCMSynthTonePartial[currentPartial].WaveTempoSync ? 1 : 0));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x35 });
            SendParameter(address, (byte)((Boolean)pCMSynthTone.pCMSynthTonePartial[currentPartial].WaveFXMSwitch ? 1 : 0));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x37 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].WaveFXMDepth));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PMT, new byte[] { 0x04 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePMT.PMTVelocityControl));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PMT, new byte[] { 0x00 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePMT.StructureType1_2));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PMT, new byte[] { 0x01 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePMT.Booster1_2));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PMT, new byte[] { 0x02 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePMT.StructureType3_4));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PMT, new byte[] { 0x03 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePMT.Booster3_4));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x03 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialRandomPitchDepth));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PMT, new byte[] { (byte)(0x09 + 0x09 * currentPMT) });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePMT.PMTKeyboardFadeWidthUpper[currentPMT]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PMT, new byte[] { (byte)(0x07 + 0x09 * currentPMT) });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePMT.PMTKeyboardRangeUpper[currentPMT]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PMT, new byte[] { (byte)(0x06 + 0x09 * currentPMT) });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePMT.PMTKeyboardRangeLower[currentPMT]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PMT, new byte[] { (byte)(0x08 + 0x09 * currentPMT) });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePMT.PMTKeyboardFadeWidthLower[currentPMT]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PMT, new byte[] { (byte)(0x0d + 0x09 * currentPMT) });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePMT.PMTVelocityFadeWidthUpper[currentPMT]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PMT, new byte[] { (byte)(0x0a + 0x09 * currentPMT) });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePMT.PMTVelocityRangeLower[currentPMT]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PMT, new byte[] { (byte)(0x0b + 0x09 * currentPMT) });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePMT.PMTVelocityRangeUpper[currentPMT]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PMT, new byte[] { (byte)(0x0c + 0x09 * currentPMT) });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePMT.PMTVelocityFadeWidthLower[currentPMT]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMON, new byte[] { 0x28 });
            SendParameter(address, (byte)((Boolean)pCMSynthTone.pCMSynthToneCommon.PMTControlSwitch ? 1 : 0));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x02 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialFineTune));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x01 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialCoarseTune));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x39 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].WavePitchKeyfollow));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMON, new byte[] { 0x29 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthToneCommon.PitchBendRangeUp));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMON, new byte[] { 0x2a });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthToneCommon.PitchBendRangeDown));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x3a });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].PitchEnv.PitchEnvDepth));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x3b });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].PitchEnv.PitchEnvVelocitySens));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x3c });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].PitchEnv.PitchEnvTime1VelocitySens));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x3d });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].PitchEnv.PitchEnvTime4VelocitySens));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x3e });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].PitchEnv.PitchEnvTimeKeyfollow));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x3f });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].PitchEnv.PitchEnvTime[0]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x40 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].PitchEnv.PitchEnvTime[1]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x41 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].PitchEnv.PitchEnvTime[2]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x42 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].PitchEnv.PitchEnvTime[3]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x43 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].PitchEnv.PitchEnvLevel[0]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x44 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].PitchEnv.PitchEnvLevel[1]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x45 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].PitchEnv.PitchEnvLevel[2]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x46 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].PitchEnv.PitchEnvLevel[3]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x47 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].PitchEnv.PitchEnvLevel[4]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x48 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFFilterType));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x49 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFCutoffFrequency));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x4d });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFResonance));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x4a });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFCutoffKeyfollow));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x4b });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFCutoffVelocityCurve));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x4c });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFCutoffVelocitySens));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x4e });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFResonanceVelocitySens));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x4f });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFEnvDepth));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x50 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFEnvVelocityCurve));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x51 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFEnvVelocitySens));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x52 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFEnvTime1VelocitySens));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x53 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFEnvTime4VelocitySens));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x54 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFEnvTimeKeyfollow));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x55 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFEnvTime[0]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x56 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFEnvTime[1]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x57 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFEnvTime[2]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x58 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFEnvTime[3]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x59 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFEnvLevel[0]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x5a });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFEnvLevel[1]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x5b });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFEnvLevel[2]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x5c });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFEnvLevel[3]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x5d });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].TVF.TVFEnvLevel[4]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x00 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialOutputLevel));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x61 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].TVA.TVALevelVelocityCurve));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x62 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].TVA.TVALevelVelocitySens));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x5e });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].BiasLevel));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x5f });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].BiasPosition));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x60 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].BiasDirection));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x04 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialPan));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x05 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialPanKeyfollow));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x06 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialRandomPanDepth));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x07 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialAlternatePanDepth));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x63 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].TVA.TVAEnvTime1VelocitySens));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x64 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].TVA.TVAEnvTime4VelocitySens));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x65 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].TVA.TVAEnvTimeKeyfollow));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x66 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].TVA.TVAEnvTime[0]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x67 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].TVA.TVAEnvTime[1]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x68 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].TVA.TVAEnvTime[2]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x69 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].TVA.TVAEnvTime[3]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x6a });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].TVA.TVAEnvLevel[0]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x6b });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].TVA.TVAEnvLevel[1]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x6c });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].TVA.TVAEnvLevel[2]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0X0c });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialOutputLevel));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0X0f });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialChorusSendLevel));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0X10 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialReverbSendLevel));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x6d });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO1.LFOWaveform));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x6e });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO1.LFORate));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x71 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO1.LFORateDetune));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x70 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO1.LFOOffset));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x72 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO1.LFODelayTime));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x73 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO1.LFODelayTimeKeyfollow));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x74 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO1.LFOFadeMode));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x75 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO1.LFOFadeTime));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x76 });
            SendParameter(address, (byte)((Boolean)pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO1.LFOKeyTrigger ? 1 : 0));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x77 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO1.LFOPitchDepth));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x78 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO1.LFOTVFDepth));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x79 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO1.LFOTVADepth));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x7a });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO1.LFOPanDepth));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x7b });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO2.LFOWaveform));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x7c });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO2.LFORate));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x7f });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO2.LFORateDetune));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x7e });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO2.LFOOffset));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x01, 0x00 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO2.LFODelayTime));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x01, 0x01 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO2.LFODelayTimeKeyfollow));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x01, 0x02 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO2.LFOFadeMode));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x01, 0x03 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO2.LFOFadeTime));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x01, 0x04 });
            SendParameter(address, (byte)((Boolean)pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO2.LFOKeyTrigger ? 1 : 0));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x01, 0x05 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO2.LFOPitchDepth));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x01, 0x06 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO2.LFOTVFDepth));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x01, 0x07 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO2.LFOTVADepth));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x01, 0x08 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].LFO2.LFOPanDepth));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x01, 0x09 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].LFOStepType));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x01, 0x0a });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].LFOStep[0]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x01, 0x0b });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].LFOStep[1]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x01, 0x0c });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].LFOStep[2]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x01, 0x0d });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].LFOStep[3]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x01, 0x0e });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].LFOStep[4]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x01, 0x0f });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].LFOStep[5]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x01, 0x10 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].LFOStep[6]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x01, 0x11 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].LFOStep[7]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x01, 0x12 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].LFOStep[8]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x01, 0x13 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].LFOStep[9]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x01, 0x14 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].LFOStep[10]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x01, 0x15 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].LFOStep[11]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x01, 0x16 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].LFOStep[12]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x01, 0x17 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].LFOStep[13]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x01, 0x18 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].LFOStep[14]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x01, 0x19 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].LFOStep[15]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x08 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialEnvMode));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x12 });
            SendParameter(address, (byte)((Boolean)pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialReceiveBender ? 1 : 0));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x13 });
            SendParameter(address, (byte)((Boolean)pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialReceiveExpression ? 1 : 0));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x14 });
            SendParameter(address, (byte)((Boolean)pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialReceiveHold_1 ? 1 : 0));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { 0x16 });
            SendParameter(address, (byte)((Boolean)pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialRedamperSwitch ? 1 : 0));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMON, new byte[] { (byte)(0x2b + 9 * currentMatrixControlPage) });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthToneCommon.MatrixControlSource[currentMatrixControlPage]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMON, new byte[] { 0x2c });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthToneCommon.MatrixControlDestination[currentMatrixControlPage][0]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMON, new byte[] { 0x2e });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthToneCommon.MatrixControlDestination[currentMatrixControlPage][1]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMON, new byte[] { 0x30 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthToneCommon.MatrixControlDestination[currentMatrixControlPage][2]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMON, new byte[] { 0x32 });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthToneCommon.MatrixControlDestination[currentMatrixControlPage][3]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { (byte)(0x17 + 4 * currentMatrixControlPage) });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialControlSwitch[currentMatrixControlPage][0]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { (byte)(0x18 + 4 * currentMatrixControlPage) });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialControlSwitch[currentMatrixControlPage][1]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { (byte)(0x19 + 4 * currentMatrixControlPage) });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialControlSwitch[currentMatrixControlPage][2]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { (byte)(0x1a + 4 * currentMatrixControlPage) });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialControlSwitch[currentMatrixControlPage][3]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { (byte)(0x17 + 4 * currentMatrixControlPage) });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialControlSwitch[currentMatrixControlPage][0]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { (byte)(0x18 + 4 * currentMatrixControlPage) });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialControlSwitch[currentMatrixControlPage][1]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { (byte)(0x19 + 4 * currentMatrixControlPage) });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialControlSwitch[currentMatrixControlPage][2]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { (byte)(0x1a + 4 * currentMatrixControlPage) });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialControlSwitch[currentMatrixControlPage][3]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { (byte)(0x17 + 4 * currentMatrixControlPage) });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialControlSwitch[currentMatrixControlPage][0]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { (byte)(0x18 + 4 * currentMatrixControlPage) });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialControlSwitch[currentMatrixControlPage][1]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { (byte)(0x19 + 4 * currentMatrixControlPage) });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialControlSwitch[currentMatrixControlPage][2]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { (byte)(0x1a + 4 * currentMatrixControlPage) });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialControlSwitch[currentMatrixControlPage][3]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { (byte)(0x17 + 4 * currentMatrixControlPage) });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialControlSwitch[currentMatrixControlPage][0]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { (byte)(0x18 + 4 * currentMatrixControlPage) });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialControlSwitch[currentMatrixControlPage][1]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { (byte)(0x19 + 4 * currentMatrixControlPage) });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialControlSwitch[currentMatrixControlPage][2]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.PARTIAL, new byte[] { (byte)(0x1a + 4 * currentMatrixControlPage) });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthTonePartial[currentPartial].PartialControlSwitch[currentMatrixControlPage][3]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMON, new byte[] { (byte)(0x2d + 9 * currentMatrixControlPage) });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthToneCommon.MatrixControlSens[currentMatrixControlPage][0]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMON, new byte[] { (byte)(0x2f + 9 * currentMatrixControlPage) });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthToneCommon.MatrixControlSens[currentMatrixControlPage][1]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMON, new byte[] { (byte)(0x31 + 9 * currentMatrixControlPage) });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthToneCommon.MatrixControlSens[currentMatrixControlPage][2]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMON, new byte[] { (byte)(0x33 + 9 * currentMatrixControlPage) });
            SendParameter(address, (byte)(pCMSynthTone.pCMSynthToneCommon.MatrixControlSens[currentMatrixControlPage][3]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMONMFX, new byte[] { 0x05 });
            SendParameter(address, (byte)(commonMFX.MFXControlSource[0]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMONMFX, new byte[] { 0x07 });
            SendParameter(address, (byte)(commonMFX.MFXControlSource[1]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMONMFX, new byte[] { 0x09 });
            SendParameter(address, (byte)(commonMFX.MFXControlSource[2]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMONMFX, new byte[] { 0x0b });
            SendParameter(address, (byte)(commonMFX.MFXControlSource[3]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMONMFX, new byte[] { 0x0d });
            SendParameter(address, (byte)(commonMFX.MFXControlAssign[0]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMONMFX, new byte[] { 0x0e });
            SendParameter(address, (byte)(commonMFX.MFXControlAssign[1]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMONMFX, new byte[] { 0x0f });
            SendParameter(address, (byte)(commonMFX.MFXControlAssign[2]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMONMFX, new byte[] { 0x10 });
            SendParameter(address, (byte)(commonMFX.MFXControlAssign[3]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMONMFX, new byte[] { 0x06 });
            SendParameter(address, (byte)(commonMFX.MFXControlSens[0]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMONMFX, new byte[] { 0x08 });
            SendParameter(address, (byte)(commonMFX.MFXControlSens[1]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMONMFX, new byte[] { 0x0a });
            SendParameter(address, (byte)(commonMFX.MFXControlSens[2]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMONMFX, new byte[] { 0x0c });
            SendParameter(address, (byte)(commonMFX.MFXControlSens[3]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMONMFX, new byte[] { 0x0d });
            SendParameter(address, (byte)(commonMFX.MFXControlAssign[0]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMONMFX, new byte[] { 0x0e });
            SendParameter(address, (byte)(commonMFX.MFXControlAssign[1]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMONMFX, new byte[] { 0x0f });
            SendParameter(address, (byte)(commonMFX.MFXControlAssign[2]));

            address = MakeAddress(ProgramType.PCM_SYNTH_TONE, ParameterPage.COMMONMFX, new byte[] { 0x10 });
            SendParameter(address, (byte)(commonMFX.MFXControlAssign[3]));

        }
    }
}
