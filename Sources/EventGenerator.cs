namespace SpeciesModGenerator
{
	public static class EventGenerator
	{
		public static void Generate()
		{
			string[] lines = File.ReadAllLines(@"templates\events\0_namespace_leader_events.txt");

			for (int l = 0; l < lines.Length; l++)
			{
				string line = lines[l];
				if (line.Contains("[[Namespace]]"))
				{
					lines[l] = line.Replace("[[Namespace]]", ModWorkload.modNamespace);
				}
				if (line.Contains("[[SetNameFlag]]"))
				{
					string spaces = line.Split("[[SetNameFlag]]")[0];
					string strTemp = "";
					strTemp += spaces + "if = {\n";
					strTemp += spaces + "	limit = {\n";
					strTemp += spaces + "		OR = {\n";
					for (int TraitIdx = 0; TraitIdx < ModWorkload.traitWorkloads.Count; TraitIdx++)
					{
						var traitWorkload = ModWorkload.traitWorkloads[TraitIdx];
						foreach (var ID in traitWorkload.leaderIDs)
							strTemp += spaces + $"			has_leader_flag = leader_name_flag_{ID}\n";
					}
					strTemp += spaces + "		}\n";
					strTemp += spaces + "	}\n";
					strTemp += spaces + "	set_leader_flag = has_leader_name_flag\n";
					strTemp += spaces + "}\n";
					strTemp += spaces + "else = {\n";
					strTemp += spaces + "	remove_leader_flag = has_leader_name_flag\n";
					strTemp += spaces + "}\n";
					strTemp += spaces + "if = {\n";
					strTemp += spaces + "	limit = { NOT = { has_leader_flag = has_leader_name_flag} }\n";
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
							strTemp += spaces + "			set_leader_flag = leader_name_flag_" + traitWorkload.leaderIDs[i] + "\n";
							strTemp += spaces + "			set_leader_flag = has_leader_name_flag\n";
							strTemp += spaces + "		}\n";
						}
					}
					strTemp += spaces + "	}\n";
					strTemp += spaces + "	if = { \n";
					strTemp += spaces + "		limit = { NOT = { has_leader_flag = has_leader_name_flag} }\n";
					strTemp += spaces + "		log = \"[This.GetName]: Reroll character failed. No leader name available\"\n";
					strTemp += spaces + "	}\n";
					strTemp += spaces + "}\n";
					lines[l] = strTemp;
				}
				if (line.Contains("[[ClearAllNameFlags]]"))
				{
					string spaces = line.Split("[[ClearAllNameFlags]]")[0];
					string strTemp = "";
					strTemp += spaces + "remove_leader_flag = has_leader_name_flag\n";
					for (int TraitIdx = 0; TraitIdx < ModWorkload.traitWorkloads.Count; TraitIdx++)
					{
						var traitWorkload = ModWorkload.traitWorkloads[TraitIdx];
						for (int i = 0; i < traitWorkload.leaderIDs.Count; i++)
						{
							strTemp += spaces + "remove_leader_flag = leader_name_flag_" + traitWorkload.leaderIDs[i] + "\n";
						}
					}
					lines[l] = strTemp;
				}
				if (line.Contains("[[ChangeNameAndPortrait]]"))
				{
					string spaces = line.Split("[[ChangeNameAndPortrait]]")[0];
					string strTemp = "";
					for (int TraitIdx = 0; TraitIdx < ModWorkload.traitWorkloads.Count; TraitIdx++)
					{
						var traitWorkload = ModWorkload.traitWorkloads[TraitIdx];
						for (int i = 0; i < traitWorkload.leaderIDs.Count; i++)
						{
							if (i == 0)
								strTemp += spaces + "if = { \n";
							else
								strTemp += spaces + "else_if = { \n";
							strTemp += spaces + "	limit = { has_leader_flag = leader_name_flag_" + traitWorkload.leaderIDs[i] + " }\n";
							strTemp += spaces + "	log = \"Change leader [This.GetName] portrait to " + traitWorkload.leaderNameLocTexts[0][i] + "\"\n";
							strTemp += spaces + "	change_leader_portrait = " + traitWorkload.leaderPortraitTokens[i] + "\n";
							strTemp += spaces + "	set_name = \"" + traitWorkload.leaderNameLocTexts[0][i] + "\"\n";
							strTemp += spaces + "}\n";
						}
					}
					lines[l] = line.Replace("[[ChangeNameAndPortrait]]", strTemp);
				}
				if (line.Contains("[[LeaderSelectorCampOptions]]"))
				{
					string strTemp = "";
					strTemp += "leader_event = {\n";
					strTemp += $"	id = rename_{ModWorkload.modNamespace}.300\n";
					strTemp += "	is_triggered_only = yes\n";
					strTemp += "	location = root\n";
					strTemp += "	auto_select = no\n";
					strTemp += "	force_open = yes\n";
					strTemp += "	title = selector_title\n";
					strTemp += "	desc = selector_desc\n";
					strTemp += "	trigger = { always = yes }\n";
					strTemp += "	option = {\n";
					strTemp += "		name = selector_cancel\n";
					strTemp += "		hidden_effect = {\n";
					strTemp += $"			owner = {{ country_event = {{ id = rename_{ModWorkload.modNamespace}.205 }} }}\n";
					strTemp += "			log = \"取消\"\n";
					strTemp += "		}\n";
					strTemp += "	}\n";
					for (int TraitIdx = 0; TraitIdx < ModWorkload.traitWorkloads.Count; TraitIdx++)
					{
						var traitWorkload = ModWorkload.traitWorkloads[TraitIdx];
						strTemp += "	option = {\n";
						strTemp += "		name = " + traitWorkload.traitName + "\n";
						strTemp += "		hidden_effect = {\n";
						strTemp += $"			leader_event = {{ id = rename_{ModWorkload.modNamespace}.{40000 + TraitIdx * 100} }}\n";
						strTemp += "		}\n";
						strTemp += "	}\n";
					}
					strTemp += "}\n";
					lines[l] = strTemp;
				}
				if (line.Contains("[[LeaderSelectorLeaderOptions]]"))
				{
					string strTemp = "";
					for (int TraitIdx = 0; TraitIdx < ModWorkload.traitWorkloads.Count; TraitIdx++)
					{
						var traitWorkload = ModWorkload.traitWorkloads[TraitIdx];
						const int numLeadersEachEvent = 8;
						int kMax = (int)Math.Ceiling(traitWorkload.leaderIDs.Count / (double)numLeadersEachEvent);
						for (int k = 0; k < kMax; k++)
						{
							strTemp += "leader_event = {\n";
							strTemp += $"	id = rename_{ModWorkload.modNamespace}.{40000 + TraitIdx * 100 + k}\n";
							strTemp += "	is_triggered_only = yes\n";
							strTemp += "	location = root\n";
							strTemp += "	title = selector_title\n";
							strTemp += "	picture = GFX_event_frame\n";
							strTemp += "	desc = \"\"\n";
							strTemp += "	trigger = { always = yes }\n";
							strTemp += "	option = {\n";
							strTemp += "		name = selector_back\n";
							strTemp += "		hidden_effect = {\n";
							strTemp += $"			leader_event = {{ id = rename_{ModWorkload.modNamespace}.300 }}\n";
							strTemp += "		}\n";
							strTemp += "	}\n";
							if (k > 0)
							{
								strTemp += "	option = {\n";
								strTemp += "		name = selector_previous_page\n";
								strTemp += "		hidden_effect = {\n";
								strTemp += $"			leader_event = {{ id = rename_{ModWorkload.modNamespace}.{40000 + TraitIdx * 100 + k - 1} }}\n";
								strTemp += "		}\n";
								strTemp += "	}\n";
							}
							else
							{
								strTemp += "	option = {\n";
								strTemp += "		name = selector_previous_page\n";
								strTemp += "		hidden_effect = {\n";
								strTemp += $"			leader_event = {{ id = rename_{ModWorkload.modNamespace}.{40000 + TraitIdx * 100 + kMax - 1} }}\n";
								strTemp += "		}\n";
								strTemp += "	}\n";
							}
							int iBegin = k * numLeadersEachEvent;
							int iEnd = Math.Min(k * numLeadersEachEvent + numLeadersEachEvent, traitWorkload.leaderIDs.Count);
							for (int i = iBegin; i < iEnd; i++)
							{
								strTemp += "	option = {\n";
								strTemp += "		name = leader_name_" + traitWorkload.leaderIDs[i] + "\n";
								strTemp += "		hidden_effect = {\n";
								strTemp += "			change_leader_portrait = " + traitWorkload.leaderPortraitTokens[i] + "\n";
								strTemp += "			set_leader_flag = leader_name_flag_" + traitWorkload.leaderIDs[i] + "\n";
								strTemp += "			set_leader_flag = has_leader_name_flag\n";
								strTemp += $"			owner = {{ country_event = {{ id = rename_{ModWorkload.modNamespace}.205 }} }}\n";
								strTemp += "		}\n";
								strTemp += "	}\n";
							}
							if (iEnd < traitWorkload.leaderIDs.Count)
							{
								strTemp += "	option = {\n";
								strTemp += "		name = selector_next_page\n";
								strTemp += "		hidden_effect = {\n";
								strTemp += $"			leader_event = {{ id = rename_{ModWorkload.modNamespace}.{40000 + TraitIdx * 100 + k + 1} }}\n";
								strTemp += "		}\n";
								strTemp += "	}\n";
							}
							else
							{
								strTemp += "	option = {\n";
								strTemp += "		name = selector_next_page\n";
								strTemp += "		hidden_effect = {\n";
								strTemp += $"			leader_event = {{ id = rename_{ModWorkload.modNamespace}.{40000 + TraitIdx * 100 + 0} }}\n";
								strTemp += "		}\n";
								strTemp += "	}\n";
							}
							strTemp += "}\n";
						}
					}
					lines[l] = strTemp;
				}
				if (line.Contains("[[HasAnyTrait]]"))
				{
					string spaces = line.Split("[[HasAnyTrait]]")[0];
					string strTemp = "";
					strTemp += spaces + "OR = { \n";
					for (int TraitIdx = 0; TraitIdx < ModWorkload.traitWorkloads.Count; TraitIdx++)
					{
						var traitWorkload = ModWorkload.traitWorkloads[TraitIdx];
						strTemp += spaces + "	has_trait = " + traitWorkload.traitName + "\n";
					}
					strTemp += spaces + "}\n";
					lines[l] = strTemp;
				}
			}

			Directory.CreateDirectory(@"output\events");
			File.WriteAllLines(@$"output\events\0_{ModWorkload.modNamespace}_leader_events.txt", lines);

			/**	localisation for leader names	**/
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