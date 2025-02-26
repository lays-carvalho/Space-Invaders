using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using SpaceInvadersEP.Game;

namespace SpaceInvadersEP
{
    public class MasterSpaceship
    {
        public Image MasterShipShape { get; set; } // Mudei de Rectangle para Image
        private Canvas _canvas;
        private Random _random;
        private DispatcherTimer _movementTimer;
        
        private DispatcherTimer _respawnTimer; // Timer para respawn da nave
        
        private bool moveRight = true; // Controla a direção do movimento (direita para esquerda ou vice-versa)
        private DateTime lastDirectionChangeTime; // Para controlar o tempo decorrido desde a última mudança de direção
        private bool isWaiting = false; // Controle para saber se a nave está esperando
        
        private bool isMasterShipDestroyed = false;  // Adiciona essa variável
        
        // Nova propriedade para obter a pontuação
        public int ScoreValue => 50 + (_random.Next(0, 6) * 10);

        

        public MasterSpaceship(Canvas canvas)
        {
            _canvas = canvas;
            _random = new Random();

            
            // Inicializa o método que cria a nave mãe
            CreateMasterShip();

            // Timer para mover a nave mãe
            _movementTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(20)
            };
            _movementTimer.Tick += MoveMasterSpaceship;
            _movementTimer.Start();
            
        }

        // Método para criar a nave mãe
        private void CreateMasterShip(){
            MasterShipShape = new Image
            {
                Width = 100,
                Height = 50,
            };

            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri("Images/Main Images/Master Spaceship/master-spaceship(1).png", UriKind.Relative);
            
            bitmapImage.EndInit();

            MasterShipShape.Source = bitmapImage;
            _canvas.Children.Add(MasterShipShape);

            _canvas.SizeChanged += (s, e) =>
            {
                Canvas.SetTop(MasterShipShape, 35);
                Canvas.SetLeft(MasterShipShape, _canvas.ActualWidth); // Começa à direita da tela
            };

            lastDirectionChangeTime = DateTime.Now;
        }
        
        private void MoveMasterSpaceship(object sender, EventArgs e){
            if (isWaiting)
            {
                if ((DateTime.Now - lastDirectionChangeTime).TotalSeconds >= 10)
                {
                    isWaiting = false;
                    lastDirectionChangeTime = DateTime.Now;
                }
                return; // Não move a nave enquanto está esperando
            }

            double newLeft = Canvas.GetLeft(MasterShipShape);

            if (MasterShipShape.Visibility == Visibility.Collapsed)
            {
                return; // Se a nave foi atingida, não move até aparecer novamente
            }

            if (moveRight)
            {
                newLeft += 4;

                if (newLeft >= _canvas.ActualWidth)
                {
                    moveRight = false;
                    Canvas.SetLeft(MasterShipShape, _canvas.ActualWidth);
                    isWaiting = true;
                    lastDirectionChangeTime = DateTime.Now;
                }
            }
            else
            {
                newLeft -= 4;

                if (newLeft <= -MasterShipShape.Width)
                {
                    moveRight = true;
                    Canvas.SetLeft(MasterShipShape, -MasterShipShape.Width);
                    isWaiting = true;
                    lastDirectionChangeTime = DateTime.Now;
                }
            }

            Canvas.SetLeft(MasterShipShape, newLeft);
        }
        
        // Função que é chamada quando a nave mãe é atingida por um tiro
        public void HitByPlayerShot() {
            if (isMasterShipDestroyed) return; // Se a nave mãe já foi destruída, não faz nada

            isMasterShipDestroyed = true;  // Marca a nave como destruída
            
            // Temporariamente oculta a nave e depois a reposiciona fora da tela
            MasterShipShape.Visibility = Visibility.Collapsed;
            Canvas.SetTop(MasterShipShape, -MasterShipShape.Height); // A nave vai para fora da tela (acima)

            // Inicia o respawn da nave após 10 segundos
            if (_respawnTimer == null)
            {
                _respawnTimer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromSeconds(10)
                };
                _respawnTimer.Tick += RespawnMasterSpaceship;
                _respawnTimer.Start();
            }
            
        }
        
        
        // Método que cria uma nova nave mãe após 10 segundos
        private void RespawnMasterSpaceship(object sender, EventArgs e)
        {
            // Para o timer de respawn
            _respawnTimer.Stop();
            _respawnTimer = null;
        
            // Cria a nova nave mãe
            CreateMasterShip();
        
            // Reposiciona a nave fora da tela (no lado oposto de onde estava)
            if (moveRight)
            {
                // A nova nave começa à direita da tela (fora da tela à direita)
                Canvas.SetLeft(MasterShipShape, _canvas.ActualWidth);
            }
            else
            {
                // A nova nave começa à esquerda da tela (fora da tela à esquerda)
                Canvas.SetLeft(MasterShipShape, -MasterShipShape.Width);
            }
            
            // Define a posição Y para garantir o alinhamento correto
            Canvas.SetTop(MasterShipShape, 35);  // Alinha no eixo Y
            
            // Define a nave visível e inicia o movimento novamente
            MasterShipShape.Visibility = Visibility.Visible;
            _movementTimer.Start();
        }
        
        public void StopMovement()
        {
            if (_movementTimer != null && _movementTimer.IsEnabled)
            {
                _movementTimer.Stop();  // Para o timer de movimento
            }
        }

        
    }
    
}

