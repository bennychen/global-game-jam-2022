/*************************************************************************************************\
// HyperCard
// Author : Bourgot Jean-Louis (Enixion)
// This code can only be used under the standard Unity Asset Store End User License Agreement,
// a copy of which is available at http://unity3d.com/company/legal/as_terms.
\*************************************************************************************************/
using System;
using System.Linq;
using UnityEngine;

namespace HyperCard
{
    [ExecuteInEditMode]
    [Serializable]
    public class Card : MonoBehaviour
    {
        [SerializeField] public CardProperties Properties;

        #region Custom sprites
        public void CreateSprite(int index)
        {
            var sprite = Instantiate(Properties.SpritePrefab, Vector3.zero, Quaternion.identity);

            sprite.transform.parent = this.gameObject.transform;
            sprite.transform.position = Vector3.zero;
            sprite.name = "HyperCardSprite-" + index;

            var spriteObject = Properties.CustomSpriteList.Last();

            spriteObject.Card = this;
            spriteObject.Renderer = sprite.GetComponent<SpriteRenderer>();
            spriteObject.IsActive = true;
            spriteObject.Color = Color.white;
            spriteObject.Key = "Sprite" + index;
            spriteObject.Scale = Vector2.one;
            spriteObject.Zoom = 1;
            spriteObject.RenderQueue = 3000;
            spriteObject.IsAffectedByFilters = true;

            Redraw();
        }

        public void RemoveSprite(int index)
        {
            DestroyImmediate(Properties.CustomSpriteList.ElementAt(index).Renderer.gameObject);
            Properties.CustomSpriteList.RemoveAt(index);

            Redraw();
        }

        public void ComputeSprites()
        {
            Properties.CustomSpriteList.ForEach(sprite => sprite.Compute());
        }
        #endregion

        #region Custom texts
        public void ComputeTexts()
        {
            Properties.CustomTextList.ForEach(text => text.Compute());
        }
        #endregion


        // Redraw
        public void Redraw()
        {
            if (Properties.Stencil < 2)
                Properties.Stencil = 2;

            if (Properties.Seed == 0)
                Properties.Seed = UnityEngine.Random.Range(1, 99999);

            Properties.FaceSide.Redraw();

            if (!Properties.CCGKitMode)
            {
                Properties.BackSide.Redraw();
            }

            ComputeSprites();
            ComputeTexts();
        }

        void Update()
        {
            if (Properties == null || Properties.Card == null)
                Properties = new CardProperties(this);

            if (Properties.FaceSide == null || Properties.BackSide == null)
                return;

            // Useful for CCG fade
            if (Properties.CCGKitMode && Properties.FaceSide.Renderer is SpriteRenderer)
            {
                Properties.Opacity = ((SpriteRenderer) Properties.FaceSide.Renderer).color.a;
            }

            Properties.CustomSpriteList.ForEach(sprite => sprite.Update());
            Properties.CustomTextList.ForEach(text => text.Update());

            Properties.FaceSide.Update();

            if(!Properties.CCGKitMode)
            {
                Properties.BackSide.Update();
            }
        }

        void OnDrawGizmos()
        {
            Properties.FaceSide.OnDrawGizmos();

            if (!Properties.CCGKitMode)
            {
                Properties.BackSide.OnDrawGizmos();
            }
        }

        void OnEnable()
        {
            Redraw();
        }
    }
}