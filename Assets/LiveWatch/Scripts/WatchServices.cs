namespace Ingvar.LiveWatch
{
    public static class WatchServices
    {
        public static WatchReferenceCreator ReferenceCreator { get; set; } = new();
        public static WatchVariableCreator VariableCreator { get; set; } = new();
        public static WatchVariableUpdater VariableUpdater { get; set; } = new();
        public static WatchCachedNamesBuilder NameBuilder { get; set; } = new();
        public static WatchConditionalFormatUpdater ValueFormatUpdater { get; set; } = new();
        public static WatchExtraTextUpdater ExtraTextUpdater { get; set; } = new();
        public static WatchStackTraceUpdater StackTraceUpdater { get; set; } = new();
        public static WatchVariableSortUpdater VariableSortUpdater { get; set; } = new();
        public static WatchSaveLoaderBase SaveLoader { get; set; } = new BinaryWriterWatchSaveLoader();
    }
}