/*************************************************************************************************\
// HyperCard
// Author : Bourgot Jean-Louis (Enixion)
// This code can only be used under the standard Unity Asset Store End User License Agreement,
// a copy of which is available at http://unity3d.com/company/legal/as_terms.
\*************************************************************************************************/
using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Rendering;

namespace HyperCard
{
    [Serializable]
    public class CustomSpriteComponent : CardComponent
    {
        [SerializeField] public SpriteRenderer Renderer;
        [SerializeField] public string Key;
        [SerializeField] public Texture2D Texture;
        [SerializeField] public Vector3 Position = Vector3.zero;
        [SerializeField] public Vector2 Scale = Vector2.one;
        [SerializeField] public float Zoom = 1;
        [SerializeField] public Color Color = Color.white;
        [SerializeField] public float Exposure;
        [SerializeField] public bool IsActive = true;
        [SerializeField] public bool ShowAdvancedSettings;
        [SerializeField] public int RenderQueue = 3000;
        [SerializeField] public bool RestrictToArtwork = false;
        [SerializeField] public Texture2D DistortionMask;
        [SerializeField] public Vector2 MaskPosition = Vector2.zero;
        [SerializeField] public Vector2 MaskScale = Vector2.one;
        [SerializeField] public float DistortionFrequency;
        [SerializeField] public float DistortionAmplitude;
        [SerializeField] public float DistortionSpeed;
        [SerializeField] public Vector2 DistortionDirection = Vector2.zero;
        [SerializeField] public Vector2 MoveDirection = Vector2.zero;
        [SerializeField] public bool IsAffectedByFilters;

        public void Compute()
        {
            Assert.IsNotNull(Properties);

            Renderer.gameObject.SetActive(IsActive);

            if (!IsActive)
                return;

            var spriteMat = Renderer.sharedMaterials;

            if (spriteMat[0] == null)
            {
                spriteMat[0] = Properties.SpritePrefab.GetComponent<SpriteRenderer>().sharedMaterial;
                Renderer.sharedMaterials = spriteMat;
            }

            var mat = new Material(Renderer.sharedMaterials[0])
            {
                name = Guid.NewGuid().ToString()
            };

            mat.SetInt("_StencilComp", (int) CompareFunction.Equal);

            if (RestrictToArtwork)
            {
                mat.SetFloat("_Stencil", Properties.Stencil + 1);
            }
            else
            {
                mat.SetFloat("_Stencil", Properties.Stencil);
            }

            mat.renderQueue = RenderQueue;
            mat.SetFloat("_Zoom", Zoom);
            mat.SetTexture("_DistortionMask", DistortionMask);
            mat.SetTextureOffset("_DistortionMask", MaskPosition);
            mat.SetTextureScale("_DistortionMask", MaskScale);
            mat.SetFloat("_DistortionFreq", DistortionFrequency);
            mat.SetFloat("_DistortionAmp", DistortionAmplitude);
            mat.SetFloat("_DistortionSpeed", DistortionSpeed);
            mat.SetVector("_DistortionDir", DistortionDirection);
            mat.SetVector("_MoveDir", MoveDirection);
            mat.SetInt("_BlackAndWhite", Properties.BlackAndWhite && IsAffectedByFilters ? 1 : 0);

            Renderer.material = mat;

            mat.SetFloat("_Exposure", Exposure);

            if (Texture != null)
            {
                var sprite = Sprite.Create(Texture, new Rect(0.0f, 0.0f, Texture.width, Texture.height), new Vector2(0.5f, 0.5f), 100.0f);
                sprite.name = Guid.NewGuid().ToString();
                Renderer.sprite = sprite;
            }

            Renderer.gameObject.transform.localPosition = new Vector3(Position.x / 10, Position.y / 10, 0.01f + Position.z / 100);
            Renderer.gameObject.transform.localScale = new Vector3(Scale.x / 10, Scale.y / 10, 1);
        }

        public void Update()
        {
            Assert.IsNotNull(Properties);

            if (Renderer == null)
                return;

            var color = Color;
            color.a *= Properties.Opacity;

            Renderer.color = color;
        }
    }
}
