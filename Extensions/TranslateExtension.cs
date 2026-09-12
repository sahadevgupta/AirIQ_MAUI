using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AirIQ.Services;

namespace AirIQ.Extensions
{
    public class TranslateExtension : BaseExtension, IMarkupExtension<BindingBase>
    {
        public string? Name { get; set; }
        public IValueConverter? Converter { get; set; }
        public BindingBase ProvideValue(IServiceProvider serviceProvider)
        {
            return new Binding
            {
                Mode = BindingMode.OneWay,
                Path = $"[{Name}]",
                Source = LocalizationResourceManager.Instance,
                Converter = Converter

            };
        }

        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
            => ProvideValue(serviceProvider);
    }

    public class BaseExtension
    {
        public CultureInfo CurrentUICulture { get; set; }
        public CultureInfo CurrentCulture { get; private set; }

        public BaseExtension()
        {
            CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
            CurrentCulture = Thread.CurrentThread.CurrentCulture;
        }

    }
}