using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class LifeUnit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().DOFade(0.5f, 3f).SetLoops(-1, LoopType.Yoyo);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
