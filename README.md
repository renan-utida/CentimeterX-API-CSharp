# Centimeter-X API
### High-Precision Positioning as a Service (HPaaS)

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=flat-square&logo=dotnet)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-Web_API-512BD4?style=flat-square&logo=dotnet)
![Oracle](https://img.shields.io/badge/Oracle-Database-F80000?style=flat-square&logo=oracle)
![EF Core](https://img.shields.io/badge/EF_Core-9.0-512BD4?style=flat-square&logo=dotnet)
![Swagger](https://img.shields.io/badge/Swagger-OpenAPI-85EA2D?style=flat-square&logo=swagger)

---

## 1. Integrantes

| Nome | RM |
|---|---|
| Rafael Duarte de Freitas | RM558644 |
| Rafael Gaspar Bragança Martins | RM557228 |
| Luiz Gustavo da Silva | RM558358 |
| Pedro Almeida e Camacho | RM556831 |
| Renan Dias Utida | RM558540 |

**Turma:** 3ESPW - Engenharia de Software  
**Disciplina:** C# Software Development  
**Professor:** Rafael Santos Novo Pereira  
**Semestre:** 1º semestre de 2026

---

## 2. Sobre o projeto

### O problema

O GPS convencional entrega precisão na ordem de metros, o que é suficiente para navegação cotidiana, mas insuficiente para operações que exigem controle milimétrico do espaço físico. Máquinas agrícolas autônomas precisam saber exatamente onde estão para plantar em linhas precisas sem sobreposição. Drones de mapeamento precisam de referência geográfica exata para gerar mapas confiáveis. Veículos de condução autônoma precisam de posicionamento centimétrico para operar com segurança. Equipes de topografia precisam de medições precisas para levantamentos profissionais.

Essa lacuna de precisão representa perdas diretas de produtividade, riscos operacionais e barreiras à adoção de automação em setores estratégicos da economia.

### A solução

O **Centimeter-X** é uma plataforma de **High-Precision Positioning as a Service (HPaaS)** que resolve esse problema combinando sinais de satélites GNSS (GPS, Galileo) com dados de estações terrestres de referência para calcular e entregar correções que transformam a precisão de metros em centímetros. Esse processo se chama **PPP - Precise Point Positioning**.

O operador cadastra seus equipamentos de campo - chamados de **rovers** - e os associa a uma estação base disponível. Ao iniciar uma sessão de correção, a plataforma processa os dados orbitais e retorna a precisão atingida classificada em três níveis:

- **FIX** - precisão centimétrica (≤ 5 cm): nível ideal para operações críticas
- **FLOAT** - precisão decimétrica (≤ 50 cm): nível intermediário
- **SINGLE** - precisão métrica (> 50 cm): sinal ainda convergindo

### Segmentos atendidos

- **Agronegócio de precisão** - tratores e colheitadeiras autônomas com navegação centimétrica
- **Drones de mapeamento** - georreferenciamento preciso para levantamentos aéreos
- **Veículos autônomos** - posicionamento seguro em ambientes urbanos e industriais
- **Topografia profissional** - levantamentos de alta precisão para engenharia civil

---

## 3. Conexão com o tema espacial e ODS

### Por que o Centimeter-X é uma solução espacial

A conexão com a infraestrutura espacial é **intrínseca e não decorativa**: sem os dados transmitidos pelos satélites GNSS em órbita, a correção que define o produto simplesmente não existe. O Centimeter-X é, por definição, a ponte entre infraestrutura orbital e aplicação terrestre - exatamente o que o desafio Space Connect propõe.

Os satélites GNSS transmitem continuamente sinais de tempo e posição a partir de órbitas a aproximadamente 20.000 km de altitude. A técnica PPP utiliza produtos orbitais precisos disponibilizados pela rede IGS (International GNSS Service) e pelo NASA CDDIS - arquivos SP3 (órbitas precisas) e CLK (relógios de satélite) - para aplicar correções que elevam a precisão de metros para centímetros.

### Objetivos de Desenvolvimento Sustentável da ONU

| ODS | Contribuição |
|---|---|
| **ODS 2 - Fome Zero e Agricultura Sustentável** | Viabiliza práticas agrícolas de precisão, reduzindo desperdício de insumos, sobreposição de plantio e perdas operacionais |
| **ODS 9 - Indústria, Inovação e Infraestrutura** | Democratiza o acesso a tecnologia de posicionamento de nível profissional, antes restrita a grandes operações |
| **ODS 11 - Cidades e Comunidades Sustentáveis** | Apoia a operação segura de veículos autônomos em ambientes urbanos e periurbanos, contribuindo para mobilidade mais eficiente |

---

## 4. Tecnologias utilizadas

| Camada | Tecnologia |
|---|---|
| Runtime | .NET 8.0 (LTS) |
| Framework | ASP.NET Core Web API |
| ORM | Entity Framework Core 9 |
| Banco de dados | Oracle (oracle.fiap.com.br:1521/ORCL) |
| Documentação | Swagger / OpenAPI |
| Pacote Oracle | Oracle.EntityFrameworkCore 9.23.60 |
| Pacote EF Tools | Microsoft.EntityFrameworkCore.Tools 9.0.0 |
| IDE | Visual Studio 2022 |
| Versionamento | Git + GitHub |
| Testes de API | Swagger UI + Postman |

---

## 5. Arquitetura e decisões técnicas

### Estrutura de pastas

```
CentimeterX.API/
├── Controllers/
│   ├── EstacoesBaseController.cs
│   ├── RoversController.cs
│   ├── SessoesCorrecaoController.cs
│   ├── OcorrenciasController.cs
│   └── UsuariosController.cs
├── Data/
│   └── AppDbContext.cs
├── Migrations/
│   └── (gerado pelo EF Core)
├── Models/
│   ├── Enums/
│   │   ├── StatusRover.cs
│   │   ├── StatusFix.cs
│   │   ├── TipoOcorrencia.cs
│   │   └── PerfilUsuario.cs
│   ├── Rover.cs                        ← classe abstrata
│   ├── MaquinaAgricola.cs              ← classe filha
│   ├── Drone.cs                        ← classe filha
│   ├── VeiculoAutonomo.cs              ← classe filha
│   ├── EstacaoBase.cs
│   ├── SessaoCorrecao.cs
│   ├── Ocorrencia.cs
│   ├── Usuario.cs
│   └── GnssConstants.cs                ← classe estática
├── Services/
│   ├── ICorrecaoService.cs             ← interface
│   ├── SessaoCorrecaoService.cs
│   ├── RoverService.cs
│   ├── EstacaoBaseService.cs
│   ├── OcorrenciaService.cs
│   └── UsuarioService.cs
├── docs/
│   ├── evidencias.md
│   ├── diagram/
│   │   ├── Diagrama_Centimeter-X_GlobalSolution.drawio
│   │   └── Diagrama_Centimeter-X_GlobalSolution.png
│   ├── postman/
│   │   └── Centimeter-X_API.postman_collection.json
│   ├── prints/
│   │   ├── (prints do Swagger)
│   │   ├── (prints dos testes - 01 ao 59)
│   │   └── (prints do banco de dados Oracle)
│   └── sql/
│       └── Centimeter-X_consultas.sql
├── appsettings.json
├── Program.cs
└── README.md
```

### Hierarquia de herança

`Rover` é a classe abstrata base que representa qualquer equipamento de campo cadastrado na plataforma. Não existe um "rover genérico" no negócio - todo rover é necessariamente um dos três tipos concretos:

- `MaquinaAgricola` - adiciona `ModeloTrator` e `LarguraImplemento`
- `Drone` - adiciona `AutonomiaVoo` e `AltitudeMaxima`
- `VeiculoAutonomo` - adiciona `NivelAutonomia` e `VelocidadeMaxima`

As demais entidades (`SessaoCorrecao`, `Ocorrencia`) se relacionam com `Rover` (a classe mãe), garantindo que o relacionamento segue o princípio correto de herança e que qualquer tipo de rover pode ter sessões e ocorrências associadas.

O discriminador `TIPO_ROVER` armazenado na tabela `CX_ROVER` identifica o tipo concreto de cada registro, seguindo o padrão Table Per Hierarchy (TPH) do EF Core.

### Interface ICorrecaoService

A interface `ICorrecaoService` define o contrato do serviço de correção GNSS com 4 métodos: `IniciarSessao`, `EncerrarSessao`, `ObterPrecisao` e `ClassificarFix`. Ela é implementada por `SessaoCorrecaoService` e injetada no `SessoesCorrecaoController` - o único controller que usa a interface diretamente, não a implementação concreta. Esse desacoplamento permite que a lógica de correção seja substituída ou expandida sem impacto no controller.

### Classe estática GnssConstants

`GnssConstants` centraliza todas as constantes do domínio GNSS utilizadas em múltiplos services:

- `FIX_THRESHOLD_CM = 5.0` e `FLOAT_THRESHOLD_CM = 50.0` - usadas em `SessaoCorrecaoService.ClassificarFix()`
- `LATITUDE_MIN/MAX` e `LONGITUDE_MIN/MAX` - usadas em `OcorrenciaService.ValidarCoordenadas()` e `EstacoesBaseController`
- `STATUS_INATIVIDADE_HORAS = 24` - usada em `RoverService.AtualizarStatusPorUltimaSessao()`

Centralizar essas constantes evita magic numbers espalhados pelo código e torna as regras do negócio GNSS explícitas e auditáveis.

### Decisão de modelagem: PUT removido de Ocorrências

O endpoint `PUT /api/ocorrencias/{id}` foi **removido intencionalmente**. Ocorrências representam eventos adversos registrados em campo - são registros históricos imutáveis. Permitir edição posterior comprometeria a integridade do histórico operacional. Essa decisão demonstra que a modelagem reflete o comportamento real do domínio, não apenas um CRUD genérico.

### Serialização de enums como string

A API está configurada para serializar enums como strings (`"Operador"` em vez de `0`), tornando os requests e responses autoexplicativos. A desserialização é case-insensitive - `"operador"`, `"Operador"` e `"OPERADOR"` são todos aceitos.

---

## 6. Diagrama de classes

![Diagrama de Classes](docs/diagram/Diagrama_Centimeter-X_GlobalSolution.png)

O diagrama completo em formato `.drawio` está disponível em [`docs/diagram/Diagrama_Centimeter-X_GlobalSolution.drawio`](docs/diagram/Diagrama_Centimeter-X_GlobalSolution.drawio).

---

## 7. Instruções de execução

### Pré-requisitos

- Visual Studio 2022 (versão 17.8 ou superior)
- .NET 8.0 SDK
- Oracle SQL Developer (para verificar as tabelas)
- Acesso à rede Oracle FIAP (`oracle.fiap.com.br:1521/ORCL`)

### Passo a passo

**1. Clonar o repositório**
```bash
git clone https://github.com/seu-usuario/CentimeterX.API.git
cd CentimeterX.API
```

**2. Configurar a connection string**

Abra `appsettings.json` e substitua com suas credenciais Oracle FIAP:
```json
{
  "ConnectionStrings": {
    "OracleConnection": "User Id=RMXXXXXX;Password=SUASENHA;Data Source=oracle.fiap.com.br:1521/ORCL"
  }
}
```

**3. Criar as tabelas no banco**

Abra o **Package Manager Console** no Visual Studio (Tools → NuGet Package Manager → Package Manager Console) e execute:
```powershell
Update-Database
```

Isso criará as 5 tabelas no Oracle:
- `CX_ESTACAO_BASE`
- `CX_USUARIO`
- `CX_ROVER`
- `CX_SESSAO_CORRECAO`
- `CX_OCORRENCIA`

**4. Executar a aplicação**

Pressione **F5** no Visual Studio. O Swagger abrirá automaticamente em:
```
https://localhost:7126/swagger
```

**5. Testar os endpoints**

Use o Swagger UI ou importe a collection Postman disponível em `docs/postman/Centimeter-X_API.postman_collection.json`.

---

## 8. Endpoints disponíveis

### Ordem recomendada para testes

Para evitar erros de chave estrangeira, siga esta ordem:

**Cadastro:** `EstacaoBase` → `Usuario` → `Rover` → `SessaoCorrecao` → `Ocorrencia`

**Deleção:** `Ocorrencia` → `SessaoCorrecao` → `Rover` → `Usuario` → `EstacaoBase`

---

### EstacoesBase - `api/estacoes`

Gerencia as estações terrestres de referência GNSS. Uma estação base é o ponto fixo cuja posição precisa é conhecida, ela é o pilar que torna a correção PPP possível. Sem uma estação base online, nenhum rover pode iniciar uma sessão de correção.

| Método | Rota | Descrição |
|---|---|---|
| GET | `/api/estacoes` | Lista todas as estações |
| GET | `/api/estacoes/{id}` | Busca estação por ID |
| GET | `/api/estacoes/proximidade?latitude=&longitude=` | Lista estações ordenadas por proximidade geográfica |
| POST | `/api/estacoes` | Cadastra nova estação |
| PUT | `/api/estacoes/{id}` | Atualiza dados da estação |
| DELETE | `/api/estacoes/{id}` | Remove estação |

---

#### `POST /api/estacoes` - Cadastrar estação base

**Request:**
```json
{
  "codigo": "EST-SP-01",
  "nome": "Estação São Paulo Centro",
  "latitude": -23.5505,
  "longitude": -46.6333,
  "online": true
}
```
**Response:** `201 Created`
```json
{
  "idEstacao": 1,
  "codigo": "EST-SP-01",
  "nome": "Estação São Paulo Centro",
  "latitude": -23.5505,
  "longitude": -46.6333,
  "online": true,
  "ultimaAtualizacao": "2026-06-04T20:00:00Z"
}
```
**Response:** `400 Bad Request` - latitude inválida
```json
{ "mensagem": "Latitude inválida: -220. Deve estar entre -90 e 90." }
```

---

#### `GET /api/estacoes/{id}` - Buscar estação por ID

**Response:** `200 OK`
```json
{
  "idEstacao": 1,
  "codigo": "EST-SP-01",
  "nome": "Estação São Paulo Centro",
  "latitude": -23.5505,
  "longitude": -46.6333,
  "online": true,
  "ultimaAtualizacao": "2026-06-04T20:00:00Z"
}
```
**Response:** `404 Not Found`
```json
{ "mensagem": "Estação base não encontrada." }
```

---

#### `GET /api/estacoes` - Listar todas as estações

**Response:** `200 OK` - array de estações base

---

#### `GET /api/estacoes/proximidade` - Listar por proximidade

**Query params:** `?latitude=-23.0&longitude=-46.0`

**Response:** `200 OK` - array de estações ordenadas pela mais próxima das coordenadas informadas

---

#### `PUT /api/estacoes/{id}` - Atualizar estação base

**Request:**
```json
{
  "idEstacao": 1,
  "codigo": "EST-SP-01",
  "nome": "Estação São Paulo Centro - Atualizada",
  "latitude": -23.5505,
  "longitude": -46.6333,
  "online": true
}
```
**Response:** `204 No Content`

**Response:** `400 Bad Request` - ID divergente
```json
{ "mensagem": "ID da URL não confere com o ID do corpo." }
```

---

#### `DELETE /api/estacoes/{id}` - Remover estação base

**Response:** `204 No Content`

**Response:** `404 Not Found`
```json
{ "mensagem": "Estação base não encontrada." }
```

**Response:** `500 Internal Server Error` - rovers associados
```json
{ "mensagem": "Erro ao deletar estação base. Verifique se existem rovers associados." }
```

---

### Usuarios - `api/usuarios`

Gerencia os usuários da plataforma com seus perfis de acesso. Cada rover é vinculado a um usuário, garantindo que operadores só gerenciem seus próprios equipamentos. Os perfis disponíveis são `Operador`, `Gestor` e `Administrador`.

| Método | Rota | Descrição |
|---|---|---|
| GET | `/api/usuarios` | Lista todos os usuários |
| GET | `/api/usuarios/{id}` | Busca usuário por ID |
| GET | `/api/usuarios/perfil/{perfil}` | Lista usuários por perfil |
| POST | `/api/usuarios` | Cadastra novo usuário |
| PUT | `/api/usuarios/{id}` | Atualiza perfil do usuário |
| DELETE | `/api/usuarios/{id}` | Remove usuário |

---

#### `POST /api/usuarios` - Cadastrar usuário

**Request:**
```json
{
  "nome": "Renan Dias Utida",
  "email": "renan@centimeterx.com",
  "perfil": "Operador"
}
```
**Response:** `201 Created`
```json
{
  "idUsuario": 1,
  "nome": "Renan Dias Utida",
  "email": "renan@centimeterx.com",
  "perfil": "Operador",
  "criadoEm": "2026-06-04T20:00:00Z"
}
```
**Response:** `400 Bad Request` - e-mail duplicado
```json
{ "mensagem": "O e-mail 'renan@centimeterx.com' já está cadastrado." }
```

---

#### `GET /api/usuarios/{id}` - Buscar usuário por ID

**Response:** `200 OK`
```json
{
  "idUsuario": 1,
  "nome": "Renan Dias Utida",
  "email": "renan@centimeterx.com",
  "perfil": "Operador",
  "criadoEm": "2026-06-04T20:00:00Z"
}
```
**Response:** `404 Not Found`
```json
{ "mensagem": "Usuário não encontrado." }
```

---

#### `GET /api/usuarios` - Listar todos os usuários

**Response:** `200 OK` - array de usuários

---

#### `GET /api/usuarios/perfil/{perfil}` - Listar por perfil

**Response:** `200 OK` - array de usuários com o perfil informado (`Operador`, `Gestor` ou `Administrador`)

---

#### `PUT /api/usuarios/{id}` - Atualizar usuário

**Request:**
```json
{
  "idUsuario": 1,
  "nome": "Renan Dias Utida Atualizado",
  "email": "renan@centimeterx.com",
  "perfil": "Operador"
}
```
**Response:** `204 No Content`

**Response:** `400 Bad Request` - administrador rebaixando próprio perfil
```json
{ "mensagem": "Um administrador não pode rebaixar seu próprio perfil." }
```

---

#### `DELETE /api/usuarios/{id}` - Remover usuário

**Response:** `204 No Content`

**Response:** `500 Internal Server Error` - rovers associados
```json
{ "mensagem": "Erro ao deletar usuário. Verifique se existem rovers associados." }
```

---

### Rovers - `api/rovers`

Gerencia os equipamentos de campo. Rovers são os dispositivos que recebem a correção GNSS e operam com precisão centimétrica. A API suporta três tipos especializados com atributos específicos de cada domínio. Os endpoints de criação, atualização e remoção são separados por tipo para garantir a integridade da herança.

| Método | Rota | Descrição |
|---|---|---|
| GET | `/api/rovers` | Lista todos os rovers |
| GET | `/api/rovers/{id}` | Busca rover por ID (atualiza status automaticamente) |
| POST | `/api/rovers/maquina-agricola` | Cadastra máquina agrícola |
| POST | `/api/rovers/drone` | Cadastra drone |
| POST | `/api/rovers/veiculo-autonomo` | Cadastra veículo autônomo |
| PUT | `/api/rovers/maquina-agricola/{id}` | Atualiza máquina agrícola |
| PUT | `/api/rovers/drone/{id}` | Atualiza drone |
| PUT | `/api/rovers/veiculo-autonomo/{id}` | Atualiza veículo autônomo |
| DELETE | `/api/rovers/maquina-agricola/{id}` | Remove máquina agrícola |
| DELETE | `/api/rovers/drone/{id}` | Remove drone |
| DELETE | `/api/rovers/veiculo-autonomo/{id}` | Remove veículo autônomo |

---

#### `POST /api/rovers/maquina-agricola` - Cadastrar máquina agrícola

**Request:**
```json
{
  "nome": "Trator Alpha-01",
  "status": "Ativo",
  "idEstacaoBase": 1,
  "idUsuario": 1,
  "modeloTrator": "John Deere 8R",
  "larguraImplemento": 12.5
}
```
**Response:** `201 Created`
```json
{
  "modeloTrator": "John Deere 8R",
  "larguraImplemento": 12.5,
  "idRover": 1,
  "nome": "Trator Alpha-01",
  "status": "Ativo",
  "idEstacaoBase": 1,
  "idUsuario": 1,
  "dataCadastro": "2026-06-04T20:00:00Z"
}
```
**Response:** `400 Bad Request` - nome duplicado
```json
{ "mensagem": "Já existe um rover com o nome 'Trator Alpha-01' cadastrado para este usuário." }
```
**Response:** `400 Bad Request` - estação offline
```json
{ "mensagem": "Estação base 'Estação São Paulo Centro' está offline e não pode ser associada a um rover." }
```

---

#### `POST /api/rovers/drone` - Cadastrar drone

**Request:**
```json
{
  "nome": "Drone Mapeador DJI-01",
  "status": "Ativo",
  "idEstacaoBase": 1,
  "idUsuario": 1,
  "autonomiaVoo": 45,
  "altitudeMaxima": 120.0
}
```
**Response:** `201 Created`
```json
{
  "autonomiaVoo": 45,
  "altitudeMaxima": 120.0,
  "idRover": 2,
  "nome": "Drone Mapeador DJI-01",
  "status": "Ativo",
  "idEstacaoBase": 1,
  "idUsuario": 1,
  "dataCadastro": "2026-06-04T20:00:00Z"
}
```

---

#### `POST /api/rovers/veiculo-autonomo` - Cadastrar veículo autônomo

**Request:**
```json
{
  "nome": "Veículo Auto-01",
  "status": "Ativo",
  "idEstacaoBase": 2,
  "idUsuario": 2,
  "nivelAutonomia": 4,
  "velocidadeMaxima": 80.0
}
```
**Response:** `201 Created`
```json
{
  "nivelAutonomia": 4,
  "velocidadeMaxima": 80.0,
  "idRover": 3,
  "nome": "Veículo Auto-01",
  "status": "Ativo",
  "idEstacaoBase": 2,
  "idUsuario": 2,
  "dataCadastro": "2026-06-04T20:00:00Z"
}
```

---

#### `GET /api/rovers/{id}` - Buscar rover por ID

> O status do rover é atualizado automaticamente com base na última sessão antes de retornar.

**Response:** `200 OK`
```json
{
  "idRover": 1,
  "nome": "Trator Alpha-01 Atualizado",
  "status": "Ativo",
  "idEstacaoBase": 1,
  "estacaoBase": {
    "idEstacao": 1,
    "codigo": "EST-SP-01",
    "nome": "Estação São Paulo Centro - Atualizada",
    "latitude": -23.5505,
    "longitude": -46.6333,
    "online": true
  },
  "idUsuario": 1,
  "usuario": {
    "idUsuario": 1,
    "nome": "Renan Dias Utida Atualizado",
    "email": "renan@centimeterx.com",
    "perfil": "Operador"
  },
  "dataCadastro": "2026-06-04T20:00:00Z"
}
```
**Response:** `404 Not Found`
```json
{ "mensagem": "Rover não encontrado." }
```

---

#### `GET /api/rovers` - Listar todos os rovers

**Response:** `200 OK` - array de rovers com `estacaoBase` e `usuario` incluídos

---

#### `PUT /api/rovers/maquina-agricola/{id}` - Atualizar máquina agrícola

**Request:**
```json
{
  "idRover": 1,
  "nome": "Trator Alpha-01 Atualizado",
  "status": "Ativo",
  "idEstacaoBase": 1,
  "idUsuario": 1,
  "modeloTrator": "John Deere 9R",
  "larguraImplemento": 15.0
}
```
**Response:** `204 No Content`

**Response:** `404 Not Found` - ID de outro tipo passado
```json
{ "mensagem": "Máquina agrícola não encontrada." }
```

---

#### `DELETE /api/rovers/maquina-agricola/{id}` - Remover máquina agrícola

**Response:** `204 No Content`

**Response:** `500 Internal Server Error` - sessões ou ocorrências associadas
```json
{ "mensagem": "Erro ao deletar máquina agrícola. Verifique se existem sessões ou ocorrências associadas." }
```

---

### SessoesCorrecao - `api/sessoes`

Gerencia o ciclo de vida completo de uma sessão de correção PPP. Este é o controller central da plataforma - é aqui que a correção acontece. Uma sessão começa com precisão desconhecida (`SINGLE`), evolui conforme o sinal GNSS converge e é encerrada com a classificação final do fix.

> O `StatusFix` é **sempre calculado automaticamente** com base na `precisaoHorizontalCm` - nunca precisa ser informado manualmente no PUT.

| Método | Rota | Descrição |
|---|---|---|
| GET | `/api/sessoes` | Lista todas as sessões |
| GET | `/api/sessoes/{id}` | Busca sessão por ID |
| GET | `/api/sessoes/rover/{roverId}` | Histórico de sessões por rover |
| POST | `/api/sessoes` | Inicia nova sessão de correção |
| PUT | `/api/sessoes/{id}` | Atualiza precisão (StatusFix reclassificado automaticamente) |
| PUT | `/api/sessoes/{id}/encerrar` | Encerra sessão ativa |
| DELETE | `/api/sessoes/{id}` | Remove sessão |

---

#### `POST /api/sessoes` - Iniciar sessão de correção

**Request:**
```json
{
  "idRover": 1,
  "idEstacaoBase": 1,
  "sistemaSatelite": "GPS+Galileo"
}
```
**Response:** `201 Created`
```json
{
  "idSessao": 1,
  "idRover": 1,
  "idEstacaoBase": 1,
  "statusFix": "SINGLE",
  "precisaoHorizontalCm": 0.0,
  "precisaoVerticalCm": 0.0,
  "sistemaSatelite": "GPS+Galileo",
  "iniciouEm": "2026-06-04T20:00:00Z",
  "encerradoEm": null
}
```
**Response:** `400 Bad Request` - sessão dupla bloqueada
```json
{ "mensagem": "Rover 'Trator Alpha-01' já possui uma sessão ativa (ID: 1). Encerre-a antes de iniciar nova sessão." }
```
**Response:** `400 Bad Request` - rover inativo
```json
{ "mensagem": "Rover 'Trator Alpha-01' não está ativo. Status atual: Inativo." }
```
**Response:** `400 Bad Request` - rover inexistente
```json
{ "mensagem": "Rover não encontrado." }
```

---

#### `GET /api/sessoes/{id}` - Buscar sessão por ID

**Response:** `200 OK`
```json
{
  "idSessao": 1,
  "idRover": 1,
  "rover": { "idRover": 1, "nome": "Trator Alpha-01 Atualizado", "status": "Ativo" },
  "idEstacaoBase": 1,
  "estacaoBase": { "idEstacao": 1, "codigo": "EST-SP-01", "nome": "Estação São Paulo Centro - Atualizada" },
  "statusFix": "SINGLE",
  "precisaoHorizontalCm": 0.0,
  "precisaoVerticalCm": 0.0,
  "sistemaSatelite": "GPS+Galileo",
  "iniciouEm": "2026-06-04T20:00:00Z",
  "encerradoEm": null
}
```
**Response:** `404 Not Found`
```json
{ "mensagem": "Sessão de correção não encontrada." }
```

---

#### `GET /api/sessoes` - Listar todas as sessões

**Response:** `200 OK` - array de sessões com `rover` e `estacaoBase` incluídos

---

#### `GET /api/sessoes/rover/{roverId}` - Histórico de sessões por rover

**Response:** `200 OK` - array de sessões do rover ordenadas por data decrescente

---

#### `PUT /api/sessoes/{id}` - Atualizar precisão da sessão

> O `StatusFix` é reclassificado automaticamente: `≤ 5 cm` → `FIX`, `≤ 50 cm` → `FLOAT`, `> 50 cm` → `SINGLE`

**Request:**
```json
{
  "idSessao": 1,
  "idRover": 1,
  "idEstacaoBase": 1,
  "sistemaSatelite": "GPS+Galileo",
  "precisaoHorizontalCm": 3.2,
  "precisaoVerticalCm": 4.1,
  "iniciouEm": "2026-06-04T20:00:00Z"
}
```
**Response:** `204 No Content` - StatusFix reclassificado para `FIX` (3.2 ≤ 5.0 cm)

---

#### `PUT /api/sessoes/{id}/encerrar` - Encerrar sessão ativa

**Response:** `200 OK`
```json
{
  "mensagem": "Sessão encerrada com sucesso.",
  "sessao": { "idSessao": 1, "statusFix": "FIX", "precisaoHorizontalCm": 3.2 },
  "statusFix": "FIX",
  "precisaoHorizontalCm": 3.2,
  "encerradoEm": "2026-06-04T20:30:00Z"
}
```
**Response:** `400 Bad Request` - sessão já encerrada
```json
{ "mensagem": "Sessão 1 já foi encerrada em 04/06/2026 20:30:00." }
```

---

#### `DELETE /api/sessoes/{id}` - Remover sessão

**Response:** `204 No Content`

---

### Ocorrencias - `api/ocorrencias`

Registra eventos adversos identificados pelo operador em campo. Uma ocorrência captura o momento, o tipo do evento, as coordenadas GPS exatas e evidência fotográfica. São **registros históricos imutáveis** - por essa razão, não existe endpoint de atualização (`PUT` foi removido intencionalmente, pois editar um evento de campo comprometeria a integridade do histórico operacional). A plataforma valida as coordenadas geográficas antes de persistir qualquer ocorrência.

| Método | Rota | Descrição |
|---|---|---|
| GET | `/api/ocorrencias` | Lista todas as ocorrências |
| GET | `/api/ocorrencias/{id}` | Busca ocorrência por ID |
| GET | `/api/ocorrencias/rover/{roverId}` | Lista ocorrências por rover |
| GET | `/api/ocorrencias/rover/{roverId}/filtro?tipo=&inicio=&fim=` | Filtra ocorrências por tipo e período |
| POST | `/api/ocorrencias` | Registra nova ocorrência |
| DELETE | `/api/ocorrencias/{id}` | Remove ocorrência |

---

#### `POST /api/ocorrencias` - Registrar ocorrência

**Request:**
```json
{
  "idRover": 1,
  "tipo": "PerdaDeSinal",
  "descricao": "Perda de sinal durante operação na fazenda Norte",
  "latitude": -23.5505,
  "longitude": -46.6333,
  "fotoUrl": "https://storage.centimeterx.com/ocorrencias/foto001.jpg"
}
```
**Response:** `201 Created`
```json
{
  "idOcorrencia": 1,
  "idRover": 1,
  "rover": { "idRover": 1, "nome": "Trator Alpha-01 Atualizado", "status": "Ativo" },
  "tipo": "PerdaDeSinal",
  "descricao": "Perda de sinal durante operação na fazenda Norte",
  "latitude": -23.5505,
  "longitude": -46.6333,
  "fotoUrl": "https://storage.centimeterx.com/ocorrencias/foto001.jpg",
  "criadaEm": "2026-06-04T20:00:00Z"
}
```
**Response:** `400 Bad Request` - coordenada inválida
```json
{ "mensagem": "Latitude inválida: -95. Deve estar entre -90 e 90." }
```
**Response:** `404 Not Found` - rover inexistente
```json
{ "mensagem": "Rover não encontrado." }
```

---

#### `GET /api/ocorrencias/{id}` - Buscar ocorrência por ID

**Response:** `200 OK` - ocorrência com dados do rover incluídos

**Response:** `404 Not Found`
```json
{ "mensagem": "Ocorrência não encontrada." }
```

---

#### `GET /api/ocorrencias` - Listar todas as ocorrências

**Response:** `200 OK` - array de ocorrências com dados do rover incluídos

---

#### `GET /api/ocorrencias/rover/{roverId}` - Ocorrências por rover

**Response:** `200 OK` - array de ocorrências do rover ordenadas por data decrescente

**Response:** `404 Not Found` - rover inexistente
```json
{ "mensagem": "Rover não encontrado." }
```

---

#### `GET /api/ocorrencias/rover/{roverId}/filtro` - Filtrar por tipo e período

**Query params opcionais:** `?tipo=PerdaDeSinal&inicio=2026-01-01&fim=2026-12-31`

**Response:** `200 OK`
```json
[
  {
    "idOcorrencia": 1,
    "idRover": 1,
    "tipo": "PerdaDeSinal",
    "descricao": "Perda de sinal durante operação na fazenda Norte",
    "latitude": -23.5505,
    "longitude": -46.6333,
    "fotoUrl": "https://storage.centimeterx.com/ocorrencias/foto001.jpg",
    "criadaEm": "2026-06-04T20:00:00Z"
  }
]
```

---

#### `DELETE /api/ocorrencias/{id}` - Remover ocorrência

**Response:** `204 No Content`

**Response:** `404 Not Found`
```json
{ "mensagem": "Ocorrência não encontrada." }
```

---

## 9. Critérios da Global Solution

### 1. Modelagem de Domínio & POO

**Herança com sentido conceitual:** `Rover` é a classe abstrata base para todos os equipamentos de campo. Não existe instância de `Rover` diretamente - todo rover é necessariamente uma `MaquinaAgricola`, um `Drone` ou um `VeiculoAutonomo`. Cada subclasse adiciona atributos específicos do seu tipo real (`ModeloTrator`, `AutonomiaVoo`, `NivelAutonomia`).

**Classes públicas:** todas as entidades, services, controllers e o `AppDbContext` são `public`. **Classes privadas:** métodos auxiliares como `EstacaoExists` nos controllers, e todos os campos `private readonly` nos services e controllers. **Classe estática:** `GnssConstants` centraliza as constantes do domínio GNSS (`FIX_THRESHOLD_CM`, `FLOAT_THRESHOLD_CM`, limites de coordenadas, horas de inatividade).

**Enums com uso real:**
- `StatusRover` (Ativo, Inativo, Offline) - controla disponibilidade dos rovers
- `StatusFix` (FIX, FLOAT, SINGLE) - classifica a precisão da correção PPP
- `TipoOcorrencia` (PerdaDeSinal, Deriva, Obstrucao, Outro) - categoriza eventos de campo
- `PerfilUsuario` (Operador, Gestor, Administrador) - controla acesso na plataforma

### 2. Abstração e Interfaces

**Classe abstrata com sentido real:** `Rover` é `abstract` porque não existe rover sem tipo - a abstração reflete a realidade do negócio, não foi criada por enfeite.

**Interface com contrato real:** `ICorrecaoService` define o contrato do serviço de correção GNSS com 4 métodos (`IniciarSessao`, `EncerrarSessao`, `ObterPrecisao`, `ClassificarFix`). Implementada por `SessaoCorrecaoService` e injetada no `SessoesCorrecaoController` - o único controller que usa a interface diretamente, demonstrando desacoplamento real.

### 3. Lógica de Fluxo, Métodos e Datas

**Métodos coesos:** cada método tem uma única responsabilidade - `ClassificarFix` só classifica, `ValidarCoordenadas` só valida coordenadas, `ValidarNomeDuplicado` só verifica duplicidade, `AtualizarStatusPorUltimaSessao` só atualiza status.

**Estruturas de controle:**
- `switch` com pattern matching - `ClassificarFix` em `SessaoCorrecaoService` classifica FIX/FLOAT/SINGLE usando `GnssConstants`
- `if` encadeados - `ValidarCoordenadas` em `OcorrenciaService` e validações em `IniciarSessao`
- `foreach` - `ListarPorProximidade` em `EstacaoBaseService` itera sobre estações calculando distância euclidiana para cada uma
- LINQ com `Where`, `OrderByDescending` e filtros opcionais - `ListarPorTipoEPeriodo` em `OcorrenciaService`

**DateTime para histórico:**
- `DataCadastro` - timestamp de cadastro de rovers
- `CriadoEm` - timestamp de criação de usuários
- `CriadaEm` - timestamp de registro de ocorrências
- `IniciouEm` / `EncerradoEm` - janela temporal da sessão de correção (`EncerradoEm` é `DateTime?` nullable para distinguir sessão ativa de encerrada)
- `UltimaAtualizacao` - atualizado automaticamente em cada PUT de estação
- `DateTime.UtcNow - ultimaSessao.EncerradoEm.Value > TimeSpan.FromHours(GnssConstants.STATUS_INATIVIDADE_HORAS)` - janela temporal de 24h para detectar inatividade

### 4. Tratamento de Exceções

`catch (DbUpdateException)` - capturado em todos os métodos de persistência nos controllers e services, com `_logger.LogError` e retorno `500` com mensagem clara.

`catch (InvalidOperationException)` - capturado em todas as validações de negócio nos services, com `_logger.LogWarning` e retorno `400` com mensagem específica.

`catch (Exception)` - fallback em todos os métodos, garantindo que nenhuma exceção não tratada chega ao runtime. A aplicação nunca quebra abruptamente.

`ILogger<T>` nativo do ASP.NET Core injetado em todos os 5 controllers, com `LogError` para falhas de banco e exceções inesperadas, e `LogWarning` para violações de regra de negócio.

### 5. Organização

**Estrutura de pastas e nomenclatura:** `Models/`, `Models/Enums/`, `Data/`, `Services/`, `Controllers/`, `docs/`. Prefixo `CX_` em todas as tabelas Oracle. Nomenclatura consistente em português para o domínio.

**README:** este documento.

**Diagrama:** disponível em [`docs/diagram/Diagrama_Centimeter-X_GlobalSolution.png`](docs/diagram/Diagrama_Centimeter-X_GlobalSolution.png).

**Prints de Evidências de execução:** disponíveis em [`docs/evidencias.md`](docs/evidencias.md) com prints de todos os 59 cenários testados no Swagger.

**Link vídeo demonstrativo dos testes: **

---

## 10. Collection Postman

A collection com todos os 59 cenários organizados por controller está disponível em:

```
docs/postman/Centimeter-X_API.postman_collection.json
```

Para importar: abra o Postman → **File → Import** → selecione o arquivo.

> **Atenção:** a collection está configurada para a porta `7126`. Se a sua API rodar em porta diferente, atualize a variável de ambiente ou substitua a porta diretamente nas requisições.

---

## 11. Evidências de execução

Segue a ordem dos principais testes realizados no Swagger, cobrindo os fluxos
críticos de cada controller e as principais validações de negócio da plataforma.

### Endpoints disponíveis no Swagger

> Prints da tela inicial do Swagger com todos os controllers e endpoints expostos

![Print Swagger endpoints](docs/prints/swagger-endpoints-1.png)
![Print Swagger endpoints](docs/prints/swagger-endpoints-2.png)

---

### EstacoesBase

---

**01 - Cadastrar Estacao Base SP (POST)**

![01](docs/prints/01.png)

---

**02 - Cadastrar Estacao Base RJ (POST)**

![02](docs/prints/02.png)

---

**03 - Cadastrar Estacao Base - Latitude Invalida - 400 (POST)**

![03](docs/prints/03.png)

---

**04 - Listar Estacoes por Proximidade (GET)**

![07](docs/prints/07.png)

---

**05 - Atualizar Estacao Base (PUT)**

![08](docs/prints/08.png)

---

### Usuarios

---

**06 - Cadastrar Usuario Operador (POST)**

![09](docs/prints/09.png)

---

**07 - Cadastrar Usuario - Email Duplicado - 400 (POST)**

![11](docs/prints/11.png)

---

**08 - Listar Usuarios por Perfil (GET)**

![15](docs/prints/15.png)

---

### Rovers

---

**09 - Cadastrar Maquina Agricola (POST)**

![17](docs/prints/17.png)

---

**10 - Cadastrar Drone (POST)**

![18](docs/prints/18.png)

---

**11 - Cadastrar Veiculo Autonomo (POST)**

![19](docs/prints/19.png)

---

**12 - Cadastrar Rover - Nome Duplicado - 400 (POST)**

![20](docs/prints/20.png)

---

**13 - Cadastrar Rover - Estacao Offline - 400 (POST)**

![22](docs/prints/22.png)

---

**14 - Listar Todos os Rovers (GET)**

![24](docs/prints/24.png)

---

### SessoesCorrecao

---

**15 - Iniciar Sessao Maquina Agricola (POST)**

![30](docs/prints/30.png)

---

**16 - Iniciar Sessao - Sessao Dupla Bloqueada - 400 (POST)**

![31](docs/prints/31.png)

---

**17 - Buscar Sessao - StatusFix SINGLE (GET)**

![32](docs/prints/32.png)

---

**18 - Atualizar Precisao para FIX (PUT)**

![33](docs/prints/33.png)

---

**19 - Encerrar Sessao - StatusFix FIX (PUT)**

![34](docs/prints/34.png)

---

**20 - Encerrar Sessao - StatusFix FLOAT (PUT)**

![36](docs/prints/36.png)

---

**21 - Iniciar Sessao - Rover Inexistente - 400 (POST)**

![41](docs/prints/41.png)

---

### Ocorrencias

---

**22 - Registrar Ocorrencia Valida (POST)**

![42](docs/prints/42.png)

---

**23 - Registrar Ocorrencia - Latitude Invalida - 400 (POST)**

![43](docs/prints/43.png)

---

**24 - Filtrar Ocorrencias por Tipo e Periodo (GET)**

![48](docs/prints/48.png)

---

### Deletes

Para deletar, é recomendado remover nessa ordem:

**Ocorrencia → Sessoes → Rovers → Usuarios → EstacoesBase**

> Delete sempre nessa ordem para evitar erros de chave estrangeira!

---

**25 - Deletar Ocorrencia (DELETE)**

![49](docs/prints/49.png)

---

### Banco de Dados (Oracle SQL Developer)

> Prints das tabelas criadas e dos dados inseridos após os testes 01 a 24
> (antes dos deletes). Scripts disponíveis em
> [`docs/sql/CentimeterX_consultas.sql`](docs/sql/CentimeterX_consultas.sql)

![sql-tabelas](docs/prints/sql-tabelas.png)

![sql-dados](docs/prints/sql-dados.png)

---

As evidências completas de todos os 59 cenários testados estão disponíveis em
[`docs/evidencias.md`](docs/evidencias.md).

- Vídeo demonstrativo - disponível na entrega via Teams, cobrindo os 6 blocos
  de testes organizados por controller

---

*Global Solution - C# Software Development | FIAP 3ESPW | 1º Semestre de 2026*
