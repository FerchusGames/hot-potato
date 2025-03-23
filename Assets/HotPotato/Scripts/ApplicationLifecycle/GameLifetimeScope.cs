using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace HotPotato.ApplicationLifecycle
{
    public class GameLifetimeScope : LifetimeScope
    {
        [Required]
        [SerializeField] private ApplicationManager _applicationManager;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(_applicationManager);
        }
    }
}