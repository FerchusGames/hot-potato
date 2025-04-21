using System;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Ingvar.LiveWatch.Editor
{
    [Serializable]
    public class InfoGUI
    {
        public WatchStorage Variables => WatchStorageSO.instance.Watches;
        
        public WatchVariable SelectedVariable { get; set; }
        public bool IsTitleSelected { get; set; }
        public int SelectedColumn { get; set; }

        private Rect contentRect;
        private float valueTextExpectedHeight;
        private Vector2 scrollPosition;
        private bool showValue = true;
        private bool showExtraText = true;
        private bool showStackTrace = true;
        private bool showName = true;
        private bool showCustomActions = true;
        private bool showCreationStackTrace = true;
        
        private const float DistBetweenGroups = 4;
        private const int CreationStackTraceSkippedLines = 2;
        private const int ValueStackTraceSkippedLines = 3;
        
        private const string Name = "NAME";
        private const string CustomActions = "CUSTOM ACTIONS";
        private const string Value = "VALUE";
        private const string ExtraText = "EXTRA TEXT";
        private const string StackTrace = "STACK TRACE";
        
        public void OnGUI(Rect areaRect)
        {
            EditorGUI.DrawRect(areaRect.Extrude(ExtrudeFlags.Top, 1), Colors.Background);

            if (SelectedVariable == null)
            {
                return;
            }

            if (IsTitleSelected)
            {
                DrawForTitle(areaRect);
            }
            else if (SelectedVariable.HasValues)
            {
                DrawForValues(areaRect);
            }
        }

        private void DrawForTitle(Rect rect)
        {
            contentRect = rect.Extrude(ExtrudeFlags.All, -2);

            GUILayout.BeginArea(contentRect);
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);
            
            DrawVariableName();
            DrawCustomActions();
            DrawCreationStackTrace();

            GUILayout.FlexibleSpace();
            GUILayout.EndScrollView();
            GUILayout.EndArea();
        }

        private void DrawForValues(Rect rect)
        {
            if (!SelectedVariable.Values.AnyAt(SelectedColumn))
            {
                return;
            }

            contentRect = rect.Extrude(ExtrudeFlags.All, -2);

            GUILayout.BeginArea(contentRect);
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUIStyle.none);
            
            DrawValueText();
            DrawValueExtraText();
            DrawValueStackTrace();
            
            GUILayout.EndScrollView();
            GUILayout.EndArea();
        }

        private void DrawVariableName()
        {
            if (DrawFoldoutHeaderGroup(ref showName, Name, () => EditorGUIUtility.systemCopyBuffer = SelectedVariable.Name))
                DrawLabel(SelectedVariable.Name, Styles.InfoValueText);
        }
        
        private void DrawCustomActions()
        {
            if (SelectedVariable.RuntimeMeta.CustomActions == null
                || SelectedVariable.RuntimeMeta.CustomActions.Count == 0)
            {
                return;
            }
            
            GUILayout.Space(DistBetweenGroups);

            if (!DrawFoldoutHeaderGroup(ref showCustomActions, CustomActions)) 
                return;

            var usedWidth = 0f;
            var maxWidth = contentRect.width;
            
            EditorGUILayout.BeginHorizontal();
            
            foreach (var customAction in SelectedVariable.RuntimeMeta.CustomActions)
            {
                var content = WatchEditorServices.GUICache.GetContent(customAction.Key);
                var width = Styles.InfoCustomButton.CalcSize(content).x;
                width = Mathf.Min(width, maxWidth - 1f);
                    
                var needNewLine = usedWidth + width > maxWidth;

                if (needNewLine)
                {
                    usedWidth = 0f;
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                }
                
                if (GUILayout.Button(content, GUILayout.Width(width)))
                    customAction.Value?.Invoke();

                usedWidth += width;
            }
            
            EditorGUILayout.EndHorizontal();
        }
        
        private void DrawCreationStackTrace()
        {
            if (string.IsNullOrEmpty(SelectedVariable.RuntimeMeta.CreationStackTrace))
            {
                return;
            }

            var formattedStackTrace = FormatStackTraceString(SelectedVariable.RuntimeMeta.CreationStackTrace, 
                CreationStackTraceSkippedLines);
            
            GUILayout.Space(DistBetweenGroups);
            
            if (DrawFoldoutHeaderGroup(ref showCreationStackTrace, StackTrace, Copy))
                DrawLabel(formattedStackTrace, Styles.InfoMetaText);
            
            void Copy()
            {
                var copyingStackTrace = FormatStackTraceString(SelectedVariable.RuntimeMeta.CreationStackTrace,
                    CreationStackTraceSkippedLines, 
                    false);
                EditorGUIUtility.systemCopyBuffer = copyingStackTrace;
            }
        }
        
        private void DrawValueText()
        {
            var selectedValueText = SelectedVariable.GetValueText(SelectedColumn);

            if (string.IsNullOrWhiteSpace(selectedValueText))
            {
                return;
            }

            if (DrawFoldoutHeaderGroup(ref showValue,Value, () => EditorGUIUtility.systemCopyBuffer = selectedValueText))
                DrawLabel(selectedValueText, Styles.InfoValueText);
        }

        private void DrawValueExtraText()
        {
            if (SelectedVariable.RuntimeMeta.ExtraTexts == null
                || SelectedVariable.RuntimeMeta.ExtraTexts.Count == 0)
            {
                return;
            }

            var selectedExtraText = SelectedVariable.RuntimeMeta.ExtraTexts[SelectedColumn];
            
            if (string.IsNullOrWhiteSpace(selectedExtraText))
            {
                return;
            }
            
            GUILayout.Space(DistBetweenGroups);
            
            if (DrawFoldoutHeaderGroup(ref showExtraText, ExtraText, () => EditorGUIUtility.systemCopyBuffer = selectedExtraText))
                DrawLabel(selectedExtraText, Styles.InfoMetaText);
        }

        private void DrawValueStackTrace()
        {
            if (SelectedVariable.RuntimeMeta.StackTraces == null
                || SelectedVariable.RuntimeMeta.StackTraces.Count == 0)
            {
                return;
            }
            
            var selectedStackTrace = SelectedVariable.RuntimeMeta.StackTraces[SelectedColumn];

            if (string.IsNullOrWhiteSpace(selectedStackTrace))
            {
                return;
            }

            var formattedStackTrace = FormatStackTraceString(selectedStackTrace, 
                ValueStackTraceSkippedLines);
            
            GUILayout.Space(DistBetweenGroups);
            
            if (DrawFoldoutHeaderGroup(ref showStackTrace, StackTrace, Copy))
                DrawLabel(formattedStackTrace, Styles.InfoMetaText);

            void Copy()
            {
                var copyingStackTrace = FormatStackTraceString(selectedStackTrace, 
                    ValueStackTraceSkippedLines,
                    false);
                EditorGUIUtility.systemCopyBuffer = copyingStackTrace;
            }
        }
        
        private void DrawLabel(string text, GUIStyle textStyle)
        {
            var content = WatchEditorServices.GUICache.GetContent(text);
            var estimatedHeight = textStyle.CalcHeight(content, contentRect.width);
            
            EditorGUILayout.SelectableLabel(
                text,   
                textStyle, 
                GUILayout.ExpandWidth(true), 
                GUILayout.ExpandHeight(false),
                GUILayout.Height(estimatedHeight));
        }

        private bool DrawFoldoutHeaderGroup(ref bool foldout, string title, Action onCopy = null)
        {
            var content = WatchEditorServices.GUICache.GetContent(title);
            var headerRect = GUILayoutUtility.GetRect(content, Styles.InfoHeaderFoldout)
                .Extrude(ExtrudeFlags.Top | ExtrudeFlags.Bottom, 1);
            var buttonCopyRect = headerRect.CropFromStartToPosition(CropEdge.RightLocal, Constants.InfoHeaderCopyButtonWidth);
            
            EditorGUI.DrawRect(headerRect, Colors.BackgroundOdd);
            foldout = EditorGUI.Foldout(headerRect, foldout, content, Styles.InfoHeaderFoldout);

            if (onCopy != null &&GUI.Button(buttonCopyRect, WatchEditorServices.GUICache.GetContent("COPY"), Styles.InfoCopyButton))
            {
                onCopy();
            }
            
            return foldout;
        }

        private string FormatStackTraceString(string stacktraceText, int skipLinesCount = 0, bool includeRefs = true)
        {
            var stringBuilder = new StringBuilder();
            var strArray = stacktraceText.Split(new [] { "\n" }, StringSplitOptions.None);
            
            for (var index = 0; index < strArray.Length; ++index)
            {
                if (index < skipLinesCount)
                    continue;

                if (includeRefs)
                {
                    var str1 = ") (at ";
                    var num1 = strArray[index].IndexOf(str1, StringComparison.Ordinal);

                    if (num1 > 0)
                    {
                        var num2 = num1 + str1.Length;

                        if (strArray[index][num2] != '<')
                        {
                            var str2 = strArray[index].Substring(num2);
                            var length = str2.LastIndexOf(":", StringComparison.Ordinal);

                            if (length > 0)
                            {
                                var num3 = str2.LastIndexOf(")", StringComparison.Ordinal);

                                if (num3 > 0)
                                {
                                    var str3 = str2.Substring(length + 1, num3 - (length + 1));
                                    var str4 = str2.Substring(0, length);

                                    stringBuilder.Append(strArray[index].Substring(0, num2));
                                    stringBuilder.Append("<a href=\"" + str4 + "\" line=\"" + str3 + "\">");
                                    stringBuilder.Append(str4 + ":" + str3);
                                    stringBuilder.Append("</a>)\n");
                                    continue;
                                }
                            }
                        }
                    }
                }

                stringBuilder.Append(strArray[index] + "\n");
            }
            
            if (stringBuilder.Length > 0)
                stringBuilder.Remove(stringBuilder.Length - 1, 1);
            
            return stringBuilder.ToString();
        }
    }
}