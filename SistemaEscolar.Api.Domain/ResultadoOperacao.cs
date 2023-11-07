namespace SistemaEscolar.Api.Domain
{
    public sealed class ResultadoOperacao<TResultObject> where TResultObject : class
    {
        public TResultObject? resultado { get; set; }
        public string? MensagemRetorno { get; set; }
        public bool ExecutouComSucesso { get; set; }
        public string? Codigo { get; set; }
        public string? Detalhe { get; set; }
    }
}