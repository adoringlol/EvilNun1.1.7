using UnityEngine;

namespace I2.Loc
{
	public class TranslationJob_WWW : TranslationJob
	{
		public WWW www;

		public override void Dispose()
		{
			if (www != null)
			{
				www.Dispose();
			}
			www = null;
		}
	}
}
