using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace BE.Observer
{
    public enum Idioma { ES, EN }

    public sealed class IdiomaManager : IIdiomaSubject
    {
        private static readonly Lazy<IdiomaManager> _inst = new Lazy<IdiomaManager>(() => new IdiomaManager());
        public static IdiomaManager Instance => _inst.Value;

        private readonly List<IIdiomaObserver> _observers = new List<IIdiomaObserver>();
        private IReadOnlyDictionary<string, string> _textos = new Dictionary<string, string>();
        public Idioma IdiomaActual { get; private set; } = Idioma.ES;

        private IdiomaManager() { CargarTextos(IdiomaActual); }

        public void SetIdioma(Idioma idioma)
        {
            if (IdiomaActual == idioma) return;
            IdiomaActual = idioma;
            CargarTextos(idioma);
            Notify();
        }

        public string T(string key) => _textos.TryGetValue(key, out var val) ? val : $"[{key}]";

        public void Subscribe(IIdiomaObserver observer)
        {
            if (!_observers.Contains(observer)) _observers.Add(observer);
            observer.OnIdiomaChanged(_textos); // push inicial
        }

        public void Unsubscribe(IIdiomaObserver observer) => _observers.Remove(observer);

        public void Notify()
        {
            foreach (var o in _observers.ToArray())
                o.OnIdiomaChanged(_textos);
        }

        private void CargarTextos(Idioma idioma)
        {
            string code = (idioma == Idioma.ES) ? "es" : "en";
            string path = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/i18n/" + code + ".json");

            string json = System.IO.File.ReadAllText(path);

            var serializer = new JavaScriptSerializer();
            _textos = serializer.Deserialize<Dictionary<string, string>>(json);
        }
    }

}
