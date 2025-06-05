using System;
using System.Runtime.Serialization;

namespace TIF.UI
{
    [Serializable]
    internal class UsuarioBloqueadoException : Exception
    {
        private readonly string _customMessage;

        public override string Message => _customMessage;

        public UsuarioBloqueadoException()
        {
            _customMessage = "El usuario se encuentra bloqueado. Por favor, contacte al administrador del sistema.";
        }

    }
}