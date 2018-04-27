<%@ Page Language="C#" AutoEventWireup="true" %>
<%@ Import  Namespace="System.Data" %>
<%@ Import  Namespace="System.Net" %>
<%@ Import  Namespace="System.IO" %>
<%@ Import  Namespace="System.Security.Cryptography" %>
<%@ Import  Namespace="System.Xml" %>


<!DOCTYPE html>
<html lang="en">
<head>
<meta charset="UTF-8">
<meta http-equiv="X-UA-Compatible" content="IE=edge">
<meta name="viewport" content="width=device-width, initial-scale=1">
<title>Expediente M&eacute;dico Electr&oacute;nico Cl&iacute;nico Internacional</title>

<!-- Bootstrap -->
<link rel="stylesheet" href="css/bootstrap.css">
<link rel="stylesheet" href="css/emeci.css">

<!-- HTML5 shim and Respond.js for IE8 support of HTML5 elements and media queries -->
<!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
<!--[if lt IE 9]>
      <script src="https://oss.maxcdn.com/html5shiv/3.7.2/html5shiv.min.js"></script>
      <script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
    <![endif]-->
    
    	
   
  
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

<body class="page">


<!-- Navigation -->
<nav id="mainNav" class="navbar navbar-default navbar-custom navbar-fixed-top">
    <div class="container">
        <!-- Brand and toggle get grouped for better mobile display -->
        <div class="navbar-header page-scroll">
            <button type="button" class="navbar-toggle" data-toggle="collapse" data-target="#bs-example-navbar-collapse-1">
                <span class="sr-only">Toggle navigation</span> Menu <i class="fa fa-bars"></i>
            </button>
            <a class="navbar-brand page-scroll" href="#page-top"><img src="images/logo-emeci.png" alt=""></a>
        </div>
        
        
        <!-- Topbar -->
        <div class="topbar hidden-xs">
            <ul class="loginbar pull-right list-inline top-links">						 
                <li><a href="page_faq.html"><img src="images/icono-acceso.png" /> Acceso a M&eacute;dicos</a></li>
                <li><a href="page_login.html"><img src="images/icono-facebook.png" /> EMECI</a></li>
                <li><a href="page_login.html"><img src="images/icono-twitter.png" /> emecimx</a></li>	
            </ul>
        </div>
        <!-- End Topbar -->
        
 
        <!-- Collect the nav links, forms, and other content for toggling -->
        <div class="collapse navbar-collapse" id="bs-example-navbar-collapse-1">
            <ul class="nav navbar-nav navbar-right">
                <li class="hidden">
                    <a href="#page-top"></a>
                </li>
                <li><a class="page-scroll" href="home.aspx">Inicio</a></li>
                <li><a class="page-scroll" href="membresias.aspx">Membresias</a></li>
                <li><a class="page-scroll" href="especialistas.html">Especialistas</a></li>
                <li><a class="page-scroll" href="tips.html">Tips</a></li>
                <li><a class="page-scroll btn btn-primary" href="https://www.emeci.com/expediente/loginExp.aspx">Acceso a Expediente</a></li>
            </ul>
        </div>
        <!-- /.navbar-collapse -->
    </div>
    <!-- /.container-fluid -->
</nav>


<style>
	.lista-caracteristicas li {
		background: transparent url(images/check.png) no-repeat center left;
		padding-left: 35px;
		margin:5px 0
		
		
	}
</style>


<section class="bodycopy">

    <div class="container">


	<div class="col-md-10 col-md-push-1">
		  <h1>Membresia EMECI </h1>
          <br>
          
          
          <div class="row">
          
          	<div class="col-md-6">
            
             
            	<div class="panel">
                
                    <div class="panel-body" style="padding: 20px 40px;">
                    
                        <h2 class="text-center">Nuevo Expediente</h2>
                        <br>
                        <ul class=" lista-caracteristicas list-unstyled">
                        	<li>Acceso desde internet los 365 dias del año.</li>
                        	<li>Almacenamiento de estudios, recetas, y archivos digitales en la nube.</li>
                        	<li>Aplicación movil para el paciente.</li>
                        	<li>Acceso seguro y controlado para médicos.</li>
                        </ul>
                         <p>&nbsp;</p> 
                        <br />
                                            
                            <form id="form" method="post"  target="_blanck"  action="https://gateway.payulatam.com/ppp-web-gateway/">
                              <input name="merchantId"    type="hidden"  value="<%= merchantId %>">
                              <input name="accountId"     type="hidden"  value="<%= accountId %>">                         
                              <input  id="DescNuevo" name="description"  type="hidden" value="NUEVO">                         
                              <input name="referenceCode" type="hidden"  value="<%= referenceCode %>">
                              <input name="amount"        type="hidden"  value="<%= tx_value %>">   
                              <input name="tax"           type="hidden"  value="0"  >
                              <input name="taxReturnBase" type="hidden"  value="0" >
                              <input name="currency"      type="hidden"  value="<%=currency %>">
                              <input name="signature"     type="hidden"  value="<%=signature %>" >
                              <input name="test"          type="hidden"  value="0" >
                              <input name="buyerEmail"    type="hidden"  value="info@emeci.com">
                              <input name="lng"    type="hidden"  value="es">   
                              <p class="text-center">       
                                 <input id="Submit" name="Submit" class="btn btn-success"  type="submit"  value="Crear Expediente" >
                              </p>
                            </form>
                      
                     
                       
                    </div>
                
                </div>
            
            
            
            </div>
          	<div class="col-md-6">
            	<div class="panel">
                
                    <div class="panel-body"  style="padding: 20px 40px;">
                    
                        <h2 class="text-center">Renovación</h2>
                        <br>
                        <p>Continue disfrutando de los beneficios de su Expediente Médico Electrónico Clínico Internacional. </p>
                        <p>Haga click en el siguiente boton para extender su  membresia 1 año.</p>
                        

                       <form id="form2" runat="server"> 
                            <asp:DropDownList ID="ddlPrecio" runat="server" style="display:none" OnSelectedIndexChanged="ddlPrecio_SelectedIndexChanged" AutoPostBack="true">
                                <asp:ListItem Value="250" Text="Anual - 250.00"></asp:ListItem>
                            </asp:DropDownList>
                        </form>
                         <center>
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
                              <input id="Submit" name="Submit" class="btn btn-primary "  type="submit"  value="PAGAR RENOVACION" >
                         </form>
                        </center>
<!--
                        <p class="text-center">
							<a href="" onclick='$("#Submit").click();return false;' class="btn btn-primary ">Pagar Renovación</a>
                        </p>
                        -->
                    </div>
                
                </div>            
            </div>
          
          </div>
          
          <div class="row">
          
              <div class="col-md-12">
              	<div class="panel">
                	<div class="panel-body" style="padding:  40px">
              
                         <p>Para su comodida puede realizar su registro o renovación con distintas formas de pago con tarjetas de debito/credito o realizar depositos  en los siguientes bancos o tienedas de conveniencia.</p> 
                         <br>
                         
                         <div class="row">
                         
                            <div class="col-md-4 text-center">
                                <h4 class="small" ><strong>Tarjetas de Credito/Debito</strong></h4>
                                <p  >  <img src="images/tarjetas.png" alt=""></p>
                            </div>
                            <div class="col-md-8 text-center">
                                <h4 class="small"><strong>Depositos Directos/Referenciados</strong></h4>
                                <p > <img src="images/depositos.png" alt=""></p>
                            </div>
                         
                         </div>
                     </div>
                
                </div>
              
              </div>
          
          </div>
		     
            <p>&nbsp;</p>
            <p class="text-center">Consideramos que es un mecanismo bastante sencillo, sin embargo, de tener dudas, con gusto el departamento de Soporte está en la mayor de las disposiciones para auxiliarlo, ya sea vía correo electrónico a <a href="mailto:soporte@emeci.com">soporte@emeci.com</a>, vía Facebook o Twitter.</p>
            <p class="text-center">Agradecemos su atención y deseamos servirle para la conservación de la salud de sus familia.</p>
		</div>	
  
  
	</div>
</section>

    <!--
<section class="bodycopy">

    <div class="container">


	<div class="col-md-10 col-md-push-1">
		  <h1>Pago por Mantenimiento Anual de Expediente M&eacute;dico Cl&iacute;nico </h1>
		  <p>&iexcl;Ahora tener Activo su Expediente M&eacute;dico es m&aacute;s Sencillo!</p>
		  <p>A efectos de agilizar los pagos de las cuotas de mantenimiento de su Expediente EMECI, usted podr&aacute; optar por escoger cualquier de los siguientes medios:</p>
            <p>&nbsp;</p>
            
            <table width="100%" border="0" cellpadding="0" cellspacing="0">
            <tbody>
                <tr>
                    <td>
                       
                        <img src="images/Img_AmericanExpress.jpg" border="0">
                    </td>
                   
                    <td>
                      
                        <img src="images/Img_MasterCard.jpg" border="0">
                    </td>
                    <td>
                       
                        <img src="images/Img_Visa.jpg" border="0">
                    </td>
                    <td>
                        <img src="images/Img_Oxxo.jpg" border="0">
                    </td>
                </tr>
                </tbody></table>
            
			<p>&nbsp;</p> -->
           
				

    <!--
<p>&nbsp;</p>
            <p>Consideramos que es un mecanismo bastante sencillo, sin embargo, de tener dudas, con gusto el departamento de Soporte est&aacute; en la mayor de las disposiciones para auxiliarlo, ya sea v&iacute;a correo electr&oacute;nico a soporte@emeci.com, v&iacute;a Facebook o Twitter.</p>
            <p>Agradecemos su atenci&oacute;n y deseamos servirle para la conservaci&oacute;n de la salud de sus hijos.</p>
		</div>	
  
  
	</div>
</section>

    -->


<footer id="page-footer">
  <div class="container">
    <div class="row">
      <div class="col-xs-6">
        <p><strong>Expediente M&eacute;dico Electr&oacute;nico Cl&iacute;nico Internacional</strong> <br>
        2016 &reg; Todos los Derechos Resevados </p>
         
      </div>
      <div class="col-xs-6">
        <ul class="accesos list-inline">
          <li><a href="#">Apps M&oacute;viles</a></li>
          <li><a href="#">Soporte</a></li>
          <li><a href="avisos-de-privacidad.html">Aviso de Privacidad</a></li>
        </ul>
      </div>
    </div>
  </div>
</footer>
<!-- jQuery (necessary for Bootstrap's JavaScript plugins) --> 
<script src="js/jquery-1.11.3.min.js"></script> 
<!-- Include all compiled plugins (below), or include individual files as needed --> 
<script src="js/bootstrap.js"></script>
  <script>
    $(window).scroll(function() {
		if ($(this).scrollTop() > 100){ // Set position from top to add class
			$('.navbar').addClass("shrink");
		} else {
			$('.navbar').removeClass("shrink");
		}
	});
    </script>
     <script src="scripts/jquery.maskedinput.js"></script>
     <script>
      $(document).ready(function () {
          
          $('#description').mask('99999-9999-9999');
      });
         </script>
		 
    
</body>
</html>
