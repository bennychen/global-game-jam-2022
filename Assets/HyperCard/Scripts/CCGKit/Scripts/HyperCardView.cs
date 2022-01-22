// Copyright (C) 2016-2017 David Pol. All rights reserved.
// Copyright (C) 2018 Enixion. All rights reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement,
// a copy of which is available at http://unity3d.com/company/legal/as_terms.
#if CCGKIT_HYPERCARD
using CCGKit;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(HyperCard.Card))]
public class HyperCardView : CardView
{
    [SerializeField]
    public HyperCard.Card HyperCardComponent;

    protected override void Awake()
    {

    }

    public override void PopulateWithInfo(RuntimeCard card)
    {
        base.card = card;

        var gameConfig = GameManager.Instance.config;

        var libraryCard = gameConfig.GetCard(card.cardId);
        Assert.IsNotNull(libraryCard);
        HyperCardComponent.Properties.CustomTextList.First(x => x.Key == "Name").Value = libraryCard.name;
        HyperCardComponent.Properties.CustomTextList.First(x => x.Key == "Description").Value = libraryCard.GetStringProperty("Text");

        var cost = libraryCard.costs.Find(x => x is PayResourceCost);
        if (cost != null)
        {
            var payResourceCost = cost as PayResourceCost;
            manaCost = payResourceCost.value;
            HyperCardComponent.Properties.CustomTextList.First(x => x.Key == "Cost").Value = manaCost.ToString();
        }
    }

    public override void PopulateWithLibraryInfo(Card card)
    {
        HyperCardComponent.Properties.CustomTextList.First(x => x.Key == "Name").Value = card.name;
        HyperCardComponent.Properties.CustomTextList.First(x => x.Key == "Description").Value = card.GetStringProperty("Text");

        var cost = card.costs.Find(x => x is PayResourceCost);
        if (cost != null)
        {
            var payResourceCost = cost as PayResourceCost;
            manaCost = payResourceCost.value;
            HyperCardComponent.Properties.CustomTextList.First(x => x.Key == "Cost").Value = manaCost.ToString();
        }
    }

    public override bool IsHighlighted()
    {
        return HyperCardComponent.Properties.IsOutlineEnabled;
    }

    public override void SetHighlightingEnabled(bool enabled)
    {
        HyperCardComponent.Properties.IsOutlineEnabled = enabled;
        HyperCardComponent.Properties.FaceSide.DrawOutline();
    }
}
#endif