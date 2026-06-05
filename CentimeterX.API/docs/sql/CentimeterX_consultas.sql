SET ECHO ON

-- ============================================================
-- Centimeter-X API - Scripts de Consulta Oracle
-- Disciplina: C# Software Development - FIAP 3ESPW 2026
-- Copie todo o código e cole em uma planilha do Oracle SQL Developer
-- Integrantes
-- Rafael Duarte de Freitas - 558644
-- Rafael Gaspar Bragança Martins - 557228
-- Luiz Gustavo da Silva - 558358
-- Pedro Almeida e Camacho - 556831
-- Renan Dias Utida - 558540
-- ============================================================

-- Consultas simples por tabela
SELECT * FROM CX_ESTACAO_BASE;

SELECT * FROM CX_USUARIO;

SELECT * FROM CX_ROVER;

SELECT * FROM CX_SESSAO_CORRECAO;

SELECT * FROM CX_OCORRENCIA;

-- ============================================================
-- Consulta completa: Rovers com Estacao Base e Usuario
-- ============================================================
SELECT
    r."ID_ROVER",
    r."NM_ROVER",
    r."TIPO_ROVER",
    r."TP_STATUS",
    e."CD_CODIGO"    AS ESTACAO_CODIGO,
    e."NM_ESTACAO"   AS ESTACAO_NOME,
    e."FL_ONLINE"    AS ESTACAO_ONLINE,
    u."NM_USUARIO"   AS USUARIO_NOME,
    u."DS_EMAIL"     AS USUARIO_EMAIL,
    u."TP_PERFIL"    AS USUARIO_PERFIL,
    r."DT_CADASTRO"
FROM CX_ROVER r
JOIN CX_ESTACAO_BASE e ON r."ID_ESTACAO_BASE" = e."ID_ESTACAO"
JOIN CX_USUARIO u ON r."ID_USUARIO" = u."ID_USUARIO";

-- ============================================================
-- Consulta completa: Sessoes com Rover e Estacao Base
-- ============================================================
SELECT
    s."ID_SESSAO",
    r."NM_ROVER"                AS ROVER_NOME,
    r."TIPO_ROVER"              AS ROVER_TIPO,
    e."CD_CODIGO"               AS ESTACAO_CODIGO,
    e."NM_ESTACAO"              AS ESTACAO_NOME,
    s."TP_STATUS_FIX"           AS STATUS_FIX,
    s."NR_PRECISAO_HORIZONTAL_CM",
    s."NR_PRECISAO_VERTICAL_CM",
    s."DS_SISTEMA_SATELITE",
    s."DT_INICIOU_EM",
    s."DT_ENCERRADO_EM"
FROM CX_SESSAO_CORRECAO s
JOIN CX_ROVER r ON s."ID_ROVER" = r."ID_ROVER"
JOIN CX_ESTACAO_BASE e ON s."ID_ESTACAO_BASE" = e."ID_ESTACAO";

-- ============================================================
-- Consulta completa: Ocorrencias com Rover
-- ============================================================
SELECT
    o."ID_OCORRENCIA",
    r."NM_ROVER"        AS ROVER_NOME,
    r."TIPO_ROVER"      AS ROVER_TIPO,
    o."TP_OCORRENCIA"   AS TIPO_OCORRENCIA,
    o."DS_DESCRICAO",
    o."NR_LATITUDE",
    o."NR_LONGITUDE",
    o."DS_FOTO_URL",
    o."DT_CRIADA_EM"
FROM CX_OCORRENCIA o
JOIN CX_ROVER r ON o."ID_ROVER" = r."ID_ROVER";
