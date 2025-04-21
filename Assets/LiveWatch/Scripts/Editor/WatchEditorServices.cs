using Ingvar.LiveWatch.Generation;

namespace Ingvar.LiveWatch.Editor
{
    public static class WatchEditorServices
    {
        public static WatchSearchEngine SearchEngine { get; } = new ();
        public static WatchGenerationSchemaParser GenerationSchemaParser { get; set; } = new ();
        public static WatchGenerationDescriptorMerger GenerationDescriptorMerger { get; set; } = new ();
        public static WatchGenerator Generator { get; set; } = new ();
        public static WatchGUICache GUICache { get; set; } = new ();
        public static WatchPreviewDrawer PreviewDrawer { get; set; } = new ();
        public static WatchCellDividerDrawer CellDividerDrawer { get; set; } = new ();
    }
}