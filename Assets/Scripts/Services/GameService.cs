namespace Services
{
    public interface IGameService
    {
        bool IsHostReady();
        void SetHostReady(bool ready);
        void SetCardsInHandsCount(int count);
        int GetCardsInHandsCount();
        void DeleteGameData();
    }
}