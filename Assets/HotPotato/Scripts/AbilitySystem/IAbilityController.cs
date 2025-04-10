namespace HotPotato.AbilitySystem
{
    public interface IAbilityController
    {
        public IAbility CurrentAbility { get; }
        public void EnableAbility();
        public void SetAbility(IAbility ability);
    }
}