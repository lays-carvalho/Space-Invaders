using System.Media;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace SpaceInvadersEP
{
    public class Player
    {
        public Image Ship { get; set; }
        public double Speed { get; set; } = 10;
        private SoundPlayer shootSoundPlayer;  // Player para o som de disparo

        public Player()
        {
            // Caminho absoluto para a imagem da nave
            string imagePath = @"Images\Main Images\player.png"; // Caminho absoluto para o arquivo da imagem

            // Cria a nave usando a imagem
            Ship = new Image
            {
                Width = 50, // Ajuste a largura da nave
                Height = 30, // Ajuste a altura da nave
                Source = new BitmapImage(new Uri(imagePath, UriKind.Relative)) // Usando Uri absoluto
            };

            // Inicializa o player de som
            shootSoundPlayer = new SoundPlayer(@"Sounds\shoot.wav");  // Certifique-se de fornecer o caminho correto para o arquivo de som
        }

        // Método para mover a nave
        public void Move(double dx, double canvasWidth)
        {
            // Obtém a posição atual da nave
            double currentLeft = Canvas.GetLeft(Ship);

            // Calcula a nova posição da nave
            double newLeft = currentLeft + dx;

            // Limita o movimento para que a nave não ultrapasse as bordas
            if (newLeft < 0)
            {
                // Se a nave tentar ultrapassar a borda esquerda, posiciona ela na borda
                newLeft = 0;
            }
            else if (newLeft > canvasWidth - Ship.Width)
            {
                // Se a nave tentar ultrapassar a borda direita, posiciona ela na borda
                newLeft = canvasWidth - Ship.Width;
            }

            // Atualiza a posição da nave
            Canvas.SetLeft(Ship, newLeft);
        }

        // Método para reproduzir o som de disparo
        public void PlayShootSound()
        {
            shootSoundPlayer.Play();  // Reproduz o som de disparo
        }
    }
}
