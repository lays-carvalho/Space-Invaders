# 🚀 Space Invaders - Bogotá 🛸

Bem-vindo ao **Space Invaders - ALiEn Doom**, uma recriação moderna do clássico jogo Space Invaders, desenvolvido em **C#** como um projeto desktop. Este jogo foi criado como parte de um trabalho em grupo para a disciplina de Programação 3, combinando nostalgia com novas funcionalidades para proporcionar uma experiência divertida e desafiadora.

---

## 🎮 Sobre o Jogo

No **Space Invaders - ALiEn Doom**, você controla uma nave espacial para derrotar ondas de inimigos alienígenas. O objetivo é acumular o máximo de pontos enquanto sobrevive às investidas dos adversários. Conforme avança, a dificuldade aumenta, tornando o jogo cada vez mais desafiador.



## 🖼️ Imagens do Jogo

Imagens da *Tela inicial*
![Sprite title](https://gitlab.com/jala-university1/cohort-3/oficial-pt-programa-o-3-cspr-231.ga.t1.25.m1/se-o-b/bogota2-group/spaceinvaders-bogota/-/raw/feature/absolute-to-relative/SpaceInvadersEP/Images/title.png?ref_type=heads)
*Título*

![Tela de Jogo](https://gitlab.com/jala-university1/cohort-3/oficial-pt-programa-o-3-cspr-231.ga.t1.25.m1/se-o-b/bogota2-group/spaceinvaders-bogota/-/raw/feature/absolute-to-relative/SpaceInvadersEP/Images/backgroundInicial.png?ref_type=heads)  
*Background Inicial*


---

## 🛠️ Funcionalidades 

### Implementadas
- **Movimentação do Jogador**: 

    - ![Sprite da Nave](https://gitlab.com/jala-university1/cohort-3/oficial-pt-programa-o-3-cspr-231.ga.t1.25.m1/se-o-b/bogota2-group/spaceinvaders-bogota/-/raw/feature/absolute-to-relative/SpaceInvadersEP/Images/Main%20Images/player.png?ref_type=heads)
    - O jogador se move para a esquerda e direita usando as setas do teclado (← e →).
    - O jogador atira projéteis pressionando a barra de espaço.
- **Ataque Alienígena**: 

    - As naves alienígenas são destruídas ao serem atingidas pelos projéteis do jogador.
    - Cada nave destruída adiciona pontos à pontuação do jogador.

-  **Sistema de Pontuação**:
    - <img src="https://gitlab.com/jala-university1/cohort-3/oficial-pt-programa-o-3-cspr-231.ga.t1.25.m1/se-o-b/bogota2-group/spaceinvaders-bogota/-/raw/feature/absolute-to-relative/SpaceInvadersEP/Images/Main%20Images/alien%201.png?ref_type=heads" alt="Sprite Alien" width="30" />: 10 pontos.
    - <img src="https://gitlab.com/jala-university1/cohort-3/oficial-pt-programa-o-3-cspr-231.ga.t1.25.m1/se-o-b/bogota2-group/spaceinvaders-bogota/-/raw/feature/absolute-to-relative/SpaceInvadersEP/Images/Main%20Images/alien%202.png?ref_type=heads" alt="Sprite Alien" width="30" />: 20 pontos.
    - <img src="https://gitlab.com/jala-university1/cohort-3/oficial-pt-programa-o-3-cspr-231.ga.t1.25.m1/se-o-b/bogota2-group/spaceinvaders-bogota/-/raw/feature/absolute-to-relative/SpaceInvadersEP/Images/Main%20Images/alien%203.png?ref_type=heads" alt="Sprite Alien" width="30
  " />: 40 pontos.

    - O jogo termina ao atingir ***500 pontos***.

- **Blocos de Proteção**: 

    - ![Sprite Shield](https://gitlab.com/jala-university1/cohort-3/oficial-pt-programa-o-3-cspr-231.ga.t1.25.m1/se-o-b/bogota2-group/spaceinvaders-bogota/-/raw/feature/absolute-to-relative/SpaceInvadersEP/Images/Main%20Images/shield1.png?ref_type=heads) 
    
    - O bloco de proteção (escudo) pode ser destruído pelo jogador.

    - O escudo suporta até 5 tiros antes de ser completamente destruído.

- **Tela Inicial**: 
    - Opção para iniciar um novo jogo.
    - Instruções sobre os controles (movimentação e disparo).
    - Quadro de Líderes (melhores pontuações)
- **Finalização**: O jogo termina quando o jogador perde todas as vidas ou os alienígenas alcançam sua nave.
---

### 🎯 Próximos Passos

- **Naves "mãe"**: 
    - Adicionar naves alienígenas vermelhas que se movem aleatoriamente (esquerda/direita) e saem do quadro.
    - Elas aparecem 1 vez a cada 2 minutos e concedem pontos variados ao serem destruídas.

- **Termino do Jogo** 
    
     O jogo deve terminar quando:
    - O jogador perde todas as vidas.
    - As naves alienígenas alcançam o jogador.

- **Blocos de Proteção**: 
    - Serão 4 blocos de proteção que mudam de cor conforme recebem dano.

- **Movimentação das Naves Alienígenas**: 
    - As naves se movem da esquerda para a direita e descem uma posição ao atingir a borda.
    - A velocidade do movimento e dos disparos aumenta a cada onda.

- **Sistema de Vidas**: 
    - A cada 1000 pontos, o jogador ganha uma vida extra (máximo de 6 vidas).

---

## 🖥️ Requisitos do Sistema

- **Sistema Operacional**: Windows, macOS ou Linux.
- **Dependências**: .NET Framework ou .NET Core.

---

## 🚀 Como Jogar

1. **Iniciar**: Selecione "Start Game" no menu principal
2. **Comandos**:
   - **Setas Esquerda/Direita**: Controle o movimento da nave.
   - **Barra de Espaço**: Atira nos inimigos.
3. **Missão**: Eliminar todos os inimigos ​​antes que invadam sua nave ou cheguem até você.
4. **Desempenho**: Registre sua pontuação ao final e tente superar seus próprios recordes.

---

## 🗂️ Estrutura do Projeto

O projeto está organizado da seguinte forma:

- **SpaceInvadersEP/**
  - **Enemi/** (Gerencia os inimigos do jogo)
    - `Alien.cs` - Classe base para os alienígenas.
    - `AlienType1.cs` - Implementação do primeiro tipo de alienígena.
    - `AlienType2.cs` - Implementação do segundo tipo de alienígena.
    - `AlienType3.cs` - Implementação do terceiro tipo de alienígena.
  - **Game/** (Elementos principais do jogo)
    - `Bullet.cs` - Gerenciamento dos projéteis disparados.
    - `Shield.cs` - Implementação do escudo de defesa.
  - **Images/** (Armazena as imagens utilizadas no jogo)
    - **Main Images/** (Sprites do jogo)
      - `alien 1.png` - Imagem do primeiro tipo de alienígena.
      - `alien 2.png` - Imagem do segundo tipo de alienígena.
      - `alien 3.png` - Imagem do terceiro tipo de alienígena.
      - `player.png` - Imagem do jogador.
      - `shield1.png` - Imagem do escudo.
      - `backgroundInicial.png` - Imagem do fundo inicial do jogo.
      - `title.png` - Imagem do título do jogo.
  - **Player/** (Gerencia o jogador)
    - `Player.cs` - Classe do jogador e suas interações.
  - **ScoreGame/** (Gerencia a pontuação do jogo)
    - `CounterModel.cs` - Modelo de contagem de pontuação.
    - `CounterViewModel.cs` - ViewModel para exibição da pontuação.
  - `App.xaml` - Configuração inicial da aplicação.
  - `AssemblyInfo.cs` - Informações do projeto.
  - **Janelas da Interface Gráfica**
    - `GameControlsWindow.xaml` - Janela com controles do jogo.
    - `GameWindow.xaml` - Janela principal do jogo.
    - `LeaderBoardWindow.xaml` - Janela do placar de líderes.
    - `MainWindow.xaml` - Janela inicial do jogo.
  - **Code-behind das janelas**
    - `GameWindow.xaml.cs` - Lógica da janela principal do jogo.
    - `LeaderBoardWindow.xaml.cs` - Lógica da janela de placar.
    - `MainWindow.xaml.cs` - Lógica da janela inicial.

**Estrutura Mermaid**
![Mermaid](https://mermaid.ink/img/pako:eNqNlP9u2jAQx1_F8t8UhYRAyaRJ9IdQplWqRNtJG1N1TY7g1rEjJ6ylwMNM-2MP0hfbOSmBTRCWv-58nzt_z754ySMdIw94YiCbsZuLDxPF6BtnEGGofkCMJr-8ZicnH1cjNKgiAUznTCiRikTnK3apMBVVUmlalA2lQNWO8r3rN4sMO01BtynolcHDKi8lpqgK0hhr9qgTvWIjSLFKsFZZ7mwuJRb1PvX6eCZQxke2GJoUXlEBEykkqOgQQmu816rsstoVCFW53ybcOhuQf6_YLbBtskMsWOO-085U0si6NeseZb2a9ZrYawkLNGGaEJ6VdhNdnVdF56XdKPosCZWgEZKEP0D0lBg9V_H7WlPijSgkVtsU1qzZ_xlXOwYQa7N6b67Kqeydno_cel0PWKZVMYe332-_aLrGkTa4HbHaLSufU38Fmiv6yWQ9bXuRO4HPW-ygDDbMsvYLpLKByHNMH-QiVFN9pKdPoFAC_SrAQitiSggbmbefUxHBit2GVeptWBa2ks-pdaNl_kWoWD_vKNlhDsU-o937TIOJDyH22v-K7UZX53Q-Jw84I4Ak5-yxkm-F3tvYBi6dfXLqK9hl9svai_4jr2R4i6dIL4KI6Rld2hya0Rk9QhMekBmDeZrwiVoTB_NCjxcq4kFh5tjiNP3JjAdTkDl58yyGAi8E0FucbpAM1Fetd10eLPkLD9zuoO25Xq_rO13f7Z76Lb7gQWfQa_f6Ts93Op4z8F2vu27x17KA2-47brc3cPy-c-oPvPUfCWfv6Q?type=png)

---
