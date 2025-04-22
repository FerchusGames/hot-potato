using System;
using UnityEngine;

namespace Ingvar.LiveWatch
{
    [Serializable]
    public class WatchVariable : ISerializationCallbackReceiver
    {
        public string Name;
        public WatchValueList Values = new ();
        [SerializeReference] public WatchVariable Parent;
        [SerializeReference] public WatchStorage Childs = new ();
        public VariableEditorMeta EditorMeta = new ();
        public VariableRuntimeMeta RuntimeMeta = new ();
        public bool HasChilds => Childs.Count > 0;
        public bool HasValues => Values.Count > 0;
        
        public void OnBeforeSerialize()
        {
            
        }

        public void OnAfterDeserialize()
        {
            foreach (var child in Childs.Items.Values)
            {
                child.Parent = this;
            }
        }
    }
}