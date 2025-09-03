# MotoManager

MotoManager � uma solu��o completa para gerenciamento de aluguel de motocicletas, desenvolvida em .NET 9. O projeto segue uma arquitetura modular, separando responsabilidades em diferentes camadas para 
facilitar manuten��o, escalabilidade e testes.

## Sum�rio
- [Como rodar](#como-rodar)
- [Estrutura do Projeto](#estrutura-do-projeto)
- [Principais Tecnologias Utilizadas](#principais-tecnologias-utilizadas)
- [Justificativa das Escolhas](#justificativa-das-escolhas)
- [Melhorias Futuras](#melhorias-futuras)
- [Status do Projeto](#status-do-projeto)

---

## Como rodar

O projeto utiliza Docker Compose para facilitar a execu��o dos servi�os e depend�ncias. Certifique-se de ter o Docker e o Docker Compose instalados.

```bash
git clone https://github.com/joaopjk/secret.git
cd secret
docker compose up --build
```

A API estar� dispon�vel em `http://localhost:5000` (ou porta configurada no docker-compose).

---

## Estrutura do Projeto

O MotoManager � dividido em m�ltiplos projetos, cada um com uma responsabilidade espec�fica:

- **MotoManager.Api**: Camada de apresenta��o, exp�e endpoints REST para intera��o com o sistema.
- **MotoManager.Application**: Camada de aplica��o, respons�vel pelas regras de neg�cio e orquestra��o dos casos de uso.
- **MotoManager.Domain**: Define as entidades, agregados e interfaces do dom�nio, seguindo DDD.
- **MotoManager.Infrastructure**: Implementa o acesso a dados, reposit�rios e integra��es externas.
- **MotoManager.Shared**: Cont�m utilit�rios e classes compartilhadas entre os projetos.
- **MotoManager.IoC**: Gerencia a inje��o de depend�ncias e configura��es dos servi�os.
- **MotoManager.Worker.ShowYear**: Servi�o de background para tarefas agendadas relacionadas ao ano das motocicletas.
- **MotoManager.Worker.MotocycleCreate**: Worker para cria��o autom�tica de motocicletas.
- **MotoManager.UnitTests**: Testes unit�rios para garantir a qualidade das regras de neg�cio.
- **MotoManager.IntegrationTests**: Testes de integra��o para validar o funcionamento entre as camadas.

---

## Principais Tecnologias Utilizadas

- **.NET 9**: Plataforma principal para desenvolvimento dos servi�os.
- **Worker Services**: Para execu��o de tarefas em background.
- **Docker & Docker Compose**: Para orquestra��o e execu��o dos servi�os em containers.
- **Entity Framework Core** (se aplic�vel): ORM para persist�ncia de dados.
- **xUnit/Moq/TestContainer**: Frameworks de teste automatizado.
- **Inje��o de Depend�ncia (IoC)**: Para desacoplamento e melhor testabilidade.

---

## Justificativa das Escolhas

- **Arquitetura em Camadas**: Facilita manuten��o, testes e escalabilidade, separando responsabilidades.
- **Worker Services**: Permite execu��o de tarefas recorrentes ou agendadas sem impactar a API principal.
- **Docker Compose**: Simplifica o setup do ambiente, tornando o projeto facilmente execut�vel em qualquer m�quina.
- **Testes Automatizados**: Garantem qualidade e evitam regress�es.
- **Inje��o de Depend�ncia**: Facilita o desacoplamento entre componentes e a troca de implementa��es.

---

## Melhorias Futuras

- Implementa��o de autentica��o e autoriza��o.
- Melhor tratamento de erros e logs. O Middleware de tratamento de erros pode ser aprimorado(melhoria na padroniza��o das respostas de erro)
- Otimiza��o dos workers e escalabilidade dos servi�os. Deve ser implementado mecanismo de circuito de falha (circuit breaker) para melhorar a resili�ncia dos servi�os.
- Utilizar Aggregate Roots para garantir a integridade dos dados no dom�nio(n�o foram utilizados em todos os casos)
- Implementa��o de seguran�a robusta
	- CORS
	- Security Headers
	- Fail Fast

---

## Status do Projeto

**Disclaimer:** Algumas melhorias ainda podem ser implementadas para tornar o sistema mais robusto e seguro. A �ltima parte do projeto, referente � altera��o da data prevista de t�rmino do aluguel da 
motocicleta, **n�o foi finalizada**.

---

## Licen�a

Este projeto est� sob a licen�a MIT.
