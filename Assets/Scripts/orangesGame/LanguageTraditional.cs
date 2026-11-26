namespace orangesGame
{
	internal class LanguageTraditional : Language
	{
		public override string GetTxt(int index)
		{
			LanguageStruct[] languageJson = Singleton<GameJson>.Instance.LanguageJson;
			if (languageJson.Length < index)
			{
				return string.Empty;
			}
			return languageJson[index - 1].Traditional;
		}
	}
}
