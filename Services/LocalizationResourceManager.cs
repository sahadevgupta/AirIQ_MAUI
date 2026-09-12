using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AirIQ.Resources.Strings;

namespace AirIQ.Services
{
    public class LocalizationResourceManager : INotifyPropertyChanged
    {
        private LocalizationResourceManager()
        {
            Culture = CultureInfo.CurrentCulture;
        }

        public CultureInfo Culture
        {
            get => CultureInfo.CurrentUICulture;
            set
            {
                CultureInfo.CurrentUICulture = CultureInfo.CurrentCulture = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
            }
        }

        public static LocalizationResourceManager Instance { get; } = new();

        public object this[string resourceKey]
            => AppResource.ResourceManager.GetObject(resourceKey, Culture) ?? Array.Empty<byte>();

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}