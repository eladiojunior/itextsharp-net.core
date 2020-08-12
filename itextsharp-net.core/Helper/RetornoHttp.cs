using System;
using itextsharp_net.core.Models;

namespace itextsharp_net.core.Helper
{
    public class RetornoHttp
    {
        public bool Status { get; set; }
        public String Mensagem { get; set; }
        public Object Data { get; set; }

        public RetornoHttp(bool status, String mensagem, Object data)
        {
            this.Status = status;
            this.Mensagem = mensagem;
            this.Data = data;
        }
        
        public static RetornoHttp ResultErro(string erro)
        {
            return new RetornoHttp(false, erro, null);
        }
        
        public static RetornoHttp ResultSucesso(Object data)
        {
            return new RetornoHttp(true, "Sucesso", data);
        }

    }
}