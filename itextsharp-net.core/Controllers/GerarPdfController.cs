using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using itextsharp_net.core.Helper;
using itextsharp_net.core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace itextsharp_net.core.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GerarPdfController : ControllerBase
    {
        private readonly ILogger<GerarPdfController> _logger;

        public GerarPdfController(ILogger<GerarPdfController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<RetornoHttp> ListarArquivos()
        {
            List<ArquivoPdf> lista = new List<ArquivoPdf>();
            DirectoryInfo pasta = new DirectoryInfo("/data");
            if (pasta.Exists)
            {
                foreach (var file in pasta.GetFiles())
                {
                    lista.Add(new ArquivoPdf(file.Name, file.Length, file.CreationTime));
                }
            }
            return RetornoHttp.ResultSucesso(lista) ;
        }
        
        [HttpGet("{nome}")]
        public async Task<IActionResult> ObterArquivo(String nome)
        {
            DirectoryInfo pasta = new DirectoryInfo("/data");
            if (!pasta.Exists)
            {
                return Content("Nenhuma pasta de arquivos PDF existente.");
            }

            FileInfo arquivo = pasta.GetFiles().FirstOrDefault(w => w.Name.Equals(nome));
            if (arquivo == null)
            {
                return Content(String.Format("Nenhuma arquivo encontrato com esse nome [{0}].", nome));
            }
            
            var memory = new MemoryStream();  
            using (var stream = new FileStream(arquivo.FullName, FileMode.Open))  
            {  
                await stream.CopyToAsync(memory);  
            }  
            memory.Position = 0;  
            return File(memory, "application/pdf", arquivo.Name); 
        }

        [HttpPost]
        public async Task<RetornoHttp> GerarHtmlToPdf(ConteudoPdf conteudo)
        {
            if (conteudo == null || String.IsNullOrEmpty(conteudo.Conteudo))
                return RetornoHttp.ResultErro("Conteudo para gerar o PDF não informado.");
            
            ArquivoPdf arquivo = null;
            
            DirectoryInfo pasta = new DirectoryInfo("/data");
            if (!pasta.Exists)
            {//Criar
                pasta.Create();
            }
            
            var rng = new Random((int)DateTime.Now.Ticks);
            String pathArquivo = pasta.FullName + "/" + string.Format("{0}.pdf", rng.Next().ToString("0000000000"));
            FileInfo arquivoPdf = new FileInfo(pathArquivo);
            
            Document doc = new Document(PageSize.A4);//criando e estipulando o tipo da folha usada
            doc.SetMargins(40, 40, 40, 80);//estibulando o espaçamento das margens que queremos
            doc.AddCreationDate();//adicionando as configuracoes

            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(arquivoPdf.FullName, FileMode.Create));
            
            doc.Open();

            //criando uma string vazia
            string dados="";

            //criando a variavel para paragrafo
            Paragraph paragrafo = new Paragraph(dados, new Font(Font.NORMAL, 14));
            
            //etipulando o alinhamneto
            paragrafo.Alignment = Element.ALIGN_JUSTIFIED;
            
            //Alinhamento Justificado adicioando texto
            paragrafo.Add(conteudo.Conteudo);
            
            //acidionado paragrafo ao documento
            doc.Add(paragrafo);
            
            //fechando documento para que seja salva as alteraçoes.
            doc.Close();
            writer.Flush();
            writer.Close();
            
            //Atualizar
            arquivo = new ArquivoPdf();
            arquivo.Nome = arquivoPdf.Name;
            arquivo.Tamanho = arquivoPdf.Length;
            arquivo.DataHora = arquivoPdf.CreationTime;
            
            return RetornoHttp.ResultSucesso(arquivo);
            
        }
        
    }
}