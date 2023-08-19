namespace SpeciesModGenerator
{
	public static class PortraitGroupsGenerator
	{
		public static void Generate()
		{
			List<string> lines = new();
			lines.Add("portraits = {");
			for (int TraitIdx = 0; TraitIdx < ModWorkload.traitWorkloads.Count; TraitIdx++)
			{
				var traitWorkload = ModWorkload.traitWorkloads[TraitIdx];
				for (int i = 0; i < traitWorkload.leaderIDs.Count; i++)
				{
					lines.Add($"\t{traitWorkload.leaderPortraitTokens[i]}= {{");
					lines.Add($"textureFile = \"{traitWorkload.leaderPortraitPaths[i]}\"");
					try
					{
						var imageFile = new DDSImage(File.ReadAllBytes("painting/" + traitWorkload.leaderPortraitPaths[i]));
						if (imageFile.HasHeight && imageFile.Height > 340)
						{
							int offset = (imageFile.Height - 340) / 2;
							lines.Add($"custom_close_up_position_offset = {{ x = 0 y = {offset} }}");
							lines.Add($"custom_mid_close_up_position_offset = {{ x = 0 y = {offset} }}");
						}
					}
					catch (System.Exception)
					{
						throw;
					}
					lines.Add("}");
				}
				lines.Add("");
				for (int i = 0; i < traitWorkload.popPortraitTokens.Count; i++)
				{
					lines.Add($"\t{traitWorkload.popPortraitTokens[i]}= {{textureFile = \"{traitWorkload.popPortraitPaths[i]}\"}}");
				}
				lines.Add("");
			}
			lines.Add("}");

			string allPortraits = "";
			for (int TraitIdx = 0; TraitIdx < ModWorkload.traitWorkloads.Count; TraitIdx++)
			{
				var traitWorkload = ModWorkload.traitWorkloads[TraitIdx];
				allPortraits += "			add = {\n";
				allPortraits += "				trigger = {\n";
				allPortraits += "					exists = species\n";
				allPortraits += "					species = { has_trait = " + traitWorkload.traitName + " }\n";
				allPortraits += "				}\n";
				allPortraits += "				portraits = {\n";
				for (int i = 0; i < traitWorkload.leaderIDs.Count; i++)
				{
					allPortraits += $"					{traitWorkload.leaderPortraitTokens[i]}\n";
				}

				allPortraits += "				}\n";
				allPortraits += "			}\n";
			}

			string allPops = "";
			for (int TraitIdx = 0; TraitIdx < ModWorkload.traitWorkloads.Count; TraitIdx++)
			{
				var traitWorkload = ModWorkload.traitWorkloads[TraitIdx];
				allPops += "			add = {\n";
				allPops += "				trigger = {\n";
				allPops += "					exists = species\n";
				allPops += "					species = { has_trait = " + traitWorkload.traitName + " }\n";
				allPops += "				}\n";
				allPops += "				portraits = {\n";
				for (int i = 0; i < traitWorkload.popPortraitTokens.Count; i++)
				{
					allPops += $"					{traitWorkload.popPortraitTokens[i]}\n";
				}

				allPops += "				}\n";
				allPops += "			}\n";
			}

			lines.Add("portrait_groups = {");
			lines.Add($"\t\"{ModWorkload.modNamespace}_portraits\" = {{");
			lines.Add($"\t\tdefault = {ModWorkload.defaultPortrait}");
			lines.Add("\t\tgame_setup = {");
			lines.Add(allPortraits);
			lines.Add("\t\t}");
			lines.Add("\t\tspecies = {");
			lines.Add(allPortraits);
			lines.Add("\t\t}");
			lines.Add("\t\tpop = {");
			lines.Add(allPops);
			lines.Add("\t\t}");
			lines.Add("\t\tleader = {");
			lines.Add(allPortraits);
			lines.Add("\t\t}");
			lines.Add("\t\truler = {");
			lines.Add(allPortraits);
			lines.Add("\t\t}");
			lines.Add("\t}");
			lines.Add("}");

			Directory.CreateDirectory(@"output\gfx\portraits\portraits");
			File.WriteAllLines(@$"output\gfx\portraits\portraits\000_{ModWorkload.modNamespace}_species_portraits.txt", lines);
		}
	}
}