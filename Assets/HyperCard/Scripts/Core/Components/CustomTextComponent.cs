/*************************************************************************************************\
// HyperCard
// Author : Bourgot Jean-Louis (Enixion)
// This code can only be used under the standard Unity Asset Store End User License Agreement,
// a copy of which is available at http://unity3d.com/company/legal/as_terms.
\*************************************************************************************************/
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Rendering;

namespace HyperCard
{
    [Serializable]
    public enum TextMeshProParamDisplayMode
    {
        Field,
        TextArea
    }

    [Serializable]
    public enum TextSide
    {
        Face,
        Back
    }

    [Serializable]
    public class CustomTextComponent : CardComponent
    {
        [SerializeField] public TMP_FontAsset FontAsset;
        [SerializeField] public Material FontMaterial;

        [SerializeField] public bool IsActive = true;
        [SerializeField] public string Key;
        [SerializeField] public TextMeshPro TmpObject;

        [SerializeField] private string _value;

        public string Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                TmpObject.text = Value;
            }
        }

        [SerializeField] public TextMeshProParamDisplayMode DisplayMode;
        [SerializeField] public TextSide TextSide = TextSide.Face;

        public virtual void Compute()
        {
            Assert.IsNotNull(Properties);

            if (TmpObject == null) return;

            if (string.IsNullOrEmpty(Key) && Application.isPlaying)
            {
                Debug.LogWarning(string.Format("A text component on GO {0} has no key !.", Properties.Card.gameObject.name));
            }

            TmpObject.enabled = IsActive;

            if (!IsActive) return;

            TmpObject.text = Value;

            TmpObject.overrideColorTags =
                (TextSide == TextSide.Face && Properties.FaceSide != null && Properties.FaceSide.OverrideTextTag) ||
                (TextSide == TextSide.Back && Properties.BackSide != null && Properties.BackSide.OverrideTextTag) ||
                Properties.BlackAndWhite;

            if (Properties.UseSortingGroups)
            {
                var cardSortingGroup = Properties.Card.GetComponent<SortingGroup>();

                if (cardSortingGroup != null)
                {
                    var sortingGroup = TmpObject.GetComponent<SortingGroup>();

                    if (sortingGroup == null)
                    {
                        sortingGroup = TmpObject.gameObject.AddComponent<SortingGroup>();
                    }

                    sortingGroup.sortingOrder = cardSortingGroup.sortingOrder;
                    sortingGroup.sortingLayerID = cardSortingGroup.sortingLayerID;
                }
                else
                {
                    Debug.LogWarning(string.Format("A SortingGroup component is required on the GameObject {0}.", Properties.Card.gameObject.name));
                }
            }

            Card.StartCoroutine(UpdateMaterial());
        }

        public virtual void Update()
        {
            Assert.IsNotNull(Properties);

            if (TmpObject == null) return;

            TmpObject.alpha = Properties.Opacity;
        }

        public virtual IEnumerator UpdateMaterial()
        {
            yield return new WaitForSeconds(0.5f);

            if (FontAsset == null || FontMaterial == null)
            {
                Debug.LogError("Unable to load font material on HyperCard component --> " + TmpObject.name + "!");
                yield return false;
            }

            TmpObject.font = FontAsset;

            var material = new Material(FontMaterial);

            material.SetFloat(ShaderUtilities.ID_StencilID, Properties.Stencil);
            material.SetFloat(ShaderUtilities.ID_StencilComp, (int)CompareFunction.Equal);
            material.SetFloat(ShaderUtilities.ShaderTag_CullMode, (int)CullMode.Back);

            TmpObject.fontSharedMaterials = new Material[] { material };
        }
    }
}
