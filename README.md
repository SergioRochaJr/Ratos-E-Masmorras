# Ratos-E-Masmorras
Projeto para desenvolvimento de jogo dungeon crawler

1. Tema
	Ratos & Masmorras será um RPG estilo dungeon crawler em pixel art, no qual o jogador assumirá controle de um rato explorador em busca do queijo divino, um item mítico que supostamente está no ponto mais profundo do esgoto.

2. Objetivo
	O objetivo de nosso projeto é criar um protótipo de dungeon crawler de turnos, no qual o jogador selecionará a classe de seu personagens e desbravará masmorras, coletando itens que o auxiliarão no processo. Ao final do desenvolvimento gostaríamos de apresentar:
•	02 classes jogáveis
•	Ao menos uma masmorra jogável
•	Mecânicas de combate funcionais
•	Sistema de experiência com aumento de nível e atributos

3. Aspectos de Jogabilidade  
   
3.1. Jogadores
Perfil
	Jogadores com gosto para jogos de RPG, mas vários que jogam jogos de ação podem se encaixar no perfil também.
Modo de Jogo
	RPG por turnos onde o personagem ao subir de nível ganha novas habilidades e poderes, que auxiliam no andamento do jogo.
Personagem
	O jogo possuirá um único personagem, mas com diferentes habilidades possíveis de evoluir, ou seja, ele poderá fazer papeis distintos conforme você avança e evolui no jogo.

3.2. Mecânicas  
•	Desafios:
	O jogador terá de manejar seus recursos, sejam eles itens no inventário ou outros sistemas como pontos de vida ou de mana, para superar as masmorras que adentrar.
•	Sorte:
	Os itens encontrados na masmorra podem variar de piso para piso e a taxa de sucesso dos golpes dos personagens e inimigos não será sempre certeira, portanto ter sorte para conseguir bons itens e acertar/desviar dos golpes poderá ser um diferencial que trará variedade para as jogatinas.
•	Aquisição de recursos:
	O jogador poderá coletar itens nas masmorras, que poderão ser usados em sua jornada. Além disso, com um sistema de aumento de nível, o jogador também obterá experiência ao enfrentar inimigos, precisando avaliar se será melhor enfrentar ou fugir das lutas.
•	Recompensas:
	Ao vencer inimigos o jogador receberá experiência, que o permitirá aumentar seu nível, recebendo atributos e novas habilidades.
•	Turnos:
	Cada ação do jogador, seja de movimento ou ataque, passará seu turno, fazendo com que inimigos no mapa possam agir.  
 
3.3. Dinâmicas  
Níveis:
	O jogador receberá pontos de experiência ao derrotar inimigos. Ao juntar determinada quantidade de pontos, o personagem aumentará de nível, recebendo atributos e novas habilidades.
Novos desafios:
	Progredir nas masmorras trará novos inimigos, itens e biomas a serem superados e explorados.  
 
3.4. Estéticas
Conquista:
	Completar masmorras e derrotar inimigos trará ao jogador a sensação de superação.
Descobertas:
	O jogador descobrirá novos biomas conforme completa ou se aprofunda em masmorras.
Realização:
	Combinar o uso inteligente do layout do mapa com as habilidades e itens disponíveis para vencer inimigos poderosos ou conservar HP trará ao jogador a sensação de melhora e de progresso no entendimento das mecânicas.  
 
3.5. Componentes  
Menu:
	A qualquer momento na masmorra o jogador poderá acessar um menu que conterá as informações do personagem como atributos e habilidades, os itens no inventário e seus usos e informações básicas de localização, como o piso e a masmorra no qual o personagem se encontra.
Inventário e itens:
	O jogador poderá coletar itens pelas masmorras, que serão adicionados ao seu inventário e poderão ser utilizados para ativar seus efeitos. Itens poderão ter efeitos diversos e irão interagir com o personagem do jogador ou com inimigos.
Personagens e classes:
	O jogador controlará um rato que poderá ser da classe guerreiro ou um mago. O personagem iniciará a masmorra em nível 1, e poderá aumentar seu nível conforme enfrenta inimigos. Os atributos e habilidades do personagem irão variar de acordo com sua classe.
Masmorras:
	A maior parte do tempo o jogador estará em masmorras, onde enfrentará inimigos e conseguirá itens. As masmorras terão andares limitados de acordo com sua dificuldade, e seus layouts serão únicos. Inimigos e itens estarão dispostos ao longo das masmorras.
Inimigos:
	NPCs inimigos estarão dispostos ao longo da masmorra e tentarão atacar ou impedir o jogador de prosseguir de acordo com seus padrões de comportamento. Eles terão seus próprios atributos e concederão experiência caso sejam derrotados.  
 
3.6. Regras do Jogo  
Ações:
	O jogador poderá escolher uma ação por turno, que será seguida por uma ação inimiga. As ações consistem em mover, atacar, usar habilidade e usar item.
Condições de vitória:
	Encontrar a saída de cada masmorra e, caso haja, vencer o chefe do mapa.
Condições de derrota:
	O jogador perde caso tenha seus pontos de vida esgotados.
