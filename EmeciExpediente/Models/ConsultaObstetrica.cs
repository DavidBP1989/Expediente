
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace EmeciExpediente.Models
{

using System;
    using System.Collections.Generic;
    
public partial class ConsultaObstetrica
{

    public int IdConsultaObstetrica { get; set; }

    public Nullable<byte> noembarazo { get; set; }

    public Nullable<byte> abortos { get; set; }

    public Nullable<System.DateTime> FechaUltmoParto { get; set; }

    public Nullable<System.DateTime> PrimerDiaUltimaMestruacuion { get; set; }

    public Nullable<byte> ToxemiasPrevias { get; set; }

    public Nullable<byte> CesareasPrevia { get; set; }

    public Nullable<byte> UsoDeForceps { get; set; }

    public Nullable<byte> Motinatos { get; set; }

    public Nullable<byte> RMVivos { get; set; }

    public Nullable<byte> EmbarazoEtopicos { get; set; }

    public Nullable<byte> EmbrazosComplicadosPrevios { get; set; }

    public string EmbrazoEtopicoExplique { get; set; }

    public string EmbarazosComplicadosExplique { get; set; }

    public Nullable<byte> NoComplicacionesPertinales { get; set; }

    public string ComplicacionesPerinatalesExplique { get; set; }

    public Nullable<byte> NoEmbrazosAnormales { get; set; }

    public string EmbarazosAnormalesExplique { get; set; }

    public Nullable<byte> FU { get; set; }

    public Nullable<byte> FCF { get; set; }

    public Nullable<byte> CC { get; set; }

    public Nullable<byte> CA { get; set; }

    public Nullable<byte> LF { get; set; }

    public Nullable<byte> DSP { get; set; }

    public string Posicion { get; set; }

    public string Presentacion { get; set; }

    public string siuacuion { get; set; }

    public string Actitud { get; set; }

    public string MovimientosFetales { get; set; }

    public Nullable<byte> PesoAproxProducto { get; set; }

    public Nullable<byte> TA { get; set; }

    public Nullable<byte> FCM { get; set; }

    public string Edema { get; set; }

    public Nullable<bool> SeHizoUf { get; set; }

    public Nullable<int> idconsulta { get; set; }

    public Nullable<bool> activaSexualmente { get; set; }

    public string EspecifiqueToxemias { get; set; }

    public Nullable<byte> Partos { get; set; }

    public Nullable<byte> TipoDistocia { get; set; }

    public Nullable<byte> MotivoDistocia { get; set; }

    public string EspecifiqueMotivoDistocia { get; set; }

    public string EspecifiqueTipoDistocia { get; set; }

    public string ultrasonido { get; set; }

    public string exploracionFisica { get; set; }

    public string Observaciones { get; set; }

    public Nullable<byte> Gestas { get; set; }

    public Nullable<byte> PartosEutocicos { get; set; }

    public Nullable<byte> PartosDistocios { get; set; }

}

}
