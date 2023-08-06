namespace SpeciesModGenerator
{
	public static class EdictGenerator
	{
		public static void Generate()
		{
			string[] lines = File.ReadAllLines("template_edict.txt");

			for (int i = 0; i < lines.Length; i++)
			{
				string line = lines[i];
				if (line.Contains("[[Namespace]]"))
				{
					lines[i] = line.Replace("[[Namespace]]", ModWorkload.modNamespace);
				}
			}

			Directory.CreateDirectory(@"output\common\edicts");
			File.WriteAllLines(@$"output\common\edicts\1_{ModWorkload.modNamespace}_rename_edict.txt", lines);

			/**	localisation for edicts	**/
			for (int LanguageIdx = 0; LanguageIdx < ModWorkload.languages.Length; LanguageIdx++)
			{
				string Language = ModWorkload.languages[LanguageIdx];
				List<string> locLines = new()
				{
					$"l_{Language}:"
				};

				for (int EdictIdx = 0; EdictIdx < ModWorkload.edictWorkload.edictNames.Count; EdictIdx++)
				{
					locLines.Add($" {ModWorkload.edictWorkload.edictNames[EdictIdx]}:0 \"{ModWorkload.edictWorkload.edictLocTexts[LanguageIdx][EdictIdx]}\"");
					locLines.Add($" {ModWorkload.edictWorkload.edictNames[EdictIdx]}_desc:0 \"{ModWorkload.edictWorkload.edictDescLocTexts[LanguageIdx][EdictIdx]}\"");
				}

				Directory.CreateDirectory(@$"output\localisation\{Language}");
				File.WriteAllLines(@$"output\localisation\{Language}\{ModWorkload.modNamespace}_edicts_l_{Language}.yml", locLines, System.Text.Encoding.UTF8);
			}
		}
	}
}