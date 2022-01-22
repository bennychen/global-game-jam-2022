/*************************************************************************************************\
// HyperCard
// Author : Bourgot Jean-Louis (Enixion)
// This code can only be used under the standard Unity Asset Store End User License Agreement,
// a copy of which is available at http://unity3d.com/company/legal/as_terms.
\*************************************************************************************************/
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HyperCard
{
    public class CardCollection : ScriptableObject
    {
        public List<GameObject> Cards;

        public GameObject InstantiateCard(int cardId)
        {
            var card = Cards.FirstOrDefault(x => x.GetComponent<Card>().Properties.Id == cardId);

            if(card == null)
            {
                Debug.LogWarning("HyperCard : Collection doesn't contain a card with id : " + cardId);
                return null;
            }

            return Instantiate(card);
        }
    }
}