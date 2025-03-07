using UnityEngine;

namespace LanguageLocalization.ExampleContent
{
    /// <summary>
    /// Example script for advanced localization of dynamic variables... Follow the code below
    /// </summary>
    public sealed class ExampleScript : MonoBehaviour
    {
        public Localization_SOURCE langLocalization;    // Language localization indication
        public string receivedTextSample = "";          // Received & converted text after translation
        public string myName = "Matt";                  // My dynamic variable (can be any - int, string etc)

        private void OnEnable()
        {
            langLocalization.OnLanguageLoaded += LanguageRefresh;
        }

        private void OnDisable()
        {
            langLocalization.OnLanguageLoaded -= LanguageRefresh;
        }

        /// <summary>
        /// Custom method for language refresh
        /// </summary>
        private void LanguageRefresh()
        {
            // Get the translated text from the localization and replace the custom variable macro with myName
            // Notice that the VariableTest key is added in LanguageLocalization component and the Assignation Type is set to None as we don't need any automation
            receivedTextSample = langLocalization.ReturnText("VariableTest").Replace("#MYNAME#", myName);
            // Debug the result!
            Debug.Log(receivedTextSample);
        }
    }
}