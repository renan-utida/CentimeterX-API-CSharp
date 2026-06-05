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

![01](prints/01-Cadastrar_Estacao_Base_SP.png)

---

**02 - Cadastrar Estacao Base RJ (POST)**

![02](prints/02-Cadastrar_Estacao_Base_RJ.png)

---

**03 - Cadastrar Estacao Base - Latitude Invalida - 400 (POST)**

![03](prints/03-Cadastrar_Estacao_Base_LATITUDE_INVALIDA.png)

---

**04 - Listar Todas as Estacoes (GET)**

![04](prints/04-Buscar_Todas_Estacoes_Base.png)

---

**05 - Buscar Estacao por ID (GET)**

![05](prints/05-Buscar_Estacoes_Base_ID.png)

---

**06 - Buscar Estacao Inexistente - 404 (GET)**

![06](prints/06-Buscar_Estacoes_Base_ID_INEXISTENTE.png)

---

**07 - Listar Estacoes por Proximidade (GET)**

![07](prints/07-Buscar_Estacoes_Base_Proximidade.png)

---

**08 - Atualizar Estacao Base (PUT)**

![08](prints/08-Atualizar_Estacoes_Base.png)

---

## Bloco 2 — Usuários

> Cadastro e gestão de usuários da plataforma. Demonstra criação de perfis Operador e Gestor, validação de e-mail duplicado, busca por ID, listagem por perfil e atualização de dados do usuário.

---

**09 - Cadastrar Usuario Operador (POST)**

![09](prints/09-Cadastrar_Usuario_Operador.png)

---

**10 - Cadastrar Usuario Gestor (POST)**

![10](prints/10-Cadastrar_Usuario_Gestor.png)

---

**11 - Cadastrar Usuario - Email Duplicado - 400 (POST)**

![11](prints/11-Cadastrar_Usuario_EMAIL_DUPLICADO.png)

---

**12 - Listar Todos os Usuarios (GET)**

![12](prints/12-Buscar_Todos_Usuarios.png)

---

**13 - Buscar Usuario por ID (GET)**

![13](prints/13-Buscar_Usuario_ID.png)

---

**14 - Buscar Usuario Inexistente - 404 (GET)**

![14](prints/14-Buscar_Usuario_ID_INEXISTENTE.png)

---

**15 - Listar Usuarios por Perfil Operador (GET)**

![15](prints/15-Buscar_Usuario_Perfil.png)

---

**16 - Atualizar Usuario (PUT)**

![16](prints/16-Atualizar_Usuario.png)

---

## Bloco 3 — Rovers

> Cadastro e gestão dos equipamentos de campo. Demonstra criação dos 3 tipos de rover — Máquina Agrícola, Drone e Veículo Autônomo — com validação de nome duplicado por usuário, bloqueio de cadastro com estação offline, e atualização de cada tipo com seus atributos específicos.

---

**17 - Cadastrar Maquina Agricola (POST)**

![17](prints/17-Cadastrar_Rover_Maquina_Agricola.png)

---

**18 - Cadastrar Drone (POST)**

![18](prints/18-Cadastrar_Rover_Drone.png)

---

**19 - Cadastrar Veiculo Autonomo (POST)**

![19](prints/19-Cadastrar_Rover_Veiculo_Autonomo.png)

---

**20 - Cadastrar Maquina Agricola - Nome Duplicado - 400 (POST)**

![20](prints/20-Cadastrar_Rover_NOME_DUPLICADO.png)

---

**21 - Colocar Estacao Offline (PUT)**

![21](prints/21-Atualizar_Estacao_Base_Offline.png)

---

**22 - Cadastrar Drone - Estacao Offline - 400 (POST)**

![22](prints/22-Cadastrar_Rover_ESTACAO_OFFLINE.png)

---

**23 - Recolocar Estacao Online (PUT)**

![23](prints/23-Recolocar_Estacao_Base_Online.png)

---

**24 - Listar Todos os Rovers (GET)**

![24](prints/24-Buscar_Todos_Rovers.png)

---

**25 - Buscar Rover por ID (GET)**

![25](prints/25-Buscar_Rover_ID.png)

---

**26 - Buscar Rover Inexistente - 404 (GET)**

![26](prints/26-Buscar_Rover_ID_INEXISTENTE.png)

---

**27 - Atualizar Maquina Agricola (PUT)**

![27](prints/27-Atualizar_Rover_Maquina_Agricola.png)

---

**28 - Atualizar Drone (PUT)**

![28](prints/28-Atualizar_Rover_Drone.png)

---

**29 - Atualizar Veiculo Autonomo (PUT)**

![29](prints/29-Atualizar_Rover_Veiculo_Autonomo.png)

---

## Bloco 4 — Sessões de Correção

> Fluxo completo de correção PPP. Demonstra inicialização de sessão por rover, bloqueio de sessão dupla para o mesmo equipamento, atualização de precisão com reclassificação automática do status de fix em três níveis — FIX (≤5cm), FLOAT (≤50cm) e SINGLE (>50cm) — encerramento com timestamp automático e histórico de sessões por rover.

---

**30 - Iniciar Sessao Maquina Agricola (POST)**

![30](prints/30-Iniciar_Sessao_Correcao_Maquina.png)

---

**31 - Iniciar Sessao - Sessao Dupla Bloqueada - 400 (POST)**

![31](prints/31-Iniciar_Sessao_Correcao_DUPLA_BLOQUEADA.png)

---

**32 - Buscar Sessao por ID - StatusFix SINGLE (GET)**

![32](prints/32-Buscar_Sessao_Correcao_ID.png)

---

**33 - Atualizar Precisao para FIX (PUT)**

![33](prints/33-Atualizar_Sessao_Correcao_FIX.png)

---

**34 - Encerrar Sessao - StatusFix FIX (PUT)**

![34](prints/34-Encerrar_Sessao_Correcao_FIX.png)

---

**35 - Iniciar Sessao Drone (POST)**

![35](prints/35-Iniciar_Sessao_Correcao_Drone.png)

---

**36 - Atualizar Precisao para FLOAT (PUT)**

![36](prints/36-Atualizar_Sessao_Correcao_FLOAT.png)

---

**37 - Encerrar Sessao - StatusFix FLOAT (PUT)**

![37](prints/37-Encerrar_Sessao_Correcao_FLOAT.png)

---

**38 - Iniciar Sessao Veiculo Autonomo (POST)**

![38](prints/38-Iniciar_Sessao_Correcao_Veiculo.png)

---

**39 - Encerrar Sessao - StatusFix FIX (precisao zerada) (PUT)**

![39](prints/39-Encerrar_Sessao_Correcao_FIX_Sem_Atualizar_Precisao.png)

---

**40 - Historico de Sessoes por Rover (GET)**

![40](prints/40-Buscar_Sessao_Correcao_ROVER_ID.png)

---

**41 - Iniciar Sessao - Rover Inexistente - 400 (POST)**

![41](prints/41-Iniciar_Sessao_Correcao_ROVER_INEXISTENTE.png)

---

## Bloco 5 — Ocorrências

> Registro de eventos adversos em campo. Demonstra criação de ocorrência com localização geográfica real, validação de coordenadas inválidas, bloqueio de ocorrência para rover inexistente, listagem geral, busca por ID, histórico por rover e filtro por tipo e período.

---

**42 - Registrar Ocorrencia Valida (POST)**

![42](prints/42-Cadastrar_Ocorrencia_Valida.png)

---

**43 - Registrar Ocorrencia - Latitude Invalida - 400 (POST)**

![43](prints/43-Cadastrar_Ocorrencia_LATITUDE_INVALIDA.png)

---

**44 - Registrar Ocorrencia - Rover Inexistente - 404 (POST)**

![44](prints/44-Cadastrar_Ocorrencia_ROVER_INEXISTENTE.png)

---

**45 - Listar Todas as Ocorrencias (GET)**

![45](prints/45-Buscar_Todas_Ocorrencias.png)

---

**46 - Buscar Ocorrencia por ID (GET)**

![46](prints/46-Buscar_Ocorrencia_ID.png)

---

**47 - Listar Ocorrencias por Rover (GET)**

![47](prints/47-Buscar_Ocorrencia_ROVER_ID.png)

---

**48 - Filtrar Ocorrencias por Tipo e Periodo (GET)**

![48](prints/48-Buscar_Ocorrencia_FILTRO.png)

---

## Bloco 6 — Deletes

> Remoção de todos os registros na ordem correta respeitando as restrições de chave estrangeira. A ordem obrigatória é: Ocorrências → Sessões → Rovers → Usuários → Estações Base.

---

**49 - Deletar Ocorrencia (DELETE)**

![49](prints/49-Deletar_Ocorrencia.png)

---

**50 - Deletar Sessao 3 (DELETE)**

![50](prints/50-Deletar_Sessao_Correcao_3.png)

---

**51 - Deletar Sessao 2 (DELETE)**

![51](prints/51-Deletar_Sessao_Correcao_2.png)

---

**52 - Deletar Sessao 1 (DELETE)**

![52](prints/52-Deletar_Sessao_Correcao_1.png)

---

**53 - Deletar Maquina Agricola (DELETE)**

![53](prints/53-Deletar_Rover_Maquina.png)

---

**54 - Deletar Drone (DELETE)**

![54](prints/54-Deletar_Rover_Drone.png)

---

**55 - Deletar Veiculo Autonomo (DELETE)**

![55](prints/55-Deletar_Rover_Veiculo.png)

---

**56 - Deletar Usuario Gestor (DELETE)**

![56](prints/56-Deletar_Usuario_2.png)

---

**57 - Deletar Usuario Operador (DELETE)**

![57](prints/57-Deletar_Usuario_1.png)

---

**58 - Deletar Estacao RJ (DELETE)**

![58](prints/58-Deletar_Estacao_2.png)

---

**59 - Deletar Estacao SP (DELETE)**

![59](prints/59-Deletar_Estacao_1.png)

---

## Banco de Dados — Oracle SQL Developer

> Prints das tabelas criadas e dos dados inseridos após os testes 01 a 48 (antes dos deletes). Scripts disponíveis em [`sql/CentimeterX_consultas.sql`](sql/CentimeterX_consultas.sql).

**Tabelas criadas e dados inseridos:**

![sql-tabelas](prints/sql-tabelas.png)

**Resultados dos SELECTs por tabela:**

![sql-resultados-1](prints/sql-resultados-1.png)

**Resultado do JOIN completo — Rovers com Estacao Base e Usuario:**

![sql-resultados-2](prints/sql-resultados-2.png)

**Resultado do JOIN completo — Sessoes de Correcao:**

![sql-resultados-3](prints/sql-resultados-3.png)

---

*Global Solution — C# Software Development | FIAP 3ESPW | 1º Semestre de 2026*
