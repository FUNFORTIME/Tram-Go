using System.Diagnostics;
using TMPro;
using UnityEngine;

public static class TextHelper
{
    /// <summary>
    /// get text of child from parent
    /// </summary>
    /// <param name="parent">parent</param>
    /// <param name="childName">name of child</param>
    /// <returns>text of child</returns>
    public static string GetTextFromChild(GameObject parent, string childName)
    {

        if (parent == null)
        {
            StackTrace stackTrace = new StackTrace();
            StackFrame stackFrame = stackTrace.GetFrame(1); 
            string methodName = stackFrame.GetMethod().Name; 
            string fileName = stackFrame.GetFileName(); 
            int lineNumber = stackFrame.GetFileLineNumber(); 

            
            UnityEngine.Debug.Log($"Parent null! Called from: {methodName} in {fileName} at line {lineNumber}");

            return string.Empty;
        }

        
        Transform child = parent.transform.Find(childName);
        if (child == null)
        {
            StackTrace stackTrace = new StackTrace();
            StackFrame stackFrame = stackTrace.GetFrame(1);
            string methodName = stackFrame.GetMethod().Name;
            string fileName = stackFrame.GetFileName();
            int lineNumber = stackFrame.GetFileLineNumber();
            UnityEngine.Debug.Log($"{childName} null! Called from: {methodName} in {fileName} at line {lineNumber}");
            return string.Empty;
        }

        
        var textComponent = child.GetComponent<UnityEngine.UI.Text>(); // Unity UI Text
        if (textComponent != null)
        {
            return textComponent.text.Trim();
        }

        var textMeshProComponent = child.GetComponent<TextMeshProUGUI>(); // TextMeshPro
        if (textMeshProComponent != null)
        {
           // UnityEngine.Debug.LogWarning(textMeshProComponent.text.Trim());
            return textMeshProComponent.text.Trim();
        }

        UnityEngine.Debug.LogError($" {childName} :No text");
        return string.Empty;
    }

    public static TextMeshPro GetMeshProFromChild(GameObject parent, string childName)
    {
        if (parent == null)
        {
            UnityEngine.Debug.LogError("parent null!");
            return null;
        }


        Transform child = parent.transform.Find(childName);
        if (child == null)
        {
            UnityEngine.Debug.LogError($"{childName} null!");
            return null;
        }

        return child.GetComponent<TextMeshPro>();
    }
}