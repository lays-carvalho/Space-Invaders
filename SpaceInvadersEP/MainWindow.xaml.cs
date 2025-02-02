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
    public MainWindow()
    {
        InitializeComponent();
        
    }
    
    private void IniciarButton_Click(object sender, RoutedEventArgs e)
    {
        // Crie a janela do jogo (por exemplo, GameWindow) e mostre
        GameWindow gameWindow = new GameWindow();
        gameWindow.Show();
            
        // Fecha a tela inicial
        this.Close();
    }

    private void IniciarButton_LeaderBoard(object sender, RoutedEventArgs e)
    {
        LeaderBoardWindow leaderBoardWindow = new LeaderBoardWindow();
        leaderBoardWindow.Show();
        
        // Fecha a tela inicial
        this.Close();
    }

    private void IniciarButton_GameControls(object sender, RoutedEventArgs e)
    {
        GameControlsWindow gameControlsWindow = new GameControlsWindow();
        gameControlsWindow.Show();
        
        // Fecha a tela inicial
        this.Close();
    }
    
}
