using AppRpgEtec.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppRpgEtec.Services.PersonagemHabilidades
{
    public class PersonagemHabilidadeService : Request
    {
        private readonly Request _request = null;

        //private const string apiUrlBase = "https://rpgapileo.azurewebsites.net/PersonagemHabilidades";
        private const string _apiUrlBase = "https://rpgapi20241pam.azurewebsites.net/PersonagemHabilidades";
        private string _token;

        public PersonagemHabilidadeService(string token)
        {
            _request = new Request();
            _token = token;
        }

        public async Task<ObservableCollection<PersonagemHabilidade>> GetPersonagemHabilidadesAsync(int personagemId)
        {
            string urlComplementar = string.Format("{0}", personagemId);
            ObservableCollection<PersonagemHabilidade> listaPH = await 
                _request.GetAsync<ObservableCollection<PersonagemHabilidade>>(_apiUrlBase + urlComplementar, _token);

            return listaPH;
        }
        public async Task<ObservableCollection<Habilidade>> GetHabilidadesAsync()
        {
            string urlComplementar = string.Format("{0}", "GetHabilidades");
            ObservableCollection<Habilidade> listaHabilidades = await 
                _request.GetAsync<ObservableCollection<Habilidade>>(_apiUrlBase + urlComplementar, _token);

            return listaHabilidades;
        }

    }
}
