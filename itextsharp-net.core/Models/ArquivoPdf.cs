using System;

namespace itextsharp_net.core.Models
{
    public class ArquivoPdf
    {
        public String Nome { get; set; }
        public long Tamanho { get; set; }
        public DateTime DataHora { get; set; }

        public ArquivoPdf()
        {
        }
        
        public ArquivoPdf(String nome, long tamanho, DateTime dataHora)
        {
            this.Nome = nome;
            this.Tamanho = tamanho;
            this.DataHora = dataHora;
        }
    }
}