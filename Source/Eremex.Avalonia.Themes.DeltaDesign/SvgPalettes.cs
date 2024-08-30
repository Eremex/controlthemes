using Avalonia.Controls;
using Avalonia.Styling;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Eremex.AvaloniaUI.Themes.DeltaDesign
{
	public class SvgPalettes : IResourceProvider
	{
		Dictionary<string, string> cachedPalette = new Dictionary<string, string>();
		private bool _invalidatePalettes = true;

		public IResourceHost? Owner { get; private set; }

		public bool HasResources => true;

		public event EventHandler? OwnerChanged;

		public void AddOwner(IResourceHost owner)
		{
			if (Owner != owner)
			{
				Owner = owner;
				OwnerChanged?.Invoke(this, EventArgs.Empty);
				_invalidatePalettes = true;
			}
		}

		public void RemoveOwner(IResourceHost owner)
		{
			if (Owner == owner)
			{
				Owner = null;
				OwnerChanged?.Invoke(this, EventArgs.Empty);
				_invalidatePalettes = true;
			}
		}

		public bool TryGetResource(object key, ThemeVariant? theme, out object? value)
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
			if (_invalidatePalettes)
			{
				cachedPalette.Clear();
				RegisterPalette(PaletteType.White, PaletteState.Disabled);
				RegisterPalette(PaletteType.White, PaletteState.Selected);
				RegisterPalette(PaletteType.White, PaletteState.Normal);
				RegisterPalette(PaletteType.Black, PaletteState.Disabled);
				RegisterPalette(PaletteType.Black, PaletteState.Selected);
				RegisterPalette(PaletteType.Black, PaletteState.Normal);
				_invalidatePalettes = false;
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

