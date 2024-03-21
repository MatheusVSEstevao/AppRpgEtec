using AppRpgEtec.Models;
using AppRpgEtec.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppRpgEtec.Services.Usuarios
{
    public class UsuarioService : Request
    {
        private readonly Request _request;
        private const string apiUrlBase = "https://rpgapi20241pam.azurewebsites.net/Usuarios";

        public UsuarioService()
        {
            _request = new Request();
        }
        public async Task<Usuario> PostRegistrarUsuarioAsync(Usuario usuario)
        {
            string uriComplementar = "/Registrar";
            usuario.Id = await _request.PostReturnIntAsync(apiUrlBase + uriComplementar, usuario);
            return usuario;
        }

        public async Task<Usuario> PostAutenticarUsuarioAsync(Usuario usuario)
        {
            string uriComplementar = "/Autenticar";
            usuario = await _request.PostAsync(apiUrlBase + uriComplementar, usuario, string.Empty);
            return usuario;
        }
    }
}