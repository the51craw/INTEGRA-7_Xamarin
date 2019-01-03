using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Xml;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ConvertXAML2Code
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Windows.UI.Xaml.Controls.Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            Deserialize();
        }

        public async void Deserialize()
        {
            Page xamlData = new Page();
            xamlData = await Page.Deserialize(xamlData);
        }

    }

    [DataContract]
    class Page
    {
        //[DataMember]
        //public FormData formData { get; set; }

        //public Page()
        //{
        //    formData = new FormData();
        //}

        [DataMember]
        List<String> Attributes { get; set; }

        [DataMember]
        List<FormData> Nodes { get; set; }

        public static async Task<Page> Deserialize<Page>(Page xamlData)
        {
            xamlData = default(Page);
            try
            {
                FileOpenPicker openPicker = new FileOpenPicker();
                openPicker.ViewMode = PickerViewMode.Thumbnail;
                openPicker.FileTypeFilter.Add(".xaml");
                //StreamReader openStudioSetFile = File.OpenText("");
                StorageFile openStudioSetFile = await openPicker.PickSingleFileAsync();

                if (openStudioSetFile != null)
                {
                    //Stream stream = openStudioSetFile.BaseStream;
                    Stream stream = await openStudioSetFile.OpenStreamForReadAsync();
                    await Task.Run(() =>
                    {
                        MemoryStream memoryStream = new MemoryStream();
                        stream.CopyTo(memoryStream);
                        memoryStream.Position = 0;
                        DataContractSerializer dataContractSerializer = new DataContractSerializer(typeof(Page));
                        XmlReader xmlReader = XmlReader.Create(memoryStream);
                        XmlDictionaryReader xmlDictionaryReader = XmlDictionaryReader.CreateDictionaryReader(xmlReader);
                        xamlData = (Page)dataContractSerializer.ReadObject(xmlDictionaryReader);
                        xmlReader.Dispose();
                    });
                }
            }
            catch (Exception e) { }
            return xamlData;
        }
    }

    [DataContract]
    public class FormData
    {
        [DataMember]
        List<String> Attributes { get; set; }

        [DataMember]
        List<FormData> Nodes { get; set; }

        public FormData()
        {
            Attributes = new List<String>();
            Nodes = new List<FormData>();
        }
    }
}
