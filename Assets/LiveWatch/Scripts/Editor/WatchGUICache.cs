using System.Collections.Generic;
using UnityEngine;

namespace Ingvar.LiveWatch.Editor
{
    public class WatchGUICache
    {
        private Dictionary<string, GUIContent> _textToGUIContentCache = new();

        public GUIContent GetContent(string text)
        {
            if (_textToGUIContentCache.TryGetValue(text, out var guiContent))
                return guiContent;

            guiContent = new GUIContent(text, text);
            _textToGUIContentCache[text] = guiContent;
            return guiContent;
        }
    }
}