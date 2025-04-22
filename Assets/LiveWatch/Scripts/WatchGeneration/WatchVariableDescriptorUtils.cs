using System;
using System.Collections.Generic;

namespace Ingvar.LiveWatch.Generation
{
    public class WatchVariableDescriptorUtils
    {
        public static Type TargetType(WatchVariableDescriptor descriptor)
        {
            return descriptor.TargetType;
        }
        
        public static bool Generate(WatchVariableDescriptor descriptor)
        {
            return descriptor.Generate;
        }
        
        public static bool AllowSelfRecursion(WatchVariableDescriptor descriptor)
        {
            return descriptor.AllowSelfRecursion;
        }
        
        public static MemberType MemberTypeMask(WatchVariableDescriptor descriptor)
        {
            return descriptor.MemberTypeMask;
        }
        
        public static HashSet<Type> IgnoredHierarchyClasses(WatchVariableDescriptor descriptor)
        {
            return descriptor.IgnoredHierarchyClasses;
        }
        
        public static HashSet<Type> AllowedHierarchyClasses(WatchVariableDescriptor descriptor)
        {
            return descriptor.AllowedHierarchyClasses;
        }
        
        public static HashSet<string> IgnoredMembers(WatchVariableDescriptor descriptor)
        {
            return descriptor.IgnoredMembers;
        }
        
        public static HashSet<string> AllowedMembers(WatchVariableDescriptor descriptor)
        {
            return descriptor.AlwaysShownMembers;
        }

        public static Dictionary<string, int> MemberSortDict(WatchVariableDescriptor descriptor)
        {
            return descriptor.MemberSortDict;
        }
        
        public static Dictionary<string, int> MemberDecimalPlacesDict(WatchVariableDescriptor descriptor)
        {
            return descriptor.MemberDecimalPlacesDict;
        }
        
        public static Dictionary<string, string> MemberRenameDict(WatchVariableDescriptor descriptor)
        {
            return descriptor.MemberRenameDict;
        }
        
        public static bool IsSelfRecursionSet(WatchVariableDescriptor descriptor)
        {
            return descriptor.IsSelfRecursionSet;
        }
        
        public static bool IsMemberTypeMaskSet(WatchVariableDescriptor descriptor)
        {
            return descriptor.IsMemberTypeMaskSet;
        }
        
        public static HashSet<string> CollapsableMembers(WatchVariableDescriptor descriptor)
        {
            return descriptor.CollapsableMembers;
        }
        
        public static HashSet<string> NonCollapsableMembers(WatchVariableDescriptor descriptor)
        {
            return descriptor.NonCollapsableMembers;
        }
        
        public static HashSet<string> TraceableMembers(WatchVariableDescriptor descriptor)
        {
            return descriptor.TraceableMembers;
        }
        
        public static HashSet<string> NonTraceableMembers(WatchVariableDescriptor descriptor)
        {
            return descriptor.NonTraceableMembers;
        }
        
        public static Dictionary<string, VariableMinMaxMeta> MemberMinMaxSetupsDict(WatchVariableDescriptor descriptor)
        {
            return descriptor.MemberMinMaxSetupsDict;
        }
    }
}