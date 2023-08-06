namespace SpeciesModGenerator
{
	public static class EventGenerator
	{
		public static void Generate()
		{
			string[] lines = File.ReadAllLines("template_events.txt");

			for (int l = 0; l < lines.Length; l++)
			{
				string line = lines[l];
				if (line.Contains("[[Namespace]]"))
				{
					lines[l] = line.Replace("[[Namespace]]", ModWorkload.modNamespace);
				}
				if (line.Contains("[[Block1]]"))
				{
					string spaces = line.Split("[[Block1]]")[0];
					string strTemp = "";
					strTemp += spaces + "if = {\n";
					strTemp += spaces + "	limit = {\n";
					strTemp += spaces + "		NOR = {\n";
					for (int TraitIdx = 0; TraitIdx < ModWorkload.traitWorkloads.Count; TraitIdx++)
					{
						var traitWorkload = ModWorkload.traitWorkloads[TraitIdx];
						foreach (var ID in traitWorkload.leaderIDs)
							strTemp += spaces + $"			has_leader_flag = leader_name_flag_{ID}\n";
					}
					strTemp += spaces + "		}\n";
					strTemp += spaces + "	}\n";
					strTemp += spaces + "	random_list = {\n";
					for (int TraitIdx = 0; TraitIdx < ModWorkload.traitWorkloads.Count; TraitIdx++)
					{
						var traitWorkload = ModWorkload.traitWorkloads[TraitIdx];
						for (int i = 0; i < traitWorkload.leaderIDs.Count; i++)
						{
							strTemp += spaces + "		1 = {\n";
							strTemp += spaces + "			modifier = {\n";
							strTemp += spaces + "				factor = 0\n";
							strTemp += spaces + "				OR = {\n";
							strTemp += spaces + "					NOT = { species = { has_trait = " + traitWorkload.traitName + " } }\n";
							strTemp += spaces + "					owner = {\n";
							strTemp += spaces + "						OR = {\n";
							strTemp += spaces + "							any_owned_leader = { has_leader_flag = leader_name_flag_" + traitWorkload.leaderIDs[i] + " }\n";
							strTemp += spaces + "							any_pool_leader = { has_leader_flag = leader_name_flag_" + traitWorkload.leaderIDs[i] + " }\n";
							strTemp += spaces + "							any_envoy = { has_leader_flag = leader_name_flag_" + traitWorkload.leaderIDs[i] + " }\n";
							strTemp += spaces + "						}\n";
							strTemp += spaces + "					}\n";
							strTemp += spaces + "				}\n";
							strTemp += spaces + "			}\n";
							strTemp += spaces + "			change_leader_portrait = " + traitWorkload.leaderPortraitTokens[i] + "\n";
							strTemp += spaces + "			log = \"Leader change portrait to " + traitWorkload.leaderNameLocTexts[0][i] + "\"" + "\n";
							strTemp += spaces + "			set_leader_flag = leader_name_flag_" + traitWorkload.leaderIDs[i] + "\n";
							strTemp += spaces + "			set_leader_flag = leader_need_rename\n";
							strTemp += spaces + "		}\n";
						}
					}
					strTemp += spaces + "	}\n";
					// strTemp += spaces + "	if = { \n";
					// strTemp += spaces + "		limit = { NOT = { has_leader_flag = leader_need_rename} }\n";
					// strTemp += spaces + "		log = \"Reroll character failed. No leader name available\"\n";
					// strTemp += spaces + "	}\n";
					strTemp += spaces + "}\n";
					lines[l] = line.Replace("[[Block1]]", strTemp);
				}
				if (line.Contains("[[Block2]]"))
				{
					string spaces = line.Split("[[Block2]]")[0];
					string strTemp = "";
					for (int TraitIdx = 0; TraitIdx < ModWorkload.traitWorkloads.Count; TraitIdx++)
					{
						var traitWorkload = ModWorkload.traitWorkloads[TraitIdx];
						strTemp += spaces + "if = {\n";
						strTemp += spaces + "	limit = {\n";
						strTemp += spaces + "		species = {\n";
						strTemp += spaces + "			has_trait = " + traitWorkload.traitName + "\n";
						strTemp += spaces + "		}\n";
						strTemp += spaces + "	}\n";
						for (int i = 0; i < traitWorkload.leaderIDs.Count; i++)
						{
							if (i == 0)
								strTemp += spaces + "	if = { \n";
							else
								strTemp += spaces + "	else_if = { \n";
							strTemp += spaces + "		limit = { has_leader_flag = leader_name_flag_" + traitWorkload.leaderIDs[i] + " }\n";
							strTemp += spaces + "		remove_leader_flag = leader_need_rename\n";
							strTemp += spaces + "		set_leader_flag = leader_renamed\n";
							strTemp += spaces + "		save_event_target_as = rename_leader\n";
							strTemp += spaces + "		owner = {\n";
							strTemp += spaces + "			log = \"Clone leader " + traitWorkload.leaderNameLocTexts[0][i] + "\"\n";
							strTemp += spaces + "			clone_leader = {\n";
							strTemp += spaces + "				target = event_target:rename_leader\n";
							strTemp += spaces + "				name = leader_name_" + traitWorkload.leaderIDs[i] + "\n";
							strTemp += spaces + "			}\n";
							strTemp += spaces + "			last_created_leader = {" + "\n";
							strTemp += spaces + "				set_leader_flag = leader_name_flag_" + traitWorkload.leaderIDs[i] + "\n";
							strTemp += spaces + "				change_leader_portrait = " + traitWorkload.leaderPortraitTokens[i] + "\n";
							strTemp += spaces + "			}" + "\n";
							strTemp += spaces + "			log = \"Clone leader succeed \"\n";
							strTemp += spaces + "		}\n";
							strTemp += spaces + "	}\n";
						}
						strTemp += spaces + "}\n";
					}
					lines[l] = line.Replace("[[Block2]]", strTemp);
				}
				if (line.Contains("[[Block3]]"))
				{
					string spaces = line.Split("[[Block3]]")[0];
					string strTemp = "";
					strTemp += spaces + "remove_leader_flag = change_portrait_success\n";
					for (int TraitIdx = 0; TraitIdx < ModWorkload.traitWorkloads.Count; TraitIdx++)
					{
						var traitWorkload = ModWorkload.traitWorkloads[TraitIdx];
						strTemp += spaces + "if = {\n";
						strTemp += spaces + "	limit = {\n";
						strTemp += spaces + "		species = {\n";
						strTemp += spaces + "			has_trait = " + traitWorkload.traitName + "\n";
						strTemp += spaces + "		}\n";
						strTemp += spaces + "	}\n";
						for (int i = 0; i < traitWorkload.leaderIDs.Count; i++)
						{
							if (i == 0)
								strTemp += spaces + "	if = { \n";
							else
								strTemp += spaces + "	else_if = { \n";
							strTemp += spaces + "		limit = { has_leader_flag = leader_name_flag_" + traitWorkload.leaderIDs[i] + " }\n";
							strTemp += spaces + "		log = \"Change leader portrait to " + traitWorkload.leaderNameLocTexts[0][i] + "\"\n";
							strTemp += spaces + "		change_leader_portrait = " + traitWorkload.leaderPortraitTokens[i] + "\n";
							strTemp += spaces + "		set_leader_flag = change_portrait_success\n";
							strTemp += spaces + "	}\n";
						}
						strTemp += spaces + "}\n";
					}
					// strTemp += spaces + "if = {\n";
					// strTemp += spaces + "	limit = {\n";
					// strTemp += spaces + "		NOT = { has_leader_flag = change_portrait_success }\n";
					// strTemp += spaces + "	}\n";
					// strTemp += spaces + "	log = \"Change portrait failed. Could not find portrait\"\n";
					// strTemp += spaces + "}\n";
					strTemp += spaces + "remove_leader_flag = change_portrait_success\n";
					lines[l] = line.Replace("[[Block3]]", strTemp);
				}
			}

			Directory.CreateDirectory(@"output\events");
			File.WriteAllLines(@$"output\events\0_{ModWorkload.modNamespace}_leader_events.txt", lines);

			/**	localisation for leader names	**/
			for (int LanguageIdx = 0; LanguageIdx < ModWorkload.languages.Length; LanguageIdx++)
			{
				string Language = ModWorkload.languages[LanguageIdx];
				List<string> locLines = new()
				{
					$"l_{Language}:"
				};
				for (int TraitIdx = 0; TraitIdx < ModWorkload.traitWorkloads.Count; TraitIdx++)
				{
					var traitWorkload = ModWorkload.traitWorkloads[TraitIdx];
					for (int i = 0; i < traitWorkload.leaderIDs.Count; i++)
					{
						string strLeaderName = traitWorkload.leaderNameLocTexts[LanguageIdx][i];
						locLines.Add($" leader_name_{traitWorkload.leaderIDs[i]}:0 \"{strLeaderName}\"");
					}
				}
				Directory.CreateDirectory(@$"output\localisation\{Language}");
				File.WriteAllLines(@$"output\localisation\{Language}\{ModWorkload.modNamespace}_leader_name_l_{Language}.yml", locLines, System.Text.Encoding.UTF8);
			}
		}
	}
}