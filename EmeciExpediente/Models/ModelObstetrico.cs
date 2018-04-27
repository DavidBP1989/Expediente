using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class ModelObstetrico
{
    public string fechaUltimoParto { get; set; }
    public string ultimoDiaRegla { get; set; }
    public int? gestas { get; set; }
    public int? partosNormales { get; set; }
    public int? partosAnormales { get; set; }
    public int? cesareas { get; set; }

    public int? rn { get; set; }
    public int? prematuro { get; set; }
    public int? forceps { get; set; }
    public int? aborto { get; set; }
    public int? obito { get; set; }
    public int? malFormado { get; set; }
    public int? rnEnfermos { get; set; }
    public int? embarazoMult { get; set; }

    public string complicacionesEmbarazo { get; set; }
    public string complicacionesParto { get; set; }
    public string complicacionesRecien { get; set; }
    public string complicacionesLact { get; set; }

    public ModelObstetrico()
    {

    }
}