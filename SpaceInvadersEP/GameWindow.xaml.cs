using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using SpaceInvadersEP.Enemi;
using SpaceInvadersEP.Game;

namespace SpaceInvadersEP;

public partial class GameWindow : Window
{
    private Player player;
    private List<Bullet> bullets; // Lista de tiros
    private DispatcherTimer gameTimer;  // Timer para atualização do jogo
    public List<Alien> aliens;  // Lista de alienígenas
    private CounterViewModel counterViewModel;
    public GameWindow()
    {
        InitializeComponent();
        
        player = new Player();
        GameCanvas.Children.Add(player.Ship);
            
        // Lista para armazenar os tiros
        bullets = new List<Bullet>();
            
        // Inicializando a lista de aliens
        aliens = new List<Alien>();
            
        InitializeComponent();
        counterViewModel = new CounterViewModel(); // Inicializando o ViewModel de contagem

        // Adicionando três linhas de alienígenas, com 3 de cada tipo por linha
        AddAliens();
            
        // Garantir que o player seja posicionado corretamente após o tamanho da tela estar pronto
        this.SizeChanged += GameWindow_SizeChanged;
            
        // Inicia o Timer para o jogo
        gameTimer = new DispatcherTimer();
        gameTimer.Interval = System.TimeSpan.FromMilliseconds(20);  // Atualiza a cada 20 ms
        gameTimer.Tick += GameTimer_Tick;  // Evento que será chamado a cada "tick"
        gameTimer.Start();
        
    }
    
    // Adicionando alienígenas no jogo
        
    private void AddAliens()
    {
        double yPosition = 50;  // Começa no topo da tela
        for (int row = 0; row < 3; row++)  // Três linhas de aliens
        {
            for (int col = 0; col < 10; col++)  // Dez aliens por linha
            {
                Alien alien;

                // Criação dos aliens de diferentes tipos com base na linha
                if (row == 0)
                    alien = new AlienType3(50 + col * 60, yPosition, GameCanvas);  // Tipo 3 atira
                else if (row == 1)
                    alien = new AlienType2(50 + col * 60, yPosition, GameCanvas);  // Tipo 2
                else
                    alien = new AlienType1(50 + col * 60, yPosition, GameCanvas);  // Tipo 1

                aliens.Add(alien);
                GameCanvas.Children.Add(alien.AlienShape);

                // Posicionar no Canvas com Canvas.SetLeft e Canvas.SetTop
                Canvas.SetLeft(alien.AlienShape, 50 + col * 60);
                Canvas.SetTop(alien.AlienShape, yPosition);
            }
            yPosition += 60;  // Desloca para a próxima linha de alienígenas
        }
    }

    private void GameCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        // Posiciona o player no centro do eixo X e na parte inferior
        Canvas.SetLeft(player.Ship, (GameCanvas.ActualWidth - player.Ship.Width) / 2);
        Canvas.SetTop(player.Ship, GameCanvas.ActualHeight - player.Ship.Height - 30);  // 30 pixels acima da borda inferior
    }
        
    // Método que será chamado quando o tamanho da janela mudar
    private void GameWindow_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        // Posiciona o player no centro do eixo X e na parte inferior
        Canvas.SetLeft(player.Ship, (GameCanvas.ActualWidth - player.Ship.Width) / 2);
        Canvas.SetTop(player.Ship, GameCanvas.ActualHeight - player.Ship.Height - 30);  // 30 pixels acima da borda inferior
    }
        
    // Evento chamado a cada "tick" do Timer
    private void GameTimer_Tick(object sender, EventArgs e)
    {
        // Mover todos os tiros
        foreach (var bullet in bullets)
        {
            bullet.Move();
        }

        // Verificar colisões entre os tiros e os aliens
        CheckCollisions();  // Adicionado para verificar colisões

        // Remover tiros que saíram da tela ou colidiram
        bullets.RemoveAll(bullet => bullet.BulletShape.Visibility == Visibility.Collapsed || Canvas.GetTop(bullet.BulletShape) < 0);
    }

    // Método para mover a nave para a esquerda ou direita
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                player.Move(-player.Speed, GameCanvas.ActualWidth);  // Passa a largura do Canvas para o método Move
            }
            else if (e.Key == Key.Right)
            {
                player.Move(player.Speed, GameCanvas.ActualWidth);  // Passa a largura do Canvas para o método Move
            }
            else if (e.Key == Key.Space)  // Quando pressionar a barra de espaço, atirar
            {
                // Criar um novo tiro na posição da nave
                Bullet newBullet = new Bullet(Canvas.GetLeft(player.Ship) + (player.Ship.Width / 2) - 2, Canvas.GetTop(player.Ship) - 10);
                bullets.Add(newBullet);
                GameCanvas.Children.Add(newBullet.BulletShape); // Adiciona o tiro ao Canvas
            }
        }
        
        private void CheckCollisions()
        {
            // Verifica se algum tiro atingiu algum alien
            foreach (var bullet in bullets)
            {
                foreach (var alien in aliens)
                {
                    if (IsColliding(bullet, alien))
                    {
                        // Se a colisão ocorreu, destrói o alien e o tiro
                        alien.Destroy();
                        bullet.BulletShape.Visibility = Visibility.Collapsed; // Colide e oculta o tiro

                        // Incrementa a pontuação com o valor do alien atingido
                        // Converte o valor do alien (double) para inteiro
                        counterViewModel.IncrementarPontuacao(Convert.ToInt32(alien.Value));

                        // Atualiza o TextBlock com a nova pontuação
                        ScoreValue.Text = counterViewModel.Counter.Pontuacao.ToString();
                        
                        // Verifica se a pontuação atingiu 500
                        // if (counterViewModel.Counter.Pontuacao >= 500)
                        // {
                        //     GameOver(); // Chama o método para terminar o jogo
                        // }
                        GameOver();

                        break;  // Se o tiro acertou um alien, não verifica mais colisões com outros aliens
                    }
                }
            }
        }

        

        private bool IsColliding(Bullet bullet, Alien alien)
        {
            // Verifica se o alien ainda está na tela
            if (alien.AlienShape == null || !GameCanvas.Children.Contains(alien.AlienShape))
                return false; // Se o alien foi destruído, não há colisão

            var bulletLeft = Canvas.GetLeft(bullet.BulletShape);
            var bulletTop = Canvas.GetTop(bullet.BulletShape);
            var alienLeft = Canvas.GetLeft(alien.AlienShape);
            var alienTop = Canvas.GetTop(alien.AlienShape);

            return bulletLeft < alienLeft + alien.AlienShape.Width &&
                   bulletLeft + bullet.BulletShape.Width > alienLeft &&
                   bulletTop < alienTop + alien.AlienShape.Height &&
                   bulletTop + bullet.BulletShape.Height > alienTop;
        }

        
        private void GameOver()
        {
            if (counterViewModel.Counter.Pontuacao >= 500)
            {
                // Exibe a mensagem na tela
                VictoryMessage.Visibility = Visibility.Visible;
    
                // Mostra o MessageBox de vitória
                // MessageBox.Show("Parabéns, você venceu! \ud83c\udfae\ud83c\udfc6", "Fim de Jogo", MessageBoxButton.OK, MessageBoxImage.Information);

                // Para o timer e o jogo
                gameTimer.Stop();
            }
        }
}