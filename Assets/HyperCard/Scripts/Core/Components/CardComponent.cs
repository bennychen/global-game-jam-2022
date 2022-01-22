/*************************************************************************************************\
// HyperCard
// Author : Bourgot Jean-Louis (Enixion)
// This code can only be used under the standard Unity Asset Store End User License Agreement,
// a copy of which is available at http://unity3d.com/company/legal/as_terms.
\*************************************************************************************************/
using System;
using UnityEngine;

namespace HyperCard
{
    [Serializable]
    public abstract class CardComponent
    {
        public CardProperties Properties
        {
            get
            {
                return Card.Properties;
            }
        }

        [SerializeField] public Card Card;
    }
}
