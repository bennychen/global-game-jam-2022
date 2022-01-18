//This class is auto-generated do not modify
namespace k
{
	public static class Scenes
	{

		public const int TOTAL_SCENES = 0;


		public static int nextSceneIndex()
		{
			if( UnityEngine.Application.loadedLevel + 1 == TOTAL_SCENES )
				return 0;
			return UnityEngine.Application.loadedLevel + 1;
		}
	}
}