// Copyright (C) 2016-2017 David Pol. All rights reserved.
// Copyright (C) 2018 Enixion. All rights reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement,
// a copy of which is available at http://unity3d.com/company/legal/as_terms.
#if CCGKIT_HYPERCARD
using CCGKit;
using DG.Tweening;
using HyperCard;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class HyperCardHumanPlayer : DemoHumanPlayer
{
    public CardCollection Collection;

    private IDictionary<int, int> ActiveStencils = new Dictionary<int, int>();

    protected override void AddCardToHand(RuntimeCard card)
    {
        var gameConfig = GameManager.Instance.config;
        var libraryCard = gameConfig.GetCard(card.cardId);

        GameObject go = Collection.InstantiateCard(card.cardId);

        if(go == null)
        {
            var cardType = gameConfig.cardTypes.Find(x => x.id == libraryCard.cardTypeId);

            if (cardType.name == "Creature")
            {
                go = Instantiate(creatureCardViewPrefab as GameObject);
            }
            else if (cardType.name == "Spell")
            {
                go = Instantiate(spellCardViewPrefab as GameObject);
            }
        }

        if(go.GetComponent<HyperCard.Card>() != null)
        {
            go.GetComponent<HyperCard.Card>().Properties.Stencil = GetStencil(card);
            go.GetComponent<HyperCard.Card>().Redraw();
        }

        var cardView = go.GetComponent<CardView>();
        cardView.PopulateWithInfo(card);

        var handCard = go.AddComponent<HandCard>();
        handCard.ownerPlayer = this;
        handCard.boardZone = GameObject.Find("PlayerBoard");

        playerHandCards.Add(cardView);

        go.GetComponent<SortingGroup>().sortingOrder = playerHandCards.Count;
    }

    //protected override IEnumerator CreateCardPreviewAsync(RuntimeCard card, Vector3 pos, bool highlight)
    //{
    //    yield return new WaitForSeconds(0.3f);

    //    var gameConfig = GameManager.Instance.config;
    //    var libraryCard = gameConfig.GetCard(card.cardId);

    //    GameObject gocurrentCardPreview = Collection.InstantiateCard(card.cardId);

    //    if (currentCardPreview == null)
    //    {
    //        var cardType = gameConfig.cardTypes.Find(x => x.id == libraryCard.cardTypeId);

    //        if (cardType.name == "Creature")
    //        {
    //            currentCardPreview = Instantiate(creatureCardViewPrefab as GameObject);
    //        }
    //        else if (cardType.name == "Spell")
    //        {
    //            currentCardPreview = Instantiate(spellCardViewPrefab as GameObject);
    //        }
    //    }

    //    var cardView = currentCardPreview.GetComponent<CardView>();
    //    cardView.PopulateWithInfo(card);
    //    cardView.SetHighlightingEnabled(highlight);
    //    cardView.isPreview = true;

    //    var newPos = pos;
    //    newPos.y += 2.0f;
    //    currentCardPreview.transform.position = newPos;
    //    currentCardPreview.transform.localRotation = Quaternion.Euler(Vector3.zero);
    //    currentCardPreview.transform.localScale = new Vector2(1.5f, 1.5f);
    //    currentCardPreview.GetComponent<SortingGroup>().sortingOrder = 1000;
    //    currentCardPreview.layer = LayerMask.NameToLayer("Ignore Raycast");
    //    currentCardPreview.transform.DOMoveY(newPos.y + 1.0f, 0.1f);
    //}

    protected override void RemoveCardFromHand(RuntimeCard card)
    {
        ActiveStencils.Remove(card.instanceId);
    }

    private int GetStencil(RuntimeCard card)
    {
        var stencilIndex = 2;

        while (ActiveStencils.Any(s => s.Value == stencilIndex))
        {
            stencilIndex += 2;
        }

        ActiveStencils.Add(card.instanceId, stencilIndex);

        return stencilIndex;
    }
}
#endif