using System.Windows.Controls;

namespace SpaceInvadersEP.Enemi;

public class AlienType3 : Alien
{
    public AlienType3(double x, double y, Canvas gameCanvas) : base(x, y, @"Images\Main Images\alien 3.png", gameCanvas)
    {
        Value = 40;  // Valor do alien tipo 3
        CanShoot = true;  // Este alien atira
    }
    

    // Lógica de disparo do AlienType3 pode ser colocada aqui
    public void Shoot()
    {
        if (CanShoot)
        {
            // Lógica para criar um tiro do alien
            
        }
    }
}