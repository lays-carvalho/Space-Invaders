using System.Windows.Controls;

namespace SpaceInvadersEP.Enemi;

public class AlienType3 : Alien
{
    public AlienType3(double x, double y, Canvas gameCanvas) : base(x, y, @"Images\Main Images\alien 3.png", gameCanvas)
    {
        Value = 40;  // Valor do alien tipo 3
        CanShoot = true;  // Este alien atira
    }

    public void Shoot(List<Bullet> alienBullets)
    {
        if (CanShoot)
        {
            if (AlienShape != null && gameCanvas != null)
            {
                // Cria um novo tiro na posição do alien (movendo para baixo)
                Bullet newBullet = new Bullet(
                    Canvas.GetLeft(AlienShape) + (AlienShape.Width / 2) - 2,
                    Canvas.GetTop(AlienShape) + AlienShape.Height,
                    gameCanvas.ActualHeight,
                    false // Indica que é um tiro do alien
                );
                alienBullets.Add(newBullet);
                gameCanvas.Children.Add(newBullet.BulletShape); // Adiciona o tiro ao Canvas
            }
        }
    }
}