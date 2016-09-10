using UnityEngine;
using strange.extensions.context.impl;

namespace Racingcow.OcrOfTheDead
{
    public class Root : ContextView
    {
        void Awake()
        {
            Debug.Log("Root is awake");
            context = new GameContext(this);
        }
    }
}