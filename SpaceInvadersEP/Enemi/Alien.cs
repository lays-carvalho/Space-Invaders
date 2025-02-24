using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace SpaceInvadersEP.Enemi;

public class Alien
{
    public Image AlienShape { get; set; }
    public double Speed { get; set; } = 5;  // Velocidade de movimento dos alienígenas
    public double Value { get; set; }  // Valor do alienígena quando destruído
    public bool CanShoot { get; set; }  // Se o alienígena pode atirar

    protected Canvas gameCanvas;  // Para armazenar a referência do Canvas

    public Alien(double x, double y, string imagePath, Canvas canvas)
    {
        AlienShape = new Image
        {
            Width = 50,
            Height = 30,
            Source = new BitmapImage(new Uri(imagePath, UriKind.Relative))
        };

        gameCanvas = canvas;  // Inicializa a referência do Canvas
        Canvas.SetLeft(AlienShape, x);
        Canvas.SetTop(AlienShape, y);
    }

    public virtual void Move()
    {
        // Movimento básico para todos os aliens (pode ser sobrescrito)
        double currentLeft = Canvas.GetLeft(AlienShape);
        Canvas.SetLeft(AlienShape, currentLeft + Speed);
    }

    public virtual void Destroy()
    {
        try
        {
            // Verifica se o alien já foi removido da tela
            if (AlienShape != null && gameCanvas.Children.Contains(AlienShape))
            {
                // Remove o alien da lista de aliens
                GameWindow gameWindow = Application.Current.MainWindow as GameWindow;
                if (gameWindow != null)
                {
                    gameWindow.aliens.Remove(this); // Remove o alien da lista de aliens
                }

                // Remove o alien do Canvas
                gameCanvas.Children.Remove(AlienShape);

                // Liberar recursos da imagem
                AlienShape.Source = null;
                AlienShape = null; // Remove a referência ao objeto de imagem
            }
        }
        catch (Exception ex)
        {
            // Log ou tratamento da exceção
            MessageBox.Show($"Erro ao destruir alien: {ex.Message}");
        }
    }
}