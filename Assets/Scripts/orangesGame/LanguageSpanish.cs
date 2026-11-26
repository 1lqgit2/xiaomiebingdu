namespace orangesGame
{
	public class LanguageSpanish : Language
	{
		public override string GetTxt(int index)
		{
			LanguageStruct[] languageJson = Singleton<GameJson>.Instance.LanguageJson;
			if (index >= languageJson.Length)
			{
				return string.Empty;
			}
			return languageJson[index - 1].Spanish;
		}
	}
}
