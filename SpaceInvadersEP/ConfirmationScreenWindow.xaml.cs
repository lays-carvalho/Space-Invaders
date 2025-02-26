using System.Windows;

namespace SpaceInvadersEP;

public partial class ConfirmationScreenWindow : Window
{
    public ConfirmationScreenWindow()
    {
        InitializeComponent();
    }
    
    private void BtnSim_Click(object sender, RoutedEventArgs e)
    {
        // Lógica para redirecionar para a tela de inserir o nome
        var nomeWindow = new ScoreSaveScreenWindow();  // Você vai criar essa tela para pegar o nome do jogador
        nomeWindow.Show();
        this.Close();  // Fecha a janela de GameOver
    }

    private void BtnNao_Click(object sender, RoutedEventArgs e)
    {
        // Fecha o jogo se o usuário não quiser salvar o score
        Application.Current.Shutdown();
    }
    
}