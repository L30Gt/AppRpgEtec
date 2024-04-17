using AppRpgEtec.ViewModels.Usuarios;

namespace AppRpgEtec.Views.Usuarios;

public partial class CadastroView : ContentPage
{
    UsuarioViewModel _viewModel;
    public CadastroView()
	{
		InitializeComponent();

        _viewModel = new UsuarioViewModel();
        BindingContext = _viewModel;
    }
}