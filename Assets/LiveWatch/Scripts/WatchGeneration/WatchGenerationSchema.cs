using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Ingvar.LiveWatch.Generation
{
    public abstract class WatchGenerationSchema
    {
        internal Dictionary<Type, WatchVariableDescriptor> GenerationWatches = new Dictionary<Type, WatchVariableDescriptor>();
        internal Dictionary<Type, WatchVariableDescriptor> DefineWatches = new Dictionary<Type, WatchVariableDescriptor>();
        internal Dictionary<Type, WatchVariableDescriptor> DefineWatchesInherited = new Dictionary<Type, WatchVariableDescriptor>();

        protected WatchVariableDescriptor Define<T>(bool withInheritors = true)
        {
            return Define(typeof(T), withInheritors);
        }

        protected WatchVariableDescriptor Define(Type type, bool withInheritors = true)
        {
            var dict = withInheritors ? DefineWatchesInherited : DefineWatches;
            
            if (dict.TryGetValue(type, out var watch))
                return watch;
            
            watch = new WatchVariableDescriptor(type, false);
            dict.Add(type, watch);
                
            return watch;
        }
        
        protected WatchVariableDescriptor Generate<T>()
        {
            return Generate(typeof(T));
        }
        
        protected WatchVariableDescriptor Generate(Type type)
        {
            if (GenerationWatches.TryGetValue(type, out var watch))
                return watch;
            
            watch = new WatchVariableDescriptor(type, true);
            GenerationWatches.Add(type, watch);
                
            return watch;
        }
        
        public virtual void OnDefine()
        {
            Define<Color>()
                .ShowOnlyMember(nameof(Color.r))
                .ShowOnlyMember(nameof(Color.g))
                .ShowOnlyMember(nameof(Color.b))
                .ShowOnlyMember(nameof(Color.a));
            
            Define<Vector2>()
                .ShowOnlyMember(nameof(Vector2.x))
                .ShowOnlyMember(nameof(Vector2.y));
            
            Define<Vector3>()
                .ShowOnlyMember(nameof(Vector3.x))
                .ShowOnlyMember(nameof(Vector3.y))
                .ShowOnlyMember(nameof(Vector3.z));

            Define<Vector4>()
                .ShowOnlyMember(nameof(Vector4.x))
                .ShowOnlyMember(nameof(Vector4.y))
                .ShowOnlyMember(nameof(Vector4.z))
                .ShowOnlyMember(nameof(Vector4.w));

            Define<Matrix4x4>()
                .ShowOnlyMember(nameof(Matrix4x4.m00))
                .ShowOnlyMember(nameof(Matrix4x4.m01))
                .ShowOnlyMember(nameof(Matrix4x4.m02))
                .ShowOnlyMember(nameof(Matrix4x4.m03))
                .ShowOnlyMember(nameof(Matrix4x4.m10))
                .ShowOnlyMember(nameof(Matrix4x4.m11))
                .ShowOnlyMember(nameof(Matrix4x4.m12))
                .ShowOnlyMember(nameof(Matrix4x4.m13))
                .ShowOnlyMember(nameof(Matrix4x4.m20))
                .ShowOnlyMember(nameof(Matrix4x4.m21))
                .ShowOnlyMember(nameof(Matrix4x4.m22))
                .ShowOnlyMember(nameof(Matrix4x4.m23))
                .ShowOnlyMember(nameof(Matrix4x4.m30))
                .ShowOnlyMember(nameof(Matrix4x4.m31))
                .ShowOnlyMember(nameof(Matrix4x4.m32))
                .ShowOnlyMember(nameof(Matrix4x4.m33));

            Define<Object>()
                .IgnoreAllMembersDeclaredInClass();
            
            Define<Component>()
                .IgnoreAllMembersDeclaredInClass();
            
            Define<Behaviour>()
                .IgnoreAllMembersDeclaredInClass();
            
            Define<MonoBehaviour>()
                .IgnoreAllMembersDeclaredInClass();
        }
        
        public abstract void OnGenerate();
    }
}