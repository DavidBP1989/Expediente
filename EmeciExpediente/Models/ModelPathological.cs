using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

public class ModelPathological
{
    public string antecendet { get; set; }
    public bool rubeola { get; set; } = false;
    public int ageRubeola { get; set; } = -1;
    public bool sarampion { get; set; } = false;
    public int ageSarampion { get; set; } = -1;
    public bool tosFerina { get; set; } = false;
    public int ageTosFerina { get; set; } = -1;
    public bool roseola { get; set; } = false;
    public int ageRoseola { get; set; } = -1;
    public bool varicela { get; set; } = false;
    public int ageVaricela { get; set; } = -1;
    public string otherDisease { get; set; }

    //Enfermedades contagiosas
    public bool faringitis { get; set; } = false;
    public bool amigdalitis { get; set; } = false;
    public bool parasitosis { get; set; } = false;
    public bool hepatitis { get; set; } = false;
    public bool infeccion { get; set; } = false;
    public bool gastro { get; set; } = false;
    public bool otitis { get; set; } = false;
    public bool conjutivitis { get; set; } = false;
    public bool micosis { get; set; } = false;
    public bool bronquitis { get; set; } = false;
    public bool bronco { get; set; } = false;
    public bool estomatitis { get; set; } = false;
    public string otherContagious { get; set; }

    public ModelPathological()
    {

    }
}