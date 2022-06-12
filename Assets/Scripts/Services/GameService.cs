namespace Services
{
    public interface IGameService
    {
        bool IsHostReady();
        void SetHostReady(bool ready);
    }
}