using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

/************************** Planeamento CMW *************************
 * 
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
 *              Local (int) - Local onde é produzido (1 - GF, 2 - IMF, 3 - Manual)
 *              Semana (int) - Semana de produção (começa a contar em 1)
 *              Id (int) - Id do produto na tabela Produtos
 *              Caixas (int) - Número de caixas a produzir
 * 
 * PlanCMWv2$Parametros
 *          Guarda os parâmetros da produção. Ex: turnos, capacidade fornos, capacidade caixas, etc.
 *          Colunas:
 *              Parametro (string) - Nome do parametro
 *              Valor (decimal) - Valor do parametro
 * 
 * ** Ordem do planeamento **
 * 
 * Este é o flow completo de todo o processo (agora este código está na Interface associada ao botão Iniciar)
 *           
 *      Init.UpdateProdutos(); //Vai à tabela SalesLine do Navision buscar as encomendas abertas, e preenche a tabela Produtos na BD do planeamento
 *      Init.ExcluiProdutosBaixaCarga(); //Calcula quais os produtos em que a liga não tem peso total suficiente para uma fusao, e exclui do planeamento
 *      Init.InicializaLigas(); //Actualiza a tabela Ligas com base no Navision
 *
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
