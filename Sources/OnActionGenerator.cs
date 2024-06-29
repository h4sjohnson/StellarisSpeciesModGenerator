namespace SpeciesModGenerator
{
	public static class OnActionGenerator
	{
		public static void Generate()
		{
			string[] lines = File.ReadAllLines(@"templates\common\on_actions\namespace_leader_on_actions.txt");

			for (int i = 0; i < lines.Length; i++)
			{
				string line = lines[i];
				if (line.Contains("[[Namespace]]"))
				{
					lines[i] = line.Replace("[[Namespace]]", ModWorkload.modNamespace);
				}
			}

			Directory.CreateDirectory(@"output\common\on_actions");
			File.WriteAllLines(@$"output\common\on_actions\{ModWorkload.modNamespace}_leader_on_actions.txt", lines);
		}
	}
}