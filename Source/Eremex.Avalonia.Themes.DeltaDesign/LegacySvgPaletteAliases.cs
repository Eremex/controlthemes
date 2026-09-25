namespace Eremex.AvaloniaUI.Themes.DeltaDesign;

internal static class LegacySvgPaletteAliases
{
	// Direct migration matches are extended to all legacy styles by their closest light/dark semantic color pair.
	private static readonly IReadOnlyDictionary<string, int[]> aliases = new Dictionary<string, int[]>
	{
		["Icons_Outline_Red"] = [5, 9, 31, 33],
		["Icons_Outline_Yellow"] = [8, 12, 13],
		["Icons_Outline_Green"] = [4, 10],
		["Icons_Outline_Turquoise"] = [6, 32, 45, 52],
		["Icons_Outline_Blue"] = [2],
		["Icons_Outline_Violet"] = [7, 53],
		["Icons_Outline_Gray"] = [1, 3, 26, 27],
		["Icons_Outline_Selected"] = [24, 44],
		["Icons_Outline_Disabled"] = [22, 25, 51, 55, 56],
		["Icons_Outline_On_Accent"] = [0, 23, 28, 30, 39, 40, 41, 46, 47],
		["Icons_Fill_Red"] = [18, 34],
		["Icons_Fill_Yellow"] = [14, 21],
		["Icons_Fill_Green"] = [11, 17],
		["Icons_Fill_Turquoise"] = [19, 42],
		["Icons_Fill_Blue"] = [15],
		["Icons_Fill_Violet"] = [20],
		["Icons_Fill_Gray"] = [16, 29, 35, 36, 37, 38, 43, 48, 49, 50, 54, 57],
		["Graphics_Yellow"] = [58],
	};

	private static readonly IReadOnlyDictionary<string, int[]> checkEditorAliases = new Dictionary<string, int[]>
	{
		["Fill_Accent_Primary_Enabled"] = [6, 32],
		["Fill_Accent_Error_Enabled"] = [5, 31, 33],
		["Fill_Neutral_Secondary_Enabled"] = [30],
		["Outline_Neutral_Transparent_Medium"] = [29],
		["Icons_Outline_On_Accent"] = [28],
	};

	public static string AddAliases(string css) => AddAliases(css, aliases);

	public static string AddCheckEditorAliases(string css) => AddAliases(css, checkEditorAliases);

	private static string AddAliases(string css, IReadOnlyDictionary<string, int[]> mappings)
	{
		foreach (var (semanticClass, legacyStyles) in mappings)
		{
			var semanticSelector = $".{semanticClass}";
			var aliasesSelector = string.Join(",", legacyStyles.Select(x => $".style{x}"));
			css = css.Replace($"{semanticSelector}{{", $"{semanticSelector},{aliasesSelector}{{", StringComparison.Ordinal);
		}

		return css;
	}
}
