/*************************************************************************************************\
// HyperCard
// Author : Bourgot Jean-Louis (Enixion)
// This code can only be used under the standard Unity Asset Store End User License Agreement,
// a copy of which is available at http://unity3d.com/company/legal/as_terms.
\*************************************************************************************************/
using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace HyperCard
{
    [Serializable]
    public class CardSide : CardComponent
    {
        private Material _currentMaterial;

        [SerializeField] public Renderer Renderer;
        [SerializeField] public bool IsEnabled = true;

        // Frame
        [SerializeField] public Texture2D FrameMap;
        [SerializeField] public Texture2D FrameDiffuse;
        [SerializeField] public Color FrameDiffuseColor = Color.white;
        [SerializeField] public Texture2D Artwork;
        [SerializeField] public Vector2 ArtworkOffset = Vector2.zero;
        [SerializeField] public Vector2 ArtworkScale = Vector2.one;
        [SerializeField] public float ArtworkScaleFactor = 1f;

        // Distortion
        [SerializeField] public bool IsDistortionEnabled;
        [SerializeField] public Texture2D DistortionMap;

        [SerializeField] public float DistortionRedFrequency;
        [SerializeField] public float DistortionRedAmplitude;
        [SerializeField] public float DistortionRedSpeed;
        [SerializeField] public Vector2 DistortionRedDirection;

        [SerializeField] public float DistortionGreenFrequency;
        [SerializeField] public float DistortionGreenAmplitude;
        [SerializeField] public float DistortionGreenSpeed;
        [SerializeField] public Vector2 DistortionGreenDirection;

        [SerializeField] public float DistortionBlueFrequency;
        [SerializeField] public float DistortionBlueAmplitude;
        [SerializeField] public float DistortionBlueSpeed;
        [SerializeField] public Vector2 DistortionBlueDirection;

        // Sprites sheet
        private int _spriteSheetIndex;
        private float _spriteSheetFxTime;
        private float _spriteSheetNextFxTime;

        [SerializeField] public bool IsSpriteSheetEnabled;
        [SerializeField] public Texture2D SpriteSheetTexture;
        [SerializeField] public Vector2 SpriteSheetSize;
        [SerializeField] public float SpriteSheetSpeed = 1;
        [SerializeField] public float SpriteSheetOffsetX = 0;
        [SerializeField] public float SpriteSheetOffsetY = 0;
        [SerializeField] public Vector2 SpriteSheetScale = Vector2.one;
        [SerializeField] public Color SpriteSheetColor = Color.white;
        [SerializeField] public bool SpriteSheetRemoveBlack;

        // Holo/Cubemap FX
        [SerializeField] public bool IsHoloFxEnabled;
        [SerializeField] public bool HoloUseArtworkCoords;      
        [SerializeField] public Texture2D HoloMask;
        [SerializeField] public Texture2D HoloMap;
        [SerializeField] public Vector2 HoloMapScale = Vector2.one;
        [SerializeField] public Cubemap HoloCube;
        [SerializeField] public Color HoloCubeColor = Color.white;
        [SerializeField] public float HoloCubeContrast = 1;
        [SerializeField] public float HoloCubeRotation;
        [SerializeField] public Vector3 HoloCubeBoundingBoxScale = Vector3.one;
        [SerializeField] public Vector3 HoloCubeBoundingBoxOffset = Vector3.zero;
        [SerializeField] public float HoloPower = 1;
        [SerializeField] public float HoloAlpha = 1;
        [SerializeField] public bool ShowHoloGuizmo;

        // Glitter FX
        [SerializeField] public bool IsGlitterFxEnabled;
        [SerializeField] public bool GlitterUseArtworkCoords;
        [SerializeField] public Color GlitterColor = Color.white;
        [SerializeField] public float GlitterContrast;
        [SerializeField] public float GlitterPower = 1;
        [SerializeField] public float GlitterSize;
        [SerializeField] public float GlitterSpeed;
        [SerializeField] public Texture2D GlitterMask;
        [SerializeField] public Vector2 GlitterMaskScale = Vector2.one;      
        [SerializeField] public Texture2D GlitterBackTex;
        [SerializeField] public Vector2 GlitterBackTexScale = Vector2.one;
        [SerializeField] public float GlitterBackPower;
        [SerializeField] public float GlitterBackContrast;
        [SerializeField] public float GlitterLight;
        [SerializeField] public Color GlitterLightColor = Color.white;
        [SerializeField] public float GlitterLightRadius;
        [SerializeField] public float GlitterOpacity;

        // Color
        [SerializeField] public bool IsSideColorEnabled;
        [SerializeField] public Color SideColor = Color.white;
        [SerializeField] public bool SideColorOverrideTextTags;

        public bool OverrideTextTag
        {
            get
            {
                return IsSideColorEnabled && SideColorOverrideTextTags;
            }
        }

        public CardSide(Card card)
        {
            Card = card;
        }

        public void Redraw()
        {
            Assert.IsNotNull(Properties);

            if (!IsEnabled || Properties.SideShader == null || Renderer == null) return;

            var materials = Renderer.sharedMaterials;

            materials[0] = new Material(Properties.SideShader);
            Renderer.sharedMaterials = materials;

            if (FrameMap != null && Renderer is SpriteRenderer)
            {
                var sprite = Sprite.Create(FrameMap, new Rect(0, 0, FrameMap.width, FrameMap.height), new Vector2(0.5f, 0.5f), 100);
                sprite.name = Guid.NewGuid().ToString();
                ((SpriteRenderer) Renderer).sprite = sprite;
            }

            _currentMaterial = new Material(Renderer.sharedMaterials[0])
            {
                name = Guid.NewGuid().ToString()
            };

            _currentMaterial.SetInt("_Stencil", Properties.Stencil);
            _currentMaterial.SetInt("_Seed", Properties.Seed);
            _currentMaterial.SetFloat("_CardOpacity", Properties.Opacity);

            // Frame
            DrawFrame();

            // Outline
            DrawOutline();

            // Distortion
            ComputeDistortion();

            // Sprites sheet
            ComputeSpriteSheet();

            // Holo/cubemap FX
            ComputeHoloCubemapFx();

            // Glitter FX
            ComputeGlitterFx();

            // Dissolve
            ComputeDissolve();

            // Color
            ComputeSideColor();

            _currentMaterial.renderQueue = Properties.RenderQueue;

            Renderer.materials = new[] { _currentMaterial };
        }

        public void DrawFrame()
        {
            _currentMaterial.SetTexture("_FrameMap", FrameMap);
            _currentMaterial.SetTexture("_FrameDiffuse", FrameDiffuse);
            _currentMaterial.SetColor("_FrameDiffuseColor", FrameDiffuseColor);
            _currentMaterial.SetTexture("_Artwork", Artwork);
            _currentMaterial.SetTextureOffset("_Artwork", ArtworkOffset / 10);
            _currentMaterial.SetTextureScale("_Artwork", ArtworkScale * ArtworkScaleFactor);
        }

        public void DrawOutline()
        {
            _currentMaterial.SetInt("_IsOutlineEnabled", Properties.IsOutlineEnabled ? 1 : 0);

            if (!Properties.IsOutlineEnabled) return;

            _currentMaterial.SetFloat("_OutlineWidth", Properties.OutlineWidth / 10);
            _currentMaterial.SetFloat("_OutlineHeight", Properties.OutlineHeight / 10);
            _currentMaterial.SetFloat("_OutlineSmoothness", Properties.OutlineSmoothness);
            _currentMaterial.SetFloat("_OutlineSmoothSpeed", Properties.OutlineSmoothSpeed);
            _currentMaterial.SetFloat("_OutlineTrimOffset", Properties.OutlineTrimOffset);
            _currentMaterial.SetVector("_OutlinePosOffset", Properties.OutlinePosOffset);
            _currentMaterial.SetColor("_OutlineStartColor", Properties.OutlineStartColor);
            _currentMaterial.SetColor("_OutlineEndColor", Properties.OutlineEndColor);
            _currentMaterial.SetFloat("_OutlineEndColorDistance", Properties.OutlineEndColorDistance);
            _currentMaterial.SetFloat("_OutlineColorExposure", Properties.OutlineColorExposure);      
            _currentMaterial.SetFloat("_OutlineNoiseFrequency", Properties.OutlineNoiseFrequency);
            _currentMaterial.SetFloat("_OutlineNoiseSpeed", Properties.OutlineNoiseSpeed);
            _currentMaterial.SetFloat("_OutlineNoiseMult", Properties.OutlineNoiseMult);
            _currentMaterial.SetFloat("_OutlineNoiseOffset", Properties.OutlineNoiseOffset);
            _currentMaterial.SetFloat("_OutlineNoiseThreshold", Properties.OutlineNoiseThreshold);
            _currentMaterial.SetFloat("_OutlineNoiseDistance", Properties.OutlineNoiseDistance);
            _currentMaterial.SetFloat("_OutlineNoiseVerticalAjust", Properties.OutlineNoiseVerticalAjust);
        }

        public void ComputeDissolve()
        {
            _currentMaterial.SetInt("_IsDissolveEnabled", Properties.IsDissolveEnabled ? 1 : 0);

            if (!Properties.IsDissolveEnabled) return;

            _currentMaterial.SetFloat("_BurnNoiseFrequency", Properties.DissolveNoiseFrequency);
            _currentMaterial.SetFloat("_BurningAmount", Properties.DissolveAmount);
            _currentMaterial.SetFloat("_BurningRotateSpeed", Properties.DissolveRotateSpeed);
            _currentMaterial.SetFloat("_BurningOutline", Properties.DissolveOutline);
            _currentMaterial.SetColor("_BurnStartColor", Properties.DissolveStartColor);
            _currentMaterial.SetColor("_BurnEndColor", Properties.DissolveEndColor);
            _currentMaterial.SetFloat("_BurnExposure", Properties.DissolveColorExposure);
            _currentMaterial.SetFloat("_BurnAlphaCut", Properties.DissolveAlphaCut);
        }

        public void ComputeDistortion()
        {
            _currentMaterial.SetInt("_IsDistortionEnabled", IsDistortionEnabled ? 1 : 0);

            if (!IsDistortionEnabled) return;

            _currentMaterial.SetTexture("_DistortionMap", DistortionMap);

            _currentMaterial.SetFloat("_DistortionRedFrequency", DistortionRedFrequency);
            _currentMaterial.SetFloat("_DistortionRedAmplitude", DistortionRedAmplitude);
            _currentMaterial.SetFloat("_DistortionRedSpeed", DistortionRedSpeed);
            _currentMaterial.SetVector("_DistortionRedDirection", DistortionRedDirection);

            _currentMaterial.SetFloat("_DistortionGreenFrequency", DistortionGreenFrequency);
            _currentMaterial.SetFloat("_DistortionGreenAmplitude", DistortionGreenAmplitude);
            _currentMaterial.SetFloat("_DistortionGreenSpeed", DistortionGreenSpeed);
            _currentMaterial.SetVector("_DistortionGreenDirection", DistortionGreenDirection);

            _currentMaterial.SetFloat("_DistortionBlueFrequency", DistortionBlueFrequency);
            _currentMaterial.SetFloat("_DistortionBlueAmplitude", DistortionBlueAmplitude);
            _currentMaterial.SetFloat("_DistortionBlueSpeed", DistortionBlueSpeed);
            _currentMaterial.SetVector("_DistortionBlueDirection", DistortionBlueDirection);
        }

        public void ComputeSpriteSheet()
        {
            _currentMaterial.SetInt("_IsSpriteSheetEnabled", IsSpriteSheetEnabled ? 1 : 0);

            if (!IsSpriteSheetEnabled) return;

            _currentMaterial.SetTexture("_SpriteSheetTexture", SpriteSheetTexture);
            _currentMaterial.SetVector("_SpriteSheetOffset", new Vector2(SpriteSheetOffsetX, SpriteSheetOffsetY));
            _currentMaterial.SetVector("_SpriteSheetScale", SpriteSheetScale);
            _currentMaterial.SetFloat("_SpriteSheetCols", SpriteSheetSize.x);
            _currentMaterial.SetFloat("_SpriteSheetRows", SpriteSheetSize.y);
            _currentMaterial.SetColor("_SpriteSheetColor", SpriteSheetColor);
            _currentMaterial.SetInt("_SpriteSheetRemoveBlack", SpriteSheetRemoveBlack ? 1 : 0);
        }

        private void UpdateSpriteSheet()
        {
            if (SpriteSheetTexture == null) return;

            if (SpriteSheetSize.x <= 0 || SpriteSheetSize.y <= 0) return;

            _spriteSheetFxTime += Time.deltaTime;

            if (_spriteSheetFxTime <= _spriteSheetNextFxTime) return;

            _spriteSheetIndex = _spriteSheetIndex % (int)(SpriteSheetSize.x * SpriteSheetSize.y);

            _currentMaterial.SetFloat("_SpriteSheetIndex", _spriteSheetIndex++);

            _spriteSheetNextFxTime = _spriteSheetFxTime + (SpriteSheetSpeed / 100f);
            _spriteSheetNextFxTime -= _spriteSheetFxTime;
            _spriteSheetFxTime = 0;
        }

        public void ComputeHoloCubemapFx()
        {
            _currentMaterial.SetInt("_IsHoloFxEnabled", IsHoloFxEnabled ? 1 : 0);

            if (!IsHoloFxEnabled) return;

            var centerBBox = Properties.Card.transform.position + HoloCubeBoundingBoxOffset;
            var bMin = centerBBox - HoloCubeBoundingBoxScale / 2;
            var bMax = centerBBox + HoloCubeBoundingBoxScale / 2;

            _currentMaterial.SetInt("_HoloUseArtworkCoords", HoloUseArtworkCoords ? 1 : 0);
            _currentMaterial.SetTexture("_HoloMask", HoloMask);
            _currentMaterial.SetTexture("_HoloMap", HoloMap);
            _currentMaterial.SetVector("_HoloMapScale", HoloMapScale);
            _currentMaterial.SetTexture("_HoloCube", HoloCube);
            _currentMaterial.SetColor("_HoloCubeColor", HoloCubeColor);
            _currentMaterial.SetFloat("_HoloCubeContrast", HoloCubeContrast);
            _currentMaterial.SetFloat("_HoloCubeRotation", HoloCubeRotation);
            _currentMaterial.SetFloat("_HoloPower", HoloPower);
            _currentMaterial.SetFloat("_HoloAlpha", HoloAlpha);
            _currentMaterial.SetVector("_HoloBBoxMin", bMin);
            _currentMaterial.SetVector("_HoloBBoxMax", bMax);
            _currentMaterial.SetVector("_HoloEnviCubeMapPos", centerBBox);
            _currentMaterial.SetInt("_HoloDebug", ShowHoloGuizmo ? 1 : 0);
        }

        public void ComputeGlitterFx()
        {
            _currentMaterial.SetInt("_IsGlitterFxEnabled", IsGlitterFxEnabled ? 1 : 0);

            if (!IsGlitterFxEnabled) return;

            _currentMaterial.SetInt("_GlitterUseArtworkCoords", GlitterUseArtworkCoords ? 1 : 0);
            _currentMaterial.SetColor("_GlitterColor", GlitterColor);
            _currentMaterial.SetFloat("_GlitterContrast", GlitterContrast);
            _currentMaterial.SetFloat("_GlitterPower", GlitterPower);
            _currentMaterial.SetFloat("_GlitterSize", GlitterSize);
            _currentMaterial.SetFloat("_GlitterSpeed", GlitterSpeed);
            _currentMaterial.SetTexture("_GlitterMask", GlitterMask);
            _currentMaterial.SetTextureScale("_GlitterMask", GlitterMaskScale);
            _currentMaterial.SetTexture("_GlitterBackTex", GlitterBackTex);
            _currentMaterial.SetTextureScale("_GlitterBackTex", GlitterBackTexScale);
            _currentMaterial.SetFloat("_GlitterBackPower", GlitterBackPower);
            _currentMaterial.SetFloat("_GlitterBackContrast", GlitterBackContrast);
            _currentMaterial.SetFloat("_GlitterLight", GlitterLight);
            _currentMaterial.SetColor("_GlitterLightColor", GlitterLightColor);
            _currentMaterial.SetFloat("_GlitterLightRadius", GlitterLightRadius);
            _currentMaterial.SetFloat("_GlitterOpacity", GlitterOpacity);
        }

        public void ComputeSideColor()
        {
            if (!IsSideColorEnabled || Properties.BlackAndWhite)
            {
                _currentMaterial.SetColor("_SideColor", Color.white);
                return;
            }

            _currentMaterial.SetColor("_SideColor", SideColor);           
        }

        public void Update()
        {
            Assert.IsNotNull(Properties);

            if (_currentMaterial == null)
                return;

            _currentMaterial.SetFloat("_BurningAmount", Properties.DissolveAmount);
            _currentMaterial.SetInt("_BlackAndWhite", Properties.BlackAndWhite ? 1 : 0);
            _currentMaterial.SetFloat("_CardOpacity", Properties.Opacity);

            UpdateSpriteSheet();
        }

        public void OnDrawGizmos()
        {
            if (!IsEnabled || Properties.SideShader == null || Renderer == null) return;

            if (!IsHoloFxEnabled || !ShowHoloGuizmo) return;

            var centerBBox = Properties.Card.transform.position + HoloCubeBoundingBoxOffset;

            Gizmos.color = new Color(0, 0, 1, 0.25f);
            Gizmos.DrawCube(centerBBox, HoloCubeBoundingBoxScale);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(centerBBox, HoloCubeBoundingBoxScale);
        }
    }
}
