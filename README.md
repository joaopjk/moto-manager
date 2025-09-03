# MotoManager

MotoManager é uma solução completa para gerenciamento de aluguel de motocicletas, desenvolvida em .NET 9. O projeto segue uma arquitetura modular, separando responsabilidades em diferentes camadas para 
facilitar manutenção, escalabilidade e testes.

## Sumário
- [Como rodar](#como-rodar)
- [Estrutura do Projeto](#estrutura-do-projeto)
- [Principais Tecnologias Utilizadas](#principais-tecnologias-utilizadas)
- [Justificativa das Escolhas](#justificativa-das-escolhas)
- [Melhorias Futuras](#melhorias-futuras)
- [Status do Projeto](#status-do-projeto)

---

## Como rodar

O projeto utiliza Docker Compose para facilitar a execução dos serviços e dependências. Certifique-se de ter o Docker e o Docker Compose instalados.

```bash
git clone https://github.com/joaopjk/secret.git
cd secret
docker compose up --build
```

A API estará disponível em `http://localhost:5000` (ou porta configurada no docker-compose).

---

## Estrutura do Projeto

O MotoManager é dividido em múltiplos projetos, cada um com uma responsabilidade específica:

- **MotoManager.Api**: Camada de apresentação, expõe endpoints REST para interação com o sistema.
- **MotoManager.Application**: Camada de aplicação, responsável pelas regras de negócio e orquestração dos casos de uso.
- **MotoManager.Domain**: Define as entidades, agregados e interfaces do domínio, seguindo DDD.
- **MotoManager.Infrastructure**: Implementa o acesso a dados, repositórios e integrações externas.
- **MotoManager.Shared**: Contém utilitários e classes compartilhadas entre os projetos.
- **MotoManager.IoC**: Gerencia a injeção de dependências e configurações dos serviços.
- **MotoManager.Worker.ShowYear**: Serviço de background para tarefas agendadas relacionadas ao ano das motocicletas.
- **MotoManager.Worker.MotocycleCreate**: Worker para criação automática de motocicletas.
- **MotoManager.UnitTests**: Testes unitários para garantir a qualidade das regras de negócio.
- **MotoManager.IntegrationTests**: Testes de integração para validar o funcionamento entre as camadas.

---

## Principais Tecnologias Utilizadas

- **.NET 9**: Plataforma principal para desenvolvimento dos serviços.
- **Worker Services**: Para execução de tarefas em background.
- **Docker & Docker Compose**: Para orquestração e execução dos serviços em containers.
- **Entity Framework Core** (se aplicável): ORM para persistência de dados.
- **xUnit/Moq/TestContainer**: Frameworks de teste automatizado.
- **Injeção de Dependência (IoC)**: Para desacoplamento e melhor testabilidade.

---

## Justificativa das Escolhas

- **Arquitetura em Camadas**: Facilita manutenção, testes e escalabilidade, separando responsabilidades.
- **Worker Services**: Permite execução de tarefas recorrentes ou agendadas sem impactar a API principal.
- **Docker Compose**: Simplifica o setup do ambiente, tornando o projeto facilmente executável em qualquer máquina.
- **Testes Automatizados**: Garantem qualidade e evitam regressões.
- **Injeção de Dependência**: Facilita o desacoplamento entre componentes e a troca de implementações.

---

## Melhorias Futuras

- Implementação de autenticação e autorização.
- Melhor tratamento de erros e logs. O Middleware de tratamento de erros pode ser aprimorado(melhoria na padronização das respostas de erro)
- Otimização dos workers e escalabilidade dos serviços. Deve ser implementado mecanismo de circuito de falha (circuit breaker) para melhorar a resiliência dos serviços.
- Utilizar Aggregate Roots para garantir a integridade dos dados no domínio(não foram utilizados em todos os casos)
- Implementação de segurança robusta
	- CORS
	- Security Headers
	- Fail Fast

---

## Status do Projeto

**Disclaimer:** Algumas melhorias ainda podem ser implementadas para tornar o sistema mais robusto e seguro. A última parte do projeto, referente à alteração da data prevista de término do aluguel da 
motocicleta, **não foi finalizada**.

---

## Licença

Este projeto está sob a licença MIT.
