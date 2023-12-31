﻿using System.Collections.Generic;

namespace SpeciesModGenerator
{
	public class ModWorkload
	{
		public static string modNamespace;
		public static string[] languages;
		public static string[] localizedNamespaces;
		public static int NumLanguages => languages.Length;
		public static List<TraitWorkload> traitWorkloads;
		public static string defaultPortrait;
		public class TraitWorkload
		{
			public TraitWorkload(string traitName)
			{
				this.traitName = traitName;
				leaderNameLocTexts = new List<string>[NumLanguages];
				for (int i = 0; i < NumLanguages; i++)
				{
					leaderNameLocTexts[i] = new();
				}
				traitNameLocTexts = new string[NumLanguages];
			}
			public string traitName;
			public List<string> leaderIDs = new();
			public List<string> leaderPortraitTokens = new();
			public List<string> leaderPortraitPaths = new();
			public List<string>[] leaderNameLocTexts;

			public List<string> popPortraitTokens = new();
			public List<string> popPortraitPaths = new();

			public string[] traitNameLocTexts;
		};

		public static void ParseModWorkLoad()
		{
			List<string[]> leaderSheet = new();
			using (var fileStream = File.Open(@"leader_portrait_list.csv", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
			using (var reader = new StreamReader(fileStream, System.Text.Encoding.UTF8))
			{
				while (!reader.EndOfStream)
				{
					var line = reader.ReadLine();
					if (line != null)
					{
						leaderSheet.Add(line.Split(','));
					}
				}
			}

			modNamespace = leaderSheet[1][0];
			defaultPortrait = leaderSheet[1][2];
			languages = new string[leaderSheet[2].Length - 4];
			localizedNamespaces = new string[languages.Length];
			for (int i = 4; i < leaderSheet[1].Length; i++)
			{
				languages[i - 4] = leaderSheet[2][i];
				localizedNamespaces[i - 4] = leaderSheet[1][i];
			}
			traitWorkloads = new();
			TraitWorkload curTraitWorkload = null;
			for (int row = 3; row < leaderSheet.Count; row++)
			{
				string[] columns = leaderSheet[row];
				if (columns[0] != "")
				{
					traitWorkloads.Add(new TraitWorkload(columns[0]));
					curTraitWorkload = traitWorkloads.Last();
					for (int LanguageIdx = 0; LanguageIdx < NumLanguages; LanguageIdx++)
						curTraitWorkload.traitNameLocTexts[LanguageIdx] = columns[4 + LanguageIdx];
				}
				else
				{
					curTraitWorkload.leaderIDs.Add(columns[1]);
					curTraitWorkload.leaderPortraitTokens.Add(columns[2]);
					curTraitWorkload.leaderPortraitPaths.Add(columns[3]);
					for (int LanguageIdx = 0; LanguageIdx < NumLanguages; LanguageIdx++)
						curTraitWorkload.leaderNameLocTexts[LanguageIdx].Add(columns[4 + LanguageIdx]);
				}
			}

			List<string[]> popSheet = new();
			using (var fileStream = File.Open(@"pop_portrait_list.csv", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
			using (var reader = new StreamReader(fileStream, System.Text.Encoding.UTF8))
			{
				while (!reader.EndOfStream)
				{
					var line = reader.ReadLine();
					if (line != null)
					{
						popSheet.Add(line.Split(','));
					}
				}
			}
			for (int row = 1; row < popSheet.Count; row++)
			{
				string[] columns = popSheet[row];
				if (columns[0] != "")
				{
					curTraitWorkload = traitWorkloads.Find((TraitWorkload t) => t.traitName == columns[0]);
				}
				else
				{
					curTraitWorkload.popPortraitTokens.Add(columns[1]);
					curTraitWorkload.popPortraitPaths.Add(columns[2]);
				}
			}
		}
	}
}