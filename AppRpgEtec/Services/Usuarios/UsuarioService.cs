using AppRpgEtec.Models;

namespace AppRpgEtec.Services.Usuarios
{
    public class UsuarioService : Request
    {
        private readonly Request _request;
        private const string apiUrlBase = "https://rpgapileo.azurewebsites.net/Usuarios";
        
        public UsuarioService()
        {
            _request = new Request();
        }

        public async Task<Usuario> PostRegistrarUsuarioAsync(Usuario u)
        {
            string uriComplementar = "/Registrar";
            u.Id = await _request.PostReturnIntAsync(apiUrlBase + uriComplementar, u);
            return u;
        }

        public async Task<Usuario> PostAutenticarUsuarioAsync(Usuario u)
        {
            string uriComplementar = "/Autenticar";
            u = await _request.PostAsync(apiUrlBase + uriComplementar, u, string.Empty);

            return u;
        }
    }
}
