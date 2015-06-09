using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

/************************** Planeamento CMW *************************
 * 
 * ** Regras actuais **
 * 
 * Planeamento é feito à semana. 
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
 * PlanCMWv2$Produtos
 *          Lista de todos os produtos em encomendas abertas
 *          Contem informação sobre quantidades, peso, liga, dataprevista, urgencia.
 *          Contem também informação sobre o planeamento: semana/dia/turno em que a Macharia, Moldação e Fusão do produto estão concluídas.
 *          Esta informação é actualizada nas respectivas fases.
 *          Os produtos estão ordenados por Urgência (valores = 1 primeiro) e por Data prevista de entrega.
 *          Colunas:
 *              Id (int) - Id do par (Encomenda, Produto)
 *              Include (bit) - Flag que indica se este produto entra no planeamento
 *              NoEnc (string) - Número de encomenda
 *              NoProd (string) - Número de produto
 *              NoMolde (string) - Número de molde
 *              Liga (string) - Código da liga do produto
 *              Descricao Liga - Descrição da liga do produto
 *              PesoPeca (decimal) - Peso de uma peça individual
 *              Peso Gitos - Peso de uma caixa de peças (inclui gitos)
 *              NoMoldes (int) - Número de peças por caixa
 *              TempoMachos (int) - Tempo total necessário de Macharia por caixa (em minutos)
 *              QtdPendente (int) - Quantidade total de unidades a produzir
 *              CaixasPendente (int) - Quantidade total de caixas a produzir. Arrendonda para cima -> CaixasPendente = Ceiling(QtdPendente / NoMoldes)
 *              Local (int) - Local onde é produzido: 1 - GF, 2 - IMF, 3 - Manual
 *              DataPrevista (date) - Data prevista de entrega
 *              Urgente (bit) - Indica se é urgente
 *              
 * PlanCMWv2$PlanoProducao
 *          Guarda o Plano de Produção (Macharia/Moldação/Fusão)
 *          Colunas:
 *              
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
            ParametrosBD.ParametrosDefault(); //Preenche a tabela de parametros na BD caso esteja vazia

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Interface());
        }
    }
}
