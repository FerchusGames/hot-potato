namespace HotPotato.Player
{
    public interface IPlayerController
    {
        public void RequestToMoveBomb();
        public void StartTurn();
        public void StartRound();
        public void Lose();
        public void WinRound();
        public void WinMatch();
        public void ResetMatchStats();
        public int WinCount { get; }
    }
}