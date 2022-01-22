// Copyright (C) 2016-2017 David Pol. All rights reserved.
// Copyright (C) 2018 Enixion. All rights reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement,
// a copy of which is available at http://unity3d.com/company/legal/as_terms.
#if CCGKIT_HYPERCARD
using CCGKit;
using System.Linq;

public class HyperCardCreatureView : HyperCardView
{
    public Stat attackStat { get; protected set; }
    public Stat defenseStat { get; protected set; }

    public override void PopulateWithInfo(RuntimeCard card)
    {
        base.PopulateWithInfo(card);
        attackStat = card.namedStats["Attack"];
        defenseStat = card.namedStats["Life"];

        HyperCardComponent.Properties.CustomTextList.First(x => x.Key == "Attack").Value = attackStat.effectiveValue.ToString();
        HyperCardComponent.Properties.CustomTextList.First(x => x.Key == "Health").Value = defenseStat.effectiveValue.ToString();

        attackStat.onValueChanged += (oldValue, newValue) => {
            HyperCardComponent.Properties.CustomTextList.First(x => x.Key == "Attack").Value = attackStat.effectiveValue.ToString();
        };

        defenseStat.onValueChanged += (oldValue, newValue) => {
            HyperCardComponent.Properties.CustomTextList.First(x => x.Key == "Health").Value = defenseStat.effectiveValue.ToString();
        };
    }

    public override void PopulateWithLibraryInfo(Card card)
    {
        base.PopulateWithLibraryInfo(card);
        HyperCardComponent.Properties.CustomTextList.First(x => x.Key == "Attack").Value = card.stats[0].effectiveValue.ToString();
        HyperCardComponent.Properties.CustomTextList.First(x => x.Key == "Health").Value = card.stats[1].effectiveValue.ToString();
    }
}
#endif