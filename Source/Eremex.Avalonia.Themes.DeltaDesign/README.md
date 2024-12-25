# Paint Themes for the Eremex Avalonia UI Controls 

[Eremex Controls for Avalonia UI](https://www.eremexcontrols.com) is a collection of visual components that help you easily build feature-rich elegant user interfaces.
The collection includes powerful UI controls – from advanced Data Grid and Tree List to classic Toolbars, Docking and high-performance charts.

## About

This repository contains the source code of the Eremex Controls paint themes. The paint themes customize styles and templates for the Eremex Controls and a set of standard Avalonia UI controls (buttons, labels, progress bars, tooltips, etc.). The standard controls' appearance settings are modified to match those of the Eremex controls.

- `Eremex.Avalonia.Themes.DeltaDesign` — The default paint theme. Supports two theme variants that help you deliver interfaces with the light and dark color palettes. The `DeltaDesign` paint theme is distributed with the [Eremex.Avalonia.Themes.DeltaDesign NuGet package](https://www.nuget.org/packages/Eremex.Avalonia.Themes.DeltaDesign).

New themes are expected to be available in the future.

The source code of paint themes is helpful if you need to modify a theme in your applications.
You can explore the source code to locate definitions of specific styles, resources or templates used by the Eremex and standard controls, and then override them in your project. 
See the following documentation topic for more information: [Modify Control Themes](https://eremexcontrols.net/articles/controls/themes/modify-control-themes.html).

You can also clone this repository, modify a theme and then rebuild it to produce a new theme library (DLL). 

The Eremex Controls paint themes are subject to change in future versions. New versions of the Eremex Controls library may not be compatible with old themes and themes derived from the old ones.
