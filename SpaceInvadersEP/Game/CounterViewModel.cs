namespace SpaceInvadersEP.Game;

public class CounterViewModel
{
    public CounterModel Counter { get; set; }

    public CounterViewModel()
    {
        Counter = new CounterModel();
    }

    // Método para chamar quando um alien for destruído
    public void IncrementarPontuacao(int valor)
    {
        Counter.IncrementarPontuacao(valor);
        AtualizarPontuacaoNaView();
    }

    // Métodos para atualizar a View
    private void AtualizarPontuacaoNaView()
    {
        
    }
}