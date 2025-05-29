using System;
using System.Runtime.Serialization;
using System.Web.Services.Description;

namespace TIF.UI
{
    [Serializable]
    internal class UsuarioInvalidoException : Exception
    {
        private readonly string _customMessage;

        public override string Message => _customMessage;

        public UsuarioInvalidoException()
        {
            _customMessage = "Usuario o password incorrectos.";
        }
    }
}