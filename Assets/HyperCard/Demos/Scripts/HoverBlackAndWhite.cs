using UnityEngine;

namespace HyperCard
{
    public class HoverBlackAndWhite : MonoBehaviour
    {
        void OnMouseEnter()
        {
            GetComponent<Card>().Properties.BlackAndWhite = true;
            GetComponent<Card>().ComputeSprites();
        }

        void OnMouseExit()
        {
            GetComponent<Card>().Properties.BlackAndWhite = false;
            GetComponent<Card>().ComputeSprites();
        }
    }
}
