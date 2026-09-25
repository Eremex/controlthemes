using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;

using System.Diagnostics.CodeAnalysis;

#nullable disable
namespace Eremex.AvaloniaUI.Themes.DeltaDesign
{
	public enum Density
	{
		Compact,
		Standard,
		Spacious
	}

	public enum Palette
	{
		Nova,
		Helios,
		Terra,
		Vega,
		Atlas,
		NovaDeep,
		HeliosDeep,
		TerraDeep,
		VegaDeep,
		AtlasDeep
	}

	public class DeltaDesignTheme : Styles
	{
		const string DensityResourcesPath = "avares://Eremex.Avalonia.Themes.DeltaDesign/DensityResources/{0}.axaml";

		const string PaletteResourcesPath = "avares://Eremex.Avalonia.Themes.DeltaDesign/PaletteResources/{0}.axaml";

		const string IconColorsPath = "Eremex.AvaloniaUI.Themes.DeltaDesign.PaletteResources.{0}_{1}_{2}.css";

		const string CheckEditorIconColorsPath = "Eremex.AvaloniaUI.Themes.DeltaDesign.PaletteResources.checkEditor_{0}_{1}_{2}.css";
		const string CheckEditorPalettePrefix = "checkeditor";

		public static readonly DirectProperty<DeltaDesignTheme, Density> DensityProperty = AvaloniaProperty.RegisterDirect<DeltaDesignTheme, Density>(
			nameof(Density), o => o.Density, (o, v) => o.Density = v, Density.Standard);

		public static readonly DirectProperty<DeltaDesignTheme, Palette> PaletteProperty = AvaloniaProperty.RegisterDirect<DeltaDesignTheme, Palette>(
			nameof(Palette), o => o.Palette, (o, v) => o.Palette = v);

		Density density = Density.Standard;
		Palette palette;
		ResourceDictionary densityResources, paletteResources, iconsColors;

		[DynamicDependency(DynamicallyAccessedMemberTypes.PublicConstructors, typeof(global::Avalonia.Controls.ColorPicker))]
		[DynamicDependency(DynamicallyAccessedMemberTypes.PublicConstructors, typeof(global::Avalonia.Controls.DataGrid))]
		public DeltaDesignTheme(IServiceProvider sp = null)
		{
			AvaloniaXamlLoader.Load(sp, this);

			LoadResources(DensityResourcesPath, Density, ref densityResources);
			LoadResources(PaletteResourcesPath, Palette, ref paletteResources);
			LoadIconColors();
		}

		public Density Density
		{
			get => density;
			set => SetAndRaise(DensityProperty, ref density, value);
		}

		public Palette Palette
		{
			get => palette;
			set => SetAndRaise(PaletteProperty, ref palette, value);
		}

		protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
		{
			base.OnPropertyChanged(change);

			if (change.Property == DensityProperty)
			{
				LoadResources(DensityResourcesPath, Density, ref densityResources);
			}
			else if (change.Property == PaletteProperty)
			{
				LoadResources(PaletteResourcesPath, Palette, ref paletteResources);
				LoadIconColors();
			}
		}

		void LoadResources(string resourcePath, object resourceName, ref ResourceDictionary resourceDictionary)
		{
			int index = Resources.MergedDictionaries.IndexOf(resourceDictionary);
			resourceDictionary = (ResourceDictionary)LoadResource(new Uri(string.Format(resourcePath, resourceName)));
			if (index == -1)
				Resources.MergedDictionaries.Insert(0, resourceDictionary);
			else
				Resources.MergedDictionaries[index] = resourceDictionary;
		}

		bool reflectionLoaderFailed = false;

		object LoadResource(Uri uri)
		{
			if (!reflectionLoaderFailed)
			{
				try
				{
					//Same loading as AvaloniaXamlLoader.Load but with correct assembly
					//AvaloniaXamlLoader.Load uses DefaultAssemblyLoadContext instead of actual one
					var loader = this.GetType().Assembly
						?.GetType("CompiledAvaloniaXaml.!XamlLoader")
						?.GetMethod("TryLoad", [typeof(IServiceProvider), typeof(string)]);
					var loaded = loader.Invoke(null, [null, uri.ToString()]);
					if (loaded != null)
						return loaded;
				}
				catch
				{
					reflectionLoaderFailed = true;
				}

#if DEBUG
				if (reflectionLoaderFailed)
					throw new InvalidOperationException();
#endif
			}

			return AvaloniaXamlLoader.Load(uri);
		}

		void LoadIconColors()
		{
			int index = Resources.MergedDictionaries.IndexOf(iconsColors);
			iconsColors = new ResourceDictionary();
			iconsColors.ThemeDictionaries.Add(ThemeVariant.Default, LoadThemeColors("light"));
			iconsColors.ThemeDictionaries.Add(ThemeVariant.Dark, LoadThemeColors("dark"));

			if (index == -1)
				Resources.MergedDictionaries.Insert(0, iconsColors);
			else
				Resources.MergedDictionaries[index] = iconsColors;
		}

		ResourceDictionary LoadThemeColors(string variantName)
		{
			var resourceDictionary = new ResourceDictionary();
			LoadColorResource(variantName, resourceDictionary, "normal", IconColorsPath);
			LoadColorResource(variantName, resourceDictionary, "selected", IconColorsPath);
			LoadColorResource(variantName, resourceDictionary, "disabled", IconColorsPath);

			LoadColorResource(variantName, resourceDictionary, "normal", CheckEditorIconColorsPath, CheckEditorPalettePrefix);
			LoadColorResource(variantName, resourceDictionary, "disabled", CheckEditorIconColorsPath, CheckEditorPalettePrefix);

			return resourceDictionary;
		}

		void LoadColorResource(string variantName, ResourceDictionary resourceDictionary, string stateName, string resourcePath, string palettePrefix = "")
		{
			string palette;
			if (palettePrefix == CheckEditorPalettePrefix)
			{
				var commonPalette = ReadCssResource(string.Format(IconColorsPath, variantName, stateName, Palette));
				var checkEditorPalette = ReadCssResource(string.Format(resourcePath, variantName, stateName, Palette));
				palette = LegacySvgPaletteAliases.AddAliases(commonPalette) + Environment.NewLine
					+ LegacySvgPaletteAliases.AddCheckEditorAliases(checkEditorPalette);
			}
			else
			{
				var commonPalette = ReadCssResource(string.Format(resourcePath, variantName, stateName, Palette));
				palette = LegacySvgPaletteAliases.AddAliases(commonPalette);
			}

			var resourceKey = stateName + "_svg_palette";
			if (!string.IsNullOrEmpty(palettePrefix))
				resourceKey = $"{palettePrefix}_{resourceKey}";
			resourceDictionary[resourceKey] = palette;
		}

		string ReadCssResource(string resourceName)
		{
			using var stream = this.GetType().Assembly.GetManifestResourceStream(resourceName);
			using var reader = new StreamReader(stream);
			return reader.ReadToEnd();
		}
	}
}
#nullable restore
