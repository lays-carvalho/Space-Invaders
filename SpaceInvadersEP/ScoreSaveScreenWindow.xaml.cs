using System.Windows;

namespace SpaceInvadersEP;

public partial class ScoreSaveScreenWindow : Window
{
    public ScoreSaveScreenWindow()
    {
        InitializeComponent();
    }
    
    private void SalvarButton_Click(object sender, RoutedEventArgs e)
    {
        string nomeJogador = NomeTextBox.Text;

        if (!string.IsNullOrEmpty(nomeJogador))
        {
            // Lógica para salvar o nome e o score
            SalvarScore(nomeJogador);

            // Fechar a janela de salvar
            this.Close();
        }
        else
        {
            MessageBox.Show("Por favor, insira um nome.");
        }
    }

    private void SalvarScore(string nomeJogador)
    {
        // Aqui você pode salvar o nome e a pontuação em um arquivo ou banco de dados
        MessageBox.Show($"Score de {nomeJogador} foi salvo com sucesso!");
        
        
        
    }
    
}