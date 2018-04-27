using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class ModelDataBirth
{
    //Nacimiento
    public string birthDate { get; set; }
    public string birthPlace { get; set; }
    public string birthHospital { get; set; }

    //Embarazo
    public byte? pregnantNumber { get; set; }
    public byte? abortions { get; set; }
    public byte? pregnantType { get; set; }
    public byte? pregnantTypeInfo { get; set; }
    public byte? obtainedByBirth { get; set; }
    public byte? mortinatos { get; set; }
    public byte? gestationalAge { get; set; }
    public byte? gestationalAgeBy { get; set; }

    //Obtenido por parto patalogico
    public byte? typeDistocia { get; set; }
    public string typeDistociaInfo { get; set; }
    public byte? reasonDistocia { get; set; }
    public string reasonDistociaInfo { get; set; }

    public float birthWeight { get; set; }
    public float birthSize { get; set; }
    public float cefalico { get; set; }
    public byte? scoreApgar1 { get; set; }
    public byte? scoreApgar5 { get; set; }
    public byte? scoreSilverman1 { get; set; }
    public byte? scoreSilverman5 { get; set; }

    public byte? tamiz { get; set; }
    public string folio { get; set; }

    //complicaciones inmediatas al nacimiento
    public string complicationIn { get; set; }
    public string controlIn { get; set; }
    //complicaciones mediatas al nacimiento
    public string complicationMe { get; set; }
    public string controlMe { get; set; }

    //lactancia
    public byte? lactancia { get; set; }
    public byte? typeLactancia { get; set; }
    public string formula { get; set; }

    public string reduccion { get; set; }
    public string suspension { get; set; }

    public string sanguineo { get; set; }
    public string date { get; set; }

    public ModelDataBirth()
    {

    }

}