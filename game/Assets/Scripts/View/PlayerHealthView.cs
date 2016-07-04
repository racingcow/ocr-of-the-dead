using UnityEngine;
using UnityEngine.UI;
using strange.extensions.mediation.impl;

namespace Racingcow.OcrOfTheDead.Views
{
    public class PlayerHealthView : View
    {
        public void UpdateHealth(int value)
        {
            var newText = string.Format("{0}%", value);
            var textBox = GetComponent<Text>();
            textBox.text = newText;
            Debug.Log(string.Format("PlayerHealthView updating text to {0}", newText));
        }
    }
}