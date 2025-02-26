using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SpaceInvadersEP;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{

	private MediaPlayer menuMusicPlayer; 

    public MainWindow()
    {
        InitializeComponent();
		
		menuMusicPlayer = new MediaPlayer();
		menuMusicPlayer.Open(new Uri("Sounds/menu_music.mp3", UriKind.Relative));
		menuMusicPlayer.MediaEnded += MenuMusicPlayer_MediaEnded;
		menuMusicPlayer.Volume = 0.1;
		menuMusicPlayer.Play();
    }

    private void MenuMusicPlayer_MediaEnded(object sender, EventArgs e)
    {
        menuMusicPlayer.Position = TimeSpan.Zero;
		menuMusicPlayer.Play();
    }

    private void IniciarButton_StartGame(object sender, RoutedEventArgs e)
    {
        try
        {

			menuMusicPlayer.Stop();			

            // Crie a janela do jogo (por exemplo, GameWindow) e mostre
            GameWindow gameWindow = new GameWindow();
            gameWindow.Show();

            // Fecha a tela inicial (manter aberta para não ter que criar volta para a Tela Inicial)
            // this.Close();
        }
        catch (Exception ex)
        {
            // Log ou tratamento da exceção
            MessageBox.Show($"Erro ao iniciar o jogo: {ex.Message}");
        }
    }

    private void IniciarButton_LeaderBoard(object sender, RoutedEventArgs e)
    {
        try
        {
            LeaderBoardWindow leaderBoardWindow = new LeaderBoardWindow();
            leaderBoardWindow.Show();
        }
        catch (Exception ex)
        {
            // Log ou tratamento da exceção
            MessageBox.Show($"Erro ao abrir o leaderboard: {ex.Message}");
        }
    }

    private void IniciarButton_GameControls(object sender, RoutedEventArgs e)
    {
        try
        {
            GameControlsWindow gameControlsWindow = new GameControlsWindow();
            gameControlsWindow.Show();
        }
        catch (Exception ex)
        {
            // Log ou tratamento da exceção
            MessageBox.Show($"Erro ao abrir os controles do jogo: {ex.Message}");
        }
    }
}