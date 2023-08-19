
namespace SpeciesModGenerator
{
	internal class Program
	{
		static void Main()
		{
			DirectoryInfo directory = Directory.CreateDirectory(Directory.GetCurrentDirectory());
			while (!directory.GetFiles().Any((file) => file.Name == "SpeciesModGenerator.csproj"))
			{
				directory = directory.Parent;
			}
			Directory.SetCurrentDirectory(directory.FullName);
			ModWorkload.ParseModWorkLoad();
			OnActionGenerator.Generate();
			EventGenerator.Generate();
			TraitGenerator.Generate();
			EdictGenerator.Generate();
			PortraitGroupsGenerator.Generate();
			SpeciesGenerator.Generate();
			LocalisationGenerator.Generate();
		}
	}
}