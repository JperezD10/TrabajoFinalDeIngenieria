namespace BE.Observer
{
    public interface IIdiomaSubject
    {
        void Subscribe(IIdiomaObserver observer);
        void Unsubscribe(IIdiomaObserver observer);
        void Notify();
    }
}
