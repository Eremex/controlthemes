using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;

namespace Eremex.AvaloniaUI.Themes.DeltaDesign
{
    public class DeltaDesignTheme : Styles
    {
        public DeltaDesignTheme(IServiceProvider? sp = null)
        {
            AvaloniaXamlLoader.Load(sp, this);
        }
    }
}
