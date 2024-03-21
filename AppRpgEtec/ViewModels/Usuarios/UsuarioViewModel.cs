using AppRpgEtec.Services.Usuarios;
using AppRpgEtec.Models;
using AppRpgEtec.Views.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AppRpgEtec.ViewModels.Usuarios
{
    public class UsuarioViewModel : BaseViewModel
    {
        private UsuarioService uService;

        #region Commands
        public ICommand RegistrarCommand { get; set; }
        public ICommand AutenticarCommand { get; set; }
        public ICommand DirecionarCadastroCommand {  get; set; }

        public UsuarioViewModel()
        {
            uService = new UsuarioService();
            InicializarCommands();
        }

        public void InicializarCommands()
        {
            AutenticarCommand = new Command(async () => await AutenticarUsuario());
            RegistrarCommand = new Command(async () => await RegistrarUsuario());
            DirecionarCadastroCommand = new Command(async () => await DirecionarParaCadastro());
        }
        #endregion

        #region AtributosPropiedades
        private string login = string.Empty;
        private string password = string.Empty;

        public string Login
        {
            get => login;
            set
            {
                login = value;
                OnPropertyChanged(Login);
            }

        }

        public string Password
        {
            get => password;
            set
            {
                password = value;
                OnPropertyChanged(Password);
            }
        }
        #endregion

        #region Methods
        public async Task AutenticarUsuario()
        {
            try
            {
                Usuario u = new Usuario();
                u.Username = Login;
                u.PasswordString = Password;

                Usuario uAutenticado = await uService.PostAutenticarUsuarioAsync(u);

                if (!string.IsNullOrEmpty(uAutenticado.Token))
                {
                    string mensagem = $"Bem-vindo(a) {uAutenticado.Username}";

                    Preferences.Set("UsuarioId", uAutenticado.Id);
                    Preferences.Set("UsuarioUsername", uAutenticado.Username);
                    Preferences.Set("UsuarioPerfil", uAutenticado.Perfil);
                    Preferences.Set("UsuarioToken", uAutenticado.Token);

                    await Application.Current.MainPage.
                        DisplayAlert("Informação", mensagem, "Ok");

                    Application.Current.MainPage = new MainPage();

                }
                else
                {
                    await Application.Current.MainPage
                        .DisplayAlert("Informação", "Dados incorretos : (", "Ok");
                }
            }

            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Informação", ex.Message + " Detalhes " + ex.InnerException, "Ok");
            }
        }

            public async Task RegistrarUsuario()
            {
                try
                {
                    Usuario u = new Usuario();
                    u.Username = Login;
                    u.PasswordString = Password;

                    Usuario uRegistrado = await uService.PostRegistrarUsuarioAsync(u);

                    if (uRegistrado.Id != 0)
                    {
                        string mensagem = $"Usuário Id {uRegistrado.Id} registrado com sucesso. ";
                        await Application.Current.MainPage.DisplayAlert("Informação", mensagem, "Ok");

                        await Application.Current.MainPage
                            .Navigation.PopAsync();
                    }
                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage
                        .DisplayAlert("Informação", ex.Message + " Detalhes: " + ex.InnerException, "Ok");

                }

                #endregion
            }

        public async Task DirecionarParaCadastro()
        {
            try
            {
                await Application.Current.MainPage.
                    Navigation.PushAsync(new CadastroView());
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                       .DisplayAlert("Informação", ex.Message + " Detalhes " + ex.InnerException, "Ok");
                
            }
        }
    }
}
