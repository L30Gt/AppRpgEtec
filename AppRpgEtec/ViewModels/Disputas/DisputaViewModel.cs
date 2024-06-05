using AppRpgEtec.Models;
using AppRpgEtec.Services.Disputas;
using AppRpgEtec.Services.PersonagemHabilidades;
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
        private DisputaService _disputaService;
        public Disputa DisputaPersonagens { get; set; }
        private PersonagemHabilidadeService _personagemHabilidadeService;
        public ObservableCollection<PersonagemHabilidade> Habilidades { get; set; }

        public DisputaViewModel()
        {
            string token = Preferences.Get("UsuarioToken", string.Empty);
            _personagemService = new PersonagemService(token);
            PersonagensEncontrados = new ObservableCollection<Personagem>();
            _disputaService = new DisputaService(token);
            _personagemHabilidadeService = new PersonagemHabilidadeService(token);

            Atacante = new Personagem();
            Oponente = new Personagem();
            DisputaPersonagens = new Disputa();

            PesquisarPersonagensCommand = new Command<string>(async (string pesquisa) => { await PesquisarPersonagens(pesquisa); });
            DisputaComArmaCommand = new Command(async () => { await ExecutarDisputaArmada(); });
            DisputaComHabilidadeCommand = new Command(async () => { await ExecutarDisputaHabilidades(); });
            DisputaGeralCommand = new Command(async () => { await ExecutarDisputaGeral(); });
        }

        #region Commands
        public ICommand PesquisarPersonagensCommand { get; set; }
        public ICommand DisputaComArmaCommand { get; set; }
        public ICommand DisputaComHabilidadeCommand { get; set; }
        public ICommand DisputaGeralCommand { get; set; }
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

        private PersonagemHabilidade habilidadeSelecionada;
        public PersonagemHabilidade HabilidadeSelecionada 
        { 
            get { return habilidadeSelecionada; }
            set
            {
                if (value != null)
                {
                    try
                    {
                        habilidadeSelecionada = value;
                        OnPropertyChanged();
                    }
                    catch (Exception ex)
                    {
                        Application.Current.MainPage.DisplayAlert("Ops", ex.Message, "Ok");
                    }
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
                    await this.ObterHabilidadesAsync(p.Id);
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

        private async Task ExecutarDisputaArmada()
        {
            try
            {
                DisputaPersonagens.AtacanteId = Atacante.Id;
                DisputaPersonagens.OponenteId = Oponente.Id;
                DisputaPersonagens = await _disputaService.PostDisputaComArmaAsync(DisputaPersonagens);

                await Application.Current.MainPage.
                    DisplayAlert("Resultado", DisputaPersonagens.Narracao, "Ok");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Ops", ex.Message + " Detalhes: " + ex.InnerException, "Ok");
            }
        }

        public async Task ObterHabilidadesAsync(int personagemId)
        {
            try
            {
                Habilidades = await _personagemHabilidadeService.GetPersonagemHabilidadesAsync(personagemId);
                OnPropertyChanged(nameof(Habilidades));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Ops", ex.Message + " Detalhes: " + ex.InnerException, "Ok");
            }
        }

        private async Task ExecutarDisputaHabilidades()
        {
            try
            {
                DisputaPersonagens.AtacanteId = Atacante.Id;
                DisputaPersonagens.OponenteId = Oponente.Id;
                DisputaPersonagens = await _disputaService.PostDisputaComHabilidadesAsync(DisputaPersonagens);

                await Application.Current.MainPage
                    .DisplayAlert("Resultado", DisputaPersonagens.Narracao, "Ok");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Ops", ex.Message + " Detalhes: " + ex.InnerException, "Ok");
            }
        }

        private async Task ExecutarDisputaGeral()
        {
            try
            {
                ObservableCollection<Personagem> lista = await _personagemService.GetPersonagensAsync();
                DisputaPersonagens.ListaIdPersonagens = lista.Select(x => x.Id).ToList();

                DisputaPersonagens = await _disputaService.PostDisputaGeralAsync(DisputaPersonagens);

                string resultados = string.Join(" | ", DisputaPersonagens.Resultados);
                await Application.Current.MainPage
                    .DisplayAlert("Resultado", resultados, "Ok");
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
