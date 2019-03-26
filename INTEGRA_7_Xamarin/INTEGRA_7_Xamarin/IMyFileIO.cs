﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integra_7_Xamarin
{
    public interface IMyFileIO
    {
        void GenericHandler(object sender, object e);

        void SetMainPagePortable(Integra_7_Xamarin.MainPage mainPage);

        String ReadFile(String filename);

        void SaveFileAsync(String content, String extension);

        //String LoadFavorites();
    }
}
