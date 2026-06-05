# Evidências de Funcionamento — Centimeter-X API

**Disciplina:** C# Software Development  
**Turma:** 3ESPW  
**Professor:** Rafael Santos Novo Pereira  

**Integrantes:**
| Nome | RM |
|---|---|
| Rafael Duarte de Freitas | RM558644 |
| Rafael Gaspar Bragança Martins | RM557228 |
| Luiz Gustavo da Silva | RM558358 |
| Pedro Almeida e Camacho | RM556831 |
| Renan Dias Utida | RM558540 |

---

## Sobre este documento

Este arquivo reúne as evidências completas de execução da API Centimeter-X, cobrindo todos os **59 cenários de teste** realizados via Swagger UI. Os testes estão organizados nos mesmos 6 blocos da gravação de vídeo e seguem a ordem recomendada para evitar erros de chave estrangeira.

Cada cenário documenta um endpoint real da plataforma, incluindo os fluxos de sucesso e os principais casos de erro — validações de negócio, entradas inválidas e recursos inexistentes. Esta é uma versão completa e complementar ao item 11 do README, que apresenta apenas os 25 cenários mais representativos.

A API foi testada localmente com o banco Oracle da FIAP (`oracle.fiap.com.br:1521/ORCL`), utilizando as tabelas `CX_ESTACAO_BASE`, `CX_USUARIO`, `CX_ROVER`, `CX_SESSAO_CORRECAO` e `CX_OCORRENCIA`, criadas via EF Core Migrations.

---

## Endpoints disponíveis no Swagger

> Tela inicial do Swagger com todos os controllers e endpoints expostos

![swagger-endpoints-1](prints/swagger-endpoints-1.png)

![swagger-endpoints-2](prints/swagger-endpoints-2.png)

---

## Bloco 1 — Estações Base

> Cadastro e consulta das estações terrestres de referência GNSS. Demonstra criação de estações com coordenadas geográficas reais, validação de coordenadas inválidas, busca por ID, listagem por proximidade geográfica e atualização de dados.

---

**01 - Cadastrar Estacao Base SP (POST)**

![01](../prints/01.png)

---

**02 - Cadastrar Estacao Base RJ (POST)**

![02](../prints/02.png)

---

**03 - Cadastrar Estacao Base - Latitude Invalida - 400 (POST)**

![03](../prints/03.png)

---

**04 - Listar Todas as Estacoes (GET)**

![04](../prints/04.png)

---

**05 - Buscar Estacao por ID (GET)**

![05](../prints/05.png)

---

**06 - Buscar Estacao Inexistente - 404 (GET)**

![06](../prints/06.png)

---

**07 - Listar Estacoes por Proximidade (GET)**

![07](../prints/07.png)

---

**08 - Atualizar Estacao Base (PUT)**

![08](../prints/08.png)

---

## Bloco 2 — Usuários

> Cadastro e gestão de usuários da plataforma. Demonstra criação de perfis Operador e Gestor, validação de e-mail duplicado, busca por ID, listagem por perfil e atualização de dados do usuário.

---

**09 - Cadastrar Usuario Operador (POST)**

![09](../prints/09.png)

---

**10 - Cadastrar Usuario Gestor (POST)**

![10](../prints/10.png)

---

**11 - Cadastrar Usuario - Email Duplicado - 400 (POST)**

![11](../prints/11.png)

---

**12 - Listar Todos os Usuarios (GET)**

![12](../prints/12.png)

---

**13 - Buscar Usuario por ID (GET)**

![13](../prints/13.png)

---

**14 - Buscar Usuario Inexistente - 404 (GET)**

![14](../prints/14.png)

---

**15 - Listar Usuarios por Perfil Operador (GET)**

![15](../prints/15.png)

---

**16 - Atualizar Usuario (PUT)**

![16](../prints/16.png)

---

## Bloco 3 — Rovers

> Cadastro e gestão dos equipamentos de campo. Demonstra criação dos 3 tipos de rover — Máquina Agrícola, Drone e Veículo Autônomo — com validação de nome duplicado por usuário, bloqueio de cadastro com estação offline, e atualização de cada tipo com seus atributos específicos.

---

**17 - Cadastrar Maquina Agricola (POST)**

![17](../prints/17.png)

---

**18 - Cadastrar Drone (POST)**

![18](../prints/18.png)

---

**19 - Cadastrar Veiculo Autonomo (POST)**

![19](../prints/19.png)

---

**20 - Cadastrar Maquina Agricola - Nome Duplicado - 400 (POST)**

![20](../prints/20.png)

---

**21 - Colocar Estacao Offline (PUT)**

![21](../prints/21.png)

---

**22 - Cadastrar Drone - Estacao Offline - 400 (POST)**

![22](../prints/22.png)

---

**23 - Recolocar Estacao Online (PUT)**

![23](../prints/23.png)

---

**24 - Listar Todos os Rovers (GET)**

![24](../prints/24.png)

---

**25 - Buscar Rover por ID (GET)**

![25](../prints/25.png)

---

**26 - Buscar Rover Inexistente - 404 (GET)**

![26](../prints/26.png)

---

**27 - Atualizar Maquina Agricola (PUT)**

![27](../prints/27.png)

---

**28 - Atualizar Drone (PUT)**

![28](../prints/28.png)

---

**29 - Atualizar Veiculo Autonomo (PUT)**

![29](../prints/29.png)

---

## Bloco 4 — Sessões de Correção

> Fluxo completo de correção PPP. Demonstra inicialização de sessão por rover, bloqueio de sessão dupla para o mesmo equipamento, atualização de precisão com reclassificação automática do status de fix em três níveis — FIX (≤5cm), FLOAT (≤50cm) e SINGLE (>50cm) — encerramento com timestamp automático e histórico de sessões por rover.

---

**30 - Iniciar Sessao Maquina Agricola (POST)**

![30](../prints/30.png)

---

**31 - Iniciar Sessao - Sessao Dupla Bloqueada - 400 (POST)**

![31](../prints/31.png)

---

**32 - Buscar Sessao por ID - StatusFix SINGLE (GET)**

![32](../prints/32.png)

---

**33 - Atualizar Precisao para FIX (PUT)**

![33](../prints/33.png)

---

**34 - Encerrar Sessao - StatusFix FIX (PUT)**

![34](../prints/34.png)

---

**35 - Iniciar Sessao Drone (POST)**

![35](../prints/35.png)

---

**36 - Atualizar Precisao para FLOAT (PUT)**

![36](../prints/36.png)

---

**37 - Encerrar Sessao - StatusFix FLOAT (PUT)**

![37](../prints/37.png)

---

**38 - Iniciar Sessao Veiculo Autonomo (POST)**

![38](../prints/38.png)

---

**39 - Encerrar Sessao - StatusFix FIX (precisao zerada) (PUT)**

![39](../prints/39.png)

---

**40 - Historico de Sessoes por Rover (GET)**

![40](../prints/40.png)

---

**41 - Iniciar Sessao - Rover Inexistente - 400 (POST)**

![41](../prints/41.png)

---

## Bloco 5 — Ocorrências

> Registro de eventos adversos em campo. Demonstra criação de ocorrência com localização geográfica real, validação de coordenadas inválidas, bloqueio de ocorrência para rover inexistente, listagem geral, busca por ID, histórico por rover e filtro por tipo e período.

---

**42 - Registrar Ocorrencia Valida (POST)**

![42](../prints/42.png)

---

**43 - Registrar Ocorrencia - Latitude Invalida - 400 (POST)**

![43](../prints/43.png)

---

**44 - Registrar Ocorrencia - Rover Inexistente - 404 (POST)**

![44](../prints/44.png)

---

**45 - Listar Todas as Ocorrencias (GET)**

![45](../prints/45.png)

---

**46 - Buscar Ocorrencia por ID (GET)**

![46](../prints/46.png)

---

**47 - Listar Ocorrencias por Rover (GET)**

![47](../prints/47.png)

---

**48 - Filtrar Ocorrencias por Tipo e Periodo (GET)**

![48](../prints/48.png)

---

## Bloco 6 — Deletes

> Remoção de todos os registros na ordem correta respeitando as restrições de chave estrangeira. A ordem obrigatória é: Ocorrências → Sessões → Rovers → Usuários → Estações Base.

---

**49 - Deletar Ocorrencia (DELETE)**

![49](../prints/49.png)

---

**50 - Deletar Sessao 3 (DELETE)**

![50](../prints/50.png)

---

**51 - Deletar Sessao 2 (DELETE)**

![51](../prints/51.png)

---

**52 - Deletar Sessao 1 (DELETE)**

![52](../prints/52.png)

---

**53 - Deletar Maquina Agricola (DELETE)**

![53](../prints/53.png)

---

**54 - Deletar Drone (DELETE)**

![54](../prints/54.png)

---

**55 - Deletar Veiculo Autonomo (DELETE)**

![55](../prints/55.png)

---

**56 - Deletar Usuario Gestor (DELETE)**

![56](../prints/56.png)

---

**57 - Deletar Usuario Operador (DELETE)**

![57](../prints/57.png)

---

**58 - Deletar Estacao RJ (DELETE)**

![58](../prints/58.png)

---

**59 - Deletar Estacao SP (DELETE)**

![59](../prints/59.png)

---

## Banco de Dados — Oracle SQL Developer

> Prints das tabelas criadas e dos dados inseridos após os testes 01 a 48 (antes dos deletes). Scripts disponíveis em `docs/sql/CentimeterX_consultas.sql`.

**Tabelas criadas e dados inseridos:**

![sql-tabelas](../prints/sql-tabelas.png)

**Resultados dos SELECTs por tabela:**

![sql-resultados-1](../prints/sql-resultados-1.png)

**Resultado do JOIN completo — Rovers com Estacao Base e Usuario:**

![sql-resultados-2](../prints/sql-resultados-2.png)

**Resultado do JOIN completo — Sessoes de Correcao:**

![sql-resultados-3](../prints/sql-resultados-3.png)

---

*Global Solution — C# Software Development | FIAP 3ESPW | 1º Semestre de 2026*
