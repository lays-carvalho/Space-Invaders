using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace SpaceInvadersEP;

public class Shield
{
    public Image ShieldImage { get; set; }
    public int Health { get; set; } = 5; // Cada escudo tem 5 vidas
    public double XPosition { get; set; }
    public double YPosition { get; set; }
    
    private Canvas GameCanvas;  // Armazenando a referência do GameCanvas

    private List<Shield> ShieldsList; // Armazenando a lista de escudos
    
    public Shield(double x, double y, Canvas gameCanvas, List<Shield> shieldsList)
    {
        XPosition = x;
        YPosition = y;
        GameCanvas = gameCanvas;
        ShieldsList = shieldsList;

        // Caminho da imagem do escudo
        string imagePath = @"C:\Rider\SpaceInvadersEP\SpaceInvadersEP\Images\Main Images\shield1.png";
            
        ShieldImage = new Image
        {
            Width = 90, // Largura do escudo
            Height = 50, // Altura do escudo
            Source = new BitmapImage(new Uri(imagePath, UriKind.Absolute))
        };

        // Define a posição do escudo no Canvas
        Canvas.SetLeft(ShieldImage, XPosition);
        Canvas.SetTop(ShieldImage, YPosition);
    }

    // Método para receber um tiro
    public void TakeDamage()
    {
        Health--;
        if (Health <= 0)
        {
            // Se o escudo foi destruído, remover do Canvas e da lista de escudos
            GameCanvas.Children.Remove(ShieldImage);  // Remove o escudo do Canvas
            ShieldsList.Remove(this);  // Remove o escudo da lista
        }
    }
}
