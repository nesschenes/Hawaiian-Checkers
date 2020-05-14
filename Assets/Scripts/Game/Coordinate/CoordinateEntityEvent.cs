using UnityEngine.Events;

namespace Konane.Game
{
    public interface ICoordinateEvent
    {
        void OnButtonDown();
    }

    public class CoordinateEntityEvent<T1, T2> : UnityEvent<T1> 
        where T1 : ICoordinateEntity<T2>
        where T2 : ICoordinateData
    {

    }
}