using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class ModelBirth
{
    //Embarazo
    public int noPregnancy { get; set; }
    public int previousAbortions { get; set; }
    public int typePregnancy { get; set; }
    public int infTypePregnancy { get; set; }
    public int birth { get; set; }
    public int mortinatos { get; set; }
    public int gestationalAge { get; set; }
    public int gestationalAgeBy { get; set; }

    public ModelBirth()
    {

    }
}