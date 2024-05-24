using AppRpgEtec.Models;
using AppRpgEtec.Services.Personagens;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace AppRpgEtec.ViewModels.Disputas
{
    public class DisputaViewModel : BaseViewModel
    {
        private PersonagemService _personagemService;
        public ObservableCollection<Personagem> PersonagensEncontrados { get; set; }
        public Personagem Atacante { get; set; }
        public Personagem Oponente { get; set; }

        public DisputaViewModel()
        {
            string token = Preferences.Get("UsuarioToken", string.Empty);
            _personagemService = new PersonagemService(token);
            PersonagensEncontrados = new ObservableCollection<Personagem>();

            Atacante = new Personagem();
            Oponente = new Personagem();

            PesquisarPersonagensCommand = new Command<string>(async (string pesquisa) => { await PesquisarPersonagens(pesquisa); });
        }

        #region Commands
        public ICommand PesquisarPersonagensCommand { get; set; }
        #endregion

        #region AtributesProperties
        public string DescricaoPersonagemAtacante
        {
            get => Atacante.Nome;
        }

        public string DescricaoPersonagemOponente
        {
            get => Oponente.Nome;
        }

        private Personagem personagemSelecionado;
        public Personagem PersonagemSelecionado 
        { 
            set
            {
                if(value != null)
                {
                    personagemSelecionado = value;
                    SelecionarPersonagem(personagemSelecionado);
                    OnPropertyChanged();
                    PersonagensEncontrados.Clear();
                }
            }
        }

        private string textoBuscaDigitado = string.Empty;
        public string TextoBuscaDigitado 
        { 
            get { return textoBuscaDigitado; } 
            set
            {
                if (value != null && !string.IsNullOrEmpty(value) && value.Length > 0)
                {
                    textoBuscaDigitado = value;
                    _ = PesquisarPersonagens(textoBuscaDigitado);
                }
                else
                {
                    PersonagensEncontrados.Clear();
                }
            } 
        }
        #endregion

        #region Methods
        public async Task PesquisarPersonagens(string textoPesquisaPersonagem)
        {
            try
            {
                PersonagensEncontrados = await _personagemService.GetByNomeAproximadoAsync(textoPesquisaPersonagem);
                OnPropertyChanged(nameof(PersonagensEncontrados));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Ops", ex.Message, " Detalhes: " + ex.InnerException, "Ok");
            }
        }

        public async void SelecionarPersonagem(Personagem p)
        {
            try
            {
                string tipoCombatente = await Application.Current.MainPage
                    .DisplayActionSheet("Atacante ou Oponente?", "Cancelar", "", "Atacante", "Oponente");

                if (tipoCombatente == "Atacante")
                {
                    Atacante = p;
                    OnPropertyChanged(nameof(DescricaoPersonagemAtacante));
                }
                else if(tipoCombatente == "Oponente")
                {
                    Oponente = p;
                    OnPropertyChanged(nameof(DescricaoPersonagemOponente));
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Ops", ex.Message + " Detalhes: " + ex.InnerException, "Ok");
            }
        }
        #endregion
    }
}
