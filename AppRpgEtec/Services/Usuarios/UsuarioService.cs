using AppRpgEtec.Models;
using System.Collections.ObjectModel;

namespace AppRpgEtec.Services.Usuarios
{
    public class UsuarioService : Request
    {
        private readonly Request _request;
        //private const string apiUrlBase = "https://rpgapileo.azurewebsites.net/Usuarios";
        private const string apiUrlBase = "https://rpgapi20241pam.azurewebsites.net/Usuarios";
        
        public UsuarioService()
        {
            _request = new Request();
        }

        private string _token;
        public UsuarioService(string token)
        {
            _request = new Request();
            _token = token;
        }

        public async Task<Usuario> PostRegistrarUsuarioAsync(Usuario u)
        {
            string uriComplementar = "/Registrar";
            u.Id = await _request.PostReturnIntAsync(apiUrlBase + uriComplementar, u, string.Empty);
            return u;
        }

        public async Task<Usuario> PostAutenticarUsuarioAsync(Usuario u)
        {
            string uriComplementar = "/Autenticar";
            u = await _request.PostAsync(apiUrlBase + uriComplementar, u, string.Empty);

            return u;
        }

        public async Task<int> PutAtualizarLocalizacaoAsync(Usuario u)
        {
            string urlComplementar = "/AtualizarLocalizacao";
            var result = await _request.PutAsync(apiUrlBase + urlComplementar, u, _token);

            return result;
        }

        public async Task<ObservableCollection<Usuario>> GetUsuariosAsync()
        {
            string urlComplementar = string.Format("{0}", "/GetAll");
            ObservableCollection<Models.Usuario> listaUsuarios = await 
                _request.GetAsync<ObservableCollection<Models.Usuario>>(apiUrlBase + urlComplementar, _token);

            return listaUsuarios;
        }
    }
}
