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
    private List<Shield> shields;  // Lista para armazenar os escudos
    
    public GameWindow()
    {
        InitializeComponent();
        
        player = new Player();
        GameCanvas.Children.Add(player.Ship);
        
        shields = new List<Shield>(); // Inicializando a lista de escudos
        
        // Criando e posicionando os escudos
        CreateShields();
            
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
        
        // Reposiciona os escudos conforme o novo tamanho da tela
        RepositionShields();
    }

    private void CreateShields()
    {
        double spacing = 120;
        double canvasCenterX = GameCanvas.ActualWidth / 2;
        double startX = canvasCenterX - 5 * spacing / 2 - 50;
        double startY = GameCanvas.ActualHeight - 100 - 60;

        // Criar 6 escudos e passar a referência do GameCanvas e da lista de escudos
        shields.Add(new Shield(startX, startY, GameCanvas, shields));  // Passa GameCanvas e a lista de escudos
        shields.Add(new Shield(startX + spacing, startY, GameCanvas, shields));
        shields.Add(new Shield(startX + 2 * spacing, startY, GameCanvas, shields));
        shields.Add(new Shield(startX + 3 * spacing, startY, GameCanvas, shields));
        shields.Add(new Shield(startX + 4 * spacing, startY, GameCanvas, shields));
        shields.Add(new Shield(startX + 5 * spacing, startY, GameCanvas, shields));

        foreach (var shield in shields)
        {
            GameCanvas.Children.Add(shield.ShieldImage);
        }
    }
    
    
    private void RepositionShields()
    {
        // Distância entre os escudos (mantendo o espaçamento maior)
        double spacing = 120;  // O espaçamento agora é 120 (você pode ajustar conforme necessário)

        // Calcular a posição central da tela
        double canvasCenterX = GameCanvas.ActualWidth / 2;

        // Calcular a posição inicial dos escudos em torno do centro da tela
        // 6 escudos => 5 espaços entre eles
        double startX = canvasCenterX - 5 * spacing / 2 - 50;  // Centraliza os 6 escudos no eixo X
        double startY = GameCanvas.ActualHeight - 100 - 60; // Fica um pouco acima da borda inferior

        // Atualiza as posições dos escudos
        for (int i = 0; i < shields.Count; i++)
        {
            double x = startX + i * spacing;
            Canvas.SetLeft(shields[i].ShieldImage, x);
            Canvas.SetTop(shields[i].ShieldImage, startY);
        }
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
                        
                        
                        GameOver();

                        break;  // Se o tiro acertou um alien, não verifica mais colisões com outros aliens
                    }
                }
                
                // Verifica colisões com os escudos
                foreach (var shield in shields)
                {
                    if (IsCollidingWithShield(bullet, shield))
                    {
                        shield.TakeDamage();  // Diminui a vida do escudo
                        bullet.BulletShape.Visibility = Visibility.Collapsed;  // Destrói o tiro
                        break; // Um tiro só pode atingir um escudo por vez
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
        
        private bool IsCollidingWithShield(Bullet bullet, Shield shield)
        {
            // Verifica se o tiro colidiu com o escudo
            var bulletLeft = Canvas.GetLeft(bullet.BulletShape);
            var bulletTop = Canvas.GetTop(bullet.BulletShape);
            var shieldLeft = Canvas.GetLeft(shield.ShieldImage);
            var shieldTop = Canvas.GetTop(shield.ShieldImage);

            return bulletLeft < shieldLeft + shield.ShieldImage.Width &&
                   bulletLeft + bullet.BulletShape.Width > shieldLeft &&
                   bulletTop < shieldTop + shield.ShieldImage.Height &&
                   bulletTop + bullet.BulletShape.Height > shieldTop;
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