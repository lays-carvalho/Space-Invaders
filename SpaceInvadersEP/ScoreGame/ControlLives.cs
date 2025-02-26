namespace SpaceInvadersEP.Game
{
    public class ControlLives
    {
        private int ultimoPontoProcessado; // Para controlar o último múltiplo de 200 pontos processado

        public int Vidas { get; private set; }
        public int Pontos { get; private set; }
        public event Action GameOver;
        public event Action VidasAlteradas;

        public ControlLives()
        {
            Vidas = 3; // O jogador começa com 3 vidas
            Pontos = 0; // O jogador começa com 0 pontos
            ultimoPontoProcessado = 0; // Inicializa com 0, garantindo que a primeira verificação funcione
        }

        public void PerderVida()
        {
            if (Vidas > 0)
            {
                Vidas--; // Diminui a vida
                VidasAlteradas?.Invoke(); // Dispara o evento para atualizar a interface
            }

            if (Vidas == 0)
            {
                GameOver?.Invoke(); // Invoca o evento Game Over
            }
        }

        public void VerificarGanhoDeVida()
        {
            // A vida só será incrementada quando a diferença de pontos ultrapassar 1000
            if (Pontos - ultimoPontoProcessado >= 1000 && Vidas < 6)
            {
                Vidas++; // Incrementa uma vida
                ultimoPontoProcessado = Pontos; // Atualiza o último ponto processado
                VidasAlteradas?.Invoke(); // Dispara o evento para atualizar a interface
            }
        }

        // Lógica para ganhar pontos
        public void AdicionarPontos(int pontosGanhos)
        {
            Pontos += pontosGanhos; // Atualiza os pontos
            VerificarGanhoDeVida(); // Verifica se o jogador ganhou uma vida
        }
    }
}