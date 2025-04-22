using System;
using System.Collections.Generic;

namespace Ingvar.LiveWatch.Generation
{
    [Flags]
    public enum MemberAccessModifier
    {
        Public = 1 << 0,
        Internal = 1 << 1,
        Protected = 1 << 2,
        Private = 1 << 3,
        
        All = ~0
    }
    
    [Flags]
    public enum MemberType
    {
        Field = 1 << 0,
        Property = 1 << 1,
        Method = 1 << 2,
        
        All = ~0
    }
    
    public class WatchVariableDescriptor
    {
        internal readonly Type TargetType;
        internal readonly bool Generate;
        internal bool AllowSelfRecursion { get; private set; } = false;
        internal MemberType MemberTypeMask { get; private set; } = MemberType.Field | MemberType.Property;
        
        internal HashSet<Type> IgnoredHierarchyClasses = new HashSet<Type>();
        internal HashSet<Type> AllowedHierarchyClasses = new HashSet<Type>();
        
        internal HashSet<string> IgnoredMembers = new HashSet<string>();
        internal HashSet<string> AlwaysShownMembers = new HashSet<string>();
        
        internal HashSet<string> CollapsableMembers = new HashSet<string>();
        internal HashSet<string> NonCollapsableMembers = new HashSet<string>();
        
        internal HashSet<string> TraceableMembers = new HashSet<string>();
        internal HashSet<string> NonTraceableMembers = new HashSet<string>();
        
        internal Dictionary<string, int> MemberSortDict = new Dictionary<string, int>();
        internal Dictionary<string, string> MemberRenameDict = new Dictionary<string, string>();
        internal Dictionary<string, int> MemberDecimalPlacesDict = new Dictionary<string, int>();
        internal Dictionary<string, VariableMinMaxMeta> MemberMinMaxSetupsDict = new();

        internal bool IsSelfRecursionSet { get; private set; }
        internal bool IsMemberTypeMaskSet { get; private set; }

        public WatchVariableDescriptor(Type targetType, bool generate = true)
        {
            TargetType = targetType;
            Generate = generate;
        }

        public WatchVariableDescriptor Reset()
        {            
            AllowSelfRecursion = false;

            IgnoredHierarchyClasses.Clear();
            IgnoredMembers.Clear();
            AlwaysShownMembers.Clear();
            MemberRenameDict.Clear();

            IsSelfRecursionSet = false;
            IsMemberTypeMaskSet = false;
            
            return this;
        }

        public WatchVariableDescriptor SetSelfRecursion(bool allow)
        {
            AllowSelfRecursion = allow;
            
            IsSelfRecursionSet = true;
            return this;
        }
        
        public WatchVariableDescriptor SetTypeMask(MemberType mask)
        {
            MemberTypeMask = mask;

            IsMemberTypeMaskSet = true;
            return this;
        }

        public WatchVariableDescriptor IgnoreAllMembersDeclaredInClass()
        {
            IgnoreAllMembersDeclaredInClass(TargetType);
            
            return this;
        }
        
        public WatchVariableDescriptor AllowAllMembersDeclaredInClass()
        {
            AllowAllMembersDeclaredInClass(TargetType);
            
            return this;
        }
        
        public WatchVariableDescriptor IgnoreAllMembersDeclaredInClass(Type hierarchyClass)
        {
            IgnoredHierarchyClasses.Add(hierarchyClass);
            AllowedHierarchyClasses.Remove(hierarchyClass);
            
            return this;
        }

        public WatchVariableDescriptor AllowAllMembersDeclaredInClass(Type hierarchyClass)
        {
            AllowedHierarchyClasses.Add(hierarchyClass);
            IgnoredHierarchyClasses.Remove(hierarchyClass);
            
            return this;
        }

        public WatchVariableDescriptor IgnoreMember(string memberName)
        {
            IgnoredMembers.Add(memberName);
            AlwaysShownMembers.Remove(memberName);
            
            return this;
        }
        
        public WatchVariableDescriptor IgnoreMembers(params string[] memberNames)
        {
            foreach (var memberName in memberNames)
            {
                IgnoreMember(memberName);
            }
            
            return this;
        }

        public WatchVariableDescriptor AllowMember(string memberName)
        {
            AlwaysShownMembers.Add(memberName);
            IgnoredMembers.Remove(memberName);
            
            return this;
        }
        
        public WatchVariableDescriptor AllowMembers(params string[] memberNames)
        {
            foreach (var memberName in memberNames)
            {
                AllowMember(memberName);
            }
            
            return this;
        }
        
        public WatchVariableDescriptor ShowOnlyMember(string memberName)
        {
            AlwaysShownMembers.Add(memberName);
            IgnoredMembers.Remove(memberName);
            IgnoreAllMembersDeclaredInClass();
            
            return this;
        }
        
        public WatchVariableDescriptor ShowOnlyMember(string childNameReal, string childNameShown)
        {
            AlwaysShownMembers.Add(childNameReal);
            MemberRenameDict.Add(childNameReal, childNameShown);
            IgnoredMembers.Remove(childNameReal);
            
            return this;
        }

        public WatchVariableDescriptor ShowOnlyMembers(params string[] memberNames)
        {
            foreach (var memberName in memberNames)
            {
                ShowOnlyMember(memberName);
            }
            
            return this;
        }

        public WatchVariableDescriptor RenameMember(string memberNameReal, string memberNameShown)
        {
            MemberRenameDict[memberNameReal] = memberNameShown;

            return this;
        }

        public WatchVariableDescriptor SetSortOrder(string memberNameReal, int sortOrder)
        {
            MemberSortDict[memberNameReal] = sortOrder;
            
            return this;
        }

        public WatchVariableDescriptor SetDecimalPlaces(string memberNameReal, int decimalPlaces)
        {
            MemberDecimalPlacesDict[memberNameReal] = decimalPlaces;

            return this;
        }
        
        public WatchVariableDescriptor SetAlwaysCollapsable(string memberNameReal)
        {
            CollapsableMembers.Add(memberNameReal);
            NonCollapsableMembers.Remove(memberNameReal);
            
            return this;
        }
        
        public WatchVariableDescriptor SetNonCollapsable(string memberNameReal)
        {
            CollapsableMembers.Remove(memberNameReal);
            NonCollapsableMembers.Add(memberNameReal);

            return this;
        }
        
        public WatchVariableDescriptor SetTraceable(string memberNameReal)
        {
            TraceableMembers.Add(memberNameReal);
            NonTraceableMembers.Remove(memberNameReal);
            
            return this;
        }
        
        public WatchVariableDescriptor SetNonTraceable(string memberNameReal)
        {
            TraceableMembers.Remove(memberNameReal);
            NonTraceableMembers.Add(memberNameReal);
            
            return this;
        }

        public WatchVariableDescriptor SetMinMaxModeAsGlobal(string memberNameReal)
        {
            MemberMinMaxSetupsDict[memberNameReal] = VariableMinMaxMeta.Global();
            
            return this;
        }
        
        public WatchVariableDescriptor SetMinMaxModeAsCustom(string memberNameReal, double minValue, double maxValue)
        {
            MemberMinMaxSetupsDict[memberNameReal] = VariableMinMaxMeta.Custom(minValue, maxValue);
            
            return this;
        }
    }
}