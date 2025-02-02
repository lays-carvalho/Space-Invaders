using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SpaceInvadersEP;

public class Bullet
{
    public Rectangle BulletShape { get; set; }
    public double Speed { get; set; } = 10;

    public Bullet(double x, double y)
    {
        // Cria o tiro (um retângulo amarelo)
        BulletShape = new Rectangle
        {
            Width = 5,
            Height = 10,
            Fill = Brushes.GreenYellow
        };

        // Coloca o tiro na posição (x, y)
        Canvas.SetLeft(BulletShape, x);
        Canvas.SetTop(BulletShape, y);
    }
    
    public void Move()
    {
        // Move o tiro para cima
        double currentTop = Canvas.GetTop(BulletShape);
        Canvas.SetTop(BulletShape, currentTop - Speed);

        // Verifica se o tiro saiu da tela
        if (currentTop <= 0)  // Se o topo do tiro estiver acima do topo da tela
        {
            // Destrói o tiro se ele passar do topo da tela
            BulletShape.Visibility = System.Windows.Visibility.Collapsed;
        }
    }
}