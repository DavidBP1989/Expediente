<%@ Page Language="C#" AutoEventWireup="true" %>
<%@ Import  Namespace="System.Data" %>
<%@ Import  Namespace="System.Net" %>
<%@ Import  Namespace="System.IO" %>
<%@ Import  Namespace="System.Security.Cryptography" %>
<%@ Import  Namespace="System.Xml" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<HTML>
	<HEAD>
		<title>EMECI - Expediente Medico Electronico Clinico Internacional</title>
		<meta content="text/html; charset=iso-8859-1" http-equiv="Content-Type">
		<LINK rel="stylesheet" type="text/css" href="../emeci.css">
			<LINK rel="stylesheet" type="text/css" href="../emeciwebsite.css">
	<script src="../scripts/jquery-1.7.1.min.js"></script>
    <script src="../scripts/jquery.maskedinput.js"></script>
</head>

    
<script runat="Server">
    public string signature = "";
    public string tx_value = "250";
    public string ApiKey = "5ualu0dhdlfptqa9fthtr2h78k";
    public string merchantId = "516707";
    public string referenceCode = DateTime.Now.ToString("ddMMMyyyymmss");
    public string accountId = "518172";
    string currency = "MXN";


    private string GetMd5Hash(MD5 md5Hash, string input)
    {

        // Convert the input string to a byte array and compute the hash. 
        byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

        // Create a new Stringbuilder to collect the bytes 
        // and create a string.
        StringBuilder sBuilder = new StringBuilder();

        // Loop through each byte of the hashed data  
        // and format each one as a hexadecimal string. 
        for (int i = 0; i < data.Length; i++)
        {
            sBuilder.Append(data[i].ToString("x2"));
        }

        // Return the hexadecimal string. 
        return sBuilder.ToString();
    }
    
    private void getSignature()
    {
        signature = ApiKey + "~" + merchantId + "~" + referenceCode + "~" + tx_value + "~" + currency;
        using (MD5 md5Hash = MD5.Create())
        {

            signature = GetMd5Hash(md5Hash, signature);
        }
    }
    
    protected void Page_Load(object sender, EventArgs e)
    {

        getSignature();
        
    }

    protected void ddlPrecio_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlPrecio.SelectedIndex == 1) tx_value = "2000";

        getSignature();
    }
</script>
	<body style="BACKGROUND-COLOR: #fff">
		<div id="bodyContent">
			<!--#INCLUDE VIRTUAL = "/includes/emeciHeader.inc"-->
			
			<div id="MenuPrincipal">
				<img src="../imagenes/menuActivoPacientes.jpg" border="0" usemap="#Map" />
				<map name="Map" id="Map">		
					<area shape="rect" coords="7,1,85,33" href="/default.aspx" alt="Inicio" />
					<area shape="rect" coords="87,1,243,34" href="/informacion_medicos.aspx" alt="Información para Médicos" />
					<area shape="rect" coords="249,0,420,35" href="/informacion_pacientes.aspx" alt="Información para Pacientes" />
					<area shape="rect" coords="425,-1,584,34" href="/directorio_pediatras.aspx" alt="Directorio de Especialistas" />
					<area shape="rect" coords="590,0,683,33" href="/contactenos.aspx" alt="Contáctenos" />
				</map>	
			</div>
			<div id="divform">
			<div id="informationBlock">
				<h2>Pago por Mantenimiento Anual de Expediente Médico Clínico</h2>
				<h3>ESQUEMA PARA CUBRIR CUOTA DE MANTENIMIENTO ANUAL EMECI, S.C.</h3>
				<P>
				¡Ahora tener Activo su Expediente Médico es más Sencillo!
				</p>
				<p>
					A efectos de agilizar los pagos de las cuotas de mantenimiento de su Expediente EMECI, usted podrá optar por escoger cualquier de los siguientes medios:
					<table width="100%" border="0"  cellpadding="0" cellspacing="0">
    <tbody>
        <tr>
            <td>
               
                <img src="/img/Img_AmericanExpress.jpg" border="0">
            </td>
           
            <td>
              
                <img src="/img/Img_MasterCard.jpg" border="0">
            </td>
            <td>
               
                <img src="/img/Img_Visa.jpg" border="0">
            </td>
            <td>
                <img src="/img/oxxo.jpg" width="77" height="47" border="0">
            </td>
        </tr>
		</table>
				</p>
	<form id="form2" runat="server"> 
        <asp:DropDownList ID="ddlPrecio" runat="server" OnSelectedIndexChanged="ddlPrecio_SelectedIndexChanged" AutoPostBack="true">
            <asp:ListItem Value="250" Text="Anual - 250.00"></asp:ListItem>
            <asp:ListItem value="2000" Text="Unico - 2,000.00"></asp:ListItem>
        </asp:DropDownList>
    </form>
				
   <form id="form" method="post"  target="_blanck"  action="https://gateway.payulatam.com/ppp-web-gateway/">
  <input name="merchantId"    type="hidden"  value="<%= merchantId %>">
  <input name="accountId"     type="hidden"  value="<%= accountId %>">
  <p>
  <span>Tarjeta EMECI</span><BR/>
  <input  id="description" name="description" data-mask="99999-9999-9999"  type="text"    >
  </p>
  <input name="referenceCode" type="hidden"  value="<%= referenceCode %>">
  <input name="amount"        type="hidden"  value="<%= tx_value %>">   
  <input name="tax"           type="hidden"  value="0"  >
  <input name="taxReturnBase" type="hidden"  value="0" >
  <input name="currency"      type="hidden"  value="<%=currency %>">
  <input name="signature"     type="hidden"  value="<%=signature %>" >
  <input name="test"          type="hidden"  value="0" >
  <input name="buyerEmail"    type="hidden"  value="info@emeci.com">
  <input name="lng"    type="hidden"  value="es">         
  <input name="Submit"  type="submit"  value="Enviar" >
</form>
 
<p>
Consideramos que es un mecanismo bastante sencillo, sin embargo, de tener dudas, con gusto el departamento de Soporte está en la mayor de las disposiciones para auxiliarlo, ya sea vía correo electrónico a soporte@emeci.com, vía Facebook o Twitter.
</p>
<p>
Agradecemos su atención y deseamos servirle para la conservación de la salud de sus hijos.
</p>
	</div>
</div>

        <iframe id="ipayu" name="ipayu" scrolling="no" style="clear:both;border-style: none; width:820px; overflow:hidden; height:1280px; list-style-type: none; table-layout: auto;">

        </iframe>
   
		</div>	
		  <script>
      $(document).ready(function () {
          
          $('#description').mask('99999-9999-9999');
      });
         </script>
	</body>
</HTML>
