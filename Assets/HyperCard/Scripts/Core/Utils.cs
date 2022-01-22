/*************************************************************************************************\
// HyperCard
// Author : Bourgot Jean-Louis (Enixion)
// This code can only be used under the standard Unity Asset Store End User License Agreement,
// a copy of which is available at http://unity3d.com/company/legal/as_terms.
\*************************************************************************************************/
using System.Collections;
using UnityEngine;

namespace HyperCard
{
    public static class Utils
    {
        public static float Remap(float value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }
    }
}
