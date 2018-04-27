using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class ModelGinecologicos
{
    public int? menarca { get; set; }
    public int? menacmia { get; set; }
    public int? relacion { get; set; }
    public string lastPapanicolaou { get; set; }
    public string nextPapanicolaou { get; set; }
    public string studies { get; set; }

    public bool dispareunia { get; set; } = false;
    public bool leucorrea { get; set; } = false;
    public string leucorreaTxt { get; set; }
    public bool lactorrea { get; set; } = false;
    public bool sickness { get; set; } = false;
    public string sicknessTxt { get; set; }
    public bool mamografia { get; set; } = false;
    public string dateMamografia { get; set; }
    public string nextMamografia { get; set; }
    public bool spQuirurgico { get; set; } = false;
    public string spQuirurgicoTxt { get; set; }
    public string observations { get; set; }



    public ModelGinecologicos()
    {

    }
}