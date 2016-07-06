using System;
using UnityEngine;
using UnityEngine.UI;
using strange.extensions.mediation.impl;

namespace Racingcow.OcrOfTheDead.Views
{
    public class BloodView : View
    {
        public void UpdateHealth(int value)
        {
            // show some red tint when health is low
            var blood = GameObject.FindGameObjectWithTag("Blood");
            var img = blood.GetComponent<RawImage>();
            img.color = new Color(img.color.r, img.color.g, img.color.b, Math.Max((1f - (value / 100f)) - .3f, 0f));
        }
    }
}