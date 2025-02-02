namespace SpaceInvadersEP.Game;

public class CounterModel
{
    public int Pontuacao { get; set; }

    public void IncrementarPontuacao(int valor)
    {
        Pontuacao += valor;
    }
}