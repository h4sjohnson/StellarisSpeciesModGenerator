namespace SpeciesModGenerator
{
	public static class SpeciesGenerator
	{
		public static void Generate()
		{
			{
				string[] lines = File.ReadAllLines(@"templates\common\species_classes\namespace_species.txt");

				for (int i = 0; i < lines.Length; i++)
				{
					string line = lines[i];
					if (line.Contains("[[Namespace]]"))
					{
						lines[i] = line.Replace("[[Namespace]]", ModWorkload.modNamespace);
					}
				}

				Directory.CreateDirectory(@"output\common\species_classes");
				File.WriteAllLines(@$"output\common\species_classes\{ModWorkload.modNamespace}_species.txt", lines);				
			}

			{
				string[] lines = File.ReadAllLines(@"templates\common\portrait_categories\00_namespace_portrait_categories.txt");

				for (int i = 0; i < lines.Length; i++)
				{
					string line = lines[i];
					if (line.Contains("[[Namespace]]"))
					{
						lines[i] = line.Replace("[[Namespace]]", ModWorkload.modNamespace);
					}
				}

				Directory.CreateDirectory(@"output\common\portrait_categories");
				File.WriteAllLines(@$"output\common\portrait_categories\00_{ModWorkload.modNamespace}_portrait_categories.txt", lines);				
			}

			{
				string[] lines = File.ReadAllLines(@"templates\common\portrait_sets\00_namespace_portrait_sets.txt");

				for (int i = 0; i < lines.Length; i++)
				{
					string line = lines[i];
					if (line.Contains("[[Namespace]]"))
					{
						lines[i] = line.Replace("[[Namespace]]", ModWorkload.modNamespace);
					}
				}

				Directory.CreateDirectory(@"output\common\portrait_sets");
				File.WriteAllLines(@$"output\common\portrait_sets\00_{ModWorkload.modNamespace}_portrait_sets.txt", lines);				
			}

			/**	localisation for species	**/
			for (int LanguageIdx = 0; LanguageIdx < ModWorkload.languages.Length; LanguageIdx++)
			{
				string Language = ModWorkload.languages[LanguageIdx];
				if (Language == "")
					continue;
				List<string> locLines = new()
				{
					$"l_{Language}:",
					$" {ModWorkload.modNamespace}_species:0 \"{ModWorkload.localizedNamespaces[LanguageIdx]}\"",
					$" {ModWorkload.modNamespace}:0 \"{ModWorkload.localizedNamespaces[LanguageIdx]}\"",
				};
				Directory.CreateDirectory(@$"output\localisation\{Language}");
				File.WriteAllLines(@$"output\localisation\{Language}\{ModWorkload.modNamespace}_species_l_{Language}.yml", locLines, System.Text.Encoding.UTF8);
			}
		}
	}
}