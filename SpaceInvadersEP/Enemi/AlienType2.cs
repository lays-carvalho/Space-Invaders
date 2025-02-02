using System.Windows.Controls;

namespace SpaceInvadersEP.Enemi;

public class AlienType2 : Alien
{
    public AlienType2(double x, double y, Canvas gameCanvas) : base(x, y, @"C:\Rider\SpaceInvadersEP\SpaceInvadersEP\Images\Main Images\alien 2.png", gameCanvas)
    {
        Value = 20;  // Valor do alien tipo 2
        CanShoot = false;  // Não atira
    }

    
}