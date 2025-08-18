using System.Collections.Generic;
using System.Linq;

namespace BE
{
    public sealed class TablaCheckResult
    {
        public TablaCheckResult()
        {
            IdsCorruptos = new List<int>();
        }

        public string Tabla { get; set; }
        public int TotalRegistros { get; set; }
        public List<int> IdsCorruptos { get; private set; }
        public string Error { get; set; }

        public IEnumerable<string> Mensajes
        {
            get
            {
                if (!string.IsNullOrEmpty(Error))
                {
                    yield return Error;
                    yield break;
                }

                if (IdsCorruptos.Count == 0)
                    yield return string.Format("{0}: OK ({1} registros)", Tabla, TotalRegistros);
                else
                    foreach (var id in IdsCorruptos)
                        yield return string.Format("{0}: registro {1} corrupto", Tabla, id);
            }
        }
    }
}
