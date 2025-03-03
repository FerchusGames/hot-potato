using HotPotato.Managers;
using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace HotPotato.ApplicationLifecycle
{
    public class ApplicationController : LifetimeScope
    {
        [Required]
        [SerializeField] private GameManager _gameManager;

        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);
            
            builder.RegisterComponent(_gameManager).As<IGameManager>();
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}