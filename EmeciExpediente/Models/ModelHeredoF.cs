using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class ModelHeredoF
{
    /* 
        propiedades del padre
    */
    public string f_name { get; set; }
    public string f_1LastName { get; set; }
    public string f_2LastName { get; set; }
    public string f_birthdate { get; set; }
    public int f_civilStatus { get; set; }
    public int f_health { get; set; }
    public string f_healthInfo { get; set; }
    public int f_education { get; set; }
    public string f_educationInfo { get; set; }
    public int f_toxicomania { get; set; } = 0;
    public string f_toxicomaniaInfo { get; set; }

    public bool f_alergia { get; set; } = false;
    public bool f_asma { get; set; } = false;
    public bool f_epilepsia { get; set; } = false;
    public bool f_diabetes { get; set; } = false;
    public bool f_hiper { get; set; } = false;
    public bool f_cancer { get; set; } = false;
    public bool f_malform { get; set; } = false;
    public bool f_cardio { get; set; } = false;
    public string f_others { get; set; }



    /*
        propiedades de la madre
    */
    public string m_name { get; set; }
    public string m_1LastName { get; set; }
    public string m_2LastName { get; set; }
    public string m_birthdate { get; set; }
    public int m_civilStatus { get; set; }
    public int m_health { get; set; }
    public string m_healthInfo { get; set; }
    public int m_education { get; set; }
    public string m_educationInfo { get; set; }
    public int m_toxicomania { get; set; } = 0;
    public string m_toxicomaniaInfo { get; set; }

    public bool m_alergia { get; set; } = false;
    public bool m_asma { get; set; } = false;
    public bool m_epilepsia { get; set; } = false;
    public bool m_diabetes { get; set; } = false;
    public bool m_hiper { get; set; } = false;
    public bool m_cancer { get; set; } = false;
    public bool m_malform { get; set; } = false;
    public bool m_cardio { get; set; } = false;
    public string m_others { get; set; }




    /*
        mas informacion
    */
    public int cefalico { get; set; } = 1;
    public int ablactacion { get; set; } = 1;
    public int sedente { get; set; } = 1;
    public int destete { get; set; } = 1;
    public int caminar { get; set; } = 1;
    public int silaba { get; set; } = 1;
    public int palabras { get; set; } = 1;

    public int guarderia { get; set; } = -1;
    public int jardin { get; set; } = 0;
    public int primaria { get; set; } = 0;
    public int secundaria { get; set; } = 0;
    public int preparatoria { get; set; } = 0;

    public int urbanizacion { get; set; } = 0;
    public int vivienda { get; set; } = 0;
    public int cohabitan { get; set; } = 1;


    public ModelHeredoF()
    {

    }
}