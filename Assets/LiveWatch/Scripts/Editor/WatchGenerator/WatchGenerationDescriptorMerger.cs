using System;
using System.Collections.Generic;
using System.Linq;
using Descriptor = Ingvar.LiveWatch.Generation.WatchVariableDescriptorUtils;
using Schema = Ingvar.LiveWatch.Generation.WatchGenerationSchemaUtils;

namespace Ingvar.LiveWatch.Generation
{
    public class WatchGenerationDescriptorMerger
    {
        public WatchVariableDescriptor GetMergedDescriptorFromSchema(Type targetType, WatchGenerationSchema schema)
        {
            var descriptorsGenerationSorted = GetRelatedDescriptorsForType(targetType, schema);
            
            if (descriptorsGenerationSorted.All(d => Descriptor.TargetType(d) != targetType))
                descriptorsGenerationSorted.Add(new WatchVariableDescriptor(targetType));
            
            return MergeSortedDescriptorsIntoOne(targetType, true, descriptorsGenerationSorted);
        }
        
        private WatchVariableDescriptor MergeSortedDescriptorsIntoOne(Type targetType, bool generate, List<WatchVariableDescriptor> descriptors)
        {
            var result = descriptors[0];

            for (var i = 1; i < descriptors.Count; i++)
            {
                result = MergeTwoDescriptors(targetType, generate, result, descriptors[i]);
            }

            return result;
        }

        private WatchVariableDescriptor MergeTwoDescriptors(Type targetType, bool generate, WatchVariableDescriptor ancestor, WatchVariableDescriptor descendant)
        {
            var resultDescriptor = new WatchVariableDescriptor(targetType);

            if (Descriptor.IsSelfRecursionSet(descendant))
                resultDescriptor.SetSelfRecursion(Descriptor.AllowSelfRecursion(descendant));
            else if (Descriptor.IsSelfRecursionSet(ancestor))
                resultDescriptor.SetSelfRecursion(Descriptor.AllowSelfRecursion(ancestor));
            
            if (Descriptor.IsMemberTypeMaskSet(descendant))
                resultDescriptor.SetTypeMask(Descriptor.MemberTypeMask(descendant));
            else if (Descriptor.IsMemberTypeMaskSet(ancestor))
                resultDescriptor.SetTypeMask(Descriptor.MemberTypeMask(ancestor));

            foreach (var ignoredHierarchyClass in Descriptor.IgnoredHierarchyClasses(descendant))
                resultDescriptor.IgnoreAllMembersDeclaredInClass(ignoredHierarchyClass);
            foreach (var ignoredHierarchyClass in Descriptor.IgnoredHierarchyClasses(ancestor))
                if (!Descriptor.AllowedHierarchyClasses(descendant).Contains(ignoredHierarchyClass))
                    resultDescriptor.IgnoreAllMembersDeclaredInClass(ignoredHierarchyClass);
            
            foreach (var allowedHierarchyClass in Descriptor.AllowedHierarchyClasses(descendant))
                resultDescriptor.AllowAllMembersDeclaredInClass(allowedHierarchyClass);
            foreach (var allowedHierarchyClass in Descriptor.AllowedHierarchyClasses(ancestor))
                if (!Descriptor.IgnoredHierarchyClasses(descendant).Contains(allowedHierarchyClass))
                    resultDescriptor.AllowAllMembersDeclaredInClass(allowedHierarchyClass);
            
            resultDescriptor.IgnoreMembers(Descriptor.IgnoredMembers(descendant).ToArray());
            foreach (var ignoredMember in Descriptor.IgnoredMembers(ancestor))
                if (!Descriptor.AllowedMembers(descendant).Contains(ignoredMember))
                    resultDescriptor.IgnoreMember(ignoredMember);

            resultDescriptor.AllowMembers(Descriptor.AllowedMembers(descendant).ToArray());
            foreach (var showAlwaysMembers in Descriptor.AllowedMembers(ancestor))
                if (!Descriptor.IgnoredMembers(descendant).Contains(showAlwaysMembers))
                    resultDescriptor.AllowMember(showAlwaysMembers);

            foreach (var nonCollapsableMember in Descriptor.NonCollapsableMembers(descendant))
                resultDescriptor.SetNonCollapsable(nonCollapsableMember);
            foreach (var nonCollapsableMember in Descriptor.NonCollapsableMembers(ancestor))
                if (!Descriptor.CollapsableMembers(descendant).Contains(nonCollapsableMember))
                    resultDescriptor.SetNonCollapsable(nonCollapsableMember);
            
            foreach (var collapsableMember in Descriptor.CollapsableMembers(descendant))
                resultDescriptor.SetAlwaysCollapsable(collapsableMember);
            foreach (var collapsableMember in Descriptor.CollapsableMembers(ancestor))
                if (!Descriptor.NonCollapsableMembers(descendant).Contains(collapsableMember))
                    resultDescriptor.SetAlwaysCollapsable(collapsableMember);
            
            foreach (var sortedMember in Descriptor.MemberSortDict(ancestor))
                Descriptor.MemberSortDict(resultDescriptor)[sortedMember.Key] = sortedMember.Value;
            foreach (var sortedMember in Descriptor.MemberSortDict(descendant))
                Descriptor.MemberSortDict(resultDescriptor)[sortedMember.Key] = sortedMember.Value;
            
            foreach (var decimalMember in Descriptor.MemberDecimalPlacesDict(ancestor))
                Descriptor.MemberDecimalPlacesDict(resultDescriptor)[decimalMember.Key] = decimalMember.Value;
            foreach (var decimalMember in Descriptor.MemberDecimalPlacesDict(descendant))
                Descriptor.MemberDecimalPlacesDict(resultDescriptor)[decimalMember.Key] = decimalMember.Value;
            
            foreach (var renamedMember in Descriptor.MemberRenameDict(ancestor))
                resultDescriptor.RenameMember(renamedMember.Key, renamedMember.Value);
            foreach (var renamedMember in Descriptor.MemberRenameDict(descendant))
                resultDescriptor.RenameMember(renamedMember.Key, renamedMember.Value);

            foreach (var traceableMember in Descriptor.TraceableMembers(descendant))
                resultDescriptor.SetTraceable(traceableMember);
            foreach (var traceableMember in Descriptor.TraceableMembers(ancestor))
                if (!Descriptor.NonTraceableMembers(descendant).Contains(traceableMember))
                    resultDescriptor.SetTraceable(traceableMember);
            
            foreach (var minMaxedMember in Descriptor.MemberMinMaxSetupsDict(ancestor))
                Descriptor.MemberMinMaxSetupsDict(resultDescriptor)[minMaxedMember.Key] = minMaxedMember.Value;
            foreach (var minMaxedMember in Descriptor.MemberMinMaxSetupsDict(descendant))
                Descriptor.MemberMinMaxSetupsDict(resultDescriptor)[minMaxedMember.Key] = minMaxedMember.Value;
            
            return resultDescriptor;
        }
        
        private List<WatchVariableDescriptor> GetRelatedDescriptorsForType(Type targetType, WatchGenerationSchema schema)
        {
            var list = new List<WatchVariableDescriptor>();
            
            foreach (var descriptor in Schema.DefineWatchesInherited(schema))
            {
                if (targetType == descriptor.Key || targetType.IsSubclassOf(descriptor.Key))
                    list.Add(descriptor.Value);
            }
            
            foreach (var descriptor in Schema.DefineWatches(schema))
            {
                if (targetType == descriptor.Key)
                    list.Add(descriptor.Value);
            }
            
            foreach (var descriptor in Schema.GenerationWatches(schema))
            {
                if (targetType == descriptor.Key)
                    list.Add(descriptor.Value);
            }

            list.Sort(new DescriptorInheritanceComparer());

            return list;
        }
        
        private class DescriptorInheritanceComparer : IComparer<WatchVariableDescriptor>
        {
            public int Compare(WatchVariableDescriptor x, WatchVariableDescriptor y)
            {
                if (Descriptor.TargetType(x) == Descriptor.TargetType(y))
                    return 0;

                if (Descriptor.TargetType(x).IsSubclassOf(Descriptor.TargetType(y)))
                    return 1;

                return -1;
            }
        }
    }
}