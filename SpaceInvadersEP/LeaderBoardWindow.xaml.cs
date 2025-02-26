using System.IO;
using System.Windows;

namespace SpaceInvadersEP;

public partial class LeaderBoardWindow : Window
{
    public LeaderBoardWindow()
    {
        InitializeComponent();
        CarregarLeaderboard();
    }
    
    public class LeaderboardEntry
    {
        public string Nome { get; set; }
        public int Score { get; set; }
        
        public string Ranking { get; set; }  // Campo para o ranking
    }

    private void CarregarLeaderboard()
    {
        string filePath = "leaderboard.txt";

        if (File.Exists(filePath))
        {
            try
            {
                // Lê as linhas do arquivo
                var lines = File.ReadAllLines(filePath)
                    .Skip(1)  // Ignora o cabeçalho
                    .Select(line => line.Split(','))
                    .Where(parts => parts.Length == 2)
                    .Select(parts => new LeaderboardEntry
                    {
                        Nome = parts[0],
                        Score = int.Parse(parts[1])
                    })
                    .OrderByDescending(entry => entry.Score) // Ordena pelo score, do maior para o menor
                    .ToList();
                
                // Calcula o ranking
                int ranking = 1;
                foreach (var entry in lines)
                {
                    if (ranking == 1)
                    {
                        entry.Ranking = ranking + "º 🏆";  
                    }
                    else
                    {
                        entry.Ranking = ranking + "º";  // Adiciona os outros rankings (2º, 3º, 4º, etc.)
                    }
                    ranking++;  // Incrementa o ranking para o próximo
                }

                leaderboardListView.ItemsSource = lines; // Vincula a lista de entradas ao ListView
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar o leaderboard: {ex.Message}");
            }
        }
        else
        {
            MessageBox.Show("Arquivo de leaderboard não encontrado.");
        }
    }
    
}