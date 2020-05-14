namespace Konane.Game
{
    /// <summary> Interface of coordinate entity </summary>
    public interface ICoordinateEntity<T> where T : ICoordinateData
    {
        T Data { get; }

        Coordinate Coordinate { get; }

        void Init(T data);
    }
}