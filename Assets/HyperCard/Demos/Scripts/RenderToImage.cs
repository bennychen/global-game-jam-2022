using UnityEngine;
using UnityEngine.UI;

namespace HyperCard
{
    public enum CardRenderTarget
	{
		Face,
		Back
	}
	
	[ExecuteInEditMode]
	public class RenderToImage : MonoBehaviour
	{
		public Card Card;
		
		public CardRenderTarget Target = CardRenderTarget.Face;

		private int _indexMat = 0;

		void Start()
		{
			if (Target == CardRenderTarget.Back)
				_indexMat = 1;
		}
		
		void Update()
		{
			if (Card == null)
				return;
			
			GetComponent<Image>().material = gameObject.GetComponent<Renderer>().sharedMaterials[_indexMat];
		}
	}
}