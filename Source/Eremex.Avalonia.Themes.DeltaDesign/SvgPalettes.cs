using Avalonia.Controls;
using Avalonia.Styling;

namespace Eremex.AvaloniaUI.Themes.DeltaDesign
{
	public class SvgPalettes : ResourceProvider
	{
		private readonly Dictionary<string, string> cachedPalette = new();
		private bool invalidatePalettes = true;

		public override bool HasResources => true;
		
		protected override void OnAddOwner(IResourceHost owner)
		{
			base.OnAddOwner(owner);
			if (Owner != owner)
			{
				invalidatePalettes = true;
			}
		}
		
		protected override void OnRemoveOwner(IResourceHost owner)
		{
			base.OnRemoveOwner(owner);
			if (Owner == owner)
			{
				invalidatePalettes = true;
			}
		}
		
		public override bool TryGetResource(object key, ThemeVariant? theme, out object? value)
		{
			value = null;
			if (key is string strKey)
			{
				EnsurePalettes();
				value = GetPalette(strKey);
			}
			return value != null;
		}

		private void EnsurePalettes()
		{
			if (invalidatePalettes)
			{
				cachedPalette.Clear();
				RegisterPalette(PaletteType.White, PaletteState.Disabled);
				RegisterPalette(PaletteType.White, PaletteState.Selected);
				RegisterPalette(PaletteType.White, PaletteState.Normal);
				RegisterPalette(PaletteType.Black, PaletteState.Disabled);
				RegisterPalette(PaletteType.Black, PaletteState.Selected);
				RegisterPalette(PaletteType.Black, PaletteState.Normal);
				invalidatePalettes = false;
			}
		}

		private void RegisterPalette(PaletteType paletteType, PaletteState paletteState)
		{
			var key = PaletteManager.GetPaletteKey(paletteType, paletteState);
			if (!cachedPalette.ContainsKey(key))
			{
				var resourceName = $"Eremex.AvaloniaUI.Themes.DeltaDesign.Palettes.{PaletteManager.GetPaletteName(paletteType, paletteState)}.css";
				var stream = typeof(SvgPalettes).Assembly?.GetManifestResourceStream(resourceName);
				if (stream != null)
				{
					var reader = new StreamReader(stream);
					var palette = reader.ReadToEnd();
					cachedPalette.Add(key, palette);
				}
			} 
			else throw new ArgumentException($"palette {key} already registered");
		}

		public string? GetPalette(string paletteKey)
		{
			if (!cachedPalette.ContainsKey(paletteKey))
				return null;
			else
				return cachedPalette[paletteKey];
		}
	}
}

