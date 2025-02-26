namespace SpaceInvadersEP.Game;

public class ControlLives
{
    public int Vidas { get; private set; }
    public int Pontos { get; private set; }  // A variável para armazenar os pontos
    public event Action GameOver;
    public event Action VidasAlteradas;


    public ControlLives()
    {
        // O jogador começa com 3 vidas e 0 pontos
        Vidas = 3;
        Pontos = 0;
    }
    
    public void PerderVida()
    {
        if (Vidas > 0)
        {
            Vidas--; // Diminui a vida
            VidasAlteradas?.Invoke(); // Dispara o evento para atualizar a interface
        }
    
        // Verifica se as vidas chegaram a 0 e chama o evento GameOver
        if (Vidas == 0)
        {
            GameOver?.Invoke(); // Invoca o evento Game Over
        }
    }
    
    // Lógica para ganhar pontos
    public void AdicionarPontos(int pontosGanhos)
    {
        Pontos += pontosGanhos;
        // VerificarGanhoDeVida(); // Verifica se o jogador ganhou uma vida
    }
    
}