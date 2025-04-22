using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using Descriptor = Ingvar.LiveWatch.Generation.WatchVariableDescriptorUtils;

namespace Ingvar.LiveWatch.Generation
{
    public partial class WatchGenerator
    {
        private HashSet<string> ignoreNamespaces = new()
        {
            "System.Security",
            "System.Reflection"
        };

        private HashSet<Type> ignoreClasses = new()
        {
            typeof(object),
            typeof(Type)
        };

        private HashSet<Type> ignoreClassMembers = new()
        {
            typeof(object),
            typeof(Type),
            typeof(IDictionary),
            typeof(IEnumerator<>),
            typeof(IEnumerable<>),
            typeof(ICollection<>),
            typeof(IReadOnlyCollection<>),
            typeof(ISet<>),
            typeof(IDictionary<,>),
            typeof(Array),
            typeof(Hashtable),
            typeof(List<>),
            typeof(HashSet<>),
            typeof(Queue<>),
            typeof(Stack<>),
            typeof(LinkedList<>),
            typeof(SortedList),
            typeof(ConcurrentBag<>),
            typeof(ConcurrentQueue<>),
            typeof(ConcurrentStack<>),
            typeof(Dictionary<,>),
            typeof(SortedDictionary<,>),
            typeof(ConcurrentDictionary<,>),
        };
        
        private List<ChildInfo> GetChildren(WatchVariableDescriptor descriptor, HashSet<Type> typesToAdd)
        {
            var childs = new List<ChildInfo>();
            
            var isDictionary = IsDictionary(Descriptor.TargetType(descriptor));
            var isCollection = IsCollection(Descriptor.TargetType(descriptor));
            
            if (IsPrimitiveType(Descriptor.TargetType(descriptor), out _) || Descriptor.TargetType(descriptor).IsEnum)
                return childs;

            var allMembers = GetMembersFromType(Descriptor.TargetType(descriptor), MemberType.All);

            ValidateAndLogWrongMembers(descriptor, allMembers);
            
            foreach (var member in allMembers)
            {
                var memberName = member.Name;
                var memberType = GetMemberUnderlyingType(member);
                
                //Ignore namespaces
                if (ignoreNamespaces.Contains(memberType.Namespace))
                    continue;
                
                //Ignore classes
                if (ignoreClasses.Contains(memberType))
                {
                    if (memberName is not ("GetType" or "Clone" or "SyncRoot"))
                        Debug.LogWarning($"Type '{memberType.Name}' is not supported by generator. Member '{memberName}' will be ignored. Parent type: {Descriptor.TargetType(descriptor).Name}");
                    
                    continue;
                }

                //Ignore member names
                if (ignoreClassMembers.Any(type => IsMemberDeclaredInType(member, type)))
                {
                    if (!GenerateCountForCollections || memberName != "Count" && memberName != "Length")
                        continue;
                }

                // Always shown members check
                if (Descriptor.AllowedMembers(descriptor).Contains(memberName))
                {
                    AddMemberToChilds(memberName, memberType, member);
                    continue;
                }
                
                //Ignore member types
                if (!IsMemberHasRightMemberType(member, Descriptor.MemberTypeMask(descriptor)))
                    continue;
                
                // Ignore members check
                if (Descriptor.IgnoredMembers(descriptor).Contains(memberName))
                    continue;
                
                // Ignore classes check 
                if (Descriptor.IgnoredHierarchyClasses(descriptor).Contains(member.DeclaringType))
                    continue;

                // Self recursion check
                if (!Descriptor.AllowSelfRecursion(descriptor) && memberType == Descriptor.TargetType(descriptor))
                    continue;

                AddMemberToChilds(memberName, memberType, member);
            }

            if (isDictionary)
            {
                var keyType = GetDictionaryElementType(Descriptor.TargetType(descriptor)).key;
                var valueType = GetDictionaryElementType(Descriptor.TargetType(descriptor)).value;

                if (keyType != null && valueType != null)
                {
                    TryAddGenerationType(keyType);
                    TryAddGenerationType(valueType);
                }
                else if (keyType == null)
                    Debug.LogWarning($"Failed to get key type for dictionary type: {Descriptor.TargetType(descriptor).FullName}");
                else if (valueType == null)
                    Debug.LogWarning($"Failed to get value type for dictionary type: {Descriptor.TargetType(descriptor).FullName}");
                
            }

            if (isCollection)
            {
                var elementType = GetCollectionElementType(Descriptor.TargetType(descriptor));

                if (elementType != null)
                    TryAddGenerationType(elementType);
                else
                    Debug.LogWarning($"Failed to get element type for collection type: {Descriptor.TargetType(descriptor).FullName}");
            }

            childs = childs.OrderBy(c => c.ShownName).ToList();
            
            void AddMemberToChilds(string memberName, Type memberType, MemberInfo memberInfo)
            {
                var childShownName = Descriptor.MemberRenameDict(descriptor).TryGetValue(memberName, out var value) ? value : memberName;
                var info = new ChildInfo()
                {
                    MemberName = memberName,
                    ShownName = childShownName,
                    IsMethod = memberInfo.MemberType == MemberTypes.Method,
                    TypeString = GetFullNameForType(memberType),
                    Type = memberType
                };

                if (Descriptor.MemberSortDict(descriptor).TryGetValue(memberName, out var sortOrder))
                {
                    info.IsSortOrderSet = true;
                    info.SortOrder = sortOrder;
                }

                if (Descriptor.MemberDecimalPlacesDict(descriptor).TryGetValue(memberName, out var decimalPlaces))
                {
                    info.IsDecimalPlacesSet = true;
                    info.DecimalPlaces = decimalPlaces;
                }
                
                if (Descriptor.CollapsableMembers(descriptor).Contains(memberName))
                {
                    info.IsAlwaysCollapsable = true;
                }
                
                if (Descriptor.TraceableMembers(descriptor).Contains(memberName))
                {
                    info.IsTraceable = true;
                }

                if (Descriptor.MemberMinMaxSetupsDict(descriptor).TryGetValue(memberName, out var minMaxSetup))
                {
                    info.IsMinMaxModeSet = true;
                    info.MinMax = minMaxSetup;
                }
                
                childs.Add(info);
                
                TryAddGenerationType(memberType);
            }

            void TryAddGenerationType(Type type)
            {
                if (PrimitiveTypesMap.ContainsKey(type) || typesToAdd.Contains(type))
                    return;
                
                typesToAdd.Add(type);
            }
            
            return childs;
        }

        private void ValidateAndLogWrongMembers(WatchVariableDescriptor descriptor, List<MemberInfo> members)
        {
            foreach (var memberName in Descriptor.AllowedMembers(descriptor))
            {
                if (!members.Any(m => m.Name.Equals(memberName)))
                    Debug.LogWarning($"No member with name: {memberName} in class: {Descriptor.TargetType(descriptor)}");
            }
            
            foreach (var memberName in Descriptor.IgnoredMembers(descriptor))
            {
                if (!members.Any(m => m.Name.Equals(memberName)))
                    Debug.LogWarning($"No member with name: {memberName} in class: {Descriptor.TargetType(descriptor)}");
            }
        }
        
        private string GetFullNameForType(Type type)
        {
            if (type.IsSubclassOf(typeof(Array)) || !type.IsGenericType)
            {
                return type.FullName == null ? type.Name : type.FullName.Replace('+', '.');
            }

            var genericType = type.GetGenericTypeDefinition();
            var typeName = GetTrueTypeName(genericType.Name);

            var arguments = type.GetGenericArguments();
            var argumentStr = new StringBuilder();
            
            for(var i = 0; i < arguments.Length; i++)
            {
                argumentStr.Append(GetFullNameForType(arguments[i]));

                if (i < arguments.Length - 1)
                    argumentStr.Append(", ");
            }

            var namespaceStr = type.Namespace;

            if (type.IsNested)
            {
                namespaceStr = GetFullNameForType(type.DeclaringType);
            }
            
            return $"{namespaceStr}.{typeName}<{argumentStr}>";
            
            string GetTrueTypeName(string name)
            {
                for (var i = 0; i < name.Length; i++)
                {
                    if (char.IsDigit(name[i]) || char.IsLetter(name[i]) || char.IsSeparator(name[i])) 
                        continue;

                    return name[..i];
                }

                return name;
            }
        }
        
        private bool IsPrimitiveType(Type targetType, out Type storeType)
        {
            return PrimitiveTypesMap.TryGetValue(targetType, out storeType);
        }

        private bool IsPrimitiveTypeOrEnum(Type targetType)
        {
            return PrimitiveTypesMap.TryGetValue(targetType, out _) || targetType.IsEnum;
        }

        private bool IsGeneratedType(Type targetType)
        {
            return PrimitiveTypesMap.TryGetValue(targetType, out _) || _typeInfos.ContainsKey(targetType);
        }
        
        private bool IsDictionary(Type targetType)
        {
            return targetType.GetInterface(nameof(IDictionary)) != null || targetType.Name == "IDictionary`2";
        }
        
        private bool IsCollection(Type targetType)
        {
            return targetType.GetInterface(nameof(IEnumerable)) != null;
        }
        
        private List<MemberInfo> GetMembersFromType(Type type, MemberType memberTypeMask, MemberAccessModifier memberAccessModifierMask = MemberAccessModifier.Public)
        {
            var members = new List<MemberInfo>();

            if (memberTypeMask.HasFlag(MemberType.Method))
            {
                var methods = type.GetMethods()
                    .Where(m => !m.Attributes.HasFlag(MethodAttributes.Static)
                                && m.GetCustomAttributes(typeof(ObsoleteAttribute), false).Length <= 0
                                && m.Name != "get_Item"
                                && !m.IsSpecialName
                                && m.ReturnType != typeof(void)
                                && m.GetParameters().Length <= 0)
                    .ToList();

                members.AddRange(methods.Where(m => 
                    memberAccessModifierMask.HasFlag(MemberAccessModifier.Public) && m.Attributes.HasFlag(MethodAttributes.Public) 
                    || memberAccessModifierMask.HasFlag(MemberAccessModifier.Internal) && m.Attributes.HasFlag(MethodAttributes.Assembly) 
                    || memberAccessModifierMask.HasFlag(MemberAccessModifier.Protected) && m.Attributes.HasFlag(MethodAttributes.Family) 
                    || memberAccessModifierMask.HasFlag(MemberAccessModifier.Private) && m.Attributes.HasFlag(MethodAttributes.Private)));
            }
            
            if (memberTypeMask.HasFlag(MemberType.Property))
            {
                var properties = type.GetProperties()
                    .Where(p => !p.GetMethod.Attributes.HasFlag(MethodAttributes.Static)
                                && p.GetCustomAttributes(typeof(ObsoleteAttribute), false).Length <= 0
                                && p.GetMethod.Name != "get_Item")
                    .ToList();

                members.AddRange(properties.Where(p => 
                    memberAccessModifierMask.HasFlag(MemberAccessModifier.Public) && p.GetMethod.Attributes.HasFlag(MethodAttributes.Public) 
                    || memberAccessModifierMask.HasFlag(MemberAccessModifier.Internal) && p.GetMethod.Attributes.HasFlag(MethodAttributes.Assembly) 
                    || memberAccessModifierMask.HasFlag(MemberAccessModifier.Protected) && p.GetMethod.Attributes.HasFlag(MethodAttributes.Family) 
                    || memberAccessModifierMask.HasFlag(MemberAccessModifier.Private) && p.GetMethod.Attributes.HasFlag(MethodAttributes.Private)));
            }

            if (memberTypeMask.HasFlag(MemberType.Field))
            {
                var fields = type.GetFields()
                    .Where(f => !f.Attributes.HasFlag(FieldAttributes.Static)
                                && f.GetCustomAttributes(typeof(ObsoleteAttribute), false).Length <= 0)
                    .ToList();

                members.AddRange(fields.Where(f => 
                    memberAccessModifierMask.HasFlag(MemberAccessModifier.Public) && f.Attributes.HasFlag(FieldAttributes.Public) 
                    || memberAccessModifierMask.HasFlag(MemberAccessModifier.Internal) && f.Attributes.HasFlag(FieldAttributes.Assembly) 
                    || memberAccessModifierMask.HasFlag(MemberAccessModifier.Protected) && f.Attributes.HasFlag(FieldAttributes.Family) 
                    || memberAccessModifierMask.HasFlag(MemberAccessModifier.Private) && f.Attributes.HasFlag(FieldAttributes.Private)));
            }

            return members;
        }

        private Type GetMemberUnderlyingType(MemberInfo memberInfo)
        {
            return memberInfo.MemberType switch
            {
                MemberTypes.Field => ((FieldInfo)memberInfo).FieldType,
                MemberTypes.Method => ((MethodInfo)memberInfo).ReturnType,
                MemberTypes.Property => ((PropertyInfo)memberInfo).PropertyType,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private bool IsMemberHasRightMemberType(MemberInfo memberInfo, MemberType memberTypes)
        {
            return memberInfo.MemberType switch
            {
                MemberTypes.Field => memberTypes.HasFlag(MemberType.Field),
                MemberTypes.Method => memberTypes.HasFlag(MemberType.Method),
                MemberTypes.Property => memberTypes.HasFlag(MemberType.Property),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private bool IsMemberDeclaredInType(MemberInfo member, Type type)
        {
            var memberDeclaredType = member.DeclaringType;

            if (memberDeclaredType == null)
                return false;

            return memberDeclaredType == type 
                   || memberDeclaredType.Namespace == type.Namespace && memberDeclaredType.Name == type.Name;
        }
        
        private class ChildInfo
        {
            public bool IsMethod;
            public string MemberName;
            public string ShownName;
            public string TypeString;
            public Type Type;
            public MemberAccessModifier AccessModifier => MemberAccessModifier.Public;
            public bool IsSortOrderSet;
            public int SortOrder;
            public bool IsDecimalPlacesSet;
            public int DecimalPlaces;
            public bool IsAlwaysCollapsable;
            public bool IsTraceable;
            public bool IsMinMaxModeSet;
            public VariableMinMaxMeta MinMax;
        }
    }
}