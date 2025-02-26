using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using SpaceInvadersEP.Enemi;
using SpaceInvadersEP.Game;


namespace SpaceInvadersEP;

public partial class GameWindow : Window
{
    private Player player;
    private List<Bullet> alienBullets; // Lista de tiros dos aliens
    private List<Bullet> bullets; // Lista de tiros do player
    private DispatcherTimer gameTimer;  // Timer para atualização do jogo
    public List<Alien> aliens;  // Lista de alienígenas
    private CounterViewModel counterViewModel;
    private List<Shield> shields;  // Lista para armazenar os escudos
    private double alienSpeed = 0.5;
    private bool movingRight = true;
    private double dropDistance = 12;
    private MediaTimeline gameMusicTimeline;
    private MediaClock gameMusicClock;
    private MediaPlayer gameMusicPlayer; 
    private Random random = new Random();
    private MasterSpaceship masterSpaceship;// Nave mãe (instância de MasterSpaceship)
    private ControlLives controlLives;
    private bool gameWon = false; // Flag para garantir que o código de vitória seja executado uma única vez.
    private bool isGameOver = false; // Nova variável para controle de finalização do jogo
	private MediaPlayer defeatSoundPlayer;
	private MediaPlayer victorySoundPlayer;
    private bool isPlayerBulletActive = false;
    
    public GameWindow()
    {
        InitializeComponent();
        controlLives = new ControlLives();
        controlLives.GameOver += FimDeJogo; // Associa o evento de Game Over com o método de fim de jogo
        controlLives.VidasAlteradas += AtualizarJogo; // Associa o evento de vidas alteradas com a atualização do jogo
        
        try
        {

			victorySoundPlayer = new MediaPlayer();
			victorySoundPlayer.Open(new Uri("Sounds/winning_sound.mp3", UriKind.Relative));
			victorySoundPlayer.Volume = 1.0;

			defeatSoundPlayer = new MediaPlayer();
			defeatSoundPlayer.Open(new Uri("Sounds/defeat_sound.mp3", UriKind.Relative));
			defeatSoundPlayer.Volume = 0.7;

            gameMusicPlayer = new MediaPlayer();
            gameMusicPlayer.Open(new Uri("Sounds/gameplay_music.mp3", UriKind.Relative));
            gameMusicPlayer.Volume = 0.1;
            gameMusicPlayer.MediaEnded += GameMusicPlayer_MediaEnded; // Para repetir a música
            gameMusicPlayer.Play(); // Inicia a reprodução

            player = new Player();
            GameCanvas.Children.Add(player.Ship);

            shields = new List<Shield>(); // Inicializando a lista de escudos

            // Criando e posicionando os escudos
            CreateShields();

            // Lista para armazenar os tiros
            bullets = new List<Bullet>();
            alienBullets = new List<Bullet>();

            // Inicializando a lista de aliens
            aliens = new List<Alien>();

            counterViewModel = new CounterViewModel(); // Inicializando o ViewModel de contagem

            // Adicionando três linhas de alienígenas, com 3 de cada tipo por linha
            AddAliens();
            
            // Criando a nave mãe e adicionando ao canvas
            masterSpaceship = new MasterSpaceship(GameCanvas);

            // Garantir que o player seja posicionado corretamente após o tamanho da tela estar pronto
            this.SizeChanged += GameWindow_SizeChanged;

            // Inicia o Timer para o jogo
            gameTimer = new DispatcherTimer();
            gameTimer.Interval = TimeSpan.FromMilliseconds(20);  // Atualiza a cada 20 ms
            gameTimer.Tick += GameTimer_Tick;  // Evento que será chamado a cada "tick"
            gameTimer.Start();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Erro ao inicializar o jogo: {ex.Message}");
        }
    }
    
    // Método para atualizar o jogo
    private void AtualizarJogo()
    {
        
        // Atualizar a interface com o número de vidas
        vidasLabel.Content = $"Lives: {controlLives.Vidas}";
        
    }
    
    
    private void JogadorFoiAtacado()
    {
        controlLives.PerderVida(); // Perde uma vida quando atingido
        AtualizarJogo(); // Atualiza a interface com as novas vidas

        // Verificar se as vidas chegaram a zero após a perda de uma vida
        if (controlLives.Vidas == 0)
        {
            FimDeJogo(); // Chama o evento de fim de jogo quando as vidas chegarem a zero
        }
    }
    

    // Chama quando o jogador perde todas as vidas (Game Over) 
    private void FimDeJogo()
    {
        
        if (isGameOver) // Se o jogo já terminou, não abre a tela novamente
            return;

        isGameOver = true; // Marca que o jogo acabou (derrota ou vitória)
        
        // Atualiza a interface para mostrar "Lives: 0"
        vidasLabel.Content = "Lives: 0";

        // Exibe a mensagem de fim de jogo
        DefeatMessage.Visibility = Visibility.Visible;

        // Para o timer de movimento da nave mãe
        if (masterSpaceship != null)
        {
            masterSpaceship.StopMovement(); // Método para parar o movimento da nave mãe
        }

        // Interrompe o timer do jogo
        gameTimer.Stop();
        	
		gameMusicPlayer.Stop(); // Para a música de fundo

        try
        {
           defeatSoundPlayer.Position = TimeSpan.Zero;
           defeatSoundPlayer.Play();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Erro ao tocar o som de derrota: {ex.Message}");
        }
        
        // Verifique se a janela de confirmação já foi aberta (para evitar duplicação)
        if (!gameWon) 
        {
            // Mostra a janela de Game Over (perguntar se quer salvar o score)
            var confirmationScreenWindow = new ConfirmationScreenWindow(counterViewModel.Counter.Pontuacao);
            confirmationScreenWindow.Show();
            
            // this.Close(); //se quiser encerrar a janela do jogo
        }
        
    }
    
    
    // Evento chamado a cada "tick" do Timer
    private void GameTimer_Tick(object sender, EventArgs e)
    {
        try
        {
            
            // Verificar colisões entre os tiros dos aliens e o jogador
            CheckAlienBulletCollisions();
    
            
            // Atualiza o jogo a cada tick
            AtualizarJogo();
            
    
            // Mover todos os tiros do jogador
            foreach (var bullet in bullets.ToList())
            {
                if (bullet != null && bullet.BulletShape != null)
                {
                    bullet.Move();
                }
            }
    
            // Mover todos os tiros dos aliens
            foreach (var bullet in alienBullets.ToList())
            {
                if (bullet != null && bullet.BulletShape != null)
                {
                    bullet.Move();
                }
            }
    
            // Verificar colisões entre os tiros do jogador e os aliens
            CheckCollisions();
    
            // Verificar colisões entre os tiros dos aliens e o jogador
            CheckAlienBulletCollisions();
    
            // Remover tiros que saíram da tela ou colidiram
            bullets.RemoveAll(bullet => bullet == null || bullet.BulletShape == null || bullet.BulletShape.Visibility == Visibility.Collapsed || Canvas.GetTop(bullet.BulletShape) < -3);
            alienBullets.RemoveAll(bullet => bullet == null || bullet.BulletShape == null || bullet.BulletShape.Visibility == Visibility.Collapsed || Canvas.GetTop(bullet.BulletShape) > GameCanvas.ActualHeight);
    
            if (bullets.Count == 0)
            {
                isPlayerBulletActive = false;
            }
            // Fazer os aliens atirarem
            MakeAliensShoot();
    
            MoveAliens();
            
            
            // Acaba o jogo com 200 pontos (Entrega Parcial)
            // if (aliens.Count == 0 || counterViewModel.Counter.Pontuacao >= 500)
            // {
            //     // Exibe a mensagem de vitória se os aliens foram todos destruídos ou a pontuação for >= 200
            //     MostrarVitoria();
            //     
            //     // Para o timer de movimento da nave mãe
            //     if (masterSpaceship != null)
            //     {
            //         masterSpaceship.StopMovement(); // Método para parar o movimento da nave mãe
            //     }
            //     
            // }
            
            // Verifica se todos os aliens foram destruídos
            if (aliens.Count == 0 && !gameWon) // Verifique se o jogo já foi vencido para não repetir
            {
                MostrarVitoria();
                gameWon = true; // Marque que o jogo foi vencido
                
            }
            
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Erro no loop do jogo: {ex.Message} \n StackTrace: {ex.StackTrace}");
            
            Console.WriteLine($"Erro no loop do jogo: {ex.StackTrace}"); // Log para depuração
        }
    }
    
    private void MostrarVitoria()
    {
        
        if (isGameOver) // Se o jogo já terminou, não abre a tela novamente
            return;

        isGameOver = true; // Marca que o jogo acabou (derrota ou vitória)
        
        
        // Exibe a mensagem de vitória
        VictoryMessage.Visibility = Visibility.Visible;

        // Para o timer do jogo
        gameTimer.Stop();
        gameMusicPlayer.Stop(); // Para a música de fundo
		
		victorySoundPlayer.Position = TimeSpan.Zero;
		victorySoundPlayer.Play();

        // Para o timer de movimento da nave mãe
        if (masterSpaceship != null)
        {
            masterSpaceship.StopMovement(); // Método para parar o movimento da nave mãe
        }
        
        // Certifique-se de marcar o jogo como vencido
        gameWon = true;
        
        // Mostra a janela de confirmação de vitória
        var confirmationScreenWindow = new ConfirmationScreenWindow(counterViewModel.Counter.Pontuacao);
        confirmationScreenWindow.Show();
        // this.Close(); //se quiser encerrar a janela do jogo

    }
    
    private void CheckCollisions()
{
    try
    {
        // Verifica se algum tiro atingiu a nave mãe
        foreach (var bullet in bullets.ToList()) // Usar ToList() para evitar modificações durante a iteração
        {
            if (bullet.BulletShape != null && masterSpaceship.MasterShipShape != null && masterSpaceship.MasterShipShape.Visibility == Visibility.Visible && IsCollidingWithMasterShip(bullet))
            {
                // Se a colisão ocorreu, destrói a nave mãe e o tiro
                masterSpaceship.HitByPlayerShot(); // Chama a função de colisão da nave mãe

                bullet.BulletShape.Visibility = Visibility.Collapsed; // Oculta o tiro

                // Acessa a pontuação diretamente da nave mãe
                int scoreValue = masterSpaceship.ScoreValue;
                Console.WriteLine($"Master Spaceship hit! Score value: {scoreValue}");

                // Adiciona os pontos ganhos ao total de pontos
                controlLives.AdicionarPontos(scoreValue);

                // Incrementa a pontuação no ViewModel
                counterViewModel.IncrementarPontuacao(scoreValue);

                // Atualiza o TextBlock com a nova pontuação
                ScoreValue.Text = counterViewModel.Counter.Pontuacao.ToString();
            }
        }

        // Verifica se algum tiro atingiu algum alien
        foreach (var bullet in bullets.ToList()) // Usar ToList() para evitar modificações durante a iteração
        {
            foreach (var alien in aliens.ToList()) // Usar ToList() para evitar modificações durante a iteração
            {
                // Verifica se o alien ainda existe e se o tiro ainda está visível
                if (alien.AlienShape != null && bullet.BulletShape != null && IsColliding(bullet, alien))
                {
                    // Se a colisão ocorreu, destrói o alien e o tiro
                    alien.Destroy();
                    bullet.BulletShape.Visibility = Visibility.Collapsed; // Oculta o tiro

                    // Remove o alien da lista
                    aliens.Remove(alien);

                    // Incrementa a pontuação com o valor do alien atingido
                    counterViewModel.IncrementarPontuacao(Convert.ToInt32(alien.Value));

                    // Adiciona os pontos ganhos
                    controlLives.AdicionarPontos(counterViewModel.Counter.Pontuacao);

                    // Atualiza o TextBlock com a nova pontuação
                    ScoreValue.Text = counterViewModel.Counter.Pontuacao.ToString();

                    break;  // Se o tiro acertou um alien, não verifica mais colisões com outros aliens
                }
            }

            // Verifica colisões com os escudos (tiros do jogador)
            foreach (var shield in shields.ToList()) // Usar ToList() para evitar modificações durante a iteração
            {
                if (bullet.BulletShape != null && shield.ShieldImage != null && IsCollidingWithShield(bullet, shield))
                {
                    shield.TakeDamage();  // Diminui a vida do escudo
                    bullet.BulletShape.Visibility = Visibility.Collapsed;  // Destrói o tiro
                    break; // Um tiro só pode atingir um escudo por vez
                }
            }
        }

        // Verifica se algum tiro dos aliens atingiu algum escudo
        foreach (var alienBullet in alienBullets.ToList()) // Usar ToList() para evitar modificações durante a iteração
        {
            foreach (var shield in shields.ToList()) // Usar ToList() para evitar modificações durante a iteração
            {
                if (alienBullet.BulletShape != null && shield.ShieldImage != null && IsCollidingWithShield(alienBullet, shield))
                {
                    shield.TakeDamage();  // Diminui a vida do escudo
                    alienBullet.BulletShape.Visibility = Visibility.Collapsed;  // Destrói o tiro do alien
                    break; // Um tiro só pode atingir um escudo por vez
                }
            }
        }
    }
    catch (Exception ex)
    {
        MessageBox.Show($"Erro ao verificar colisões: {ex.Message}");
    }
}
    
    private void MakeAliensShoot()
    {
        foreach (var alien in aliens.ToList())
        {
            if (alien is AlienType3 alienType3)
            {
                if (random.Next(0, 500) < 1)
                {
                    alienType3.Shoot(alienBullets);
                }
            }
        }
    }
    
    private void CheckAlienBulletCollisions()
    {
        try
        {
            foreach (var bullet in alienBullets.ToList())
            {
                if (bullet.BulletShape != null && IsCollidingWithPlayer(bullet))
                {
                    JogadorFoiAtacado(); // Usando o método centralizado para tratar a perda de vida
                    bullet.BulletShape.Visibility = Visibility.Collapsed; // Oculta o tiro
                    alienBullets.Remove(bullet); // Remove o tiro do alien
                    break; // Impede que o mesmo tiro seja processado múltiplas vezes
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Erro ao verificar colisões com o jogador: {ex.Message}");
        }
    }


    private bool IsCollidingWithPlayer(Bullet bullet)
    {
        try
        {
            var bulletLeft = Canvas.GetLeft(bullet.BulletShape);
            var bulletTop = Canvas.GetTop(bullet.BulletShape);
            var playerLeft = Canvas.GetLeft(player.Ship);
            var playerTop = Canvas.GetTop(player.Ship);

            return bulletLeft < playerLeft + player.Ship.Width &&
                   bulletLeft + bullet.BulletShape.Width > playerLeft &&
                   bulletTop < playerTop + player.Ship.Height &&
                   bulletTop + bullet.BulletShape.Height > playerTop;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Erro ao verificar colisão com o jogador: {ex.Message}");
            return false;
        }
    }
    
    
    
    private void GameMusicPlayer_MediaEnded(object sender, EventArgs e)
    {
        gameMusicPlayer.Position = TimeSpan.Zero;
        gameMusicPlayer.Play();
    }

    // Adicionando alienígenas no jogo
    private void AddAliens()
    {
        try
        {
            double yPosition = 100;  // Começa no topo da tela
            for (int row = 0; row < 5; row++)  // Cinco linhas de aliens
            {
                for (int col = 0; col < 11; col++)  // Onze aliens por linha
                {
                    Alien alien;

                    // Criação dos aliens de diferentes tipos com base na linha
                    if (row == 0)
                        alien = new AlienType3(50 + col * 60, yPosition, GameCanvas);  // Tipo 3 atira
                    else if (row == 1)
                        alien = new AlienType2(50 + col * 60, yPosition, GameCanvas);  // Tipo 2
					else if (row == 2)
                        alien = new AlienType2(50 + col * 60, yPosition, GameCanvas);
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
        catch (Exception ex)
        {
            MessageBox.Show($"Erro ao adicionar alienígenas: {ex.Message}");
        }
    }

    private void GameCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        try
        {
            // Posiciona o player no centro do eixo X e na parte inferior
            Canvas.SetLeft(player.Ship, (GameCanvas.ActualWidth - player.Ship.Width) / 2);
            Canvas.SetTop(player.Ship, GameCanvas.ActualHeight - player.Ship.Height - 30);  // 30 pixels acima da borda inferior

            // Reposiciona os escudos conforme o novo tamanho da tela
            RepositionShields();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Erro ao ajustar o tamanho da tela: {ex.Message}");
        }
    }

    private void CreateShields()
    {
        try
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
        catch (Exception ex)
        {
            MessageBox.Show($"Erro ao criar escudos: {ex.Message}");
        }
    }

    private void RepositionShields()
    {
        try
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
        catch (Exception ex)
        {
            MessageBox.Show($"Erro ao reposicionar escudos: {ex.Message}");
        }
    }

    // Método que será chamado quando o tamanho da janela mudar
    private void GameWindow_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        try
        {
            // Posiciona o player no centro do eixo X e na parte inferior
            Canvas.SetLeft(player.Ship, (GameCanvas.ActualWidth - player.Ship.Width) / 2);
            Canvas.SetTop(player.Ship, GameCanvas.ActualHeight - player.Ship.Height - 30);  // 30 pixels acima da borda inferior
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Erro ao ajustar o tamanho da janela: {ex.Message}");
        }
    }

    
    // Método para mover a nave para a esquerda ou direita
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
        try
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
                if (!isPlayerBulletActive)
                {

                    // Criar um novo tiro na posição da nave
                    Bullet newBullet = new Bullet(Canvas.GetLeft(player.Ship) + (player.Ship.Width / 2) - 2,
                        Canvas.GetTop(player.Ship) - 10, GameCanvas.ActualHeight);
                    bullets.Add(newBullet);
                    GameCanvas.Children.Add(newBullet.BulletShape); // Adiciona o tiro ao Canvas

                    player.PlayShootSound();
                    isPlayerBulletActive = true;
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Erro ao processar tecla pressionada: {ex.Message}");
        }
    }

    private void MoveAliens()
    {
        try
        {
            bool shouldDescend = false;

            // Verifica se algum alienígena atingiu a borda da tela
            foreach (var alien in aliens.ToList()) // Usar ToList() para evitar modificações durante a iteração
            {
                // Verifica se o AlienShape ainda existe
                if (alien.AlienShape == null || !GameCanvas.Children.Contains(alien.AlienShape))
                {
                    continue; // Se o alienígena foi destruído, pula para o próximo
                }

                double alienX = Canvas.GetLeft(alien.AlienShape);

                // Verifica se o alienígena atingiu a borda direita ou esquerda da tela
                if ((movingRight && alienX + alien.AlienShape.Width >= GameCanvas.ActualWidth) ||
                    (!movingRight && alienX <= 0))
                {
                    shouldDescend = true;
                    break;
                }

                // Verifica se o alien colidiu com a nave do jogador
                if (IsCollidingWithPlayer(alien))
                {
                    // Destrói o alien
                    alien.Destroy();
                    aliens.Remove(alien);

                    // Faz o jogador perder uma vida
                    JogadorFoiAtacado();

                    // Se o jogador tiver apenas uma vida, destrói a nave do jogador
                    if (controlLives.Vidas == 1)
                    {
                        // Remove a nave do jogador do Canvas
                        GameCanvas.Children.Remove(player.Ship);

                        // Encerra o jogo
                        FimDeJogo();
                    }

                    break; // Sai do loop após detectar a colisão
                }
            }

            // Se algum alienígena atingiu a borda, todos devem descer
            if (shouldDescend)
            {
                movingRight = !movingRight;
                alienSpeed += 0.5;  // Aumenta a velocidade dos aliens

                foreach (var alienDescend in aliens.ToList()) // Usar ToList() para evitar modificações durante a iteração
                {
                    // Verifica se o AlienShape ainda existe
                    if (alienDescend.AlienShape == null || !GameCanvas.Children.Contains(alienDescend.AlienShape))
                    {
                        continue; // Se o alienígena foi destruído, pula para o próximo
                    }

                    double currentY = Canvas.GetTop(alienDescend.AlienShape);
                    Canvas.SetTop(alienDescend.AlienShape, currentY + dropDistance); // Move o alienígena para baixo
                }
            }

            // Move todos os alienígenas na direção atual
            double movement = movingRight ? alienSpeed : -alienSpeed;
            foreach (var alienMove in aliens.ToList()) // Usar ToList() para evitar modificações durante a iteração
            {
                // Verifica se o AlienShape ainda existe
                if (alienMove.AlienShape == null || !GameCanvas.Children.Contains(alienMove.AlienShape))
                {
                    continue; // Se o alienígena foi destruído, pula para o próximo
                }

                double currentX = Canvas.GetLeft(alienMove.AlienShape);
                Canvas.SetLeft(alienMove.AlienShape, currentX + movement); // Move o alienígena horizontalmente
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Erro ao mover alienígenas: {ex.Message}");
        }
    }
    private bool IsCollidingWithMasterShip(Bullet bullet)
    {
        try
        {
            // Verifica se o tiro colidiu com a nave mãe
            var bulletLeft = Canvas.GetLeft(bullet.BulletShape);
            var bulletTop = Canvas.GetTop(bullet.BulletShape);
            var masterShipLeft = Canvas.GetLeft(masterSpaceship.MasterShipShape);
            var masterShipTop = Canvas.GetTop(masterSpaceship.MasterShipShape);

            return bulletLeft < masterShipLeft + masterSpaceship.MasterShipShape.Width &&
                   bulletLeft + bullet.BulletShape.Width > masterShipLeft &&
                   bulletTop < masterShipTop + masterSpaceship.MasterShipShape.Height &&
                   bulletTop + bullet.BulletShape.Height > masterShipTop;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Erro ao verificar colisão com nave mãe: {ex.Message}");
            return false;
        }
    }
    
    private bool IsColliding(Bullet bullet, Alien alien)
    {
        try
        {
            // Verifica se o alien e o tiro ainda estão na tela
            if (alien.AlienShape == null || bullet.BulletShape == null || 
                !GameCanvas.Children.Contains(alien.AlienShape) || 
                !GameCanvas.Children.Contains(bullet.BulletShape))
            {
                return false; // Se o alien ou o tiro foram destruídos, não há colisão
            }

            var bulletLeft = Canvas.GetLeft(bullet.BulletShape);
            var bulletTop = Canvas.GetTop(bullet.BulletShape);
            var alienLeft = Canvas.GetLeft(alien.AlienShape);
            var alienTop = Canvas.GetTop(alien.AlienShape);

            return bulletLeft < alienLeft + alien.AlienShape.Width &&
                   bulletLeft + bullet.BulletShape.Width > alienLeft &&
                   bulletTop < alienTop + alien.AlienShape.Height &&
                   bulletTop + bullet.BulletShape.Height > alienTop;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Erro ao verificar colisão com alien: {ex.Message}");
            return false;
        }
    }

    private bool IsCollidingWithShield(Bullet bullet, Shield shield)
    {
        try
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
        catch (Exception ex)
        {
            MessageBox.Show($"Erro ao verificar colisão com escudo: {ex.Message}");
            return false;
        }
    }

	private bool IsCollidingWithPlayer(Alien alien)
{
    try
    {
        // Verifica se o alien e a nave do jogador ainda estão na tela
        if (alien.AlienShape == null || player.Ship == null || 
            !GameCanvas.Children.Contains(alien.AlienShape) || 
            !GameCanvas.Children.Contains(player.Ship))
        {
            return false; // Se o alien ou a nave do jogador foram destruídos, não há colisão
        }

        var alienLeft = Canvas.GetLeft(alien.AlienShape);
        var alienTop = Canvas.GetTop(alien.AlienShape);
        var playerLeft = Canvas.GetLeft(player.Ship);
        var playerTop = Canvas.GetTop(player.Ship);

        return alienLeft < playerLeft + player.Ship.Width &&
               alienLeft + alien.AlienShape.Width > playerLeft &&
               alienTop < playerTop + player.Ship.Height &&
               alienTop + alien.AlienShape.Height > playerTop;
    }
    catch (Exception ex)
    {
        MessageBox.Show($"Erro ao verificar colisão com o jogador: {ex.Message}");
        return false;
    }
}	

    
}