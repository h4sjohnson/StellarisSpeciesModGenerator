namespace SpeciesModGenerator
{
	public static class EdictGenerator
	{
		public static void Generate()
		{
			string[] lines = File.ReadAllLines(@"templates\common\edicts\1_namespace_rename_edict.txt");

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
		}
	}
}