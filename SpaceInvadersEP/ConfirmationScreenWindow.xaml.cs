using System.Windows;

namespace SpaceInvadersEP;

public partial class ConfirmationScreenWindow : Window
{
    private int score; // Para armazenar a pontuação do jogador
    
    public ConfirmationScreenWindow(int scoreJogador)
    {
        InitializeComponent();
        score = scoreJogador;  // Armazena a pontuação
    }
    
    private void BtnSim_Click(object sender, RoutedEventArgs e)
    {
        // Lógica para redirecionar para a tela de inserir o nome
        var nomeWindow = new ScoreSaveScreenWindow(score);  // Você vai criar essa tela para pegar o nome do jogador
        nomeWindow.Show();
        this.Close();  // Fecha a janela de GameOver
    }

    private void BtnNao_Click(object sender, RoutedEventArgs e)
    {
        // Fecha o jogo se o usuário não quiser salvar o score
        Application.Current.Shutdown();
    }
    
}