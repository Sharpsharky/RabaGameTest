using System.Globalization;
using TMPro;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    public class SetTextMeshProText : FsmStateAction
    {

        public GameObject TextMeshProGameObject;
        public FsmString Text;
        private TextMeshPro text;
        private TextMeshProUGUI textUGUI;

        public override void Reset()
        {

            Text = null;
            TextMeshProGameObject = null;
            text = null;
            textUGUI = null;
        }

        public override void OnEnter()
        {

            if (TextMeshProGameObject)
            {
                text = TextMeshProGameObject.GetComponent<TextMeshPro>();
                if (text)
                {
                    string textValue = Text.Value;

                    //due to the way strings are serialized \n gets converted to \\n and is incorrectly displayed

                    textValue = textValue.Replace("\\n", "\n");
                    text.text = textValue;
                }
                else
                {
                    textUGUI = TextMeshProGameObject.GetComponent<TextMeshProUGUI>();

                    if (textUGUI)
                    {
                        string textValue = Text.Value;

                        //due to the way strings are serialized \n gets converted to \\n and is incorrectly displayed

                        textValue = textValue.Replace("\\n", "\n");
                        textUGUI.text = textValue;
                    }
                    else
                    {

                        Debug.LogError("SetTextMeshProText: Can't find TextMeshProComponent on " + TextMeshProGameObject.gameObject);
                    }
                }
            }
            else
            {
                Debug.LogError("SetTextMeshProText: Can't find game object");
            }

            Finish();
            return;
        }


    }
}
