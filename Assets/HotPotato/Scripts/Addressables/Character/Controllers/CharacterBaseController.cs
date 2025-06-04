using System.Threading;
using Character.Models;
using Character.Views;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Character.Controllers
{
    public class CharacterBaseController : ICharacterBaseController
    {
        private readonly ICharacterView characterView;
        private ICharacterData characterData;
        private readonly CancellationTokenRegistration cancellationTokenRegistration;
        private readonly Transform transform;

        public CharacterBaseController(ICharacterView characterView, ICharacterData characterData, CancellationToken gameToken)
        {
            this.characterView = characterView;
            this.characterData = characterData;
            
            transform = characterView.Transform;
            characterView.Color = characterData.Color;
            
            cancellationTokenRegistration = gameToken.Register(Dispose);
            
            MovementCycleTask(gameToken).Forget();
        }

        private async UniTask MovementCycleTask(CancellationToken gameToken)
        {
            while (!gameToken.IsCancellationRequested)
            {
                var direction = characterView.Direction;
                var horizontal = direction.x;

                var flipX = characterView.FlipSprite;
                flipX = horizontal < 0 || !(horizontal > 0) && flipX;
                characterView.FlipSprite = flipX;
                
                await UniTask.NextFrame();
            }
        }
        
        public void Dispose()
        {
            cancellationTokenRegistration.Dispose();
        }
    }
}