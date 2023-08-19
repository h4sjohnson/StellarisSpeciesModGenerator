namespace SpeciesModGenerator
{
    public class LocalisationGenerator
    {
        public static void Generate()
        {
            /**	localisation	**/
            List<string[]> localisationSheet = new();
            using (var fileStream = File.Open(@"localisation.csv", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var reader = new StreamReader(fileStream, System.Text.Encoding.UTF8))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (line != null)
                    {
                        line = line.Replace("_Namespace_", ModWorkload.modNamespace);
                        localisationSheet.Add(line.Split(','));
                    }
                }
            }
            for (int LanguageIdx = 0; LanguageIdx < ModWorkload.languages.Length; LanguageIdx++)
            {
                string Language = ModWorkload.languages[LanguageIdx];
                if (Language == "")
                    continue;
                List<string> locLines = new()
                {
                    $"l_{Language}:"
                };

                for (int i = 1; i < localisationSheet.Count; i++)
                {
                    var text = localisationSheet[i][LanguageIdx + 1];
                    text = text.Replace("[[LocalizedNamespace]]", ModWorkload.localizedNamespaces[LanguageIdx]);
                    locLines.Add($" {localisationSheet[i][0]}:0 \"{text}\"");
                }

                Directory.CreateDirectory(@$"output\localisation\{Language}");
                File.WriteAllLines(@$"output\localisation\{Language}\{ModWorkload.modNamespace}_l_{Language}.yml", locLines, System.Text.Encoding.UTF8);
            }
        }
    }
}