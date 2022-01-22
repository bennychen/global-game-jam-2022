/*************************************************************************************************\
// HyperCard
// Author : Bourgot Jean-Louis (Enixion)
// This code can only be used under the standard Unity Asset Store End User License Agreement,
// a copy of which is available at http://unity3d.com/company/legal/as_terms.
\*************************************************************************************************/
using System;
using System.Collections.Generic;
using UnityEngine;

namespace HyperCard
{
    [Serializable]
    public class CardProperties
    {
        public Card Card;

        [SerializeField] public int Id;

        [SerializeField] public int Stencil = 2;
        [SerializeField] public int RenderQueue = -1;
        [SerializeField] public int Seed;

        [SerializeField] public float Opacity = 1;
        [SerializeField] public Shader SideShader;
        [SerializeField] public GameObject SpritePrefab;

        [SerializeField] public List<CustomTextComponent> CustomTextList = new List<CustomTextComponent>(0);
        [SerializeField] public List<CustomSpriteComponent> CustomSpriteList = new List<CustomSpriteComponent>(0);

        [SerializeField] public CardSide FaceSide;
        [SerializeField] public CardSide BackSide;

        // Outline
        [SerializeField] public bool IsOutlineEnabled;
        [SerializeField] public float OutlineWidth;
        [SerializeField] public float OutlineHeight;
        [SerializeField] public float OutlineSmoothness;
        [SerializeField] public float OutlineSmoothSpeed = 1;
        [SerializeField] public float OutlineTrimOffset = 0.03f;
        [SerializeField] public Vector2 OutlinePosOffset = Vector2.zero;
        [SerializeField] public Color OutlineStartColor = Color.white;
        [SerializeField] public Color OutlineEndColor = Color.cyan;
        [SerializeField] public float OutlineEndColorDistance = 0.15f;
        [SerializeField] public float OutlineColorExposure = 5;

        [SerializeField] public float OutlineNoiseFrequency = 5;
        [SerializeField] public float OutlineNoiseSpeed = 0.2f;
        [SerializeField] public float OutlineNoiseMult = 0.5f;
        [SerializeField] public float OutlineNoiseOffset = 0.5f;
        [SerializeField] public float OutlineNoiseThreshold = 1f;
        [SerializeField] public float OutlineNoiseDistance = 0.1f;
        [SerializeField] public float OutlineNoiseVerticalAjust = 0;

        // Dissolve
        [SerializeField] public bool IsDissolveEnabled;
        [SerializeField] public float DissolveNoiseFrequency = 3;
        [SerializeField] public float DissolveAmount;
        [SerializeField] public float DissolveRotateSpeed;
        [SerializeField] public float DissolveOutline = 0.05f;
        [SerializeField] public Color DissolveStartColor = Color.white;
        [SerializeField] public Color DissolveEndColor = Color.red;
        [SerializeField] public float DissolveColorExposure = 1;
        [SerializeField] public float DissolveAlphaCut = 0.25f;

        // Filters
        [SerializeField] public bool BlackAndWhite;

        // Mods
        [SerializeField] public bool UseFullscreenCanvas;
        [SerializeField] public bool CCGKitMode;
        [SerializeField] public bool UseSortingGroups;
        [SerializeField] public bool UseCollection;

        public CardProperties(Card card)
        {
            Card = card;
            FaceSide = new CardSide(card);
            BackSide = new CardSide(card);
        }
    }
}
