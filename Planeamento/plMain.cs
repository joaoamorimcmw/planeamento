using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

/************************** Planeamento CMW *************************
 * 
 * ** Regras actuais **
 * 
 * Moldação de um produto só pode começar no turno seguinte à Macharia desse produto estar concluída
 * Fusão de um produto só pode ser feita no turno seguinte à Moldação estar concluída
 * Produtos em que a liga tem uma carga total inferior à fusão mínima não são incluídos no planeamento
 * 
 * ** Fases **
 * 
 * Init     Lê a Sales Line do Navision e preenche a tabela Produtos na BD do Planeamento
 *          Exclui (muda uma flag na tabela) os produtos de ligas com pouca carga
 *          Actualiza informação das ligas (tabela ligas)
 * Macharia
 * Moldacao
 * Fusao
 * 
 * ** Tabelas da BD Planeamento **
 * 
 * PlanCMW$Produtos
 *          Lista de todos os produtos em encomendas abertas
 *          Contem informação sobre quantidades, peso, liga, dataprevista, urgencia.
 *          Contem também informação sobre o planeamento: semana/dia/turno em que a Macharia, Moldação e Fusão do produto estão concluídas.
 *          Esta informação é actualizada nas respectivas fases.
 *          Os produtos estão ordenados por Urgência (valores = 1 primeiro) e por Data prevista de entrega.
 *          Colunas:
 *              Id (int) - Id do par (Encomenda, Produto)
 *              Include (bit) - Flag que indica se este produto entra no planeamento
 *              NoEnc (string) - Número de encomenda
 *              NoProd (string) - Numero de produto
 *              Liga (string) - Código da liga do produto
 *              PesoPeca (decimal) - Peso de uma peça individual
 *              NoMoldes (int) - Número de moldes necessários para o produto
 *              Local (int) - Local onde é produzido: 1 - GF, 2 - IMF, 3 - Manual
 *              QtdPendente (int) - Quantidade a produzir
 *              DataPrevista (date) - Data prevista de entrega
 *              Urgente (bit) - Indica se é urgente
 *              SemanaMacharia (int) - Semana onde são concluídos todos os machos
 *              DiaMacharia (int) - Dia onde são concluídos todos os machos
 *              SemanaMoldacao (int) - Semana onde é concluida toda a moldação
 *              DiaMoldacao (int) - Dia onde é concluida toda a moldação
 *              TurnoMoldacao (int) - Turno onde é concluida toda a moldação
 *              SemanaFusao (int) - Semana onde é concluida toda a fusão
 *              DiaFusao (int) - Dia onde é concluida toda a fusão
 *              TurnoFusao (int) - Turno onde é concluida toda a fusão
 *              PesoVazadas (decimal) - Peso das peças já vazadas
 *              QtdVazadas (int) - Quantidade de peças já vazadas
 *              PesoEmFalta (decimal) - Peso que falta vazar
 *              
 * PlanCMW$Ligas
 *          Lista de todas as ligas existentes e das classes
 *          Colunas
 *              Liga (string) - Código da liga
 *              Descrição (string)  - Nome da liga
 *              Codigo Classe - Código da classe da liga
 *              Descricao Classe - Descrição da classe
 *              
 * PlanCMW$Macharia
 *          Plano de fabrico de todos os machos associados aos produtos
 *          Colunas
 *              Id (int) - Id do produto associado
 *              CodMach (string) - Código do macho a produzir
 *              Fabrica (int) - Fábrica onde é feito o macho (1 ou 2)
 *              Semana (int) - Semana onde é produzido o macho
 *              Dia (int) - Dia onde é produzido o macho
 *              Qtd (decimal) - Quantidade de machos produzidos
 *              Tempo (int) - Tempo total de fabrico dos machos (tempo individual * qtd)
 *              Acc (int) - Tempo acumulado de fabrico de machos nesse dia
 *              
 * PlanCMW$Moldacao
 *          Plano de fabrico dos moldes
 *          Colunas
 *              Id (int) - Id do produto associado
 *              Local (int) - Local onde é feito o molde (1 - GF, 2 - IMF, 3 - Manual)
 *              Semana (int) - Semana onde é feito o molde
 *              Dia (int) - Dia onde é feito o molde
 *              Turno (int) - Turno em que é feito o molde
 *              Caixas (int) - Numero de caixas
 *              CaixasAcc (int) - Caixas produzidas acumuladas nesse turno
 *              
 * PlanCMW$Fusao
 *          Plano de fusões
 *          Colunas
 *              Semana (int)
 *              Dia (int)
 *              Turno (int)
 *              Forno (int) - Forno em que a fusão é feita
 *              NoFusao (int) - Qual o número da fusão feita por forno e turno
 *              Liga (string) - Código da liga
 *              PesoTotal (decimal)  - Carga total da fusão
 *              
 * PlanCMW$Rebarbagem
 *          Plano da rebarbagem
 *          Colunas
 *              Id (int) - Id do produto associado
 *              Semana (int) - Semana de rebarbagem
 *              Dia (int) - Dia de rebarbagem
 *              Turno (int) - Turno de rebarbagem
 *              Posto (string) - Centro máquina onde passa o produto
 *              QtdPecas (int) - Quantidade de peças feitas por máquina, turno e produto
 *              Tempo (decimal) - Tempo total gasto nas peças por máquina e turno
 *              
 * PlanCMW$PostosRebarbagem
 *          Lista os vários centros máquinas e quais os postos de trabalho associados, bem como a quantidade existente desse posto
 *          Colunas
 *              CodCentroMaquina - Código do centro máquina
 *              DescCentroMaquina - Descricao do centro máquina
 *              CodPosto - Código do posto de trabalho
 *              DescPosto - Descrição do posto de trabalho
 *              QtdPosto - Quantidade existe do posto de trabalho           
 * 
 * ** Ordem do planeamento **
 * 
 * Este é o flow completo de todo o processo (agora este código está na Interface associada ao botão Iniciar)
 *           
 *      Init.UpdateProdutos(); //Vai à tabela SalesLine do Navision buscar as encomendas abertas, e preenche a tabela Produtos na BD do planeamento
 *      Init.ExcluiProdutosBaixaCarga(); //Calcula quais os produtos em que a liga não tem peso total suficiente para uma fusao, e exclui do planeamento
 *      Init.InicializaLigas(); //Actualiza a tabela Ligas com base no Navision
 *
 *      Macharia macharia = new Macharia();
 *      macharia.LimpaBDMacharia(); //Limpa a tabela Macharia na BD do planeamento
 *      macharia.Executa(1); //Faz o plano de Macharia da CMW1 e guarda o resultado na BD
 *      macharia.Executa(2); //Faz o plano de Macharia da CMW2 e guarda o resultado na BD
 *      macharia.LimpaTabelas(); //Limpa as DataTables intermédias
 *
 *      Moldacao moldacao = new Moldacao();
 *      moldacao.LimpaBDMoldacao(); //Limpa a tabela Moldação na BD do planeamento
 *      moldacao.Executa(1); //Faz o plano de Moldação da GF e guarda o resultado na BD
 *      moldacao.Executa(2); //Faz o plano de Moldação da GF e guarda o resultado na BD
 *      moldacao.Executa(3); //Faz o plano de Moldação da GF e guarda o resultado na BD
 *      moldacao.LimpaTabelas(); //Limpa as DataTables intermédias
 *
 *      Fusao fusao = new Fusao();
 *      fusao.LimpaBDFusao(); //Limpa a tabela Fusão na BD do planeamento
 *      fusao.Executa(1); //Faz o plano de Fusão da CMW1
 *      fusao.Executa(2); //Faz o plano de Fusão da CMW2
 *      fusao.EscreveBD(); //Guarda o plano de Fusão na BD
 *      fusao.ListaProdutosEmFalta(); //Lista os produtos que não têm fusão planeada por falta de carga
 *      fusao.LimpaTabelas(); //Limpa as DataTables intermédias
*/

namespace Planeamento
{
    static class plMain
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Interface());
        }
    }
}
