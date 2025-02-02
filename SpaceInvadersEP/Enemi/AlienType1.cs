using System.Windows.Controls;

namespace SpaceInvadersEP.Enemi;

public class AlienType1 : Alien
{
    public AlienType1(double x, double y, Canvas gameCanvas) : base(x, y, @"C:\Rider\SpaceInvadersEP\SpaceInvadersEP\Images\Main Images\alien 1.png", gameCanvas)
    {
        Value = 10;  // Valor do alien tipo 1
        CanShoot = false;  // Não atira
    }

    
}