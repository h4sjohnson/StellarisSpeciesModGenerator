namespace SpeciesModGenerator
{
	public class TraitGenerator
	{
		public static void Generate()
		{
			string strTraitNamePattern = "[[trait_name]]";
			string[] lines = File.ReadAllLines("template_trait.txt");
			List<string> outLines = new();
			string templateString = string.Join('\n', lines);
			for (int TraitIdx = 0; TraitIdx < ModWorkload.traitWorkloads.Count; TraitIdx++)
			{
				var traitWorkload = ModWorkload.traitWorkloads[TraitIdx];
				if (templateString.Contains(strTraitNamePattern))
				{
					outLines.Add(templateString.Replace(strTraitNamePattern, traitWorkload.traitName));
				}
			}
			Directory.CreateDirectory(@"output\common\traits");
			File.WriteAllLines(@$"output\common\traits\0_{ModWorkload.modNamespace}_portraits_traits.txt", outLines);

			/**	localisation for species traits	**/
			for (int LanguageIdx = 0; LanguageIdx < ModWorkload.languages.Length; LanguageIdx++)
			{
				string Language = ModWorkload.languages[LanguageIdx];
				if (Language == "")
					continue;
				List<string> locLines = new()
				{
					$"l_{Language}:"
				};
				for (int TraitIdx = 0; TraitIdx < ModWorkload.traitWorkloads.Count; TraitIdx++)
				{
					var traitWorkload = ModWorkload.traitWorkloads[TraitIdx];
					locLines.Add($" {traitWorkload.traitName}:0 \"{traitWorkload.traitNameLocTexts[LanguageIdx]}\"");
					locLines.Add($" {traitWorkload.traitName}_desc:0 \"{traitWorkload.traitNameLocTexts[LanguageIdx]}\"");
				}
				Directory.CreateDirectory(@$"output\localisation\{Language}");
				File.WriteAllLines(@$"output\localisation\{Language}\{ModWorkload.modNamespace}_portraits_traits_l_{Language}.yml", locLines, System.Text.Encoding.UTF8);
			}
		}
	}
}