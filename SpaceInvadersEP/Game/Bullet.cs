using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SpaceInvadersEP;

public class Bullet
{
    public Rectangle BulletShape { get; set; }
    public double Speed { get; set; } = 10;
    public bool IsPlayerBullet { get; set; }
    private double WindowHeight { get; } // Armazena a altura da janela

    public Bullet(double x, double y, double windowHeight, bool isPlayerBullet = true)
    {
        BulletShape = new Rectangle
        {
            Width = 5,
            Height = 10,
            Fill = isPlayerBullet ? Brushes.GreenYellow : Brushes.Red
        };

        Canvas.SetLeft(BulletShape, x);
        Canvas.SetTop(BulletShape, y);

        WindowHeight = windowHeight; // Armazena a altura da janela
        IsPlayerBullet = isPlayerBullet;
    }

    public void Move()
    {
        if (BulletShape == null)
        {
            return; // Sai do método se BulletShape for nulo
        }

        double currentTop = Canvas.GetTop(BulletShape);

        if (IsPlayerBullet)
        {
            // Move o tiro do jogador para cima
            Canvas.SetTop(BulletShape, currentTop - Speed);
        }
        else
        {
            // Move o tiro do alien para baixo
            Canvas.SetTop(BulletShape, currentTop + Speed);
        }

        // Verifica se o tiro saiu da tela
        if (currentTop <= 0 || currentTop >= WindowHeight)
        {
            // Destrói o tiro se ele sair da tela
            BulletShape.Visibility = Visibility.Collapsed;
        }
    }
}