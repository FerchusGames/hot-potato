using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Ingvar.LiveWatch.Editor
{
    public class ToolbarGUI
    {
        public bool Search { get; set; }
        public bool Live
        {
            get => Watch.IsLive;
            set => Watch.IsLive = value;
        }
        public bool Collapse 
        {
            get => WatchStorageSO.instance.Collapse;
            set => WatchStorageSO.instance.Collapse = value;
        }

        public WatchStorage Watches
        {
            get => WatchStorageSO.instance.Watches;
            set
            {
                Watch.DestroyAll();
                WatchStorageSO.instance.Watches = value;
            }
        }
        
        public Rect WindowRect { get; set; }
        private Rect areaRect;
        private float xOffsetRight;
        private float xOffsetLeft;
        private GUIContent saveButtonContent;
        private GUIContent loadButtonContent;
        private GUIContent optionsButtonContent;
        private List<WatchVariable> variables = new();

        public void OnGUI(Rect areaRect)
        {
            this.areaRect = areaRect;
            GUI.Label(areaRect, string.Empty, EditorStyles.toolbar);

            xOffsetLeft = 0;
            DoSearchButton(CropEdge.LeftLocal, ref xOffsetLeft);

            var centerBlockWidth = Constants.ToolbarLiveButtonWidth + Constants.ToolbarCollapseButtonWidth + Constants.ToolbarClearButtonWidth;
            xOffsetLeft = areaRect.width/2 - centerBlockWidth/2;
            DoLiveButton(CropEdge.LeftLocal, ref xOffsetLeft);
            DoCollapseButton(CropEdge.LeftLocal, ref xOffsetLeft);
            DoClearButton(CropEdge.LeftLocal, ref xOffsetLeft);
            
            xOffsetRight = 0;
            DoOptionsButton(CropEdge.RightLocal, ref xOffsetRight);
            DoSaveButton(CropEdge.RightLocal, ref xOffsetRight);
            DoLoadButton(CropEdge.RightLocal, ref xOffsetRight);
            DoViewSettingsButton(CropEdge.RightLocal, ref xOffsetRight);
        }

        private void DoSaveButton(CropEdge edge, ref float offset)
        {
            saveButtonContent ??= EditorGUIUtility.TrIconContent("SaveAs", "Save watches to a binary file");
            var rect = areaRect.CropFromPositionWithSize(edge, offset,  Constants.ToolbarSaveButtonWidth);

            GUI.enabled = !WatchServices.SaveLoader.IsSaving && !WatchServices.SaveLoader.IsLoading && Watches.Count > 0;
            
            if (GUI.Button(rect, saveButtonContent, EditorStyles.toolbarButton))
            {
                var filePath = EditorUtility.SaveFilePanel(
                    "Save watches", 
                    string.Empty, 
                    $"Watches.{WatchServices.SaveLoader.Extension}", 
                    WatchServices.SaveLoader.Extension);
                
                if (string.IsNullOrWhiteSpace(filePath))
                    return;
                
                WatchServices.SaveLoader.Save(filePath, Watches, (succeed, _) =>
                {
                    var success = succeed;
                    
                    MainThreadDispatcher.Dispatch(() =>
                    {
                        EditorUtility.ClearProgressBar();
                        
                        if (success)
                            EditorUtility.RevealInFinder(filePath);
                    });
                });
            }
            GUI.enabled = true;
            
            offset += Constants.ToolbarSaveButtonWidth;
        }

        private void DoLoadButton(CropEdge edge, ref float offset)
        {
            loadButtonContent ??= EditorGUIUtility.TrIconContent("Profiler.Open", "Load watches from a binary file");
            var rect = areaRect.CropFromPositionWithSize(edge, offset, Constants.ToolbarLoadButtonWidth);
            
            GUI.enabled = !WatchServices.SaveLoader.IsSaving && !WatchServices.SaveLoader.IsLoading;

            if (GUI.Button(rect, loadButtonContent, EditorStyles.toolbarButton))
            {
                var filePath = EditorUtility.OpenFilePanel("Load watches",
                    string.Empty,
                    WatchServices.SaveLoader.Extension);
                
                if (string.IsNullOrWhiteSpace(filePath))
                    return;
                
                WatchServices.SaveLoader.Load(filePath, (succeed, watches, _) =>
                {
                    var success = succeed;

                    MainThreadDispatcher.Dispatch(() =>
                    {
                        EditorUtility.ClearProgressBar();

                        if (success)
                            Watches = watches;
                    });
                });
            }

            GUI.enabled = true;
            
            offset += Constants.ToolbarLoadButtonWidth;
        }

        private void DoOptionsButton(CropEdge edge, ref float offset)
        {
            optionsButtonContent ??= EditorGUIUtility.TrIconContent("_Menu", "Extra options");
            var rect = areaRect.CropFromPositionWithSize(edge, offset, Constants.ToolbarOptionsButtonWidth);
            
            if (GUI.Button(rect, optionsButtonContent, EditorStyles.toolbarButton))
            {
                GenericMenu genericMenu = new GenericMenu();
                
                genericMenu.AddItem(new GUIContent(
                        "Preferences", "Go to Live Watch preferences"), 
                    false, 
                    new GenericMenu.MenuFunction(OpenPreferences));
                genericMenu.AddItem(new GUIContent(
                        "Collapse all rows", "Collapses all watched variables recursively"), 
                    false, 
                    new GenericMenu.MenuFunction(CollapseAll));
                genericMenu.AddItem(new GUIContent(
                        "Generate empty all", "Calls GenerateEmpty() on all generators in project"), 
                    false, 
                    new GenericMenu.MenuFunction(WatchGenerationManager.GenerateEmptyAll));
                genericMenu.AddItem(new GUIContent(
                        "Generate all", "Calls Generate() on all generators in project"), 
                    false, 
                    new GenericMenu.MenuFunction(WatchGenerationManager.GenerateAll));
                
                genericMenu.DropDown(rect);
            }
            
            offset += Constants.ToolbarOptionsButtonWidth;
            
            void OpenPreferences()
            {
                SettingsService.OpenUserPreferences("Preferences/Live Watch");
            }

            void CollapseAll()
            {
                Watches.GetAllChildRecursive(variables, WatchFilters.NoChilds, WatchFilters.None);

                foreach (var variable in variables)
                    variable.EditorMeta.IsExpanded = false;
            }
        }
        
        private void DoSearchButton(CropEdge edge, ref float offset)
        {
            var rect = areaRect.CropFromPositionWithSize(edge, offset, Constants.ToolbarSearchButtonWidth);
            var content = new GUIContent("Search", Search ? "Close search menu" : "Open search menu");
            
            Search = GUI.Toggle(rect, Search, content, EditorStyles.toolbarButton);
            
            offset += Constants.ToolbarSearchButtonWidth;
        }

        private void DoViewSettingsButton(CropEdge edge, ref float offset)
        {
            var rect = areaRect.CropFromPositionWithSize(edge, offset, Constants.ToolbarViewButtonWidth);
            var content = new GUIContent("View");
            
            if (GUI.Button(rect, content, EditorStyles.toolbarDropDown))
            {
                var buttonWorldRect = new Rect(rect.x + WindowRect.x, rect.yMax + WindowRect.y, rect.width, rect.height);
                CellSettingsDropdownWindow.Create(buttonWorldRect);
            }
            
            offset += Constants.ToolbarViewButtonWidth;
        }
        
        private void DoClearButton(CropEdge edge, ref float offset)
        {
            var rect = areaRect.CropFromPositionWithSize(edge, offset, Constants.ToolbarClearButtonWidth);
            var content = new GUIContent("Clear", "Clear all watch values");
            
            if (GUI.Button(rect, content, EditorStyles.toolbarButton))
            {
                Watch.ClearAll();
            }
            
            offset += Constants.ToolbarClearButtonWidth;
        }
        
        private void DoCollapseButton(CropEdge edge, ref float offset)
        {
            var rect = areaRect.CropFromPositionWithSize(edge, offset, Constants.ToolbarCollapseButtonWidth);
            var content = new GUIContent("Collapse", Collapse ? "Show columns without unique values" : "Hide columns without unique values");
            
            Collapse = GUI.Toggle(rect, Collapse, content, EditorStyles.toolbarButton);
            
            offset += Constants.ToolbarCollapseButtonWidth;
        }

        private void DoLiveButton(CropEdge edge, ref float offset)
        {
            var rect = areaRect.CropFromPositionWithSize(edge, offset, Constants.ToolbarLiveButtonWidth);
            var content = new GUIContent("Live", Live ? "Watches are recording. Click to pause" : "Watches are not recording. Click to unpause");
            
            Live = GUI.Toggle(rect, Live, content, EditorStyles.toolbarButton);
            
            offset += Constants.ToolbarLiveButtonWidth;
        }
    }
}