﻿<%@ Page Language="C#" AutoEventWireup="true"  %>
<%@ Import  Namespace="System.Data" %>
<%@ Import  Namespace="EmeciCommon" %>
<%@ Import  Namespace="EmeciFacade" %>

<script runat="Server">
    protected void Page_Load(object sender, EventArgs e)
    {
        DataSet   dsPac;

        string emeci= Request.QueryString["Emeci"].ToString();
        string opt= Request.QueryString["opt"].ToString();
        string posicion = Request.QueryString["posicion"].ToString();
        string dato = Request.QueryString["dato"].ToString();


        dsPac = (new clsFARegistro()).GetPacienteTarjetaEmeci(dato, emeci, posicion);


        int idpaciente=0;

        this.Session["PacEmeci"] = emeci;
        this.Session["idpaciente"] = dsPac.Tables[0].Rows[0]["idpaciente"];
        this.Session["idregistro"] = dsPac.Tables[0].Rows[0]["idregistro"];
        //agregado para //
        this.Session["IdPatient"] = dsPac.Tables[0].Rows[0]["idpaciente"];

        this.Session["idMedEmeci"] = emeci.Split('-')[0] + emeci.Split('-')[1];

        if( !(dsPac == null) && dsPac.Tables[0].Rows.Count > 0)

            idpaciente = (int)dsPac.Tables[0].Rows[0]["idpaciente"];
        else
        {
            Response.Write("error paciente o clave incorrecta");
            Response.End();
        }

        switch(opt)
        {
            case "E": Response.Redirect("Expedient/GeneralInfo?d=" + DateTime.Now.ToString()); break;
            case "V": Response.Redirect("Expedient/Vaccines?d=" + DateTime.Now.ToString()); break;
            case "C": Response.Redirect("Expedient/Consultation?d=" + DateTime.Now.ToString()); break;
            case "EST": Response.Redirect("Expedient/Studies?d=" + DateTime.Now.ToString()); break;
        }





    }

</script>
