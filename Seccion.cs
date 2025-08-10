using System;

namespace Cerene_App
{
    public class Seccion : IEquatable<Seccion>
    {
        private string nombre = "General";

        public string Nombre
        {
            get => nombre;
            set => nombre = string.IsNullOrWhiteSpace(value) ? "General" : value;
        }

        public override string ToString() => Nombre;

        public bool Equals(Seccion? other)
        {
            if (other is null) return false;
            return string.Equals(Nombre, other.Nombre, StringComparison.Ordinal);
        }

        public override bool Equals(object? obj) => obj is Seccion other && Equals(other);

        public override int GetHashCode() => Nombre.GetHashCode();
    }
}
