using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Planeamento
{
     class PrimeiroPlano
    {
       
            
        public PrimeiroPlano(){

            
            //***************************  DESCRICAO da FUNCAO  *********************************   1//
//des            BDMacharia bdMac = new BDMacharia();
            //inicializa as tabelas "Produtos Plan"; "Plan Macharia"; e as DataTables planMachariaCMW1; planMachariaCMW2
            //vai ler a tabela "Sales Line" e preenche o DataSet produtosCMW1      
            //insere na tabela "Produtos Plan" a lista dos produtos CMW1 a planear
            //por cada produto a planear, verifica os machos associados e preenche machosCMW1
            //por cada macho, vê o tempo de fabrico, faz os calculos correspondentes em função da quantidade
            //insere na DataTable planMachariaCMW1/planMachariaCMW2 que é o plano final da macharia
            //**********************************************************************************//

            //***************************  DESCRICAO da FUNCAO  *********************************   2//

            //getPlanoMachariaCMW1 retorna a lista de machos a planear
//des             PlanMacharia pM = new PlanMacharia(bdMac.getListaMachariaCMW1(), bdMac.getListaMachariaCMW2());
            //apagar os planos da macharia recebidos de BDMacharia
            //verifica as capacidades das macharias através da consulta na tabela Parametros
            //inicializada os DataTables planoCMW1 e planoCMW2
            //por cada linha da lista de machos, insere no planeamento da macharia planoCMW1 e planoCMW2

            //**********************************************************************************//

            //***************************  DESCRICAO da FUNCAO  *********************************   3//

            //insere o planeamento feito no Plan Macharia, na tabela "Plan Macharia"
//des             bdMac.inserePlanos(pM.getPlanoCMW1(), pM.getPlanoCMW2());
           
            //**********************************************************************************//

            //***************************  DESCRICAO da FUNCAO  *********************************   4//
            //recebe os planeamentos de macharia e preenche o plano de moldacao dos 3 locais de producao
            //os planeamentos dos 3 locais de producao estão em: planeamentoMoldacGF;planeamentoMoldacIMF;planeamentoMoldacMAN

//des             PlanMold pMolde = new PlanMold(bdMac.getCMW1(), bdMac.getCMW2());

            //imprimir os 3 planos de moldacao
            //pMolde.imprimePlan();


            //***************************  DESCRICAO da FUNCAO  *********************************   5//
            //escrever os 3 planeamentos na B.D. tabela "Plan Mold"
//des              BDMolde bdMolde = new BDMolde(pMolde.getPlanMoldGF(), pMolde.getPlanMoldIMF(), pMolde.getPlanMoldMAN());

            //**********************************************************************************//


            //***************************  DESCRICAO da FUNCAO  *********************************   6//
            //lê da B.D. o planeamento de moldacao e pôe em fusaoCMW1,fusaoCMW2
            BDFusao bdFus = new BDFusao();
            //bdFus.imprime();


            //***************************  DESCRICAO da FUNCAO  *********************************   7//
            //recebe os 3 planeamentos da bdFus e atribui os fornos às cargas
            PlanFusao pFus = new PlanFusao(bdFus.getFusCMW1(),bdFus.getFusCMW2());

            bdFus.inserePlano(pFus.getPlanoCMW1(),pFus.getPlanoCMW2());

            //**********************************************************************************//




        }
    }
}
