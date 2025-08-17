using System.Collections.Generic;

namespace BE.Observer
{
    public interface IIdiomaObserver
    {
        void OnIdiomaChanged(IReadOnlyDictionary<string, string> textos);
    }
}
