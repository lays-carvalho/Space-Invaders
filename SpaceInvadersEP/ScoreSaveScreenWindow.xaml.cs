using System.IO;
using System.Windows;

namespace SpaceInvadersEP;

public partial class ScoreSaveScreenWindow : Window
{
    private int score;  // Campo para armazenar o score atual
    
    public ScoreSaveScreenWindow(int scoreJogador)
    {
        InitializeComponent();
        score = scoreJogador;  // Armazena o score
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
        // Usa o score atual (dinâmico)
        int score = this.score;  // usa o valor que foi passado para o ScoreSaveScreenWindow

        // Caminho do arquivo onde os scores serão salvos
        string filePath = "leaderboard.txt";

        // Vamos adicionar o nome e o score ao arquivo
        try
        {
            // Verifica se o arquivo já existe
            if (File.Exists(filePath))
            {
                // Se o arquivo existe, vamos adicionar uma nova linha
                using (StreamWriter sw = File.AppendText(filePath))
                {
                    sw.WriteLine($"{nomeJogador},{score}");
                }
            }
            else
            {
                // Se não existe, cria um novo arquivo e escreve o cabeçalho e o primeiro score
                using (StreamWriter sw = new StreamWriter(filePath))
                {
                    sw.WriteLine("Nome,Score");  // Cabeçalho
                    sw.WriteLine($"{nomeJogador},{score}");
                }
            }

            MessageBox.Show($"Score de {nomeJogador} foi salvo com sucesso!");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Erro ao salvar o score: {ex.Message}");
        }
    }
    
}