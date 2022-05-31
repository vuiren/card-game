using UnityEngine;

namespace Services
{
    public interface IActorsIdService
    {
        int GetLastId();
        int GetNewId();
    }

    public class ActorsIdService : MonoBehaviour, IActorsIdService
    {
        private int _lastId = -1;

        public int GetLastId()
        {
            return _lastId;
        }

        public int GetNewId()
        {
            _lastId++;
            return _lastId;
        }
    }
}